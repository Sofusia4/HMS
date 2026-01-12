using HMS.Data;
using HMS.Interfaces;
using HMS.Models;
using Microsoft.EntityFrameworkCore;

namespace HMS.Repositories
{
	public class HotelRepository : IHotel
	{
		private readonly ApplicationContext _context;
		public HotelRepository(ApplicationContext context)
		{
			_context = context;
		}

		public async Task AddHotelAsync(Hotel hotel)
		{
			_context.Hotels.Add(hotel);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteHotelAsync(Hotel hotel)
		{
			_context.Hotels.Remove(hotel);
			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<Hotel>> GetAllHotelsAsync()
		{
			return await _context.Hotels.ToListAsync();
		}

		public async Task<IEnumerable<Hotel>> GetAllHotelsWithRoomsAsync()
		{
			return await _context.Hotels.Include(e => e.Rooms).ToListAsync();
		}

		public async Task<Hotel> GetHotelAsync(string id)
		{
			return await _context.Hotels.FirstOrDefaultAsync(e => e.Id.ToString() == id);
		}

		public async Task UpdateHotelAsync(Hotel hotel)
		{
			_context.Hotels.Update(hotel);
			await _context.SaveChangesAsync();

		}
	}
}
