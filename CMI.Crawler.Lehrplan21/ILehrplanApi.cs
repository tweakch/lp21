namespace CMI.Crawler.Lehrplan21;

public interface ILehrplanApi
{
    Task<HttpResponseMessage> GetAsync(string id, string language = "DE", string canton = "ZH");
}
