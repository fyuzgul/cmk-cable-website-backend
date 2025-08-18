using System;
using System.Collections.Generic;

namespace DTOs.CreateDTOs
{
	public class CreateCompanyTypeWithTranslationsDTO
	{
		public string Name { get; set; }
		public bool IsActive { get; set; } = true;
		public List<CompanyTypeTranslationItemDTO> Translations { get; set; }
	}

	public class CompanyTypeTranslationItemDTO
	{
		public int LanguageId { get; set; }
		public string Name { get; set; }
	}
}
