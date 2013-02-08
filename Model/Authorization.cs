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
        private IWebDriver _driver;

        public Authorization(IWebDriver driver, string login, string password)
        {
            _driver = driver;
            Login = login;
            Password = password;
        }

        protected string FieldLogin;
        public string Login
        {
            get { return FieldLogin; }
            set
            {
                IWebElement webelement = _driver.FindElement(By.Id("signinLogin"));
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
                IWebElement webelement = _driver.FindElement(By.Id("signinPassword"));
                webelement.SendKeys(value);
                FieldPassword = value;
            }
        }

        public void Submit()
        {
            IWebElement webelement = _driver.FindElement(By.Id("signin"));
            webelement.Click();
        }
    }
}
