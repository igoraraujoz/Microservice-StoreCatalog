using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekBurger.StoreCatalog.Model
{
    public class Production
    {
        [Key]
        public Guid ProductionId { get; set; }
        public List<Restrictions> Restrictions { get; set; }        
        public bool On { get; set; }
    }
}
