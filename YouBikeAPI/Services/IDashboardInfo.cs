using System.Threading.Tasks;
using YouBikeAPI.Models;

namespace YouBikeAPI.Services
{
    public interface IDashboardInfo
    {
        Task<Dashboard> GetDashboardInfo();
    }
}