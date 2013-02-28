using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Task.Model.Pictograms
{
    public class PicHistoryModel
    {
        public IWebDriver driver;

        protected string GetFieldIdPkInTable;
        public string GetIdPkInTable
        {
            get
            {
                IWebElement webelement = driver.FindElement(By.Id("table_id"));
                GetFieldIdPkInTable = webelement.GetAttribute("value");
                return GetFieldIdPkInTable;
            }
            set
            {
                GetFieldIdPkInTable = value;
            }
        }
    }
}
