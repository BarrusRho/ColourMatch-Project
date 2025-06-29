using System.Threading.Tasks;

namespace ColourMatch
{
    public interface IView
    {
        bool IsVisible { get; }
        void Show();
        void Hide();
        Task ShowAsync();
        Task HideAsync();
    }
}
