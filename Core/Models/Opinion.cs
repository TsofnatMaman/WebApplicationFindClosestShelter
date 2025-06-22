using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Opinion
    {
        [Key]
        public int Code { get; set; }
        public Address Address { get; set; }
        public int AddressCode { get; set; }
        public int Stars { get; set; }
        public string Text { get; set; }
        public string[] Images { get; set; }
    }

}
