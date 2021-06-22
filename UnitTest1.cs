using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;


namespace RakletTest
{
    [TestClass]
    public class UnitTest1
    {
        public const string HomePage = "https://hello.raklet.net/";
        public static IWebDriver driver;

        [TestInitialize]
        public void Initiliaze()
        {
            driver = new ChromeDriver();
            System.Environment.SetEnvironmentVariable("webdriver.chrome.driver", @"/home/ella/PycharmProjects");
            ChromeOptions options = new ChromeOptions();
            options.AddExcludedArgument("disable-popup-blocking");
            options.AddUserProfilePreference("profile.default_content_setting_values.cookies", 2);
        }

        [TestCleanup]
        public void CleanUp()
        {
            driver.Quit();
        }

        [TestMethod]
        public void TestMain()
        {
            IWebElement body = driver.GoToUrl(HomePage, "ApplicationContainer");
            IList<IWebElement> href = body.FindElements(By.TagName("a"));
            string originalWindow = driver.CurrentWindowHandle;

            for (int i = 0; i < href.Count; i++)
            {
                body = driver.GoToUrl(HomePage, "ApplicationContainer");
                href = body.FindElements(By.TagName("a"));
                string link = href[i].Text;
                driver.click_element(href[i]);
                if (driver.WindowHandles.Count > 1)
                {
                    driver.SwitchTo().Window(driver.WindowHandles[1]);
                    driver.Close();
                    driver.SwitchTo().Window(originalWindow);
                }
                else
                {
                    if (driver.Url == HomePage)
                    {
                        System.Console.WriteLine("Empty link:" + link);
                    }
                    else
                    {
                        driver.Navigate().Back();
                    }
                }
            }
        }

        [TestMethod]
        public void TestFeatures()
        {
            IWebElement body = driver.GoToUrl(HomePage, "ApplicationContainer");
            driver.click_text("Features");
            IList<IWebElement> href = body.FindElements(By.TagName("a"));
            string originalWindow = driver.CurrentWindowHandle;

            for (int i = 0; i < href.Count; i++)
            {
                body = driver.FindElement(By.ClassName("ApplicationContainer"));
                href = body.FindElements(By.TagName("a"));
                string link = href[i].Text;
                driver.click_element(href[i]);
                if (driver.WindowHandles.Count > 1)
                {
                    driver.SwitchTo().Window(driver.WindowHandles[1]);
                    driver.Close();
                    driver.SwitchTo().Window(originalWindow);
                }
                else
                {
                    if (driver.Url == HomePage)
                    {
                        System.Console.WriteLine("Main link:" + link);
                    }
                    driver.Navigate().Back();
                    if (driver.Url == HomePage)
                    {
                        System.Console.WriteLine("Empty link:" + link);
                        driver.Navigate().Forward();
                    }
                }
            }
        }

        [TestMethod]
        public void TestReferences()
        {
            IWebElement body = driver.GoToUrl(HomePage, "ApplicationContainer");
            driver.click_text("References");
            IList<IWebElement> href = body.FindElements(By.TagName("a"));
            string originalWindow = driver.CurrentWindowHandle;

            for (int i = 0; i < href.Count; i++)
            {
                body = driver.FindElement(By.ClassName("ApplicationContainer"));
                href = body.FindElements(By.TagName("a"));
                string link = href[i].Text;
                driver.click_element(href[i]);
                if (driver.WindowHandles.Count > 1)
                {
                    driver.SwitchTo().Window(driver.WindowHandles[1]);
                    driver.Close();
                    driver.SwitchTo().Window(originalWindow);
                }
                else
                {
                    if (driver.Url == HomePage)
                    {
                        System.Console.WriteLine("Main link:" + link);
                    }
                    driver.Navigate().Back();
                    if (driver.Url == HomePage)
                    {
                        System.Console.WriteLine("Empty link:" + link);
                        driver.Navigate().Forward();
                    }
                }
            }
        }

        [TestMethod]
        public void Test_Pricing()
        {
            IWebElement body = driver.GoToUrl(HomePage, "ApplicationContainer");
            driver.click_text("Pricing");
            IList<IWebElement> href = body.FindElements(By.TagName("a"));
            string originalWindow = driver.CurrentWindowHandle;
            for (int i = 0; i < href.Count; i++)
            {
                body = driver.FindElement(By.ClassName("ApplicationContainer"));
                href = body.FindElements(By.TagName("a"));
                string link = href[i].Text;
                driver.click_element(href[i]);
                if (driver.WindowHandles.Count > 1)
                {
                    driver.SwitchTo().Window(driver.WindowHandles[1]);
                    driver.Close();
                    driver.SwitchTo().Window(originalWindow);
                }
                else
                {
                    if (driver.Url == HomePage)
                    {
                        System.Console.WriteLine("Main link:" + link);
                    }
                    driver.Navigate().Back();
                    if (driver.Url == HomePage)
                    {
                        System.Console.WriteLine("Empty link:" + link);
                        driver.Navigate().Forward();
                    }
                }
            }

            String xpath = "//*[@class=\"container\"]/div[2]/div[1]/div[1]/div[4]//";

            for (int i = 1; i <= 8; i++)
            {
                var e = driver.FindElement(By.XPath(xpath + "select/option[" + i + "]"));
                driver.click_element(e);
                Console.WriteLine(driver.FindElement(By.XPath(xpath + "div[2]")).Text);
            }           
        }

        [TestMethod]
        public void TestResources()
        {
            IWebElement body = driver.GoToUrl(HomePage, "ApplicationContainer");
            driver.click_text("Resources");
            IList<IWebElement> href = body.FindElements(By.TagName("a"));
            string originalWindow = driver.CurrentWindowHandle;

            for (int i = 0; i < href.Count; i++)
            {
                body = driver.FindElement(By.ClassName("ApplicationContainer"));
                href = body.FindElements(By.TagName("a"));
                string link = href[i].Text;
                driver.click_element(href[i]);
                if (driver.WindowHandles.Count > 1)
                {
                    driver.SwitchTo().Window(driver.WindowHandles[1]);
                    driver.Close();
                    driver.SwitchTo().Window(originalWindow);
                }
                else
                {
                    if (driver.Url == HomePage)
                    {
                        System.Console.WriteLine("Main link:" + link);
                    }
                    driver.Navigate().Back();
                    if (driver.Url == HomePage)
                    {
                        System.Console.WriteLine("Empty link:" + link);
                        driver.Navigate().Forward();
                    }
                }
            }
        }
    
    }
}
