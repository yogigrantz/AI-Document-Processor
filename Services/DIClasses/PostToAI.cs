using Microsoft.Extensions.Logging;
using Services.DTOs;
using Services.Interfaces;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.DIClasses;

public class PostToAI : IPostToAI
{
    private readonly HttpClient _httpClient;

    public PostToAI(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResumeAnalysis> Post(string url, string content)
    {
        var payload = new
        {
            model = "gpt-4.1-mini",
            input = $$"""
                    You are a resume parser.

                    Return ONLY valid JSON.

                    Do not include markdown.
                    Do not include explanations.
                    Do not include any text before or after the JSON.

                    Return JSON in exactly this format:

                    {
                        "fullName": "",
                        "currentTitle": "",
                        "skills": [],
                        "summary": ""
                    }

                    Rules:

                    - fullName is a string.
                    - currentTitle is a string.
                    - skills is an array of strings.
                    - summary is a string.
                    - If you cannot determine a value, use an empty string or an empty array.

                    Resume:

                    {{content}}
                    """
        };

        string json = JsonSerializer.Serialize(payload);

        using var httpContent = new StringContent(json, Encoding.UTF8,"application/json");

        using HttpResponseMessage result = await _httpClient.PostAsync(url, httpContent);

        string responseBody = await result.Content.ReadAsStringAsync();
        if (!result.IsSuccessStatusCode)
        {
            throw new Exception(responseBody);
        }

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        OpenAIResponse response = JsonSerializer.Deserialize<OpenAIResponse>(responseBody, options)!;

        string aiText = response.Output.FirstOrDefault()?.Content.FirstOrDefault(c => c.Type == "output_text")?.Text ?? "";
        aiText = aiText.Replace("```json", "").Replace("```", "").Trim();
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        ResumeAnalysis resume = JsonSerializer.Deserialize<ResumeAnalysis>(aiText, jsonOptions)!;

        result.EnsureSuccessStatusCode();

        return resume;
    }

}
