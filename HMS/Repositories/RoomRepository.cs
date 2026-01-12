using HMS.Data;
using HMS.Interfaces;
using HMS.Models;
using HMS.Models.Pages;
using Microsoft.EntityFrameworkCore;

namespace HMS.Repositories
{
	public class RoomRepository : IRoom
	{
		private readonly ApplicationContext _context;
		public RoomRepository(ApplicationContext context)
		{
			_context = context;
		}

		public async Task AddRoomAsync(Room room)
		{
			if (!String.IsNullOrEmpty(room.HotelId))
			{
				var hotel = _context.Hotels.FirstOrDefault(e => e.Id.ToString() == room.HotelId);
				room.Hotel = hotel;
			}

			_context.Rooms.Add(room);
			await _context.SaveChangesAsync();

		}

		public async Task DeleteRoomAsync(Room room)
		{
			_context.Rooms.Remove(room);
			await _context.SaveChangesAsync();

		}

        public PagedList<Room> GetAll(QueryOptions options)
        {
            return new PagedList<Room>(_context.Rooms.AsNoTracking().Include(e => e.Hotel), options);
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
		{
			return await _context.Rooms.AsNoTracking().ToListAsync();
		}

        public PagedList<Room> GetAllRoomsByHotel(QueryOptions options, string hotelId)
        {
            return new PagedList<Room>(_context.Rooms.Include(e => e.Hotel).Where(e => e.HotelId == hotelId), options);
        }

        public async Task<IEnumerable<Room>> GetAllRoomsByHotelAsync(string hotelId)
		{
			return await _context.Rooms.Include(e => e.Hotel).Where(e => e.HotelId.Equals(hotelId)).ToListAsync();
		}

		public async Task<IEnumerable<Room>> GetAllRoomsWithHotelsAsync()
		{
			return await _context.Rooms.Include(e => e.Hotel).ToListAsync();
		}

		public async Task<Room> GetRoomAsync(string id)
		{
			return await _context.Rooms.FirstOrDefaultAsync(e => e.Id.ToString() == id);
		}

		public async Task UpdateRoomAsync(Room room)
		{
			var hotel = await _context.Hotels.FirstOrDefaultAsync(e => room.HotelId.Equals(e.Id));

			var currentRoom = await _context.Rooms.Include(e => e.Hotel).FirstOrDefaultAsync(e => e.Id.Equals(room.Id));
			currentRoom.Number = room.Number;
			currentRoom.RoomType = room.RoomType;
			currentRoom.PricePerNight = room.PricePerNight;
			currentRoom.Capacity = room.Capacity;
			currentRoom.Description = room.Description;
			currentRoom.FullImageName = room.FullImageName;
			currentRoom.Image = room.Image;
			currentRoom.HotelId = room.HotelId;
			currentRoom.Hotel = hotel;

			_context.Rooms.Update(currentRoom);
			await _context.SaveChangesAsync();

		}
	}
}
