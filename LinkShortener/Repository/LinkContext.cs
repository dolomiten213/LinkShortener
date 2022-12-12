using LinkShortener.Domain;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Repository;

public class LinkContext : DbContext
{
    private static bool _isMigrated;
    
//==============================================================//

    public DbSet<UriEntity> Links { get; set; } = null!;
    
//==============================================================//

    public LinkContext() {}

    
    public LinkContext(DbContextOptions<LinkContext> options) : base(options)
    {
        if (_isMigrated) return;
        Database.Migrate();
        _isMigrated = true;
    }
    
//==============================================================//
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost:5432;Database=Links;Username=postgres;Password=1;");
    }
}