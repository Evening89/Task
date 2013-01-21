using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Task.Model
{
    public class Authorization
    {
        //protected string URL = "https://admin.dt00.net/cab/";

        public IWebDriver driver;

        protected string FieldLogin;
        public string Login
        {
            get { return FieldLogin; }
            set
            {
                IWebElement webelement = driver.FindElement(By.Id("signinLogin"));
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
                IWebElement webelement = driver.FindElement(By.Id("signinPassword"));
                webelement.SendKeys(value);
                FieldPassword = value;
            }
        }

        public void Submit()
        {
            IWebElement webelement = driver.FindElement(By.Id("signin"));
            webelement.Click();
        }
    }
}
