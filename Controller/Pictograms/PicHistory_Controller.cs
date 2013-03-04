using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Task.Model.Pictograms;
using Task.Utils;

namespace Task.Controller.Pictograms
{
    public class PicHistoryController
    {
        private IWebDriver _driver;
        private PicHistoryModel _historyModel;
        //private readonly string _baseUrl = "https://admin.dt00.net/cab/overall-log/show-log/table_id/" + Registry.hashTable["pkId"] + "/table/g_partners_1";
        public List<string> Errors = new List<string>();

        public void ViewHistory()
        {
            GetDriver();
            CheckUrlOnIdPkPresence();
        }

        private void GetDriver()
        {
            _driver = (IWebDriver)Registry.hashTable["driver"]; //забираем из хештаблицы сохраненный ранее драйвер
            _driver.Navigate().GoToUrl(Paths.UrlPicHistory); //заходим по ссылке
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
        }

        private void CheckUrlOnIdPkPresence()
        {
            _historyModel = new PicHistoryModel();
            _historyModel.driver = _driver;

            LogTrace.WriteInLog(_driver.Url);

            string pkIdCurrent = Registry.hashTable["pkId"].ToString();
            if (_driver.Url.Contains(pkIdCurrent))
            {
                if(_historyModel.GetIdPkInTable == pkIdCurrent)
                    return;
                Errors.Add("В поле ввода фильтрации содержится неверный id РК");
            }
            else
            {
                Errors.Add("В URL страницы содержится неверный id РК");
            }
        }
    }
}
