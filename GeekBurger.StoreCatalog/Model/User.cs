using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GeekBurger.StoreCatalog.Model
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        public List<string> Restrictions { get; set; }
    }
}
