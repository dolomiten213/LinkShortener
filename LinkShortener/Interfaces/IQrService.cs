namespace LinkShortener.Interfaces;

public interface IQrService
{
    Stream MakeQr(string content);
}