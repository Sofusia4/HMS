using HMS.Data;
using HMS.Interfaces;
using HMS.Models;
using HMS.Models.Pages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HMS.Repositories
{
	public class RoomOrderRepository : IRoomOrder
	{
		private readonly ApplicationContext _context;
		public RoomOrderRepository(ApplicationContext context)
		{
			_context = context;
		}
		public async Task AddRoomOrderAsync(RoomOrder order)
		{
			if (!String.IsNullOrEmpty(order.RoomId))
			{
				var room = _context.Rooms.FirstOrDefault(e => e.Id.ToString() == order.RoomId);
				order.Room = room;
			}
			if (!String.IsNullOrEmpty(order.UserId))
			{
				var user = _context.Users.FirstOrDefault(e => e.Id.ToString() == order.UserId);
				order.User = user;
			}

			_context.RoomOrders.Add(order);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteRoomOrderAsync(RoomOrder order)
		{
			_context.RoomOrders.Remove(order);
			await _context.SaveChangesAsync();
		}

		public PagedList<RoomOrder> GetAll(QueryOptions options)
		{
			return new PagedList<RoomOrder>(_context.RoomOrders.AsNoTracking().Include(e => e.Room).Include(e => e.User), options);
		}

		public async Task<IEnumerable<RoomOrder>> GetAllRoomOrdersAsync()
		{
			return await _context.RoomOrders.AsNoTracking().ToListAsync();
		}

		public async Task<IEnumerable<RoomOrder>> GetAllRoomOrdersByHotelAsync(string hotelId)
		{
			return await _context.RoomOrders.Include(e => e.Room).ThenInclude(e => e.Hotel).Where(e => e.Room.HotelId == hotelId).ToListAsync();
		}

		public PagedList<RoomOrder> GetAllRoomOrdersByRoom(QueryOptions options, string roomId)
		{
			return new PagedList<RoomOrder>(_context.RoomOrders.Include(e => e.Room).ThenInclude(e => e.Hotel).Where(e => e.RoomId == roomId), options);
		}

		public async Task<IEnumerable<RoomOrder>> GetAllRoomOrdersByRoomAsync(string roomId)
		{
			return await _context.RoomOrders.Include(e => e.Room).ThenInclude(e => e.Hotel).Where(e => e.RoomId == roomId).ToListAsync();
		}

		public async Task<IEnumerable<RoomOrder>> GetAllRoomOrdersWithRoomsAndUsersAsync()
		{
			return await _context.RoomOrders.AsNoTracking().Include(e => e.Room).Include(e => e.User).ToListAsync();
		}

		public async Task<RoomOrder> GetRoomOrderAsync(string id)
		{
			return await _context.RoomOrders.FirstOrDefaultAsync(e => e.Id.ToString() == id);
		}

		public async Task<IEnumerable<RoomOrder>> GetRoomOrdersWithPageAsync(int page, int pageSize)
		{
			return await _context.RoomOrders.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
		}

		public async Task UpdateRoomOrderAsync(RoomOrder order)
		{
			var room = await _context.Rooms.FirstOrDefaultAsync(e => order.RoomId.Equals(e.Id));
			var user = await _context.Users.FirstOrDefaultAsync(e => order.UserId.Equals(e.Id));

			var currentOrder = await _context.RoomOrders.Include(e => e.Room).Include(e => e.User).FirstOrDefaultAsync(e => e.Id.Equals(order.Id));
			currentOrder.RoomId = order.RoomId;
			currentOrder.Room = room;
			currentOrder.StartDate = order.StartDate;
			currentOrder.EndDate = order.EndDate;
			currentOrder.PeopleCount = order.PeopleCount;
			currentOrder.TotalPrice = order.TotalPrice;
			currentOrder.UserId = order.UserId;
			currentOrder.User = user;

			_context.RoomOrders.Update(currentOrder);
			await _context.SaveChangesAsync();
		}
	}
}
