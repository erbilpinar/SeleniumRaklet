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
    class AccountTests
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
            catch (Exception e)
            {
                Assert.Fail("Cannot find the file" + e.Message + e.StackTrace);

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
        public void TestAccountLogout()
        {
            LoginWithCookies();
            String xpath = "//*[@class=\"Manager-navigationAccount\"]/li[3]/a";
            Driver.ClickElement(Driver.CheckElementExist(By.XPath(xpath)));
            xpath = "//*[@class=\"Manager-navigationAccount\"]/li[3]/div/a";
            Driver.ClickElement(Driver.CheckElementExist(By.XPath(xpath + "[7]")));
            string text = Driver.CheckSiteLoaded("alert").Text;
            ErrorLogs = Driver.CheckEqual(text, "You have been successfully logged out.", "Cannot logged out sucessfully", ErrorLogs);
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
            ErrorLogs = Driver.TestLinks(body, "Reports", ErrorLogs);
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
            ErrorLogs = Driver.TestLinks(body, "Reports", ErrorLogs);
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
            ErrorLogs = Driver.TestLinks(body, "Settings", ErrorLogs);
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
            ErrorLogs = Driver.TestLinks(body, "ProfileHeader", ErrorLogs);
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
            ErrorLogs = Driver.TestLinks(body, "Signup", ErrorLogs);
        }

        [TestMethod]
        public void TestAccountHelpCenter()
        {
            LoginWithCookies();
            String xpath = "//*[@class=\"Manager-navigationAccount\"]/li[2]/a";
            Driver.ClickElement(Driver.CheckElementExist(By.XPath(xpath)));
            IWebElement body = Driver.CheckSiteLoaded("ManagerContent-body");
            ErrorLogs = Driver.TestLinks(body, "ProgressBar", ErrorLogs);
        }

        [TestMethod]
        public void TestAccountSite()
        {
            LoginWithCookies();
            String xpath = "//*[@class=\"Manager-navigationAccount\"]/li[1]/a";
            Driver.ClickElement(Driver.CheckElementExist(By.XPath(xpath)));
            IWebElement body = Driver.CheckSiteLoaded("SocialContent");
            ErrorLogs = Driver.TestLinks(body, "SocialContent-bodyWithMenu", ErrorLogs);
        }
    }
}
