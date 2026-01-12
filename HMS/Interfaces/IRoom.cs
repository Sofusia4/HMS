using HMS.Models;
using HMS.Models.Pages;

namespace HMS.Interfaces
{
	public interface IRoom
	{
        PagedList<Room> GetAllRoomsByHotel(QueryOptions options, string hotelId);
        PagedList<Room> GetAll(QueryOptions options);
        Task<IEnumerable<Room>> GetAllRoomsAsync();
		Task<IEnumerable<Room>> GetAllRoomsWithHotelsAsync();
		Task<IEnumerable<Room>> GetAllRoomsByHotelAsync(string hotelId);
		Task<Room> GetRoomAsync(string id);

		Task AddRoomAsync(Room room);
		Task UpdateRoomAsync(Room room);
		Task DeleteRoomAsync(Room room);
	}
}
