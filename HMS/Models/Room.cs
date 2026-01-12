using Org.BouncyCastle.Bcpg;

namespace HMS.Models
{
	public class Room
	{
		public Guid Id { get; set; }
		public int Number { get; set; }
		public RoomType RoomType { get; set; }
		public double PricePerNight { get; set; }
		public int Capacity { get; set; }
		public string Description { get; set; }
		public string? FullImageName { get; set; }
		public string? Image { get; set; }
		public string HotelId { get; set; }
		public Hotel Hotel { get; set; }

	}
	public enum RoomType {
		Standard,
		Luxury,
		Suite 
	}
}
