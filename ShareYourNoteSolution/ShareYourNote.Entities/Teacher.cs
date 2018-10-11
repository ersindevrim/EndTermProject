using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace ShareYourNote.Entities
{
    public class Teacher
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Öğretmen"),Required]
        public string Name { get; set; }
        public int SchoolId { get; set; }
        public bool IsApproved { get; set; }


        //Keys
        public virtual School School { get; set; }
        public virtual List<Note> Notes { get; set; }

        public Teacher()
        {
            Notes = new List<Note>();
        }
        
    }
}