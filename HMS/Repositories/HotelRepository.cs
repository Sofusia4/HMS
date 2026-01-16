using HMS.Data;
using HMS.Interfaces;
using HMS.Models;
using HMS.Models.Pages;
using Microsoft.EntityFrameworkCore;

namespace HMS.Repositories
{
	public class HotelRepository : IHotel
	{
		private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        public HotelRepository(ApplicationContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task AddHotelAsync(Hotel hotel)
		{
			_context.Hotels.Add(hotel);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteHotelAsync(Hotel hotel)
		{
			var rooms = _context.Rooms.Where(r => r.HotelId.Equals(hotel.Id.ToString()));
			if (rooms != null)
			{
				foreach (var r in rooms)
				{
                    if (System.IO.File.Exists(_appEnvironment.WebRootPath + r.FullImageName))
                    {
                        System.IO.File.Delete(_appEnvironment.WebRootPath + r.FullImageName);
                    }
                }
			}            
            _context.Hotels.Remove(hotel);
			await _context.SaveChangesAsync();
		}

		public PagedList<Hotel> GetAll(QueryOptions options)
		{
			return new PagedList<Hotel>(_context.Hotels, options);
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
