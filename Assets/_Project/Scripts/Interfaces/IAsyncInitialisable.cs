using System.Threading.Tasks;

namespace ColourMatch
{
    public interface IAsyncInitialisable
    {
        Task InitialiseAsync();
    }
}