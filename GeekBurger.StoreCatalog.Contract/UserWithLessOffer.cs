using System;
using System.Collections.Generic;
using System.Text;

namespace GeekBurger.StoreCatalog.Contract
{
    public class UserWithLessOffer
    {
        public Guid UserId { get; set; }
        public List<string> Restrictions { get; set; }
    }
}
