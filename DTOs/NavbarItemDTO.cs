using CmkCable.Entities;
using System;
using System.Collections.Generic;

namespace DTOs
{
    public class NavbarItemDto
    {
        public int Id { get; set; }
        public string Route { get; set; }
        public string Title { get; set; }
        public int LanguageId { get; set; }
        public List<NavbarItemDto> SubItems { get; set; }
    }

}
