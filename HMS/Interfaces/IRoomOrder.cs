using HMS.Models.Pages;
using HMS.Models;

namespace HMS.Interfaces
{
	public interface IRoomOrder
	{
		Task<IEnumerable<RoomOrder>> GetRoomOrdersWithPageAsync(int page, int pageSize);
		PagedList<RoomOrder> GetAllRoomOrdersByRoom(QueryOptions options, string roomId);
		PagedList<RoomOrder> GetAll(QueryOptions options);
		//PagedList<Room> GetRoomsWithAdditionalOptions(string hotelId, RoomType[] type, string city, int capacity, QueryOptions options);
		Task<IEnumerable<RoomOrder>> GetAllRoomOrdersAsync();
		Task<IEnumerable<RoomOrder>> GetAllRoomOrdersWithRoomsAndUsersAsync();
		Task<IEnumerable<RoomOrder>> GetAllRoomOrdersByHotelAsync(string hotelId);
		Task<IEnumerable<RoomOrder>> GetAllRoomOrdersByRoomAsync(string roomId);
		Task<RoomOrder> GetRoomOrderAsync(string id);

		Task AddRoomOrderAsync(RoomOrder order);
		Task UpdateRoomOrderAsync(RoomOrder order);
		Task DeleteRoomOrderAsync(RoomOrder order);
	}
}
