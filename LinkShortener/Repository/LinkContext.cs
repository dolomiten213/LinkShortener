using LinkShortener.Domain;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Repository;

public class LinkContext : DbContext
{
    private static bool _isMigrated;
    
//==============================================================//

    public DbSet<UriEntity> Links { get; set; } = null!;
    
//==============================================================//

    public LinkContext(DbContextOptions<LinkContext> options) : base(options)
    {
        if (_isMigrated) return;
        Database.Migrate();
        _isMigrated = true;
    }
    
}