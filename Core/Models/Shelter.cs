using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Shelter
    {
        [Key]
        public int Code { get; set; }
        //public ShelterTypes Name { get; set; }
        public string Name { get; set; }
    }
}
