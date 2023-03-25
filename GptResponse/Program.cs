using System.Net.Http.Headers;
using System.Net.Http.Json;

public class OpenAIApiClient
{
    private readonly HttpClient _httpClient;

    public OpenAIApiClient(string apiKey)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
    }

    public async Task<string> SendPrompt(string prompt, string model)
    {
        var requestBody = new
        {
            prompt = prompt,
            model = model,
            max_tokens = 150,
            temperature = 0.5
        };

        var response = await _httpClient.PostAsJsonAsync("completions", requestBody);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content;//.Content.ReadAsStringAsync();

        var result = await responseBody.Result;
        return result;
    }
}

internal static class Program {
    private static void Main(string[] args) {
        var client = new OpenAIApiClient("you-api");
        while (true) {
            Console.Write("> ");
            var request = Console.ReadLine();
            if (string.IsNullOrEmpty(request)) {
                Console.WriteLine("Try harder...");
                continue;
            }
            //var response = client.SendPrompt(request, "gpt-3.5-turbo-0301").Result;
            //var response = client.SendPrompt(request, "davinci").Result;
            var results = client.SendPrompt(request, "gpt-4");
            //var response = results.Result;
                
            Console.WriteLine(results.Result
            );
        }
    }
}
