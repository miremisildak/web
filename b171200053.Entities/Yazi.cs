using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace b171200053.Entities
{
    [Table("Yazilar")]
    public class Yazi:MyEntityBase
    {
        [Required, StringLength(60)]
        public string Title { get; set; }

        [Required, StringLength(2000)]
        public string Text { get; set; }

        public bool IsDraft { get; set; }

        public int LikeCount { get; set; }

        public int CategoryId { get; set; }

        public virtual List<Comment> Comments { get; set; }
        public virtual TheStoryUser Owner { get; set; }
        public virtual Tur Tur { get; set; }
        public virtual List<Liked> Likeds { get; set; }

    }
}
