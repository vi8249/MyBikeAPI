using System.Threading.Tasks;
using YouBikeAPI.Models;

namespace YouBikeAPI.Services
{
    public interface IPaidmentService
    {
        Task<(bool, string)> PayBill(string userId, string lan, string lon);
        Task CreateHistoryRoute(HistoryRoute historyRoute);
    }
}
