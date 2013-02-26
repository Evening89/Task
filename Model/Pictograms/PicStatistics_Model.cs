using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Task.Utils;

namespace Task.Model.Pictograms
{
    public class PicStatisticsModel
    {
        public IWebDriver driver;

        protected bool ClickButtonExportData;
        public bool ButtonExportData
        {
            get { return ClickButtonExportData; }
            set
            {
                ClickButtonExportData = value;
                IWebElement webelement = driver.FindElement(By.CssSelector("a[href *= '/cab/goodhits/campaigns-stat-export/campaign_id/" + Registry.hashTable["pkId"] + "']"));
                webelement.Click();
            }
        }
    }
}
