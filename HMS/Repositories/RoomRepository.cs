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

		public PagedList<Room> GetRoomsWithAdditionalOptions(string hotelId, RoomType[] type, int[] pricePerNight, QueryOptions options)
		{
			IQueryable<Room> rooms = _context.Rooms.Include(e => e.Hotel);


			if (pricePerNight != null && pricePerNight.Length > 0)
			{
				if (!pricePerNight.Contains(1))
				{
					rooms = rooms.Except(rooms.Where(e => e.PricePerNight < 100));
				}
				if (!pricePerNight.Contains(2))
				{
					rooms = rooms.Except(rooms.Where(e => e.PricePerNight >= 100 && e.PricePerNight < 200));
				}
				if (!pricePerNight.Contains(3))
				{
					rooms = rooms.Except(rooms.Where(e => e.PricePerNight >= 200 && e.PricePerNight < 300));
				}
				if (!pricePerNight.Contains(4))
				{
					rooms = rooms.Except(rooms.Where(e => e.PricePerNight >= 300 && e.PricePerNight < 400));
				}
				if (!pricePerNight.Contains(5))
				{
					rooms = rooms.Except(rooms.Where(e => e.PricePerNight >= 400 && e.PricePerNight < 500));
				}
				if (!pricePerNight.Contains(6))
				{
					rooms = rooms.Except(rooms.Where(e => e.PricePerNight >= 500));
				}
			}


			if (hotelId != "all" && hotelId != null)
			{
				rooms = rooms.Where(e => e.HotelId.Equals(hotelId));
			}

			if (type != null && type.Length > 0)
			{
				if (!type.Contains(RoomType.Standard))
				{
					rooms = rooms.Where(e => e.RoomType != RoomType.Standard);
				}
				if (!type.Contains(RoomType.Luxury))
				{
					rooms = rooms.Where(e => e.RoomType != RoomType.Luxury);
				}
				if (!type.Contains(RoomType.Suite))
				{
					rooms = rooms.Where(e => e.RoomType != RoomType.Suite);
				}
			}


			return new PagedList<Room>(rooms, options);
		}

		public async Task<IEnumerable<Room>> GetRoomWithPageAsync(int page, int pageSize)
		{
			return await _context.Rooms.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
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
