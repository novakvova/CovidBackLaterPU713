using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CovidBack.Enities
{
    [Table("tblUserProfiles")]
    public class UserProfile
    {
        [Key, ForeignKey("User")]
        public long Id { get; set; }

        public string Image { get; set; }

        public string Phone { get; set; }

        public string BirthDate { get; set; }

        public string Firstname { get; set; }

        public string Lastname  { get; set; }

        public string Address { get; set; }

        public virtual DbUser User { get; set; }

    }
}
