using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Entities
{
    public class OpinionDTO
    {
        [Key]
        public int Code { get; set; }
        public AddressDTO Address { get; set; }
        public int Stars { get; set; }
        public string Text { get; set; }
        public string[] Images { get; set; }
    }

}
