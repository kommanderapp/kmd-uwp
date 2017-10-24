using System.Threading.Tasks;

namespace kmd.Core.Services.Contracts
{
    public interface IPromptService
    {
        Task<string> Prompt(string title, string buttonContent, string initialValue = null);
    }
}
