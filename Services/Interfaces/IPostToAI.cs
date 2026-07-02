using Services.DTOs;
using System.Threading.Tasks;

namespace Services.Interfaces;

public interface IPostToAI
{
    Task<ResumeAnalysis> Post(string url, string json);
}