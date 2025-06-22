using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Entities
{
    public class ShelterDTO
    {
        [Key]
        public int Code { get; set; }
        public string NameStr { get; set; }
    }
}
