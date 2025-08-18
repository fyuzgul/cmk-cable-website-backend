using System;
using System.Collections.Generic;

namespace DTOs.CreateDTOs
{
	public class CreateHelpTypeWithTranslationsDTO
	{
		public string Name { get; set; }
		public bool IsActive { get; set; } = true;
		public List<HelpTypeTranslationItemDTO> Translations { get; set; }
	}

	public class HelpTypeTranslationItemDTO
	{
		public int LanguageId { get; set; }
		public string Name { get; set; }
	}
}
