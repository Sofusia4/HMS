using HMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using HMS.Models;
using System.ComponentModel.DataAnnotations;

namespace HMS.ViewModels
{
	public class CreateOrUpdateRoomViewModel
	{
		[Key]
		public string Id { get; set; }

		[Required]
		[Display(Name = "Number")]
		public int Number { get; set; }

        [Required]
        [Display(Name = "RoomType")]
        public RoomType RoomType { get; set; }

        [Required]
        [Display(Name = "PricePerNight")]
        public double PricePerNight { get; set; }

        [Required]
        [Display(Name = "Capacity")]
        public int Capacity { get; set; }

        [Required]
		[Display(Name = "Description")]
		public string? Description { get; set; }

		[Required]
		[Display(Name = "HotelId")]
		public string HotelId { get; set; }


		[Required]
		[Display(Name = "Image")]
		public string? Image { get; set; }

		[Required]
		[Display(Name = "FullImageName")]
		public string? FullImageName { get; set; }


		[Display(Name = "ImageFile")]
		public IFormFile? File { get; set; }


        [Display(Name = "Hotels")]
        public IEnumerable<SelectListItem>? AllHotels { get; set; }
    }
}
