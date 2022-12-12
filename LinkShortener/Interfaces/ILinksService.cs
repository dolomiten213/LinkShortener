using LinkShortener.Dtos;

namespace LinkShortener.Interfaces;

public interface ILinksService
{
    Task<UriDto?> AddLinkAsync(string longUri);
    Task<UriDto?> GetLinkAsync(string shortUri);
}