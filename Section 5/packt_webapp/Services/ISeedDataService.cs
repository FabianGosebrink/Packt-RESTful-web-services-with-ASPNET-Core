using System.Threading.Tasks;

namespace PacktWebapp.Services
{
    public interface ISeedDataService
    {
        Task EnsureSeedData();
    }
}
