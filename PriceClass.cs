using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RakletTest
{
    class PriceClass
    {
        string price;
        int contacts;
        int custom_fields;
        int storage;
        string admins;
        string email_sender;
        string API_limits;
        public PriceClass()
        {
            this.price = "";
            this.contacts = 0;
            this.custom_fields = 0;
            this.storage = 0;
            this.admins = "";
            this.email_sender = "";
            this.API_limits = "";
        }

        //getters and setters
        public string GetPrice()
        {
            return this.price;
        }
        public int GetContacts()
        {
            return this.contacts;
        }
        public int GetCustomFields()
        {
            return this.custom_fields;
        }
        public int GetStorage()
        {
            return this.storage;
        }
        public string GetAdmins()
        {
            return this.admins;
        }
        public string GetEmailSender()
        {
            return this.email_sender;
        }
        public string GetAPILimits()
        {
            return this.API_limits;
        }
        public void SetPrice(string price)
        {
            this.price = price;
        }
        public void SetContacts(int contacts)
        {
            this.contacts = contacts;
        }
        public void SetCustomFields(int custom_fields)
        {
            this.custom_fields = custom_fields;
        }
        public void SetStorage(int storage)
        {
            this.storage = storage;
        }
        public void SetAdmins(string admins)
        {
            this.admins = admins;
        }
        public void SetEmailSender(string email_sender)
        {
            this.email_sender = email_sender;
        }
        public void SetAPILimits(string API_limits)
        {
            this.API_limits = API_limits;
        }
    }
}
