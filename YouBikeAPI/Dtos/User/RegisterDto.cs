using System.ComponentModel.DataAnnotations;

namespace YouBikeAPI.Dtos
{
	public class RegisterDto
	{
		[Required]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }

		[Required]
		[Compare("Password", ErrorMessage = "密碼不一致")]
		public string ConfirmPassword { get; set; }

		public decimal Money { get; set; }
	}
}
