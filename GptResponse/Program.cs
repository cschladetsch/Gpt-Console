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
        var responseBody = response.Content.ReadAsStringAsync();

        return responseBody.Result;
    }
}

internal static class Program {
    private static void Main(string[] args) {
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        if (string.IsNullOrEmpty(apiKey)) {
            Console.WriteLine("Please set the OPENAI_API_KEY environment variable.");
            return;
        }   
        var client = new OpenAIApiClient(apiKey);
        while (true) {
            Console.Write("> ");
            var request = Console.ReadLine();
            if (string.IsNullOrEmpty(request)) {
                Console.WriteLine("Try harder...");
                continue;
            }
            //var response = client.SendPrompt(request, "gpt-3.5-turbo-0301").Result;
            //var response = client.SendPrompt(request, "davinci").Result;
            //var model = "GPT-4";
            var model = "davinci";
            var results = client.SendPrompt(request, model).Result;

            Console.WriteLine(results);
        }
    }
}
