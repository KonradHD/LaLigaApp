

public class APIManager
{
    private HttpClient client = new HttpClient();

    private HttpRequestMessage getRequest(string url)
    {
        HttpRequestMessage request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url),
            Headers =
                {
                    { "x-rapidapi-key", "0d6737ba6emshbace7481a631987p185cddjsn51c040c979c8" },
                    { "x-rapidapi-host", "api-football-v1.p.rapidapi.com" },
                },
        };
        return request;
    }
}