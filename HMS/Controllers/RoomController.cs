using HMS.Interfaces;
using HMS.Models.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HMS.ViewModels;
using HMS.Models;

namespace HMS.Controllers
{
    [Authorize(Roles = "Admin,Editor")]
    public class RoomController : Controller
    {
        private readonly IRoom _room;
        private readonly IWebHostEnvironment _appEnvironment;

        public RoomController(IRoom room, IWebHostEnvironment appEnvironment)
        {
            _room = room;
            _appEnvironment = appEnvironment;
        }

        public IActionResult Rooms(QueryOptions options)
        {
            return View(_room.GetAll(options));
        }


        [Route("room/delete-room")]
        [HttpDelete]
        public async Task<IActionResult> DeleteRoom(string roomId)
        {
            var room = await _room.GetRoomAsync(roomId);
            if (room != null)
            {
                await _room.DeleteRoomAsync(room);

                if (System.IO.File.Exists(_appEnvironment.WebRootPath + room.FullImageName))
                {
                    System.IO.File.Delete(_appEnvironment.WebRootPath + room.FullImageName);
                }
            }
            else
            {
                return NotFound();
            }

            return Ok();
        }


        [Route("room/create-update-room")]
        [HttpGet]
        public async Task<IActionResult> CreateOrUpdateRoom([FromServices] IHotel _hotel, string roomId)
        {
            CreateOrUpdateRoomViewModel model;

            if (roomId != null)
            {
                Room room = await _room.GetRoomAsync(roomId);
                if (room == null)
                {
                    return NotFound();
                }
                model = new CreateOrUpdateRoomViewModel()
                {
                    Id = roomId,
                    Number = room.Number,
                    RoomType = room.RoomType,
                    PricePerNight = room.PricePerNight,
                    Capacity = room.Capacity,
                    Description = room.Description,
                    Image = room.Image,
                    FullImageName = room.FullImageName,
                    HotelId = room.HotelId
                };

                var allHotels2 = await _hotel.GetAllHotelsAsync();
                model.AllHotels = allHotels2.Select(e => new SelectListItem
                {
                    Text = e.Name,
                    Value = e.Id.ToString()
                });

                return View(model);
            }
            model = new CreateOrUpdateRoomViewModel()
            {
                Id = "create",
                Number = 0,
                RoomType = RoomType.Standard,
                PricePerNight = 0,
                Capacity = 0,
                Description = "",
                Image = "",
                FullImageName = "",
                HotelId = ""
            };

            var allHotels = await _hotel.GetAllHotelsAsync();

            model.AllHotels = allHotels.Select(e => new SelectListItem
            {
                Text = e.Name,
                Value = e.Id.ToString()
            });

            return View(model);
        }

        [Route("room/create-update-room")]
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateRoom([FromServices] IHotel _hotel, CreateOrUpdateRoomViewModel model)
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

                        imagePath = "/roomPhotos/" + Guid.NewGuid() + fileImageName;

                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + imagePath, FileMode.Create))
                        {

                            await model.File.CopyToAsync(fileStream);
                        }
                    }

                    Hotel hotel = await _hotel.GetHotelAsync(model.HotelId);


                    Room room = new Room()
                    {
                        Number = model.Number,
                        RoomType = model.RoomType,
                        PricePerNight = model.PricePerNight,
                        Capacity = model.Capacity,
                        Description = model.Description,
                        Image = fileImageName,
                        FullImageName = imagePath,
                        Hotel = hotel,
                        HotelId = model.HotelId
                    };

                    await _room.AddRoomAsync(room);

                    return RedirectToAction(nameof(Rooms));
                }
                else
                {
                    Room room = await _room.GetRoomAsync(model.Id);
                    if (room == null)
                    {
                        return NotFound();
                    }

                    string? fileImageName = null, imagePath = null;
                    if (model.File != null)
                    {
                        if (System.IO.File.Exists(_appEnvironment.WebRootPath + room.FullImageName))
                        {
                            System.IO.File.Delete(_appEnvironment.WebRootPath + room.FullImageName);
                        }

                        fileImageName = model.File.FileName;

                        if (fileImageName.Contains("\\"))
                        {
                            fileImageName = fileImageName.Substring(fileImageName.LastIndexOf('\\') + 1);
                        }

                        imagePath = "/roomPhotos/" + Guid.NewGuid() + fileImageName;

                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + imagePath, FileMode.Create))
                        {
                            await model.File.CopyToAsync(fileStream);
                        }

                        room.FullImageName= imagePath;
                        room.Image = fileImageName;
                    }

                    Hotel hotel = await _hotel.GetHotelAsync(model.HotelId);

                    room.Number = model.Number;
                    room.RoomType = model.RoomType;
                    room.PricePerNight = model.PricePerNight;
                    room.Capacity = model.Capacity;
                    room.Description = model.Description;
                    room.Hotel = hotel;
                    room.HotelId = model.HotelId;
                    
                    await _room.UpdateRoomAsync(room);

                    return RedirectToAction(nameof(Rooms));
                }
            }
            return View(model);
        }
    }
}
