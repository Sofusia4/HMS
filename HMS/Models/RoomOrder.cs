using Microsoft.AspNetCore.Components.Forms;

namespace HMS.Models
{
	public class RoomOrder
	{
		public Guid Id { get; set; }
		public string RoomId { get; set; }
		public Room Room { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int PeopleCount { get; set; }
		public double TotalPrice { get; set; }
		public string UserId { get; set; }
		public User User { get; set; }
	}
}
