using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidBack.DTO
{
    public class UserEditDTO
    {
        public string Email { get; set; }

        public string Image { get; set; }

        public string imageBase64 { get; set; }

        public string Phone { get; set; }

        public string BirthDate { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Address { get; set; }
    }
}
