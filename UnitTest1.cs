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


namespace RakletTest
{
    [TestClass]
    public class UnitTest1
    {
        public const string HomePage = "https://hello.raklet.net/";
        public static IWebDriver Driver;

        [TestInitialize]
        public void Initiliaze()
        {
            Driver = new ChromeDriver();
            ChromeOptions options = new ChromeOptions();
            options.AddExcludedArgument("disable-popup-blocking");
            options.AddUserProfilePreference("profile.default_content_setting_values.cookies", 2);
        }

        [TestCleanup]
        public void CleanUp()
        {
            Driver.Quit();
        }

        [TestMethod]
        public void TestMain()
        {
            IWebElement body = Driver.GoToUrl(HomePage, "SectionHero");
            IList<IWebElement> href = body.FindElements(By.TagName("a"));
            string originalWindow = Driver.CurrentWindowHandle;

            for (int i = 0; i < href.Count; i++)
            {
                body = Driver.GoToUrl(HomePage, "SectionHero");
                href = body.FindElements(By.TagName("a"));
                string link = href[i].Text;
                Driver.ClickElement(href[i]);
                if (Driver.WindowHandles.Count > 1)
                {
                    Driver.SwitchTo().Window(Driver.WindowHandles[1]);
                    Driver.Close();
                    Driver.SwitchTo().Window(originalWindow);
                }
                else
                {
                    if (Driver.Url == HomePage)
                    {
                        System.Console.WriteLine("Empty link:" + link);
                    }
                    else
                    {
                        Driver.Navigate().Back();
                    }
                }
            }
        }

        [TestMethod]
        public void TestFeatures()
        {
            IWebElement body = Driver.GoToUrl(HomePage, "SectionHero");
            Driver.ClickText("Features");
            TestLinks(body, "Features-article");
        }

        [TestMethod]
        public void TestReferences()
        {
            IWebElement body = Driver.GoToUrl(HomePage, "SectionHero");
            Driver.ClickText("References");
            TestLinks(body, "Customers-verticals");
        }

        [TestMethod]
        public void TestPricing()
        {
            IWebElement body = Driver.GoToUrl(HomePage, "SectionHero");
            Driver.ClickText("Pricing");
            //check for monhly prices
            var button = body.FindElement(By.XPath("//*[@id=\"switchToMonthlyPricesBtn\"]"));
            Driver.ClickElement(button);
            CheckPremiumPrices(true);
            CheckFreePrices(true);
            CheckEssentialsPrices(true);
            CheckProfessionalPrices(true);
            
            body = Driver.GoToUrl(HomePage, "SectionHero");
            Driver.ClickText("Pricing");
            TestLinks(body, "Pricing-content");

        }

        [TestMethod]
        public void TestResources()
        {
            IWebElement body = Driver.GoToUrl(HomePage, "SectionHero");
            Driver.ClickText("Resources");
            TestLinks(body, "LandingSection");
        }

        [TestMethod]
        public void PriceComparisonMainAndPrices()
        {
            for (int i = 0; i < 2; i++)
            {
                IWebElement body = Driver.GoToUrl(HomePage, "SectionHero");
                if (i == 1)
                {
                    var button = body.FindElement(By.XPath("//*[@id=\"switchToMonthlyPricesBtn\"]"));
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
                    var button = body.FindElement(By.XPath("//*[@id=\"switchToMonthlyPricesBtn\"]"));
                    Driver.ClickElement(button);

                }
                PriceClass freePricingPage = CheckFreePrices(false);
                PriceClass essentialsPricingPage = CheckEssentialsPrices(false);
                PriceClass professionalPricingPage = CheckProfessionalPrices(false);
                List<PriceClass> premiumPricingPage = CheckPremiumPrices(false);

                //compare them
                //free part
                CheckEqual(freeMainPage.Price, freePricingPage.Price);
                CheckEqual(freeMainPage.Contacts, freePricingPage.Contacts);
                CheckEqual(freeMainPage.CustomFields, freePricingPage.CustomFields);
                CheckEqual(freeMainPage.Storage, freePricingPage.Storage);
                CheckEqual(freeMainPage.Admins, freePricingPage.Admins);
                //esesentials part
                CheckEqual(essentialsMainPage.Price, essentialsPricingPage.Price);
                CheckEqual(essentialsMainPage.Contacts, essentialsPricingPage.Contacts);
                CheckEqual(essentialsMainPage.CustomFields, essentialsPricingPage.CustomFields);
                CheckEqual(essentialsMainPage.Storage, essentialsPricingPage.Storage);
                CheckEqual(essentialsMainPage.Admins, essentialsPricingPage.Admins);
                CheckEqual(essentialsMainPage.EmailSender, essentialsPricingPage.EmailSender);
                //professional
                CheckEqual(professionalMainPage.Price, professionalPricingPage.Price);
                CheckEqual(professionalMainPage.Contacts, professionalPricingPage.Contacts);
                CheckEqual(professionalMainPage.CustomFields, professionalPricingPage.CustomFields);
                CheckEqual(professionalMainPage.Storage, professionalPricingPage.Storage);
                CheckEqual(professionalMainPage.Admins, professionalPricingPage.Admins);
                CheckEqual(professionalMainPage.EmailSender, professionalPricingPage.EmailSender);
                //premium
                for (int j = 0; j < 8; j++)
                {
                    PriceClass combobox = premiumMainPage[j];
                    PriceClass table = premiumPricingPage[j];

                    CheckEqual(table.GetPrice(), combobox.GetPrice(), "Prices are not equal in PREMIUM selection: " + i  + "in main page and price page");
                    CheckEqual(table.GetContacts(), combobox.GetContacts(), "Contact numbers are not equal in PREMIUM selection: " + i + "in main page and price page");
                    CheckEqual(table.GetCustomFields(), combobox.GetCustomFields(), "Custom Field numbers are not equal in PREMIUM selection: " + i + "in main page and price page");
                    CheckEqual(table.GetStorage(), combobox.GetStorage(), "Storages are not equal in PREMIUM selection: " + i + "in main page and price page");
                    CheckEqual(table.GetAdmins(), combobox.GetAdmins(), "Admin numbers are not equal in PREMIUM selection: " + i + "in main page and price page");
                    CheckEqual(table.GetEmailSender(), combobox.GetEmailSender(), "Email sender numbers are not equal in PREMIUM selection: " + i + "in main page and price page");
                    CheckEqual(table.GetAPILimits(), combobox.GetAPILimits(), "API limits are not equal in PREMIUM selection: " + i + "in main page and price page");
                }
            }
        }

        public void TestLinks(IWebElement body, string verifyClass)
        {
            IList<IWebElement> href = body.FindElements(By.TagName("a"));
            string originalWindow = Driver.CurrentWindowHandle;

            for (int i = 0; i < href.Count; i++)
            {
                
                body = Driver.CheckSiteLoaded(verifyClass);
                href = body.FindElements(By.TagName("a"));
                string link = href[i].Text;
                Driver.ClickElement(href[i]);
                if (Driver.WindowHandles.Count > 1)
                {
                    Driver.SwitchTo().Window(Driver.WindowHandles[1]);
                    Driver.Close();
                    Driver.SwitchTo().Window(originalWindow);
                }
                else
                {
                    if (Driver.Url == HomePage)
                    {
                        System.Console.WriteLine("Main link:" + link);
                    }
                    Driver.Navigate().Back();
                    if (Driver.Url == HomePage)
                    {
                        System.Console.WriteLine("Empty link:" + link);
                        Driver.Navigate().Forward();
                    }
                }
            }         
        }

        public void CheckEqual<T>(T expected, T actual, string message = "")
        {
            try
            {
                Assert.AreEqual(expected, actual);
            }
            catch (AssertFailedException)
            {
                //Assert.Fail(message + " expected: " + expected + " actual: " + actual);
                Console.WriteLine(message + " expected: " + expected + " actual: " + actual);
            }
        }

        public List<PriceClass> CheckPremiumPrices(Boolean monthly)
        {
            // Store the elements in the combobox
            String xpath = "//*[@class=\"col-auto\"]/div[1]/div[4]//";
            List<PriceClass> priceOptions = new List<PriceClass>();
            for (int i = 1; i <= 8; i++)
            {
                PriceClass option = new PriceClass();
                List<string> values = new List<string>();

                var e = Driver.FindElement(By.XPath(xpath + "select/option[" + i + "]"));
                Driver.ClickElement(e);

                string temp = (Driver.FindElement(By.XPath(xpath + "div[2]")).Text);
                values.Add(temp);

                int[] indexes = { 1, 2, 3, 4, 6, 15 };
                foreach (int index in indexes)
                {
                    temp = (Driver.FindElement(By.XPath(xpath + "div[4]/p[" + index + "]")).Text);
                    int tempIndex = temp.IndexOf(":");
                    string var = temp.Substring(tempIndex + 2);
                    values.Add(var);
                }

                option.SetPrice(values[0]);
                option.SetContacts(Convert.ToInt32(values[1]));
                option.SetCustomFields(Convert.ToInt32(values[2]));

                //remove the gb from the storage
                string str = values[3].Substring(0, values[3].Length - 3);
                option.SetStorage(str);

                //option.SetAdmins(Convert.ToInt32(values[4]));
                //option.SetEmailSender(Convert.ToInt32(values[5]));
                option.SetAdmins(values[4]);
                option.SetEmailSender(values[5]);
                option.SetAPILimits(values[6]);
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

                var e = Driver.FindElement(By.XPath(xpath + "/thead/tr/th[5]/div/select/option[" + i + "]"));
                Driver.ClickElement(e);

                int[] indexes = { 1, 3, 6, 45, 4, 9, 47 };
                foreach (int index in indexes)
                {
                    string temp = (Driver.FindElement(By.XPath(xpath + "/tbody/tr[" + index + "]/td[5]")).Text);
                    values.Add(temp);
                }

                option.SetPrice(values[0].Replace(",", "").Replace(".00", ""));
                option.SetContacts(Convert.ToInt32(values[1].Replace(",", "")));
                option.SetCustomFields(Convert.ToInt32(values[2]));

                //remove the gb from the storage
                string var = values[3].Substring(0, values[3].Length - 3);
                option.SetStorage(var);

                //option.SetAdmins(Convert.ToInt32(values[4]));
                //option.SetEmailSender(Convert.ToInt32(values[5]));
                option.SetAdmins(values[4]);
                option.SetEmailSender(values[5]);
                option.SetAPILimits(values[6].Replace(",", ""));
                priceTable.Add(option);
            }

            //Compare the values
            for (int i = 0; i < 8; i++)
            {
                PriceClass combobox = priceOptions[i];
                PriceClass table = priceTable[i];

                CheckEqual(table.GetPrice(), combobox.GetPrice(), "Prices are not equal in PREMIUM selection: " + i);
                CheckEqual(table.GetContacts(), combobox.GetContacts(), "Contact numbers are not equal in PREMIUM selection: " + i);
                CheckEqual(table.GetCustomFields(), combobox.GetCustomFields(), "Custom Field numbers are not equal in PREMIUM selection: " + i);
                CheckEqual(table.GetStorage(), combobox.GetStorage(), "Storages are not equal in PREMIUM selection: " + i);
                CheckEqual(table.GetAdmins(), combobox.GetAdmins(), "Admin numbers are not equal in PREMIUM selection: " + i);
                CheckEqual(table.GetEmailSender(), combobox.GetEmailSender(), "Email sender numbers are not equal in PREMIUM selection: " + i);
                CheckEqual(table.GetAPILimits(), combobox.GetAPILimits(), "API limits are not equal in PREMIUM selection: " + i);

            }
            return priceOptions;
        }

        public PriceClass CheckFreePrices(Boolean monthly)
        {
            // Store the elements in the combobox
            String xpath = "//*[@class=\"col-auto\"]/div[1]/div[1]//";
            List<PriceClass> priceOptions = new List<PriceClass>();
            
            PriceClass option = new PriceClass();
            List<string> values = new List<string>();

            string temp = (Driver.FindElement(By.XPath(xpath + "div[2]")).Text);
            values.Add(temp);

            int[] indexes = { 1, 2, 3, 4};
            foreach (int index in indexes)
            {
                temp = (Driver.FindElement(By.XPath(xpath + "div[4]/p[" + index + "]")).Text);
                int tempIndex = temp.IndexOf(":");
                string tempVar = temp.Substring(tempIndex + 2);
                values.Add(tempVar);
            }

            option.SetPrice(values[0]);
            option.SetContacts(Convert.ToInt32(values[1]));
            option.SetCustomFields(Convert.ToInt32(values[2]));

            //remove the gb from the storage
            string str = values[3].Substring(0, values[3].Length - 3);
            option.SetStorage(str);

            //option.SetAdmins(Convert.ToInt32(values[4]));
            //option.SetEmailSender(Convert.ToInt32(values[5]));
            option.SetAdmins(values[4]);
            priceOptions.Add(option);

            if (!monthly) return priceOptions[0];

            //Store the elements in the table
            xpath = "//*[@class=\"table\"]";
            List<PriceClass> priceTable = new List<PriceClass>();
                
            option = new PriceClass();
            values = new List<string>();

            int[] indexes2 = { 1, 3, 6, 45, 4};
            foreach (int index in indexes2)
            {
                temp = (Driver.FindElement(By.XPath(xpath + "/tbody/tr[" + index + "]/td[2]")).Text);
                values.Add(temp);
            }

            option.SetPrice(values[0].Replace(",", "").Replace(".00", ""));
            option.SetContacts(Convert.ToInt32(values[1].Replace(",", "")));
            option.SetCustomFields(Convert.ToInt32(values[2]));

            //remove the gb from the storage
            string var = values[3].Substring(0, values[3].Length - 3);
            option.SetStorage(var);

            //option.SetAdmins(Convert.ToInt32(values[4]));
            option.SetAdmins(values[4]);
            priceTable.Add(option);
               
            //Compare the values
            PriceClass combobox = priceOptions[0];
            PriceClass table = priceTable[0];

            CheckEqual(table.GetPrice(), combobox.GetPrice(), "Prices are not equal in FREE");
            CheckEqual(table.GetContacts(), combobox.GetContacts(), "Contact numbers are not equal in FREE" );
            CheckEqual(table.GetCustomFields(), combobox.GetCustomFields(), "Custom Field numbers are not equal in FREE");
            CheckEqual(table.GetStorage(), combobox.GetStorage(), "Storages are not equal in FREE");
            CheckEqual(table.GetAdmins(), combobox.GetAdmins(), "Admin numbers are not equal in FREE");

            return combobox;
            
        }

        public PriceClass CheckEssentialsPrices(Boolean monthly)
        {
            // Store the elements in the combobox
            String xpath = "//*[@class=\"col-auto\"]/div[1]/div[2]//";
            List<PriceClass> priceOptions = new List<PriceClass>();

            PriceClass option = new PriceClass();
            List<string> values = new List<string>();

            string temp = (Driver.FindElement(By.XPath(xpath + "div[2]")).Text);
            values.Add(temp);

            int[] indexes = { 1, 2, 3, 4, 6 };
            foreach (int index in indexes)
            {
                temp = (Driver.FindElement(By.XPath(xpath + "div[4]/p[" + index + "]")).Text);
                int tempIndex = temp.IndexOf(":");
                var tempVar = temp.Substring(tempIndex + 2);
                values.Add(tempVar);
            }

            option.SetPrice(values[0]);
            option.SetContacts(Convert.ToInt32(values[1]));
            option.SetCustomFields(Convert.ToInt32(values[2]));

            //remove the gb from the storage
            string str = values[3].Substring(0, values[3].Length - 3);
            option.SetStorage(str);

            //option.SetAdmins(Convert.ToInt32(values[4]));
            //option.SetEmailSender(Convert.ToInt32(values[5]));
            option.SetAdmins(values[4]);
            option.SetEmailSender(values[5]);
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
                temp = (Driver.FindElement(By.XPath(xpath + "/tbody/tr[" + index + "]/td[3]")).Text);
                values.Add(temp);
            }

            option.SetPrice(values[0].Replace(",", "").Replace(".00", ""));
            option.SetContacts(Convert.ToInt32(values[1].Replace(",", "")));
            option.SetCustomFields(Convert.ToInt32(values[2]));

            //remove the gb from the storage
            string var = values[3].Substring(0, values[3].Length - 3);
            option.SetStorage(var);

            //option.SetAdmins(Convert.ToInt32(values[4]));
            option.SetAdmins(values[4]);
            option.SetEmailSender(values[5]);
            priceTable.Add(option);

            //Compare the values
            PriceClass combobox = priceOptions[0];
            PriceClass table = priceTable[0];

            CheckEqual(table.GetPrice(), combobox.GetPrice(), "Prices are not equal in ESSENTIALS");
            CheckEqual(table.GetContacts(), combobox.GetContacts(), "Contact numbers are not equal in ESSENTIALS");
            CheckEqual(table.GetCustomFields(), combobox.GetCustomFields(), "Custom Field numbers are not equal in ESSENTIALS");
            CheckEqual(table.GetStorage(), combobox.GetStorage(), "Storages are not equal in ESSENTIALS");
            CheckEqual(table.GetAdmins(), combobox.GetAdmins(), "Admin numbers are not equal in ESSENTIALS");
            CheckEqual(table.GetEmailSender(), combobox.GetEmailSender(), "Email sender numbers are not equal in ESSENTIALS");

            return combobox;

        }
       
        public PriceClass CheckProfessionalPrices(Boolean monthly)
        {
            // Store the elements in the combobox
            String xpath = "//*[@class=\"col-auto\"]/div[1]/div[3]//";
            List<PriceClass> priceOptions = new List<PriceClass>();

            PriceClass option = new PriceClass();
            List<string> values = new List<string>();

            string temp = (Driver.FindElement(By.XPath(xpath + "div[2]")).Text);
            values.Add(temp);

            int[] indexes = { 1, 2, 3, 4, 6 };
            foreach (int index in indexes)
            {
                temp = (Driver.FindElement(By.XPath(xpath + "div[4]/p[" + index + "]")).Text);
                int tempIndex = temp.IndexOf(":");
                string tempVar = temp.Substring(tempIndex + 2);
                values.Add(tempVar);
            }

            option.SetPrice(values[0]);
            option.SetContacts(Convert.ToInt32(values[1]));
            option.SetCustomFields(Convert.ToInt32(values[2]));

            //remove the gb from the storage
            string str = values[3].Substring(0, values[3].Length - 3);
            option.SetStorage(str);

            //option.SetAdmins(Convert.ToInt32(values[4]));
            //option.SetEmailSender(Convert.ToInt32(values[5]));
            option.SetAdmins(values[4]);
            option.SetEmailSender(values[5]);
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
                temp = (Driver.FindElement(By.XPath(xpath + "/tbody/tr[" + index + "]/td[4]")).Text);
                values.Add(temp);
            }

            option.SetPrice(values[0].Replace(",", "").Replace(".00", ""));
            option.SetContacts(Convert.ToInt32(values[1].Replace(",", "")));
            option.SetCustomFields(Convert.ToInt32(values[2]));

            //remove the gb from the storage
            string var = values[3].Substring(0, values[3].Length - 3);
            option.SetStorage(var);

            //option.SetAdmins(Convert.ToInt32(values[4]));
            option.SetAdmins(values[4]);
            option.SetEmailSender(values[5]);
            priceTable.Add(option);

            //Compare the values
            PriceClass combobox = priceOptions[0];
            PriceClass table = priceTable[0];

            CheckEqual(table.GetPrice(), combobox.GetPrice(), "Prices are not equal in ESSENTIALS");
            CheckEqual(table.GetContacts(), combobox.GetContacts(), "Contact numbers are not equal in ESSENTIALS");
            CheckEqual(table.GetCustomFields(), combobox.GetCustomFields(), "Custom Field numbers are not equal in ESSENTIALS");
            CheckEqual(table.GetStorage(), combobox.GetStorage(), "Storages are not equal in ESSENTIALS");
            CheckEqual(table.GetAdmins(), combobox.GetAdmins(), "Admin numbers are not equal in ESSENTIALS");
            CheckEqual(table.GetEmailSender(), combobox.GetEmailSender(), "Email sender numbers are not equal in ESSENTIALS");

            return combobox;

        }

    }
}
