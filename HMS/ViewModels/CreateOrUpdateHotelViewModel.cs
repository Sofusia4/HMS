using System.ComponentModel.DataAnnotations;

namespace HMS.ViewModels
{
    public class CreateOrUpdateHotelViewModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

		[Required]
		[Display(Name = "NameEng")]
		public string NameEng { get; set; }

		[Required]
        [Display(Name = "Description")]
        public string? Description { get; set; }

		[Required]
		[Display(Name = "DescriptionEng")]
		public string? DescriptionEng { get; set; }

		[Required]
		[Display(Name = "City")]
		public string City { get; set; }

		[Required]
		[Display(Name = "CityEng")]
		public string CityEng { get; set; }

		[Required]
		[Display(Name = "Address")]
		public string Address { get; set; }

		[Required]
		[Display(Name = "AddressEng")]
		public string AddressEng { get; set; }

		[Required]
        [Display(Name = "Image")]
        public string? Image { get; set; }

        [Required]
        [Display(Name = "FullImageName")]
        public string? FullImageName { get; set; }


        [Display(Name = "ImageFile")]
        public IFormFile? File { get; set; }
    }
}
