using Microsoft.AspNetCore.Identity;

namespace HMS.Models
{
	public class User : IdentityUser
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		
	}

}
