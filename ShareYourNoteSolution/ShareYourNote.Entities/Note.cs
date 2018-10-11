using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShareYourNote.Entities
{
    public class Note
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required,DisplayName("Başlık"), StringLength(50)]
        public string Title { get; set; }
        [Required,DisplayName("Açıklama"), StringLength(4000)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Boş Geçilemez")]
        public string UploadedFile { get; set; }
        public DateTime UploadDate { get; set; }
        public bool IsApproved { get; set; }
        public int OwnerId { get; set; }
        public int TeacherId { get; set; }
        public string OlmayanOgretmen { get; set; } // Öğrenci Buraya Öğretmen Belirtilmemişse belirticek admin onayına gidecek.


        //Keys

        //public School School { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual ShareYourNoteUser Owner { get; set; }

        public Note()
        {
            Comments= new List<Comment>();
        }
        

        /*
         * select * from Notes
         * Join Teachers On Notes.TeacherId = Teachers.Id
         * Join Schools on Teachers.SchoolId = Schools.Id
         * Where Schools.Id=1
         */
    }
}