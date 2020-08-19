using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace b171200053.Entities
{
    [Table("Turler")]
    public class Tur: MyEntityBase
    {
       [Required,StringLength(50)]
        public string Title { get; set; }
        [StringLength(150)]
        public string Description { get; set; }
         
        public virtual List<Yazi> Yazilar { get; set; }
    }
}
