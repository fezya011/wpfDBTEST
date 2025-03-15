using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfDBTEST.Model
{
    public class Product
    {
      public  int ID { get; set; }
       public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateOnly CreateDate { get; set; }
        public short ExpireDate { get; set; }
    }
}
