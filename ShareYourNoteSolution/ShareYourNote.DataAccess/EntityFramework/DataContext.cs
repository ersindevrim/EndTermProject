using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShareYourNote.Entities;

namespace ShareYourNote.DataAccess.EntityFramework
{
    public class DataContext : DbContext
    {
        public DataContext() : base("NotPaylas")
        {
            Database.SetInitializer(new DataInitializer());
        }

        public DbSet<ShareYourNoteUser> ShareYourNoteUsers { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
    }
}
