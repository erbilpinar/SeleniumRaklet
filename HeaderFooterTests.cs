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
    public class HeaderFooterTests
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
            ErrorLogs = Driver.CheckEqual(logo.GetAttribute("src"), "https://hello.raklet.net/images/_shared/logo/color/black/128_.png", "Logo not loaded correctly - " + url, ErrorLogs);
            //*[@class="container-fluid"]//li[1]/a
            IWebElement features = Driver.CheckElementExist(By.XPath("//*[@class=\"container-fluid\"]//li[1]/a"));
            ErrorLogs = Driver.CheckEqual(features.GetAttribute("href"), "https://hello.raklet.net/features/app-store/", "Features not loaded correctly - " + url, ErrorLogs);

            IWebElement references = Driver.CheckElementExist(By.XPath("//*[@class=\"container-fluid\"]//li[2]/a"));
            ErrorLogs = Driver.CheckEqual(references.GetAttribute("href"), "https://hello.raklet.net/customers", "References not loaded correctly - " + url, ErrorLogs);

            IWebElement pricing = Driver.CheckElementExist(By.XPath("//*[@class=\"container-fluid\"]//li[3]/a"));
            ErrorLogs = Driver.CheckEqual(pricing.GetAttribute("href"), "https://hello.raklet.net/pricing/", "Pricing not loaded correctly - " + url, ErrorLogs);

            IWebElement resources = Driver.CheckElementExist(By.XPath("//*[@class=\"container-fluid\"]//li[4]/a"));
            ErrorLogs = Driver.CheckEqual(resources.GetAttribute("href"), "https://hello.raklet.net/knowledge-center/", "Resources not loaded correctly - " + url, ErrorLogs);
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
            ErrorLogs = Driver.CheckEqual(img.GetAttribute("src"), "https://hello.raklet.net/images/_shared/logo/color/white/128.png", "Image not loaded in footer - " + url, ErrorLogs);

            IWebElement p = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-content\"]//*[@class=\"col-sm-4\"]/div/p"));
            ErrorLogs = Driver.CheckEqual(p.Text, "Raklet is a modern cloud platform that provides plug and play solutions for contacts, messsages and payments.", "Text not correct in footer - " + url, ErrorLogs);

            //*[@class="Footer-content"]//*[@class="col-sm-4"]/div/address
            IWebElement address = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-content\"]//*[@class=\"col-sm-4\"]/ div/address"));
            ErrorLogs = Driver.CheckEqual(address.Text, "4347 20th Street, San Francisco CA 94114", "Adress not correct in footer - " + url, ErrorLogs);

            //*[@class="Footer-content"]//*[@class="col-sm-4"]/div/a[2]
            IWebElement phone = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-content\"]//*[@class=\"col-sm-4\"]/div/a[2]"));
            ErrorLogs = Driver.CheckEqual(phone.Text, "+1.415.234.0554", "Phone number not correct in footer - " + url, ErrorLogs);

            //*[@class="Footer-content"]//*[@class="col-sm-4"]/div/a[3]
            IWebElement mail = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-content\"]//*[@class=\"col-sm-4\"]/ div/a[3]"));
            ErrorLogs = Driver.CheckEqual(mail.Text, "hello@raklet.com", "Mail not correct in footer - " + url, ErrorLogs);

            //*[@class="Footer-content"]//*[@class="col-sm-4"]//ul/li[1]/a/img
            IWebElement andorid = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-content\"]//*[@class=\"col-sm-4\"]//ul/li[1]/a/img"));
            ErrorLogs = Driver.CheckEqual(andorid.GetAttribute("src"), "https://hello.raklet.net/images/_shared/android.png", "Andorid logo not loaded in footer - " + url, ErrorLogs);

            //*[@class="Footer-content"]//*[@class="col-sm-4"]//ul/li[2]/a/img
            IWebElement ios = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-content\"]//*[@class=\"col-sm-4\"]//ul/li[2]/a/img"));
            ErrorLogs = Driver.CheckEqual(ios.GetAttribute("src"), "https://hello.raklet.net/images/_shared/ios.png", "IOS logo not loaded in footer - " + url, ErrorLogs);

            //*[@class="Footer-content"]//*[@class="col-sm-4"]//ul/li[1]/a/div
            IWebElement facebook = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-content\"]//*[@class=\"col-sm-4\"]//ul/li[1]/a/div"));

            //*[@class="Footer-content"]//*[@class="col-sm-4"]//ul/li[2]/a/div
            IWebElement twitter = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-content\"]//*[@class=\"col-sm-4\"]//ul/li[2]/a/div"));

            //*[@class="Footer-content"]//*[@class="col-sm-4"]//ul/li[3]/a/div
            IWebElement instagram = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-content\"]//*[@class=\"col-sm-4\"]//ul/li[3]/a/div"));

            //*[@class="Footer-language"]//span
            IWebElement language = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-language\"]//span"));
            ErrorLogs = Driver.CheckEqual(language.Text, "Language:", "Language text not correct in footer - " + url, ErrorLogs);

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
            ErrorLogs = Driver.CheckEqual(e.Text, "English", "Footer language not correct- " + url, ErrorLogs);
            Driver.ClickElement(e);

            for (int i = 0; i < languages.Count; i++)
            {
                IWebElement ee = Driver.CheckElementExist(By.XPath("//translation-selector//li[" + (i + 1) + "]/a"));
                ErrorLogs = Driver.CheckEqual(ee.Text, languages[i], "Footer languages dropdown list not correct - " + url, ErrorLogs);
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
                ErrorLogs = Driver.CheckEqual(col2Names[i - 1], element.Text, "Footer not loaded correctly - " + url, ErrorLogs);
                ErrorLogs = Driver.CheckEqual(col2Href[i - 1], element.GetAttribute("href"), "Footer link not correct for " + element.Text + " - " + url, ErrorLogs);
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
                ErrorLogs = Driver.CheckEqual(col3Names[i - 1], element.Text, "Footer not loaded correctly - " + url, ErrorLogs);
                ErrorLogs = Driver.CheckEqual(col3Href[i - 1], element.GetAttribute("href"), "Footer link not correct for " + element.Text + " - " + url, ErrorLogs);
            }
        }


        public void FooterExtra(string url)
        {
            IWebElement a = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-extras\"]//*[@class=\"alpha\"]"));
            ErrorLogs = Driver.CheckEqual(a.Text, "Made with love", "Extra footer not loaded correctly - " + url, ErrorLogs);

            IWebElement o = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-extras\"]//*[@class=\"omega\"]"));
            ErrorLogs = Driver.CheckEqual(o.Text, "in San Francisco, Berlin & Istanbul", "Extra footer not loaded correctly - " + url, ErrorLogs);

            IWebElement t = Driver.CheckElementExist(By.XPath("//*[@class=\"Footer-extras\"]//*[@class=\"Footer-extrasNav list-inline\"]"));
            ErrorLogs = Driver.CheckEqual(t.Text, "Terms of UsePrivacy Policy©2013-2021", "Extra footer not loaded correctly - " + url, ErrorLogs);
        }
    }
}
