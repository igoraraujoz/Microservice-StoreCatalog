using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Model
{
    public class Restrictions
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("ProductionId")]
        public Guid ProductionId { get; set; }
        public string Restriction { get; set; }
    }
}
