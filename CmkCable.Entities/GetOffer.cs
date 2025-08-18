using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CmkCable.Entities
{
	public class GetOffer
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string FirstName { get; set; }

		[Required]
		[MaxLength(50)]
		public string LastName { get; set; }

		[Required]
		[MaxLength(100)]
		[EmailAddress]
		public string WorkEmail { get; set; }

		[ForeignKey("Role")]
		public int RoleId { get; set; }
		public virtual Role Role { get; set; }

		[Required]
		[MaxLength(100)]
		public string Country { get; set; }

		[Required]
		[MaxLength(150)]
		public string Company { get; set; }

		[ForeignKey("CompanyType")]
		public int CompanyTypeId { get; set; }
		public virtual CompanyType CompanyType { get; set; }

		[Required]
		[MaxLength(20)]
		public string TelephoneNumber { get; set; }

		[ForeignKey("HelpType")]
		public int HelpTypeId { get; set; }
		public virtual HelpType HelpType { get; set; }

		[Required]
		[MaxLength(2000)]
		public string Message { get; set; }

		[MaxLength(45)]
		public string? IpAddress { get; set; }

		public bool AcikRiza { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public override string ToString()
		{
			return $"FirstName: {FirstName}, LastName: {LastName}, WorkEmail: {WorkEmail}, RoleId: {RoleId}, Country: {Country}, Company: {Company}, CompanyTypeId: {CompanyTypeId}, TelephoneNumber: {TelephoneNumber}, HelpTypeId: {HelpTypeId}, Message: {Message}";
		}
	}
}
