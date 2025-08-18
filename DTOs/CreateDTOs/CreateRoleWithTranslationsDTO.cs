using System;
using System.Collections.Generic;

namespace DTOs.CreateDTOs
{
	public class CreateRoleWithTranslationsDTO
	{
		public string Name { get; set; }
		public bool IsActive { get; set; } = true;
		public List<RoleTranslationItemDTO> Translations { get; set; }
	}

	public class RoleTranslationItemDTO
	{
		public int LanguageId { get; set; }
		public string Name { get; set; }
	}
}



