using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentalBusiness.Models
{
	public class UserModel : IdentityUser
	{
		public string Name1 { get; set; }
		public string Name2 { get; set; }

		public string Street { get; set; }

		public string City { get; set; }

		public int  PostalCode { get; set; }

	}
}
