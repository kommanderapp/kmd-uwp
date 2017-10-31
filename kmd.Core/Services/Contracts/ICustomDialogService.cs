using kmd.Core.Explorer.Controls.ContentDialogs;
using System.Threading.Tasks;

namespace kmd.Core.Services.Contracts
{
    public interface ICustomDialogService
    {
        Task<string> Prompt(string title, string initialValue = null);
        Task<NameCollisionDialogResult> NameCollisionDialog(string filename, bool isForSingleFile = true);
    }
}
