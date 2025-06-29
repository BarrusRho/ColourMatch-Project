using System.Threading.Tasks;

namespace ColourMatch
{
    public interface IController
    {
        void Init(IView view);
        void Show();
        void Hide();
        Task ShowAsync();
        Task HideAsync();
    }
}