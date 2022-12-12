using LinkShortener.Dtos;
using LinkShortener.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.Controllers;

[ApiController]
[Route("[controller]")]
public class UriController : ControllerBase
{
    private readonly ILogger<UriController> _logger;
    private readonly ILinksService _linksService;
    private readonly IQrService _qrService;
    private readonly string? _url;

    public UriController(
        ILogger<UriController> logger
        , ILinksService linksService
        , IQrService qrService
        , IConfiguration configuration
        )
    {
        _logger = logger;
        _linksService = linksService;
        _qrService = qrService;
        _url = configuration.GetSection("Urls").Value;
    }

    [HttpPost("add-uri")]
    public async Task<IActionResult> AddLink(UriDto uri)
    {
        var res = await _linksService.AddLinkAsync(uri.Uri);
        return res is null
            ? BadRequest($"{uri.Uri} is not a valid uri")
            : Ok(res.Uri);
    }
    
    [HttpGet("get-uri")]
    public async Task<IActionResult> GetLink(string shortLink)
    {
        var res = await _linksService.GetLinkAsync(shortLink);
        return res is null
            ? NotFound($"Uri with short version {shortLink} has not been created yet")
            : Redirect(res.Uri);
    }
    
    [HttpGet("get-qr")]
    public IActionResult GetQr(string token)
    {
        var qr = _qrService.MakeQr($"{_url}/Uri/get-uri?shortLink={token}");        
        return File(qr, "image/jpeg");
    }
}