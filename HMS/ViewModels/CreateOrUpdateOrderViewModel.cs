using HMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HMS.ViewModels
{
	public class CreateOrUpdateOrderViewModel
	{
		[Key]
		public string Id { get; set; }

		[Required]
		[Display(Name = "People")]
		public int People { get; set; }

		[Required]
		[Display(Name = "StartDate")]
		public DateTime StartDate { get; set; }

		[Required]
		[Display(Name = "EndDate")]
		public DateTime EndDate { get; set; }

		[Required]
		[Display(Name = "TotalPrice")]
		public double TotalPrice { get; set; }

		[Required]
		[Display(Name = "RoomId")]
		public string RoomId { get; set; }

		[Required]
		[Display(Name = "UserId")]
		public string UserId { get; set; }
				
	}
}
