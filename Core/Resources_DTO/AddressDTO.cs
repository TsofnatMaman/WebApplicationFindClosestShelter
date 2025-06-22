using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Models
{
    public class AddressDTO
    {
        [Key]
        public int Code { get; set; }
        public string Location { get; set; }
        public ShelterDTO Shelter { get; set; }
        public bool IsOpen24_7 { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonPhone { get; set; }
        public bool ContactPersonHasSMS { get; set; }
        public int Capacity { get; set; }
        public int CurrentNumberPeople { get; set; }
        public DateTime AddedSystem { get; private set; }

        public AddressDTO() {
            AddedSystem = DateTime.Now;
        }

    }
}
