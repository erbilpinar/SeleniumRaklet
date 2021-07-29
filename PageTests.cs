using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.IO;
using OpenQA.Selenium.Interactions;

namespace RakletTest
{
    [TestClass]
    public class PageTests
    {
        public const string HomePage = "https://hello.raklet.net/";
        public string LoginEmail = "perbil18@ku.edu.tr";
        public string LoginPassword = "rakletdemo123";
        public string DemoEmail = "pnar.erbl" + DateTime.Now.Millisecond + "@gmail.com";

        public static IWebDriver Driver;
        public List<string> ErrorLogs = new List<string>();

        [TestInitialize]
        public void Initiliaze()
        {
            var temp = new Driver();
            Driver = temp.DriverOptions();
        }

        [TestCleanup]
        public void CleanUp()
        {
            Driver.Quit();
            CheckForErrors();
        }

        public void CheckForErrors()
        {
            if (ErrorLogs.Count > 0)
            {
                string error = "\n";
                foreach (string message in ErrorLogs)
                {
                    error += message + "\n";
                }
                Assert.Fail(error);

            }
        }

        [TestMethod]
        public void TestMainPage()
        {
            IWebElement body = Driver.GoToUrl(HomePage, "SectionHero");
            IList<IWebElement> href = Driver.CheckElementsExist(By.TagName("a"), body);
            for (int i = 0; i < href.Count; i++)
            {
                body = Driver.CheckSiteLoaded("SectionHero");
                href = Driver.CheckElementsExist(By.TagName("a"), body);
                string link = href[i].GetProperty("href");
                string onclick = href[i].GetProperty("onclick");
                if (link == "#" && onclick == null)
                {
                    ErrorLogs.Add("link is empty: " + href[i].Text);
                }
            }
        }

        [TestMethod]
        public void TestFeaturesPage()
        {
            IWebElement body = Driver.GoToUrl(HomePage, "SectionHero");
            Driver.ClickText("Features");
            ErrorLogs = Driver.TestLinks(body, "Features-article", ErrorLogs);
        }

        [TestMethod]
        public void TestReferencesPage()
        {
            IWebElement body = Driver.GoToUrl(HomePage, "SectionHero");
            Driver.ClickText("References");
            ErrorLogs = Driver.TestLinks(body, "Customers-verticals", ErrorLogs);
        }

        [TestMethod]
        public void TestPricingPage()
        {
            IWebElement body = Driver.GoToUrl(HomePage, "SectionHero");
            Driver.ClickText("Pricing");
            ErrorLogs = Driver.TestLinks(body, "Pricing-content", ErrorLogs);
        }

        [TestMethod]
        public void TestResourcesPage()
        {
            IWebElement body = Driver.GoToUrl(HomePage, "SectionHero");
            Driver.ClickText("Resources");
            ErrorLogs = Driver.TestLinks(body, "LandingSection", ErrorLogs);
        }

        [TestMethod]
        public void TestPriceMainAndPricesPage()
        {
            for (int i = 0; i < 2; i++)
            {
                IWebElement body = Driver.GoToUrl(HomePage, "SectionHero");
                if (i == 1)
                {
                    var button = Driver.CheckElementExist(By.XPath("//*[@id=\"switchToMonthlyPricesBtn\"]"), body);
                    Driver.ClickElement(button);
                }
                //collect the prices on the main page
                PriceClass freeMainPage = CheckFreePrices(false);
                PriceClass essentialsMainPage = CheckEssentialsPrices(false);
                PriceClass professionalMainPage = CheckProfessionalPrices(false);
                List<PriceClass> premiumMainPage = CheckPremiumPrices(false);

                //collect the prices on the pricing page
                Driver.ClickText("Pricing");
                if (i == 1)
                {
                    var button = Driver.CheckElementExist(By.XPath("//*[@id=\"switchToMonthlyPricesBtn\"]"), body);
                    Driver.ClickElement(button);
                }
                PriceClass freePricingPage = CheckFreePrices(false);
                PriceClass essentialsPricingPage = CheckEssentialsPrices(false);
                PriceClass professionalPricingPage = CheckProfessionalPrices(false);
                List<PriceClass> premiumPricingPage = CheckPremiumPrices(false);

                //compare them
                //free part
                ErrorLogs = Driver.CheckEqual(freeMainPage.Price, freePricingPage.Price, "Prices are not equal in Main page and Prices page for FREE", ErrorLogs);
                ErrorLogs = Driver.CheckEqual(freeMainPage.Contacts, freePricingPage.Contacts, "Contact numbers are not equal in Main page and Prices page for FREE", ErrorLogs);
                ErrorLogs = Driver.CheckEqual(freeMainPage.CustomFields, freePricingPage.CustomFields, "Custom Field numbers are not equal in Main page and Prices page for FREE", ErrorLogs);
                ErrorLogs = Driver.CheckEqual(freeMainPage.Storage, freePricingPage.Storage, "Storage numbers are not equal in Main page and Prices page for FREE", ErrorLogs);
                ErrorLogs = Driver.CheckEqual(freeMainPage.Admins, freePricingPage.Admins, "Admin numbers are not equal in Main page and Prices page for FREE", ErrorLogs);
                //esesentials part
                ErrorLogs = Driver.CheckEqual(essentialsMainPage.Price, essentialsPricingPage.Price, "Prices are not equal in Main page and Prices page for Essentials", ErrorLogs);
                ErrorLogs = Driver.CheckEqual(essentialsMainPage.Contacts, essentialsPricingPage.Contacts, "Contact numbers are not equal in Main page and Prices page for Essentials", ErrorLogs);
                ErrorLogs = Driver.CheckEqual(essentialsMainPage.CustomFields, essentialsPricingPage.CustomFields, "Custom Field numbers are not equal in Main page and Prices page for Essentials", ErrorLogs);
                ErrorLogs = Driver.CheckEqual(essentialsMainPage.Storage, essentialsPricingPage.Storage, "Storage numbers are not equal in Main page and Prices page for Essentials", ErrorLogs);
                ErrorLogs = Driver.CheckEqual(essentialsMainPage.Admins, essentialsPricingPage.Admins, "Admin numbers are not equal in Main page and Prices page for Essentials", ErrorLogs);
                ErrorLogs = Driver.CheckEqual(essentialsMainPage.EmailSender, essentialsPricingPage.EmailSender, "Emain Sender numbers are not equal in Main page and Prices page for Essentials", ErrorLogs);
                //professional
                ErrorLogs = Driver.CheckEqual(professionalMainPage.Price, professionalPricingPage.Price, "Prices are not equal in Main page and Prices page for Professional", ErrorLogs);
                ErrorLogs = Driver.CheckEqual(professionalMainPage.Contacts, professionalPricingPage.Contacts, "Contact numbers are not equal in Main page and Prices page for Professional", ErrorLogs);
                ErrorLogs = Driver.CheckEqual(professionalMainPage.CustomFields, professionalPricingPage.CustomFields, "Custom Field numbers are not equal in Main page and Prices page for Professional", ErrorLogs);
                ErrorLogs = Driver.CheckEqual(professionalMainPage.Storage, professionalPricingPage.Storage, "Storage numbers are not equal in Main page and Prices page for Professional", ErrorLogs);
                ErrorLogs = Driver.CheckEqual(professionalMainPage.Admins, professionalPricingPage.Admins, "Admin numbers are not equal in Main page and Prices page for Professional", ErrorLogs);
                ErrorLogs = Driver.CheckEqual(professionalMainPage.EmailSender, professionalPricingPage.EmailSender, "Email Sender numbers are not equal in Main page and Prices page for Professional", ErrorLogs);
                //premium
                for (int j = 0; j < 8; j++)
                {
                    PriceClass combobox = premiumMainPage[j];
                    PriceClass table = premiumPricingPage[j];

                    ErrorLogs = Driver.CheckEqual(table.Price, combobox.Price, "Prices are not equal in PREMIUM selection: " + i + "in main page and price page", ErrorLogs);
                    ErrorLogs = Driver.CheckEqual(table.Contacts, combobox.Contacts, "Contact numbers are not equal in PREMIUM selection: " + i + "in main page and price page", ErrorLogs);
                    ErrorLogs = Driver.CheckEqual(table.CustomFields, combobox.CustomFields, "Custom Field numbers are not equal in PREMIUM selection: " + i + "in main page and price page", ErrorLogs);
                    ErrorLogs = Driver.CheckEqual(table.Storage, combobox.Storage, "Storages are not equal in PREMIUM selection: " + i + "in main page and price page", ErrorLogs);
                    ErrorLogs = Driver.CheckEqual(table.Admins, combobox.Admins, "Admin numbers are not equal in PREMIUM selection: " + i + "in main page and price page", ErrorLogs);
                    ErrorLogs = Driver.CheckEqual(table.EmailSender, combobox.EmailSender, "Email sender numbers are not equal in PREMIUM selection: " + i + "in main page and price page", ErrorLogs);
                    ErrorLogs = Driver.CheckEqual(table.ApiLimits, combobox.ApiLimits, "API limits are not equal in PREMIUM selection: " + i + "in main page and price page", ErrorLogs);
                }
            }
        }

        [TestMethod]
        public void TestPriceValues()
        {
            IWebElement body = Driver.GoToUrl(HomePage, "SectionHero");
            Driver.ClickText("Pricing");
            //check for monhly prices
            var button = Driver.CheckElementExist(By.XPath("//*[@id=\"switchToMonthlyPricesBtn\"]"), body);
            Driver.ClickElement(button);
            CheckPremiumPrices(true);
            CheckFreePrices(true);
            CheckEssentialsPrices(true);
            CheckProfessionalPrices(true);
        }

        public List<PriceClass> CheckPremiumPrices(bool monthly)
        {
            // Store the elements in the combobox
            String xpath = "//*[@class=\"col-auto\"]/div[1]/div[4]//";
            List<PriceClass> priceOptions = new List<PriceClass>();
            for (int i = 1; i <= 8; i++)
            {
                PriceClass option = new PriceClass();
                List<string> values = new List<string>();

                var e = Driver.CheckElementExist(By.XPath(xpath + "select/option[" + i + "]"));
                Driver.ClickElement(e);

                string temp = (Driver.CheckElementExist(By.XPath(xpath + "div[2]")).Text);
                values.Add(temp);

                int[] indexes = { 1, 2, 3, 4, 6, 15 };
                foreach (int index in indexes)
                {
                    temp = (Driver.CheckElementExist(By.XPath(xpath + "div[4]/p[" + index + "]")).Text);
                    int tempIndex = temp.IndexOf(":");
                    string var = temp.Substring(tempIndex + 2);
                    values.Add(var);
                }

                option.Price = values[0];
                option.Contacts = Convert.ToInt32(values[1]);
                option.CustomFields = Convert.ToInt32(values[2]);

                //remove the gb from the storage
                string str = values[3].Substring(0, values[3].Length - 3);
                option.Storage = str;
                option.Admins = values[4];
                option.EmailSender = values[5];
                option.ApiLimits = values[6];
                priceOptions.Add(option);
            }

            if (!monthly) return priceOptions;

            //Store the elements in the table
            xpath = "//*[@class=\"table\"]";
            List<PriceClass> priceTable = new List<PriceClass>();
            for (int i = 1; i <= 8; i++)
            {
                PriceClass option = new PriceClass();
                List<string> values = new List<string>();

                var e = Driver.CheckElementExist(By.XPath(xpath + "/thead/tr/th[5]/div/select/option[" + i + "]"));
                Driver.ClickElement(e);

                int[] indexes = { 1, 3, 6, 45, 4, 9, 47 };
                foreach (int index in indexes)
                {
                    string temp = (Driver.CheckElementExist(By.XPath(xpath + "/tbody/tr[" + index + "]/td[5]")).Text);
                    values.Add(temp);
                }

                option.Price = values[0].Replace(",", "").Replace(".00", "");
                option.Contacts = Convert.ToInt32(values[1].Replace(",", ""));
                option.CustomFields = Convert.ToInt32(values[2]);
                //remove the gb from the storage
                string var = values[3].Substring(0, values[3].Length - 3);
                option.Storage = var;
                option.Admins = values[4];
                option.EmailSender = values[5];
                option.ApiLimits = values[6].Replace(",", "");
                priceTable.Add(option);
            }

            //Compare the values
            for (int i = 0; i < 8; i++)
            {
                PriceClass combobox = priceOptions[i];
                PriceClass table = priceTable[i];

                ErrorLogs = Driver.CheckEqual(table.Price, combobox.Price, "Prices are not equal in PREMIUM selection: " + (i + 1), ErrorLogs);
                ErrorLogs = Driver.CheckEqual(table.Contacts, combobox.Contacts, "Contact numbers are not equal in PREMIUM selection: " + (i + 1), ErrorLogs);
                ErrorLogs = Driver.CheckEqual(table.CustomFields, combobox.CustomFields, "Custom Field numbers are not equal in PREMIUM selection: " + (i + 1), ErrorLogs);
                ErrorLogs = Driver.CheckEqual(table.Storage, combobox.Storage, "Storages are not equal in PREMIUM selection: " + (i + 1), ErrorLogs);
                ErrorLogs = Driver.CheckEqual(table.Admins, combobox.Admins, "Admin numbers are not equal in PREMIUM selection: " + (i + 1), ErrorLogs);
                ErrorLogs = Driver.CheckEqual(table.EmailSender, combobox.EmailSender, "Email sender numbers are not equal in PREMIUM selection: " + (i + 1), ErrorLogs);
                ErrorLogs = Driver.CheckEqual(table.ApiLimits, combobox.ApiLimits, "API limits are not equal in PREMIUM selection: " + (i + 1), ErrorLogs);
            }
            return priceOptions;
        }

        public PriceClass CheckFreePrices(bool monthly)
        {
            // Store the elements in the combobox
            String xpath = "//*[@class=\"col-auto\"]/div[1]/div[1]//";
            List<PriceClass> priceOptions = new List<PriceClass>();

            PriceClass option = new PriceClass();
            List<string> values = new List<string>();

            string temp = (Driver.CheckElementExist(By.XPath(xpath + "div[2]")).Text);
            values.Add(temp);

            for (int index = 1; index <= 4; index++)
            {
                temp = (Driver.CheckElementExist(By.XPath(xpath + "div[4]/p[" + index + "]")).Text);
                int tempIndex = temp.IndexOf(":");
                string tempVar = temp.Substring(tempIndex + 2);
                values.Add(tempVar);
            }

            option.Price = values[0];
            option.Contacts = Convert.ToInt32(values[1]);
            option.CustomFields = Convert.ToInt32(values[2]);

            //remove the gb from the storage
            string str = values[3].Substring(0, values[3].Length - 3);
            option.Storage = str;
            option.Admins = values[4];
            priceOptions.Add(option);

            if (!monthly) return priceOptions[0];

            //Store the elements in the table
            xpath = "//*[@class=\"table\"]";
            List<PriceClass> priceTable = new List<PriceClass>();

            option = new PriceClass();
            values = new List<string>();

            int[] indexes2 = { 1, 3, 6, 45, 4 };
            foreach (int index in indexes2)
            {
                temp = (Driver.CheckElementExist(By.XPath(xpath + "/tbody/tr[" + index + "]/td[2]")).Text);
                values.Add(temp);
            }

            option.Price = values[0].Replace(",", "").Replace(".00", "");
            option.Contacts = Convert.ToInt32(values[1].Replace(",", ""));
            option.CustomFields = Convert.ToInt32(values[2]);

            //remove the gb from the storage
            string var = values[3].Substring(0, values[3].Length - 3);
            option.Storage = var;
            option.Admins = values[4];
            priceTable.Add(option);

            //Compare the values
            PriceClass combobox = priceOptions[0];
            PriceClass table = priceTable[0];

            ErrorLogs = Driver.CheckEqual(table.Price, combobox.Price, "Prices are not equal in FREE", ErrorLogs);
            ErrorLogs = Driver.CheckEqual(table.Contacts, combobox.Contacts, "Contact numbers are not equal in FREE", ErrorLogs);
            ErrorLogs = Driver.CheckEqual(table.CustomFields, combobox.CustomFields, "Custom Field numbers are not equal in FREE", ErrorLogs);
            ErrorLogs = Driver.CheckEqual(table.Storage, combobox.Storage, "Storages are not equal in FREE", ErrorLogs);
            ErrorLogs = Driver.CheckEqual(table.Admins, combobox.Admins, "Admin numbers are not equal in FREE", ErrorLogs);

            return combobox;

        }

        public PriceClass CheckEssentialsPrices(bool monthly)
        {
            // Store the elements in the combobox
            String xpath = "//*[@class=\"col-auto\"]/div[1]/div[2]//";
            List<PriceClass> priceOptions = new List<PriceClass>();

            PriceClass option = new PriceClass();
            List<string> values = new List<string>();

            string temp = (Driver.CheckElementExist(By.XPath(xpath + "div[2]")).Text);
            values.Add(temp);

            int[] indexes = { 1, 2, 3, 4, 6 };
            foreach (int index in indexes)
            {
                temp = (Driver.CheckElementExist(By.XPath(xpath + "div[4]/p[" + index + "]")).Text);
                int tempIndex = temp.IndexOf(":");
                var tempVar = temp.Substring(tempIndex + 2);
                values.Add(tempVar);
            }

            option.Price = values[0];
            option.Contacts = Convert.ToInt32(values[1]);
            option.CustomFields = Convert.ToInt32(values[2]);

            //remove the gb from the storage
            string str = values[3].Substring(0, values[3].Length - 3);
            option.Storage = str;
            option.Admins = values[4];
            option.EmailSender = values[5];
            priceOptions.Add(option);

            if (!monthly) return priceOptions[0];

            //Store the elements in the table
            xpath = "//*[@class=\"table\"]";
            List<PriceClass> priceTable = new List<PriceClass>();

            option = new PriceClass();
            values = new List<string>();

            int[] indexes2 = { 1, 3, 6, 45, 4, 9 };
            foreach (int index in indexes2)
            {
                temp = (Driver.CheckElementExist(By.XPath(xpath + "/tbody/tr[" + index + "]/td[3]")).Text);
                values.Add(temp);
            }

            option.Price = values[0].Replace(",", "").Replace(".00", "");
            option.Contacts = Convert.ToInt32(values[1].Replace(",", ""));
            option.CustomFields = Convert.ToInt32(values[2]);

            //remove the gb from the storage
            string var = values[3].Substring(0, values[3].Length - 3);
            option.Storage = var;
            option.Admins = values[4];
            option.EmailSender = values[5];
            priceTable.Add(option);

            //Compare the values
            PriceClass combobox = priceOptions[0];
            PriceClass table = priceTable[0];

            ErrorLogs = Driver.CheckEqual(table.Price, combobox.Price, "Prices are not equal in ESSENTIALS", ErrorLogs);
            ErrorLogs = Driver.CheckEqual(table.Contacts, combobox.Contacts, "Contact numbers are not equal in ESSENTIALS", ErrorLogs);
            ErrorLogs = Driver.CheckEqual(table.CustomFields, combobox.CustomFields, "Custom Field numbers are not equal in ESSENTIALS", ErrorLogs);
            ErrorLogs = Driver.CheckEqual(table.Storage, combobox.Storage, "Storages are not equal in ESSENTIALS", ErrorLogs);
            ErrorLogs = Driver.CheckEqual(table.Admins, combobox.Admins, "Admin numbers are not equal in ESSENTIALS", ErrorLogs);
            ErrorLogs = Driver.CheckEqual(table.EmailSender, combobox.EmailSender, "Email sender numbers are not equal in ESSENTIALS", ErrorLogs);

            return combobox;
        }

        public PriceClass CheckProfessionalPrices(bool monthly)
        {
            // Store the elements in the combobox
            String xpath = "//*[@class=\"col-auto\"]/div[1]/div[3]//";
            List<PriceClass> priceOptions = new List<PriceClass>();

            PriceClass option = new PriceClass();
            List<string> values = new List<string>();

            string temp = (Driver.CheckElementExist(By.XPath(xpath + "div[2]")).Text);
            values.Add(temp);

            int[] indexes = { 1, 2, 3, 4, 6 };
            foreach (int index in indexes)
            {
                temp = (Driver.CheckElementExist(By.XPath(xpath + "div[4]/p[" + index + "]")).Text);
                int tempIndex = temp.IndexOf(":");
                string tempVar = temp.Substring(tempIndex + 2);
                values.Add(tempVar);
            }

            option.Price = values[0];
            option.Contacts = Convert.ToInt32(values[1]);
            option.CustomFields = Convert.ToInt32(values[2]);

            //remove the gb from the storage
            string str = values[3].Substring(0, values[3].Length - 3);
            option.Storage = str;
            option.Admins = values[4];
            option.EmailSender = values[5];
            priceOptions.Add(option);

            if (!monthly) return priceOptions[0];

            //Store the elements in the table
            xpath = "//*[@class=\"table\"]";
            List<PriceClass> priceTable = new List<PriceClass>();

            option = new PriceClass();
            values = new List<string>();

            int[] indexes2 = { 1, 3, 6, 45, 4, 9 };
            foreach (int index in indexes2)
            {
                temp = (Driver.CheckElementExist(By.XPath(xpath + "/tbody/tr[" + index + "]/td[4]")).Text);
                values.Add(temp);
            }

            option.Price = values[0].Replace(",", "").Replace(".00", "");
            option.Contacts = Convert.ToInt32(values[1].Replace(",", ""));
            option.CustomFields = Convert.ToInt32(values[2]);

            //remove the gb from the storage
            string var = values[3].Substring(0, values[3].Length - 3);
            option.Storage = var;
            option.Admins = values[4];
            option.EmailSender = values[5];
            priceTable.Add(option);

            //Compare the values
            PriceClass combobox = priceOptions[0];
            PriceClass table = priceTable[0];

            ErrorLogs = Driver.CheckEqual(table.Price, combobox.Price, "Prices are not equal in PROFESSIONAL", ErrorLogs);
            ErrorLogs = Driver.CheckEqual(table.Contacts, combobox.Contacts, "Contact numbers are not equal in PROFESSIONAL", ErrorLogs);
            ErrorLogs = Driver.CheckEqual(table.CustomFields, combobox.CustomFields, "Custom Field numbers are not equal in PROFESSIONAL", ErrorLogs);
            ErrorLogs = Driver.CheckEqual(table.Storage, combobox.Storage, "Storages are not equal in PROFESSIONAL", ErrorLogs);
            ErrorLogs = Driver.CheckEqual(table.Admins, combobox.Admins, "Admin numbers are not equal in PROFESSIONAL", ErrorLogs);
            ErrorLogs = Driver.CheckEqual(table.EmailSender, combobox.EmailSender, "Email sender numbers are not equal in PROFESSIONAL", ErrorLogs);
            return combobox;

        }

        [TestMethod]
        public void TestUtmParameters()
        {
            string utm = "?utm_source=source&utm_medium=medium&utm_campaign=campaign&utm_term=term&utm_content=content";
            List<string> urls = new List<string>()
            {
                HomePage, HomePage + "features/app-store/",
                HomePage + "customers/", HomePage + "pricing",
                HomePage + "knowledge-center/"
            };
            List<string> isLoaded = new List<string>()
            {
                "SectionHero", "Features-article",
                "Customers-verticals", "Pricing-content",
                "LandingSection"
            };
            int i = 0;
            foreach (string url in urls)
            {
                Driver.GoToUrl(url + utm, isLoaded[i++]);
            }
        }       
    }
}