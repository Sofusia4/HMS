using HMS.Interfaces;
using HMS.Models;
using HMS.Models.Pages;
using HMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Controllers
{
	[Authorize(Roles = "Admin,Editor")]
	public class HotelController : Controller
	{
		private readonly IHotel _hotels;
		private readonly IWebHostEnvironment _appEnvironment;

		public HotelController(IHotel hotels, IWebHostEnvironment appEnvironment)
		{
			_hotels = hotels;
			_appEnvironment = appEnvironment;
		}

		public IActionResult Hotels(QueryOptions options)
		{
			return View(_hotels.GetAll(options));
		}

		[Route("hotel/delete-hotel")]
		[HttpDelete]
		public async Task<IActionResult> DeleteHotel(string hotelId)
		{
			var hotel = await _hotels.GetHotelAsync(hotelId);
			if (hotel != null)
			{
				await _hotels.DeleteHotelAsync(hotel);

				if (System.IO.File.Exists(_appEnvironment.WebRootPath + hotel.FullImageName))
				{
					System.IO.File.Delete(_appEnvironment.WebRootPath + hotel.FullImageName);
				}
			}
			else
			{
				return NotFound();
			}

			return Ok();
		}

		[Route("hotel/create-update-hotel")]
		[HttpGet]
		public async Task<IActionResult> CreateOrUpdateHotel(string hotelId)
		{
			CreateOrUpdateHotelViewModel model;

			if (hotelId != null)
			{
				Hotel hotel = await _hotels.GetHotelAsync(hotelId);
				if (hotel == null)
				{
					return NotFound();
				}
				model = new CreateOrUpdateHotelViewModel()
				{
					Id = hotelId,
					Name = hotel.Name,
					NameEng = hotel.NameEng,
					Description = hotel.Description,
					DescriptionEng = hotel.DescriptionEng,
					City = hotel.City,
					CityEng = hotel.CityEng,
					Address = hotel.Address,
					AddressEng = hotel.AddressEng,
					Image = hotel.Image,
					FullImageName = hotel.FullImageName
				};
				return View(model);
			}
			model = new CreateOrUpdateHotelViewModel()
			{
				Id = "create",
				Name = "",
				NameEng = "",
				Description = "",
				DescriptionEng = "",
				City = "",
				CityEng = "",
				Address = "",
				AddressEng = "",
				Image = "",
				FullImageName = ""
			};

			return View(model);
		}

		[Route("hotel/create-update-hotel")]
		[HttpPost]
		public async Task<IActionResult> CreateOrUpdateHotel(CreateOrUpdateHotelViewModel model)
		{
			if (ModelState.IsValid)
			{
				if (model.Id == "create")
				{
					string? fileImageName = null, imagePath = null;
					if (model.File != null)
					{
						fileImageName = model.File.FileName;

						if (fileImageName.Contains("\\"))
						{
							fileImageName = fileImageName.Substring(fileImageName.LastIndexOf('\\') + 1);
						}

						imagePath = "/hotelPhotos/" + Guid.NewGuid() + fileImageName;

						using (var fileStream = new FileStream(_appEnvironment.WebRootPath + imagePath, FileMode.Create))
						{

							await model.File.CopyToAsync(fileStream);
						}
					}

					Hotel hotel = new Hotel()
					{
						Name = model.Name,
						NameEng = model.NameEng,
						Description = model.Description,
						DescriptionEng = model.DescriptionEng,
						City = model.City,
						CityEng = model.CityEng,
						Address = model.Address,
						AddressEng = model.AddressEng,
						Image = fileImageName,
						FullImageName = imagePath
					};

					await _hotels.AddHotelAsync(hotel);

					return RedirectToAction(nameof(Hotels));
				}
				else
				{
					Hotel hotel = await _hotels.GetHotelAsync(model.Id);
					if (hotel == null)
					{
						return NotFound();
					}

					string? fileImageName = null, imagePath = null;
					if (model.File != null)
					{
						if (System.IO.File.Exists(_appEnvironment.WebRootPath + hotel.FullImageName))
						{
							System.IO.File.Delete(_appEnvironment.WebRootPath + hotel.FullImageName);
						}

						fileImageName = model.File.FileName;

						if (fileImageName.Contains("\\"))
						{
							fileImageName = fileImageName.Substring(fileImageName.LastIndexOf('\\') + 1);
						}

						imagePath = "/hotelPhotos/" + Guid.NewGuid() + fileImageName;

						using (var fileStream = new FileStream(_appEnvironment.WebRootPath + imagePath, FileMode.Create))
						{
							await model.File.CopyToAsync(fileStream);
						}

						hotel.FullImageName = imagePath;
						hotel.Image = fileImageName;
					}
					else
					{
						fileImageName = hotel.FullImageName;
						imagePath = hotel.Image;
					}

					hotel.Name = model.Name;
					hotel.NameEng = model.NameEng;
					hotel.Description = model.Description;
					hotel.DescriptionEng = model.DescriptionEng;
					hotel.City = model.City;
					hotel.CityEng = model.CityEng;
					hotel.Address = model.Address;
					hotel.AddressEng = model.AddressEng;

					await _hotels.UpdateHotelAsync(hotel);

					return RedirectToAction(nameof(Hotels));
				}
			}
			return View(model);
		}

	}
}
