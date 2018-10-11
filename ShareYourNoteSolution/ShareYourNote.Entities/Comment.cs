using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace ShareYourNote.Entities
{
    public class Comment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [DisplayName("Yorum:")]
        public string Text { get; set; }
        public int NoteId { get; set; }

        public virtual Note Note { get; set; }
        public virtual ShareYourNoteUser Owner { get; set; }
    }
}