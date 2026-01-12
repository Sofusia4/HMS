namespace HMS.Models
{
	public class Hotel
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string NameEng { get; set; }
		public string Description { get; set; }
		public string DescriptionEng { get; set; }
		public string City { get; set; }
		public string CityEng { get; set; }
		public string Address { get; set; }
		public string AddressEng { get; set; }
		public List<Room> Rooms { get; set; } = new List<Room>();
	}
}
