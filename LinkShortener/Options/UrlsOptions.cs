namespace LinkShortener.Options;

public class UrlsOptions
{
    public const string Urls = nameof(Urls);

    public string Title { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
}