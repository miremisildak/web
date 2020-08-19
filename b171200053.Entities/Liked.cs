using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace b171200053.Entities
{
  [Table("Likes")]
   public class Liked
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual Yazi Yazi { get; set; }
        public virtual TheStoryUser LikedUser { get; set; }

    }
}
