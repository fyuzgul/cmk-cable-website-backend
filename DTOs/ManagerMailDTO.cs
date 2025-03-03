using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class ManagerMailDTO
    {
        
        public int Id { get; set; }
        public string Email { get; set; }
        public List<int> FormTypeIds { get; set; }
    }
}
