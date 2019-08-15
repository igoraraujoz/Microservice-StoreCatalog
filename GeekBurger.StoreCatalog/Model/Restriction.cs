using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekBurger.StoreCatalog.Model
{
    public class Restriction
    {
        [Key]
        public Guid RestrictionId { get; set; }
        public string Name { get; set; }

        [ForeignKey("ProductionId")]
        public Production Production { get; set; }
    }
}
