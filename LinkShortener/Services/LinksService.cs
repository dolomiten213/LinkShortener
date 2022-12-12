using LinkShortener.Domain;
using LinkShortener.Dtos;
using LinkShortener.Interfaces;
using LinkShortener.Repository;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Services;

public class LinksService : ILinksService
{
    private readonly LinkContext _linkContext;

    public LinksService(LinkContext linkContext)
    {
        _linkContext = linkContext;
    }


    public async Task<UriDto?> AddLinkAsync(string uri)
    {
        if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
        {
            return null;
        }
        
        // check for links which have been already added
        var cachedLink = await _linkContext
            .Links
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Link == uri);

        if (cachedLink is not null) return new UriDto(cachedLink.Token);

        // generate new id
        var maxId = await _linkContext
            .Links
            .AsNoTracking()
            .Select(x => x.Id)
            .DefaultIfEmpty()
            .MaxAsync();

        if (maxId == 0) maxId--;
        // encode id into short uri
        var shortUrl = Encode(++maxId);
        
        var entity = new UriEntity
        {
            Link = uri,
            Token = shortUrl,
            Id = maxId
        };
        _linkContext.Links.Add(entity);
        try
        {
            await _linkContext.SaveChangesAsync();
            return new UriDto(shortUrl);
        }
        
        catch (DbUpdateException)
        {
             // detach entity if SaveChanges failed
             _linkContext.Entry(entity).State = EntityState.Detached;
             // try again
             return await AddLinkAsync(uri);
        }
    }

    public async Task<UriDto?> GetLinkAsync(string token)
    {
        return await _linkContext
            .Links
            .AsNoTracking()
            .Where(x => x.Token == token)
            .Select(x => new UriDto(x.Link))
            .FirstOrDefaultAsync();
    }

    private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private static readonly int Base = Alphabet.Length;

    private static string Encode(int i)
    {
        if (i == 0) return Alphabet[0].ToString();

        var s = string.Empty;

        while (i > 0)
        {  
            s += Alphabet[i % Base];
            i /= Base;
        }

        return string.Join(string.Empty, s.Reverse());
    }
    
}