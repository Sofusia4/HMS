using HMS.Models;

namespace HMS.Interfaces
{
	public interface IHotel
	{
		Task<IEnumerable<Hotel>> GetAllHotelsAsync();
		Task<IEnumerable<Hotel>> GetAllHotelsWithRoomsAsync();
		Task<Hotel> GetHotelAsync(string id);
		Task AddHotelAsync(Hotel hotel);
		Task UpdateHotelAsync(Hotel hotel);
		Task DeleteHotelAsync(Hotel hotel);

	}
}
