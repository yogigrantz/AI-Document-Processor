using System.Collections.Generic;

namespace Services.DTOs;

public class OpenAIResponse
{
    public List<OpenAIOutput> Output { get; set; } = [];
}

public class OpenAIOutput
{
    public List<OpenAIContent> Content { get; set; } = [];
}

public class OpenAIContent
{
    public string Type { get; set; } = "";

    public string Text { get; set; } = "";
}

public class ResumeAnalysis
{
    public string FullName { get; set; } = "";

    public string CurrentTitle { get; set; } = "";

    public List<string> Skills { get; set; } = [];

    public string Summary { get; set; } = "";
}