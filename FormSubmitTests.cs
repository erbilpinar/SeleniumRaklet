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
    class FormSubmitTests
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

        public void WelcomeMat(IWebElement e, string str)
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
            catch (OpenQA.Selenium.WebDriverTimeoutException er)
            {
                ErrorLogs.Add("TimeoutException - WelcomeMat did not appear for:" + str + er.Message + er.StackTrace);
            }
            finally
            {
                Driver.Quit();
                Initiliaze();
            }
        }
    }
}
