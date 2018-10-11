using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareYourNote.Entities
{
    public class ShareYourNoteUser
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [StringLength(30)] // user_01.jpg
        public string ProfilePhotoName { get; set; }
        public bool IsActive { get; set; } // Activate Maili için
        [Required]
        public Guid ActivateGuid { get; set; }
        public bool IsAdmin { get; set; }

        public virtual List<Note> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }
    }
}
