using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RakletTest
{
    public class PriceClass
    {
        public string Price { get; set; }
        public int Contacts { get; set; }
        public int CustomFields { get; set; }
        public string Storage { get; set; }
        public string Admins { get; set; }
        public string EmailSender { get; set; }
        public string ApiLimits { get; set; }
        public string Bar { get; set; }
        public PriceClass()
        {
            this.Price = "";
            this.Contacts = 0;
            this.CustomFields = 0;
            this.Storage = "";
            this.Admins = "";
            this.EmailSender = "";
            this.ApiLimits = "";
        }
    }
}
