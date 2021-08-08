using System.Threading.Tasks;

namespace YouBikeAPI.Services
{
	public interface IPaidmentService
	{
		Task<(bool, string)> PayBill(string userId, string lan, string lon);
	}
}
