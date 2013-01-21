using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Task.Model
{
    public class GoodsEditSite_Model
    {
        public IWebDriver driver;

        #region Заполнение полей
            protected string FieldSiteName;
            public string SiteName
            {
                get { return FieldSiteName; }
                set
                {
                    FieldSiteName = value;
                    IWebElement webelement = driver.FindElement(By.Id("name"));
                    webelement.Clear();
                    webelement.SendKeys(value);
                }
            }

            protected string FieldComments;
            public string Comments
            {
                get { return FieldComments; }
                set
                {
                    FieldComments = value;
                    IWebElement webelement = driver.FindElement(By.Id("comments"));
                    webelement.Clear();
                    webelement.SendKeys(value);
                }
            }

            protected bool CheckboxAddTeasersToSubdomains;
            public bool AddTeasersToSubdomains
            {
                get { return CheckboxAddTeasersToSubdomains; }
                set
                {
                    CheckboxAddTeasersToSubdomains = value;
                    IWebElement webelement = driver.FindElement(By.Id("sites_statuses-add_goods_on_subdomains"));
                    webelement.Click();
                }
            }

            protected bool CheckboxAllowAddSiteOtherClients;
            public bool AllowAddSiteOtherClients
            {
                get { return CheckboxAllowAddSiteOtherClients; }
                set
                {
                    CheckboxAllowAddSiteOtherClients = value;
                    IWebElement webelement = driver.FindElement(By.Id("allow_other_clients"));
                    webelement.Click();
                }
            }
        #endregion

        #region Проверка заполнения
            protected string GetFieldSiteName;
            public string GetSiteName
            {
                get
                {
                    IWebElement webelement = driver.FindElement(By.Id("name"));
                    GetFieldSiteName = webelement.GetAttribute("value");
                    return GetFieldSiteName;
                }
                set
                {
                    GetFieldSiteName = value;
                }
            }

            protected string GetFieldComments;
            public string GetComments
            {
                get
                {
                    IWebElement webelement = driver.FindElement(By.Id("comments"));
                    GetFieldComments = webelement.GetAttribute("value");
                    return GetFieldComments;
                }
                set
                {
                    GetFieldComments = value;
                }
            }

            protected bool GetCheckboxAddTeasersToSubdomains;
            public bool GetAddTeasersToSubdomains
            {
                get
                {
                    IWebElement webelement = driver.FindElement(By.Id("sites_statuses-add_goods_on_subdomains"));
                    try
                    {
                        string _checked = webelement.GetAttribute("checked");
                        return true; //есть атрибут checked--значит чекбокс выбран
                    }
                    catch (Exception) { return false; } //нет атрибута checked--значит чекбокс не выбран
                }
                set { GetCheckboxAddTeasersToSubdomains = value; }
            }

            protected bool GetCheckboxAllowAddSiteOtherClients;
            public bool GetAllowAddSiteOtherClients
            {
                get
                {
                    IWebElement webelement = driver.FindElement(By.Id("allow_other_clients"));
                    try
                    {
                        string _checked = webelement.GetAttribute("checked");
                        return true; //есть атрибут checked--значит чекбокс выбран
                    }
                    catch (Exception) { return false; } //нет атрибута checked--значит чекбокс не выбран
                }
                set { GetCheckboxAllowAddSiteOtherClients = value; }
            }
        #endregion

        public void Submit()
        {
            try
            {
                IWebElement webelement = driver.FindElement(By.Id("submit"));
                webelement.Click();
            }
            catch (Exception)
            { }
        }
    }
}
