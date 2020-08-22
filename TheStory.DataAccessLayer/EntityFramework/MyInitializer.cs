using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using TheStory.Entities;

namespace TheStory.DataAccessLayer.EntityFramework
{
    public class MyInitializer:CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {

            //kullanıcı admin ekleme
            TheStoryUser admin = new TheStoryUser()
            {
                Name = "Mirem",
                Surname ="Işıldak",
                Email ="mirem.isildak@sakarya.edu.tr",
                ActivateGuid = Guid.NewGuid(),
                IsActive =true,
                IsAdmin = true,
                Username="miremisildak",
                Password ="123",
                CreatedOn =DateTime.Now,
                ModifiedOn =DateTime.Now.AddMinutes(5),
                ModifiedUsername="miremisildak"
            };

            TheStoryUser standartUser = new TheStoryUser()
            {
                Name = "Beyza",
                Surname = "Karaca",
                Email = "miremisildak99@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "KrcByz",
                Password = "456",
                CreatedOn = DateTime.Now.AddHours(1),
                ModifiedOn = DateTime.Now.AddMinutes(65),
                ModifiedUsername = "KrcByz"
            };
            context.TheStoryUsers.Add(admin);
            context.TheStoryUsers.Add(standartUser);

            for (int i = 0; i < 8; i++)
            {
                TheStoryUser user = new TheStoryUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    Username =$"user{i}",
                    Password = "456",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = $"user{i}"
                };
                context.TheStoryUsers.Add(user);

            }

            context.SaveChanges();

            //kullanıcı listesini kullanmak için listeyi aldık.
            List<TheStoryUser> userlist = context.TheStoryUsers.ToList();

            // Adding Fake category
            for (int i = 0; i < 10; i++)
            {
                Category cat = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUsername = "miremisildak"

                };
                context.Categories.Add(cat);

                //Not ekleme
                for (int k = 0; k < FakeData.NumberData.GetNumber(5,9); k++)
                {

                    TheStoryUser owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];

                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),

                        IsDraft = false,
                        LikeCount =FakeData.NumberData.GetNumber(1,9),
                        Owner = owner,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1),DateTime.Now),
                        ModifiedOn= FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername =owner.Username,

                    };

                    cat.Notes.Add(note);


                    // comments ekleme
                    for (int j = 0; j < FakeData.NumberData.GetNumber(3, 5); j++)
                    {

                        TheStoryUser comment_owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];
                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            Owner= comment_owner,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUsername = comment_owner.Username

                        };

                        note.Comments.Add(comment);
                    }

                    //like ekleme 
                   
                    for (int m = 0; m < note.LikeCount;m++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = userlist[m]
                        };

                        note.Likes.Add(liked);
                    }
                }
            }

            context.SaveChanges();

        }
    }
}
