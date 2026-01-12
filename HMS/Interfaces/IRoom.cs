using HMS.Models;

namespace HMS.Interfaces
{
	public interface IRoom
	{
		Task<IEnumerable<Room>> GetAllRoomsAsync();
		Task<IEnumerable<Room>> GetAllRoomsWithHotelsAsync();
		Task<IEnumerable<Room>> GetAllRoomsByHotelAsync(string hotelId);
		Task<Room> GetRoomAsync(string id);

		Task AddRoomAsync(Room room);
		Task UpdateRoomAsync(Room room);
		Task DeleteRoomAsync(Room room);
	}
}
