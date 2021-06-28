using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RakletTest
{
    public class PriceClass
    {
        public string Price;
        public int Contacts;
        public int CustomFields;
        public string Storage;
        public string Admins;
        public string EmailSender;
        public string ApiLimits;
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

        //getters and setters
        public string GetPrice()
        {
            return this.Price;
        }
        public int GetContacts()
        {
            return this.Contacts;
        }
        public int GetCustomFields()
        {
            return this.CustomFields;
        }
        public string GetStorage()
        {
            return this.Storage;
        }
        public string GetAdmins()
        {
            return this.Admins;
        }
        public string GetEmailSender()
        {
            return this.EmailSender;
        }
        public string GetAPILimits()
        {
            return this.ApiLimits;
        }
        public void SetPrice(string price)
        {
            this.Price = price;
        }
        public void SetContacts(int contacts)
        {
            this.Contacts = contacts;
        }
        public void SetCustomFields(int customFields)
        {
            this.CustomFields = customFields;
        }
        public void SetStorage(string storage)
        {
            this.Storage = storage;
        }
        public void SetAdmins(string admins)
        {
            this.Admins = admins;
        }
        public void SetEmailSender(string emailSender)
        {
            this.EmailSender = emailSender;
        }
        public void SetAPILimits(string ApiLimits)
        {
            this.ApiLimits = ApiLimits;
        }
    }
}
