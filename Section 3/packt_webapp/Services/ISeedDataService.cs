using System.Threading.Tasks;

namespace packt_webapp.Services
{
    public interface ISeedDataService
    {
        Task EnsureSeedData();
    }
}
