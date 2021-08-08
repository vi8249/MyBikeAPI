using System.ComponentModel.DataAnnotations;

namespace YouBikeAPI.Dtos
{
	public class LoginDto
	{
		[Required]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
