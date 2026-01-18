using HMS.Models;
using HMS.Models.Pages;

namespace HMS.Interfaces
{
	public interface IHotel
	{
		PagedList<Hotel> GetAll(QueryOptions options);
		Task<IEnumerable<Hotel>> GetAllHotelsAsync();
		Task<IEnumerable<Hotel>> GetAllHotelsWithRoomsAsync();
		Task<List<string>> GetAllCitiesAsync();
		Task<Hotel> GetHotelAsync(string id);
		Task AddHotelAsync(Hotel hotel);
		Task UpdateHotelAsync(Hotel hotel);
		Task DeleteHotelAsync(Hotel hotel);

	}
}
