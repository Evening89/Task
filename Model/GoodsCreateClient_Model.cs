using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Task.Model
{
    public class GoodsCreateClient_Model
    {
        public IWebDriver driver;

        #region Necessary fields
            protected string FieldLogin;
            public string Login
            {
                get { return FieldLogin; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("login"));
                    webelement.SendKeys(value);
                    FieldLogin = value;
                }
            }

            protected string FieldPassword;
            public string Password
            {
                get { return FieldPassword; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("password"));
                    webelement.SendKeys(value);
                    FieldPassword = value;
                }
            }

            protected string FieldUserName;
            public string UserName
            {
                get { return FieldUserName; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("name"));
                    webelement.SendKeys(value);
                    FieldUserName = value;
                }
            }

            protected string FieldEmail;
            public string Email
            {
                get { return FieldEmail; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("email"));
                    webelement.SendKeys(value);
                    FieldEmail = value;
                }
            }
        #endregion

        #region Unnecessary fields
            protected string FieldPhone;
            public string Phone
            {
                get { return FieldPhone; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("phoneTextArea"));
                    webelement.SendKeys(value);
                    FieldPhone = value;
                }
            }

            protected string FieldSkype;
            public string Skype
            {
                get { return FieldSkype; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("skype"));
                    webelement.SendKeys(value);
                    FieldSkype = value;
                }
            }

            protected string FieldIcq;
            public string Icq
            {
                get { return FieldIcq; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("icq"));
                    webelement.SendKeys(value);
                    FieldIcq = value;
                }
            }

            protected bool CheckboxExchangeInCabinet;
            public bool ExchangeInCabinet
            {
                get { return CheckboxExchangeInCabinet; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("hide_exchange_flag"));
                    webelement.Click();
                    CheckboxExchangeInCabinet = value;
                }
            }

            protected bool CheckboxNewsInCabinet;
            public bool NewsInCabinet
            {
                get { return CheckboxNewsInCabinet; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("hide_news_flag"));
                    webelement.Click();
                    CheckboxNewsInCabinet = value;
                }
            }

            protected bool CheckboxTestClient;
            public bool TestClient
            {
                get { return CheckboxTestClient; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("is_test"));
                    webelement.Click();
                    CheckboxTestClient = value;
                }
            }

            protected string FieldLimitNumOfCampaigns;
            public string LimitNumOfCampaigns
            {
                get { return FieldLimitNumOfCampaigns; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("goodhits_campaigns_limit"));
                    webelement.SendKeys(value);
                    FieldLimitNumOfCampaigns = value;
                }
            }

            protected bool CheckboxAllowViewFilterByPlatform;
            public bool AllowViewFilterByPlatform
            {
                get { return CheckboxAllowViewFilterByPlatform; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("goodhits_platform_flag"));
                    webelement.Click();
                    CheckboxAllowViewFilterByPlatform = value;
                }
            }

            protected string FieldComments;
            public string Comments
            {
                get { return FieldComments; }
                set
                {
                    IWebElement webelement = driver.FindElement(By.Id("comments"));
                    webelement.SendKeys(value);
                    FieldComments = value;
                }
            }
        #endregion

        public void Submit()
        {
            IWebElement webelement = driver.FindElement(By.Id("save"));
            webelement.Click();
        }

        public string PhoneError()
        {
            try
            {
                IWebElement webelement = driver.FindElement(By.Id("error_image"));
                return webelement.GetAttribute("title");
            }
            catch (Exception)
            {
                return "";
            }
        }

        public List<string> GetErrors()
        {
            List<IWebElement> list = driver.FindElements(By.CssSelector(".errors > li")).ToList(); //проверка, есть ли на странице ошибки заполнения _обязательных_ полей
            List<string> result = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                result.Add(list[i].Text);
            }

            string phoneError = PhoneError();
            if (phoneError != "") result.Add(PhoneError());
            return result;
        }
    }
}
