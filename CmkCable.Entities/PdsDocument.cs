using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CmkCable.Entities
{
    public class PdsDocument
    {
        public int Id { get; set; }
        public string FileContent { get; set; }
        public string Image { get; set; }
    }
}
