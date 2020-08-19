using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using b171200053.Entities;

namespace b171200053.DataAccessLayer
{
     public class DatabaseContext
    {

        public DbSet<TheStoryUser> TheStoryUser { get; set; }
        public DbSet<Yazi> Yazilar { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tur> Categories { get; set; }
        public DbSet<Liked> Likes { get; set; }
        
    }
}
