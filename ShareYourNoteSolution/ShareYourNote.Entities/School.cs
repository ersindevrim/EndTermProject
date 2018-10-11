using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShareYourNote.Entities
{
    public class School
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Okul Adı")]
        public string Name { get; set; }
        public bool IsApproved { get; set; }


        public virtual List<Teacher> Teachers { get; set; }

        public School()
        {
            Teachers = new List<Teacher>();
        }

    }
}