using System.Runtime.InteropServices.ComTypes;
using Windows.UI;

namespace Gritsenko.Universal.Abstract
{
    public interface IStatusBarService
    {
        void Show(string statusText = "");
        void Hide();

        void ShowHeader(string statusText, Color foregroundColor, Color backgroundColor, double opacity = 1);
        void HideHeader();
    }
}