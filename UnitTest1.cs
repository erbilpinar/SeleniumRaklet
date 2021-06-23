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
        public static IWebDriver driver;

        [TestInitialize]
        public void Initiliaze()
        {
            driver = new ChromeDriver();
            System.Environment.SetEnvironmentVariable("webdriver.chrome.driver", @"/home/ella/PycharmProjects");
=========
>>>>>>>>> Temporary merge branch 2
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
                driver.ClickElement(href[i]);
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
            driver.ClickText("Features");
            TestLinks(body);
        }

        [TestMethod]
        public void TestReferences()
            {
            IWebElement body = driver.GoToUrl(HomePage, "ApplicationContainer");
            driver.ClickText("References");
            TestLinks(body);
                }

        [TestMethod]
        public void TestPricing()
                {
            IWebElement body = driver.GoToUrl(HomePage, "ApplicationContainer");
            driver.ClickText("Pricing");
            TestLinks(body);

            // Store the elements in the combobox
            String xpath = "//*[@class=\"container\"]/div[2]/div[1]/div[1]/div[4]//";
            List<PriceClass> price_options = new List<PriceClass>();
            for (int i = 1; i <= 8; i++)
                    {
                PriceClass option = new PriceClass();
                List<string> values = new List<string>();

                var e = driver.FindElement(By.XPath(xpath + "select/option[" + i + "]"));
                driver.ClickElement(e);

                string temp = (driver.FindElement(By.XPath(xpath + "div[2]")).Text);
                values.Add(temp);

                int[] indexes = { 1, 2, 3, 4, 6, 15 };
                foreach (int index in indexes)
                    {
                    temp = (driver.FindElement(By.XPath(xpath + "div[4]/p[" + index + "]")).Text);
                    int temp_index = temp.IndexOf(":");
                    string var = temp.Substring(temp_index + 2);
                    values.Add(var);
                    }

                option.SetPrice(values[0]);
                option.SetContacts(Convert.ToInt32(values[1]));
                option.SetCustomFields(Convert.ToInt32(values[2]));

                //remove the gb from the storage
                string str = values[3].Substring(0, values[3].Length - 3);
                option.SetStorage(Convert.ToInt32(str));

                //option.SetAdmins(Convert.ToInt32(values[4]));
                //option.SetEmailSender(Convert.ToInt32(values[5]));
                option.SetAdmins(values[4]);
                option.SetEmailSender(values[5]);
                option.SetAPILimits(values[6]);
                price_options.Add(option);
                }
            }
        }

            //Store the elements in the table
            xpath = "//*[@class=\"table\"]";
            List<PriceClass> price_table = new List<PriceClass>();
            for (int i = 1; i <= 8; i++)
        {
                PriceClass option = new PriceClass();
                List<string> values = new List<string>();

                var e = driver.FindElement(By.XPath(xpath + "/thead/tr/th[5]/div/select/option[" + i + "]"));
                driver.ClickElement(e);

                int[] indexes = { 1, 3, 6, 45, 4, 9, 47 };
                foreach (int index in indexes)
            {
                    string temp = (driver.FindElement(By.XPath(xpath + "/tbody/tr[" + index + "]/td[5]")).Text);
                    values.Add(temp);
                }

                option.SetPrice(values[0].Replace(",", ""));
                option.SetContacts(Convert.ToInt32(values[1].Replace(",", "")));
                option.SetCustomFields(Convert.ToInt32(values[2]));

                //remove the gb from the storage
                string var = values[3].Substring(0, values[3].Length - 3);
                option.SetStorage(Convert.ToInt32(var));

                //option.SetAdmins(Convert.ToInt32(values[4]));
                //option.SetEmailSender(Convert.ToInt32(values[5]));
                option.SetAdmins(values[4]);
                option.SetEmailSender(values[5]);
                option.SetAPILimits(values[6].Replace(",", ""));
                price_table.Add(option);
                    }

            //Compare the values
            for (int i = 0; i < 8; i++)
                    {
                PriceClass combobox = price_options[i];
                PriceClass table = price_table[i];

                CheckEqual(table.GetPrice(), combobox.GetPrice(), "Prices are not equal in selection: " + i);
                CheckEqual(table.GetContacts(), combobox.GetContacts(), "Contact numbers are not equal in selection: " + i);
                CheckEqual(table.GetCustomFields(), combobox.GetCustomFields(), "Custom Field numbers are not equal in selection: " + i);
                CheckEqual(table.GetStorage(), combobox.GetStorage(), "Storages are not equal in selection: " + i);
                CheckEqual(table.GetAdmins(), combobox.GetAdmins(), "Admin numbers are not equal in selection: " + i);
                CheckEqual(table.GetEmailSender(), combobox.GetEmailSender(), "Email sender numbers are not equal in selection: " + i);
                CheckEqual(table.GetAPILimits(), combobox.GetAPILimits(), "API limits are not equal in selection: " + i);

            }
        }

        [TestMethod]
        public void TestResources()
        {
            IWebElement body = driver.GoToUrl(HomePage, "ApplicationContainer");
            driver.ClickText("Resources");
            TestLinks(body);
        }


        public void TestLinks(IWebElement body)
        {
            IList<IWebElement> href = body.FindElements(By.TagName("a"));
            string originalWindow = driver.CurrentWindowHandle;

            for (int i = 0; i < href.Count; i++)
            {
                body = driver.FindElement(By.ClassName("ApplicationContainer"));
                href = body.FindElements(By.TagName("a"));
                string link = href[i].Text;
                driver.ClickElement(href[i]);
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
                driver.ClickElement(e);
                Console.WriteLine(driver.FindElement(By.XPath(xpath + "div[2]")).Text);
            }           
        }

        public void CheckEqual<T>(T expected, T actual, string message)
                {
            try
                    {
                Assert.AreEqual(expected, actual);
                    }
            catch (AssertFailedException)
                    {
                Console.WriteLine(message + " expected: " + expected + " actual: " + actual);
            }
        }
    
    }
}
