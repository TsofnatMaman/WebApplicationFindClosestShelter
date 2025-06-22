using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Core.Models
{
    public class Address
    {
        [Key]
        public int Code { get; set; }
        public Location Location { get; set; }
        public Shelter Shelter { get; set; }
        public int ShelterCode { get; set; }
        public bool IsOpen24_7 { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonPhone { get; set; }
        public bool ContactPersonHasSMS { get; set; }
        public int Capacity { get; set; }
        public int CurrentNumberPeople { get; set; }
        public DateTime AddedSystem { get; private set; }

        public Address() {
            AddedSystem = DateTime.Now;
        }

    }
}
