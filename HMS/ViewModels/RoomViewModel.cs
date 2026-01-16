using Org.BouncyCastle.Bcpg.OpenPgp;
using HMS.Models;

namespace HMS.ViewModels
{
	public class RoomViewModel
	{
		public Room Room {  get; set; }
		//public List<Comment> Comments { get; set; }
		public string ReturnUrl { get; set; }
	}
}
