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
    public class UnitTest1
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
            TestLinks(body, "Features-article");
        }

        [TestMethod]
        public void TestReferencesPage()
        {
            IWebElement body = Driver.GoToUrl(HomePage, "SectionHero");
            Driver.ClickText("References");
            TestLinks(body, "Customers-verticals");
        }

        [TestMethod]
        public void TestPricingPage()
        {
            IWebElement body = Driver.GoToUrl(HomePage, "SectionHero");
            Driver.ClickText("Pricing");
            TestLinks(body, "Pricing-content");
        }

        [TestMethod]
        public void TestResourcesPage()
        {
            IWebElement body = Driver.GoToUrl(HomePage, "SectionHero");
            Driver.ClickText("Resources");
            TestLinks(body, "LandingSection");
        }
        
        public void TestLinks(IWebElement body, string verifyClass)
        {
            IList<IWebElement> href = Driver.CheckElementsExist(By.TagName("a"), body);

            for (int i = 0; i < href.Count; i++)
            {
                body = Driver.CheckSiteLoaded(verifyClass);
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
                CheckEqual(freeMainPage.Price, freePricingPage.Price, "Prices are not equal in Main page and Prices page for FREE");
                CheckEqual(freeMainPage.Contacts, freePricingPage.Contacts, "Contact numbers are not equal in Main page and Prices page for FREE");
                CheckEqual(freeMainPage.CustomFields, freePricingPage.CustomFields, "Custom Field numbers are not equal in Main page and Prices page for FREE");
                CheckEqual(freeMainPage.Storage, freePricingPage.Storage, "Storage numbers are not equal in Main page and Prices page for FREE");
                CheckEqual(freeMainPage.Admins, freePricingPage.Admins, "Admin numbers are not equal in Main page and Prices page for FREE");
                //esesentials part
                CheckEqual(essentialsMainPage.Price, essentialsPricingPage.Price, "Prices are not equal in Main page and Prices page for Essentials");
                CheckEqual(essentialsMainPage.Contacts, essentialsPricingPage.Contacts, "Contact numbers are not equal in Main page and Prices page for Essentials");
                CheckEqual(essentialsMainPage.CustomFields, essentialsPricingPage.CustomFields, "Custom Field numbers are not equal in Main page and Prices page for Essentials");
                CheckEqual(essentialsMainPage.Storage, essentialsPricingPage.Storage, "Storage numbers are not equal in Main page and Prices page for Essentials");
                CheckEqual(essentialsMainPage.Admins, essentialsPricingPage.Admins, "Admin numbers are not equal in Main page and Prices page for Essentials");
                CheckEqual(essentialsMainPage.EmailSender, essentialsPricingPage.EmailSender, "Emain Sender numbers are not equal in Main page and Prices page for Essentials");
                //professional
                CheckEqual(professionalMainPage.Price, professionalPricingPage.Price, "Prices are not equal in Main page and Prices page for Professional");
                CheckEqual(professionalMainPage.Contacts, professionalPricingPage.Contacts, "Contact numbers are not equal in Main page and Prices page for Professional");
                CheckEqual(professionalMainPage.CustomFields, professionalPricingPage.CustomFields, "Custom Field numbers are not equal in Main page and Prices page for Professional");
                CheckEqual(professionalMainPage.Storage, professionalPricingPage.Storage, "Storage numbers are not equal in Main page and Prices page for Professional");
                CheckEqual(professionalMainPage.Admins, professionalPricingPage.Admins, "Admin numbers are not equal in Main page and Prices page for Professional");
                CheckEqual(professionalMainPage.EmailSender, professionalPricingPage.EmailSender, "Email Sender numbers are not equal in Main page and Prices page for Professional");
                //premium
                for (int j = 0; j < 8; j++)
                {
                    PriceClass combobox = premiumMainPage[j];
                    PriceClass table = premiumPricingPage[j];

                    CheckEqual(table.Price, combobox.Price, "Prices are not equal in PREMIUM selection: " + i + "in main page and price page");
                    CheckEqual(table.Contacts, combobox.Contacts, "Contact numbers are not equal in PREMIUM selection: " + i + "in main page and price page");
                    CheckEqual(table.CustomFields, combobox.CustomFields, "Custom Field numbers are not equal in PREMIUM selection: " + i + "in main page and price page");
                    CheckEqual(table.Storage, combobox.Storage, "Storages are not equal in PREMIUM selection: " + i + "in main page and price page");
                    CheckEqual(table.Admins, combobox.Admins, "Admin numbers are not equal in PREMIUM selection: " + i + "in main page and price page");
                    CheckEqual(table.EmailSender, combobox.EmailSender, "Email sender numbers are not equal in PREMIUM selection: " + i + "in main page and price page");
                    CheckEqual(table.ApiLimits, combobox.ApiLimits, "API limits are not equal in PREMIUM selection: " + i + "in main page and price page");
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

        public void CheckEqual<T>(T expected, T actual, string message)
        {
            try
            {
                Assert.AreEqual(expected, actual);
            }
            catch (AssertFailedException)
            {
                ErrorLogs.Add(message);
            }
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

                CheckEqual(table.Price, combobox.Price, "Prices are not equal in PREMIUM selection: " + (i + 1));
                CheckEqual(table.Contacts, combobox.Contacts, "Contact numbers are not equal in PREMIUM selection: " + (i + 1));
                CheckEqual(table.CustomFields, combobox.CustomFields, "Custom Field numbers are not equal in PREMIUM selection: " + (i + 1));
                CheckEqual(table.Storage, combobox.Storage, "Storages are not equal in PREMIUM selection: " + (i + 1));
                CheckEqual(table.Admins, combobox.Admins, "Admin numbers are not equal in PREMIUM selection: " + (i + 1));
                CheckEqual(table.EmailSender, combobox.EmailSender, "Email sender numbers are not equal in PREMIUM selection: " + (i + 1));
                CheckEqual(table.ApiLimits, combobox.ApiLimits, "API limits are not equal in PREMIUM selection: " + (i + 1));
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

            for(int index = 1; index <= 4; index++)
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

            CheckEqual(table.Price, combobox.Price, "Prices are not equal in FREE");
            CheckEqual(table.Contacts, combobox.Contacts, "Contact numbers are not equal in FREE");
            CheckEqual(table.CustomFields, combobox.CustomFields, "Custom Field numbers are not equal in FREE");
            CheckEqual(table.Storage, combobox.Storage, "Storages are not equal in FREE");
            CheckEqual(table.Admins, combobox.Admins, "Admin numbers are not equal in FREE");

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

            CheckEqual(table.Price, combobox.Price, "Prices are not equal in ESSENTIALS");
            CheckEqual(table.Contacts, combobox.Contacts, "Contact numbers are not equal in ESSENTIALS");
            CheckEqual(table.CustomFields, combobox.CustomFields, "Custom Field numbers are not equal in ESSENTIALS");
            CheckEqual(table.Storage, combobox.Storage, "Storages are not equal in ESSENTIALS");
            CheckEqual(table.Admins, combobox.Admins, "Admin numbers are not equal in ESSENTIALS");
            CheckEqual(table.EmailSender, combobox.EmailSender, "Email sender numbers are not equal in ESSENTIALS");

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

            CheckEqual(table.Price, combobox.Price, "Prices are not equal in PROFESSIONAL");
            CheckEqual(table.Contacts, combobox.Contacts, "Contact numbers are not equal in PROFESSIONAL");
            CheckEqual(table.CustomFields, combobox.CustomFields, "Custom Field numbers are not equal in PROFESSIONAL");
            CheckEqual(table.Storage, combobox.Storage, "Storages are not equal in PROFESSIONAL");
            CheckEqual(table.Admins, combobox.Admins, "Admin numbers are not equal in PROFESSIONAL");
            CheckEqual(table.EmailSender, combobox.EmailSender, "Email sender numbers are not equal in PROFESSIONAL");
            return combobox;

        }

        [TestMethod]
        public void TestLogin()
        {
            LoginWithoutCookies();
            Driver.Quit();
            Initiliaze();
            LoginWithCookies();
        }

        public Cookie GetCookie()
        {
            Cookie cookie = null;
            try
            {
                StreamReader reader = new StreamReader("../../cookie.txt");
                String name = reader.ReadLine();
                String value = reader.ReadLine();
                reader.Close();
                //expires = Wed 07 / 14 / 2051 13:19:53 UTC; path =/; domain =.raklet.net; secure; httpOnly
                DateTime time = new DateTime(2021, 07, 28, 13, 19, 53);
                cookie = new Cookie(name.Trim(), value.Trim(), ".raklet.net", "/", time);
            }
            catch
            {
                Assert.Fail("Cannot find the file");

            }
            return cookie;
        }

        public void LoginWithoutCookies()
        {
            Driver.GoToUrl(HomePage, "SectionHero");
            string xpath = "//*[@class=\"navbar-auth-login\"]";
            IWebElement b = (Driver.CheckElementExist(By.XPath(xpath)));
            Driver.ClickElement(b);
            Driver.CheckSiteLoaded("form-horizontal");

            //e-mail part
            IWebElement email = (Driver.CheckElementExist(By.Id("Email")));
            email.Clear();
            email.SendKeys(LoginEmail);
            xpath = "//*[@id=\"loginForm\"]/form/div[2]/div";
            Driver.CheckElementExist(By.XPath(xpath)).Click();
            Driver.CheckSiteLoaded("RememberMeCheck");

            //password part
            IWebElement password = (Driver.CheckElementExist(By.Id("Password")));
            password.Clear();
            password.SendKeys(LoginPassword);
            xpath = "//*[@id=\"loginForm\"]/form/div[3]/div/input";
            Driver.CheckElementExist(By.XPath(xpath)).Click();
            Driver.CheckSiteLoaded("Manager-HeaderSubmenuLogo", 30);
        }

        public void LoginWithCookies()
        {
            Driver.GoToUrl(HomePage, "SectionHero");
            Cookie cookie = GetCookie();
            Driver.Manage().Cookies.AddCookie(cookie);

            string xpath = "//*[@class=\"navbar-auth-login\"]";
            var b = (Driver.CheckElementExist(By.XPath(xpath)));
            Driver.ClickElement(b);
            Driver.CheckSiteLoaded("Manager-HeaderSubmenuLogo", 30);
        }

        [TestMethod]
        public void TestAccountBilling()
        {
            LoginWithCookies();
            String xpath = "//*[@class=\"Manager-navigationAccount\"]/li[3]/a";
            Driver.ClickElement(Driver.CheckElementExist(By.XPath(xpath)));
            xpath = "//*[@class=\"Manager-navigationAccount\"]/li[3]/div/a";
            Driver.ClickElement(Driver.CheckElementExist(By.XPath(xpath + "[1]")));
            IWebElement body = Driver.CheckSiteLoaded("ManagerContent-body");
            TestLinks(body, "Reports");
        }

        [TestMethod]
        public void TestAccountReports()
        {
            LoginWithCookies();
            String xpath = "//*[@class=\"Manager-navigationAccount\"]/li[3]/a";
            Driver.ClickElement(Driver.CheckElementExist(By.XPath(xpath)));
            xpath = "//*[@class=\"Manager-navigationAccount\"]/li[3]/div/a";
            Driver.ClickElement(Driver.CheckElementExist(By.XPath(xpath + "[2]")));
            IWebElement body = Driver.CheckSiteLoaded("ManagerContent-body");
            TestLinks(body, "Reports");
        }

        [TestMethod]
        public void TestAccountSettings()
        {
            LoginWithCookies();
            String xpath = "//*[@class=\"Manager-navigationAccount\"]/li[3]/a";
            Driver.ClickElement(Driver.CheckElementExist(By.XPath(xpath)));
            xpath = "//*[@class=\"Manager-navigationAccount\"]/li[3]/div/a";
            Driver.ClickElement(Driver.CheckElementExist(By.XPath(xpath + "[3]")));
            IWebElement body = Driver.CheckSiteLoaded("ManagerContent-body");
            TestLinks(body, "Settings");
        }

        [TestMethod]
        public void TestAccountMyProfile()
        {
            LoginWithCookies();
            String xpath = "//*[@class=\"Manager-navigationAccount\"]/li[3]/a";
            Driver.ClickElement(Driver.CheckElementExist(By.XPath(xpath)));
            xpath = "//*[@class=\"Manager-navigationAccount\"]/li[3]/div/a";
            Driver.ClickElement(Driver.CheckElementExist(By.XPath(xpath + "[4]")));
            IWebElement body = Driver.CheckSiteLoaded("SocialContent");
            TestLinks(body, "ProfileHeader");
        }

        [TestMethod]
        public void TestAccountNewOrganization()
        {
            LoginWithCookies();
            String xpath = "//*[@class=\"Manager-navigationAccount\"]/li[3]/a";
            Driver.ClickElement(Driver.CheckElementExist(By.XPath(xpath)));
            xpath = "//*[@class=\"Manager-navigationAccount\"]/li[3]/div/a";
            Driver.ClickElement(Driver.CheckElementExist(By.XPath(xpath + "[5]")));
            IWebElement body = Driver.CheckSiteLoaded("Login-content");
            TestLinks(body, "Signup");
        }

        [TestMethod]
        public void TestAccountLogout()
        {
            LoginWithCookies();
            String xpath = "//*[@class=\"Manager-navigationAccount\"]/li[3]/a";
            Driver.ClickElement(Driver.CheckElementExist(By.XPath(xpath)));
            xpath = "//*[@class=\"Manager-navigationAccount\"]/li[3]/div/a";
            Driver.ClickElement(Driver.CheckElementExist(By.XPath(xpath + "[7]")));
            string text = Driver.CheckSiteLoaded("alert").Text;
            CheckEqual(text, "You have been successfully logged out.", "Cannot logged out sucessfully");
        }

        [TestMethod]
        public void TestAccountHelpCenter()
        {
            LoginWithCookies();
            String xpath = "//*[@class=\"Manager-navigationAccount\"]/li[2]/a";
            Driver.ClickElement(Driver.CheckElementExist(By.XPath(xpath)));
            IWebElement body = Driver.CheckSiteLoaded("ManagerContent-body");
            TestLinks(body, "ProgressBar");
        }

        [TestMethod]
        public void TestAccountSite()
        {
            LoginWithCookies();
            String xpath = "//*[@class=\"Manager-navigationAccount\"]/li[1]/a";
            Driver.ClickElement(Driver.CheckElementExist(By.XPath(xpath)));
            IWebElement body = Driver.CheckSiteLoaded("SocialContent");
            TestLinks(body, "SocialContent-bodyWithMenu");
        }

        [TestMethod]
        public void TestEmailSubmitFeatures()
        {
            //contact database and paid newsletter are empty
            List<int> empty = new List<int>() { 1, 11 };
            string xpath = "//*[@id=\"Features-app-store\"]//div/div[";
            for (int i = 1; i <= 12; i++)
            {
                if (empty.Contains(i)) continue;
                Driver.GoToUrl(HomePage + "features/app-store/", "Features-article");
                var e = Driver.CheckElementExist(By.XPath(xpath + i + "]//a"));
                FillEmailForm(e);
                CheckEmailSubmit(e);
                
            }
        }

        [TestMethod]
        public void TestEmailSubmitReferences()
        {
            //creators and publishers are empty
            List<int> empty = new List<int>() { 1, 2 };
            string xpath = "//*[@class=\"Customers-verticals\"]//div/div[";
            for (int i = 1; i <= 17; i++)
            {
                if (empty.Contains(i)) continue;
                Driver.GoToUrl(HomePage + "customers", "Customers-verticals");
                var e = Driver.CheckElementExist(By.XPath(xpath + i + "]//a"));
                FillEmailForm(e);
                CheckEmailSubmit(e);
            }
        }
        
        public void FillEmailForm(IWebElement element)
        {
            string buttonXpath = "//form//button";
            string formXpath = "//form//input";
            Driver.ClickElement(element);
            Driver.CheckSiteLoaded("input-group");
            IWebElement email = (Driver.CheckElementExist(By.XPath(formXpath)));
            email.Clear();
            email.SendKeys(DemoEmail);
            Driver.CheckElementExist(By.XPath(buttonXpath)).Click();
        }

        public void CheckEmailSubmit(IWebElement element)
        {
            var temp = (Driver.CheckSiteLoaded("Login-content", 5, false));
            var temp1 = Driver.CheckSiteLoaded("Onboarding-title", 5, false);
            if (temp == null && temp1 == null)
            {
                ErrorLogs.Add("Email Submit not sucessfull for " + element.Text);
            }
        }

        [TestMethod]
        public void TestEmailSubmitResources()
        {            
            Driver.GoToUrl(HomePage + "knowledge-center/", "LandingSection");

            string buttonXpath = "//form/div[1]/div/div//button";
            string formXpath = "//form/div[1]/div/div/input";
            IWebElement email = (Driver.CheckElementExist(By.XPath(formXpath)));
            email.Clear();
            email.SendKeys(DemoEmail);
            Driver.CheckElementExist(By.XPath(buttonXpath)).Click();
            var temp = (Driver.CheckSiteLoaded("Login-content", 5, false));
            var temp1 = Driver.CheckSiteLoaded("Onboarding-title", 5, false);
            if (temp == null && temp1 == null)
            {
                ErrorLogs.Add("Email Submit not sucessfull for newsletter");
            }
        }
    
        [TestMethod]
        public void TestPopUpCaseStudies()
        {
            for(int i = 1; i <= 7; i++)
            {
                Driver.GoToUrl(HomePage + "knowledge-center/", "LandingSection");
                string originalWindow = Driver.CurrentWindowHandle;
                string xpath = "//*[@class=\"list-unstyled\"]//*[@class=\"my-2\"]//a";
                xpath = "//*[@class=\"list-unstyled\"]//*[@class=\"my-2\"][";

                IWebElement stories = Driver.CheckElementExist(By.XPath(xpath + i + "]//a"));
                IWebElement story = stories;

                try
                {
                    Driver.ClickElement(story);
                    xpath = "//*[@id=\"PopupSignupForm_0\"]/div[2]/div[2]";
                    Driver.Wait(40, "ElementExists", By.XPath(xpath), null);
                }
                catch (Exception e)
                {
                    ErrorLogs.Add("TimeoutException - Popup did not appear for:" + story.Text + e.Message + e.StackTrace);
                }
                finally
                {
                    if (Driver.WindowHandles.Count > 1)
                    {
                        Driver.SwitchTo().Window(Driver.WindowHandles[1]);
                        Driver.Close();
                        Driver.SwitchTo().Window(originalWindow);
                    }
                }
            }
        }
    
       [TestMethod]
        public void TestWelcomeMatFeatures()
        {
            //contact database and paid newsletter are empty
            List<int> empty = new List<int>() { 1, 11 };

            string xpath = "//*[@id=\"Features-app-store\"]//div/div[";
            for (int i = 1; i <= 12; i++)
            {
                if (empty.Contains(i)) continue;

                Driver.GoToUrl(HomePage + "features/app-store/", "Features-article");
                var e = Driver.CheckElementExist(By.XPath(xpath + i + "]//a"));
                WelcomeMat(e, e.Text);
            }
        }

        [TestMethod]
        public void TestWelcomeMatReferences()
        {
            //creators and publishers are empty
            List<int> empty = new List<int>() { 1, 2 };

            string xpath = "//*[@class=\"Customers-verticals\"]//div/div[";
            
            for (int i = 1; i <= 17; i++)
            {
                if (empty.Contains(i)) continue;

                Driver.GoToUrl(HomePage + "customers", "Customers-verticals");
                var e = Driver.CheckElementExist(By.XPath(xpath + i + "]//a"));
                WelcomeMat(e, e.Text);  
            }
        }

        public void WelcomeMat(IWebElement e , string str)
        {
            string className = "landing-closingTitle";
            string buttonXpath = "//body/div[1]//form//button";
            string formXpath = "//body/div[1]//form//input";
            Driver.ClickElement(e);
            Driver.CheckSiteLoaded("lead");
            try
            {
                Driver.Wait(40, "ElementExists", By.ClassName(className), null);
                //email-submit
                IWebElement email = (Driver.CheckElementExist(By.XPath(formXpath)));
                email.Clear();
                email.SendKeys(DemoEmail);
                Driver.CheckElementExist(By.XPath(buttonXpath)).Click();
                var temp = (Driver.CheckSiteLoaded("Login-content", 5, false));
                var temp1 = Driver.CheckSiteLoaded("Onboarding-title", 5, false);
                if (temp == null && temp1 == null)
                {
                    ErrorLogs.Add("Email Submit not sucessfull for " + str);
                }
            }
            catch (OpenQA.Selenium.WebDriverTimeoutException)
            {
                ErrorLogs.Add("TimeoutException - WelcomeMat did not appear for:" + str);
            }
            finally
            {
                Driver.Quit();
                Initiliaze();
            }
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
    
        [TestMethod]
        public void TestHeaderFooter()
        {
            List<string> urls = new List<string>()
            {
                HomePage, HomePage + "features/app-store/",
                HomePage + "customers/", HomePage + "pricing",
                HomePage + "knowledge-center/"
            };
            List<string> loaded = new List<string>()
            {
                "SectionHero", "Features-article",
                "Customers-verticals", "Pricing-content",
                "LandingSection"
            };
            int i = 0;
            foreach (string url in urls)
            {
                Driver.GoToUrl(url, loaded[i++]);
                checkHeader(url);
                checkFooter(url);
            }
        }

        public void checkHeader(string url)
        {
            Driver.CheckElementExist(By.XPath("//*[@class=\"container-fluid\"]"));
            //*[@class="container-fluid"]//img
            IWebElement logo = Driver.CheckElementExist(By.XPath("//*[@class=\"container-fluid\"]//img"));
            CheckEqual(logo.GetAttribute("src"), "https://hello.raklet.net/images/_shared/logo/color/black/128_.png", "Logo not loaded correctly - " + url);
            //*[@class="container-fluid"]//li[1]/a
            IWebElement features = Driver.CheckElementExist(By.XPath("//*[@class=\"container-fluid\"]//li[1]/a"));
            CheckEqual(features.GetAttribute("href"), "https://hello.raklet.net/features/app-store/", "Features not loaded correctly - " + url);
            
            IWebElement references = Driver.CheckElementExist(By.XPath("//*[@class=\"container-fluid\"]//li[2]/a"));
            CheckEqual(references.GetAttribute("href"), "https://hello.raklet.net/customers", "References not loaded correctly - " + url);
            
            IWebElement pricing = Driver.CheckElementExist(By.XPath("//*[@class=\"container-fluid\"]//li[3]/a"));
            CheckEqual(pricing.GetAttribute("href"), "https://hello.raklet.net/pricing/", "Pricing not loaded correctly - " + url);
            
            IWebElement resources = Driver.CheckElementExist(By.XPath("//*[@class=\"container-fluid\"]//li[4]/a"));
            CheckEqual(resources.GetAttribute("href"), "https://hello.raklet.net/knowledge-center/", "Resources not loaded correctly - " + url);
        }

        public void checkFooter(string url)
        {
            FooterFirstColoumn(url);
            FooterSecondColoumn(url);
            FooterThirdColoumn(url);
            FooterExtra(url);             
        }

        public void FooterFirstColoumn(string url)
        {
            Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-content\"]//*[@class=\"col-sm-4\"]"));

            IWebElement img = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-content\"]//*[@class=\"col-sm-4\"]/div/a[1]/img"));
            CheckEqual(img.GetAttribute("src"), "https://hello.raklet.net/images/_shared/logo/color/white/128.png", "Image not loaded in footer - " + url);

            IWebElement p = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-content\"]//*[@class=\"col-sm-4\"]/div/p"));
            CheckEqual(p.Text, "Raklet is a modern cloud platform that provides plug and play solutions for contacts, messsages and payments.", "Text not correct in footer - " + url);

            //*[@class="Footer-content"]//*[@class="col-sm-4"]/div/address
            IWebElement address = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-content\"]//*[@class=\"col-sm-4\"]/ div/address"));
            CheckEqual(address.Text, "4347 20th Street, San Francisco CA 94114", "Adress not correct in footer - " + url);

            //*[@class="Footer-content"]//*[@class="col-sm-4"]/div/a[2]
            IWebElement phone = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-content\"]//*[@class=\"col-sm-4\"]/div/a[2]"));
            CheckEqual(phone.Text, "+1.415.234.0554", "Phone number not correct in footer - " + url);

            //*[@class="Footer-content"]//*[@class="col-sm-4"]/div/a[3]
            IWebElement mail = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-content\"]//*[@class=\"col-sm-4\"]/ div/a[3]"));
            CheckEqual(mail.Text, "hello@raklet.com", "Mail not correct in footer - " + url);

            //*[@class="Footer-content"]//*[@class="col-sm-4"]//ul/li[1]/a/img
            IWebElement andorid = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-content\"]//*[@class=\"col-sm-4\"]//ul/li[1]/a/img"));
            CheckEqual(andorid.GetAttribute("src"), "https://hello.raklet.net/images/_shared/android.png", "Andorid logo not loaded in footer - " + url);

            //*[@class="Footer-content"]//*[@class="col-sm-4"]//ul/li[2]/a/img
            IWebElement ios = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-content\"]//*[@class=\"col-sm-4\"]//ul/li[2]/a/img"));
            CheckEqual(ios.GetAttribute("src"), "https://hello.raklet.net/images/_shared/ios.png", "IOS logo not loaded in footer - " + url);

            //*[@class="Footer-content"]//*[@class="col-sm-4"]//ul/li[1]/a/div
            IWebElement facebook = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-content\"]//*[@class=\"col-sm-4\"]//ul/li[1]/a/div"));

            //*[@class="Footer-content"]//*[@class="col-sm-4"]//ul/li[2]/a/div
            IWebElement twitter = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-content\"]//*[@class=\"col-sm-4\"]//ul/li[2]/a/div"));

            //*[@class="Footer-content"]//*[@class="col-sm-4"]//ul/li[3]/a/div
            IWebElement instagram = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-content\"]//*[@class=\"col-sm-4\"]//ul/li[3]/a/div"));

            //*[@class="Footer-language"]//span
            IWebElement language = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-language\"]//span"));
            CheckEqual(language.Text, "Language:", "Language text not correct in footer - " + url);

            //*[@class="Footer-language"]//translation-selector/div/div
            //translation-selector/div/div/ul/li/a
            List<string> languages = new List<string>()
            {
                "Deutsche", "English", "Española", "Français", "Italiano",
                "Slovenščina", "Türkçe", "Português", "Polskie", "Svenska", "Nederlands",
                "Norsk", "български", "čeština", "Eestlane", "Ελληνικά", "हिन्दी",
                "Gaeilge", "日本人", "Latvietis", "Lietuvis", "Lëtzebuergesch",
                "മലയാളം", "Română", "Slovenský", "Suomalainen", "dansk",
                "عربى", "中文", "русский"
            };
            IWebElement dropdown = Driver.CheckElementExist(By.XPath("//translation-selector//ul"));

            IWebElement e = Driver.CheckElementExist(By.XPath("//translation-selector//*[@class=\"dropdown-toggle\"]"));
            CheckEqual(e.Text, "English", "Footer language not correct- " + url);
            Driver.ClickElement(e);

            for (int i = 0; i < languages.Count; i++)
            {
                IWebElement ee = Driver.CheckElementExist(By.XPath("//translation-selector//li[" + (i + 1) + "]/a"));
                CheckEqual(ee.Text, languages[i], "Footer languages dropdown list not correct - " + url);
            }
        }

        public void FooterSecondColoumn(string url)
        {
            Driver.CheckElementExist(By.XPath("//footer/div[1]//div[2]"));
            List<string> col2Names = new List<string>()
            {
                "Features",
                "References",
                "Pricing",
                "Resources",
                "Help Center",
                "Blog",
                "About",
                "Explore",
                "Countries we serve",
                "We're hiring!",
                "API & Developers",
                "Affiliate Program",
                "Contact",
                "Schedule Demo"
            };
            List<string> col2Href = new List<string>()
            {
                "https://hello.raklet.net/features/app-store/",
                "https://hello.raklet.net/customers/",
                "https://hello.raklet.net/pricing/",
                "https://hello.raklet.net/knowledge-center/",
                "https://help.raklet.com/",
                "https://blog.raklet.com/",
                "https://hello.raklet.net/our-story/",
                "https://hello.raklet.net/explore/",
                "https://hello.raklet.net/countries-we-serve/",
                "https://angel.co/raklet/jobs",
                "https://api.raklet.com/swagger/ui/index/",
                "https://hello.raklet.net/affiliate/",
                "https://hello.raklet.net/contact/",
                "https://hello.raklet.net/schedule-demo/"
            };
            for (int i = 1; i <= 14; i++)
            {
                IWebElement element = Driver.CheckElementExist(By.XPath("//footer/div[1]//div[2]//*[@class=\"list-unstyled\"]/li[" + i + "]/a"));
                CheckEqual(col2Names[i - 1], element.Text, "Footer not loaded correctly - " + url);
                CheckEqual(col2Href[i - 1], element.GetAttribute("href"), "Footer link not correct for " + element.Text + " - " + url);
            }
        }

        public void FooterThirdColoumn(string url)
        {
            Driver.CheckElementExist(By.XPath("//footer/div[1]//div[3]"));
            List<string> col3Names = new List<string>()
            {
                "Membership Management Software",
                "Event Management Software",
                "Club Management Software",
                "Alumni Engagement Platform",
                "Digital Membership Card",
                "Non-Profit Fundraising Software",
                "Association Management Software",
                "Chamber of Commerce Software",
                "Church Software",
                "Political Party Software",
                "Comparison of Raklet with other platforms",
                "Integrations",
                "Reviews"
            };
            List<string> col3Href = new List<string>()
            {
                "https://hello.raklet.net/membership-management-software/",
                "https://hello.raklet.net/event-management-software/",
                "https://hello.raklet.net/club-management-software/",
                "https://hello.raklet.net/alumni-engagement-software/",
                "https://hello.raklet.net/digital-membership-card/",
                "https://hello.raklet.net/non-profit-software/",
                "https://hello.raklet.net/association-management-software/",
                "https://hello.raklet.net/chamber-of-commerce-software/",
                "https://hello.raklet.net/church-software/",
                "https://hello.raklet.net/political-party-software/",
                "https://hello.raklet.net/alternative/all-platforms/",
                "https://hello.raklet.net/integrations/",
                "https://hello.raklet.net/reviews/"
            };
            for (int i = 1; i <= 13; i++)
            {
                IWebElement element = Driver.CheckElementExist(By.XPath("//footer/div[1]//div[3]//*[@class=\"list-unstyled\"]/li[" + i + "]/a"));
                CheckEqual(col3Names[i - 1], element.Text, "Footer not loaded correctly - " + url);
                CheckEqual(col3Href[i - 1], element.GetAttribute("href"), "Footer link not correct for " + element.Text + " - " + url);
            }
        }

        public void FooterExtra(string url)
        {
            IWebElement a = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-extras\"]//*[@class=\"alpha\"]"));
            CheckEqual(a.Text, "Made with love", "Extra footer not loaded correctly - " + url);

            IWebElement o = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-extras\"]//*[@class=\"omega\"]"));
            CheckEqual(o.Text, "in San Francisco, Berlin & Istanbul", "Extra footer not loaded correctly - " + url);

            IWebElement t = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-extras\"]//*[@class=\"Footer-extrasNav list-inline\"]"));
            CheckEqual(t.Text, "Terms of UsePrivacy Policy©2013-2021", "Extra footer not loaded correctly - " + url);
        }
    }
}
