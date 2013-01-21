using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Task.Model
{
    public class GoodsCreateSite_Model
    {
        public IWebDriver driver;

        #region Necessary fields
        protected string FieldDomain;
        public string Domain
        {
            get { return FieldDomain; }
            set
            {
                IWebElement webelement = driver.FindElement(By.Id("domain"));
                webelement.SendKeys(value);
                FieldDomain = value;
            }
        }
        #endregion

        #region Unnecessary fields
        protected string FieldName;
        public string Name
        {
            get { return FieldName; }
            set
            {
                IWebElement webelement = driver.FindElement(By.Id("name"));
                webelement.SendKeys(value);
                FieldName = value;
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

        protected bool CheckboxAddTeasersInSubdomains;
        public bool AddTeasersInSubdomains
        {
            get { return CheckboxAddTeasersInSubdomains; }
            set
            {
                IWebElement webelement = driver.FindElement(By.Id("sites_statuses-add_goods_on_subdomains"));
                webelement.Click();
                CheckboxAddTeasersInSubdomains = value;
            }
        }

        protected bool CheckboxAllowAddSiteOtherClients;
        public bool AllowAddSiteOtherClients
        {
            get { return CheckboxAllowAddSiteOtherClients; }
            set
            {
                IWebElement webelement = driver.FindElement(By.Id("allow_other_clients"));
                webelement.Click();
                CheckboxAllowAddSiteOtherClients = value;
            }
        }
        #endregion

        public void Submit()
        {
            IWebElement webelement = driver.FindElement(By.Id("save"));
            webelement.Click();
        }

        public List<string> GetErrors()
        {
            List<IWebElement> list = driver.FindElements(By.CssSelector(".errors > li")).ToList(); //проверка, есть ли на странице ошибки заполнения _обязательных_ полей
            List<string> result = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                result.Add(list[i].Text);
            }
            return result;
        }
    }
}
