using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShareYourNote.Entities;

namespace ShareYourNote.DataAccess.EntityFramework
{
    public class DataInitializer : DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            ShareYourNoteUser admin = new ShareYourNoteUser()
            {
                Name = "Ersin",
                Surname = "Devrim",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                ProfilePhotoName = "defaultImage.png",
                IsAdmin = true,
                Username = "ersindevrim",
                Password = "123456",
                Email = "ersindevrim@outlook.com"
            };

            ShareYourNoteUser standartUser = new ShareYourNoteUser()
            {
                Name = "Birkan",
                Surname = "Köse",
                Email = "birkankose@gmail.com",
                ProfilePhotoName = "defaultImage.png",
                ActivateGuid = Guid.NewGuid(),
                IsAdmin = false,
                IsActive = true,
                Username = "kosebirkan",
                Password = "123456"
            };

            context.ShareYourNoteUsers.Add(admin);
            context.ShareYourNoteUsers.Add(standartUser);

            for (int i = 0; i < 8; i++)
            {
                ShareYourNoteUser shareYourNoteUsertUser = new ShareYourNoteUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ProfilePhotoName = "defaultImage.png",
                    ActivateGuid = Guid.NewGuid(),
                    IsAdmin = false,
                    IsActive = true,
                    Username = $"user{i}",
                    Password = "123456"
                };
                context.ShareYourNoteUsers.Add(shareYourNoteUsertUser);
            }

            context.SaveChanges();

            List<ShareYourNoteUser> userList = context.ShareYourNoteUsers.ToList();

            //Fake Schools
            for (int i = 0; i < 10; i++)
            {
                School school = new School()
                {
                    Name = FakeData.PlaceData.GetStreetName() + " Universitesi",                    
                    IsApproved = true

                };
                context.Schools.Add(school);

                //fake Teachers
                for (int j = 0; j < FakeData.NumberData.GetNumber(2, 10); j++)
                {
                    Teacher teacher = new Teacher()
                    {
                        Name = FakeData.NameData.GetFullName(),
                        IsApproved = true,
                        School = school,

                    };
                    school.Teachers.Add(teacher);

                    //fake notes
                    for (int k = 0; k < FakeData.NumberData.GetNumber(3, 5); k++)
                    {
                        ShareYourNoteUser shareYourNoteUser = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];

                        Note note = new Note()
                        {
                            Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                            Description = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                            IsApproved = true,
                            Owner = shareYourNoteUser,
                            UploadDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            UploadedFile = "test.pdf"
                            //TODO: School ID mevzusunu çöz.
                            //ÇÖZÜLDÜ
                        };
                        teacher.Notes.Add(note);                                                                                                

                        //fake comments
                        for (int l = 0; l < FakeData.NumberData.GetNumber(1, 3); l++)
                        {
                            ShareYourNoteUser commentOwner = userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];

                            Comment comment = new Comment()
                            {
                                Text = FakeData.TextData.GetSentence(),
                                Owner = commentOwner
                            };

                            note.Comments.Add(comment);
                        }
                    }
                }
            }
            context.SaveChanges();
        }
    }
}
