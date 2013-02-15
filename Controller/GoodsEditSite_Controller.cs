using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Task.Model;
using Task.Utils;

namespace Task.Controller
{
    public class GoodsEditSite_Controller
    {
        private IWebDriver _driver;
        private GoodsEditSite_Model _siteEditModel;
        private readonly string _baseUrl = "https://admin.dt00.net/cab/goodhits/sites-edit/id/" + Registry.hashTable["siteId"] + "/filters/%252Fid%252F" + Registry.hashTable["siteId"];
        readonly Randoms _randoms = new Randoms();//класс генерации случайных строк
        private string _siteName;
        private string _comments;

        public List<string> Errors = new List<string>(); //список ошибок
        public bool WasMismatch = false; //наличие/отсутствие несовпадений
        
        public void EditSite()
        {
            GetDriver();
            SetUpFields();
            CreationIsSuccessful();
        }

        private void GetDriver()
        {
            _driver = (IWebDriver)Registry.hashTable["driver"]; //забираем из хештаблицы сохраненный ранее драйвер
            _driver.Navigate().GoToUrl(_baseUrl); //заходим по ссылке
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
        }

        private void SetUpFields()
        {
            _siteEditModel = new GoodsEditSite_Model();
            _siteEditModel.driver = _driver;
            LogTrace.WriteInLog("          " + _driver.Url);

            #region Заполнение
                _siteName = _randoms.RandomString(8) + ".ru";
                _siteEditModel.SiteName = _siteName;
                LogTrace.WriteInLog("          Заполняю поле Название. Было введено: " + _siteEditModel.SiteName);

                _comments = _randoms.RandomString(30);
                _siteEditModel.Comments = _comments;
                LogTrace.WriteInLog("          Заполняю поле Комментарии. Было введено: " + _siteEditModel.Comments);

                _siteEditModel.AddTeasersToSubdomains = true;
                LogTrace.WriteInLog("          Выбран checkbox 'Добавлять тизеры на поддомены'");
                _siteEditModel.AllowAddSiteOtherClients = true;
                LogTrace.WriteInLog("          Выбран checkbox 'Разрешить добавлять сайт другим клиентам'");
            #endregion
        }

        private void CreationIsSuccessful()
        {
            string editSiteUrl = _driver.Url; //запоминаем url страницы

            _siteEditModel.Submit(); //пытаемся сохранить форму
            LogTrace.WriteInLog("          Нажал кнопку Сохранить");
            LogTrace.WriteInLog("");

            //если editSiteUrl и текущий url совпали - мы никуда не перешли и значит есть ошибки заполнения полей
            if (_driver.Url == editSiteUrl)
            {
                LogTrace.WriteInLog("          Не удалось покинуть страницу редактирования сайта");
                Errors.Add("          Не удалось покинуть страницу редактирования сайта");
            }
            else
            {
                LogTrace.WriteInLog("          Сайт успешно отредактирован");
                LogForClickers.WriteInLog("          Сайт успешно отредактирован");
            }
            Registry.hashTable["driver"] = _driver;
            LogTrace.WriteInLog("          " + _driver.Url);
        }
        
        public void CheckEditingSite()
        {
            GetDriver();

            if(!CheckFields())
            {
                LogTrace.WriteInLog("          ОК, всё ранее введенное совпадает с текущими значениями");
                LogForClickers.WriteInLog("          ОК, всё ранее введенное совпадает с текущими значениями");
            }
        }

        private bool CheckFields()
        {
            LogTrace.WriteInLog("          " + _driver.Url);

            #region Проверка заполнения
                if (_siteName == _siteEditModel.GetSiteName) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля Название ({0}) и введенное при редактировании ({1})", _siteEditModel.GetSiteName, _siteName)); }
                else
                {
                    LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Название ({0}) и введенное при редактировании ({1})", _siteEditModel.GetSiteName, _siteName));
                    WasMismatch = true;
                }
                if (_comments == _siteEditModel.GetComments) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля Комментарии ({0}) и введенное при редактировании ({1})", _siteEditModel.GetComments, _comments)); }
                else
                {
                    LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Комментарии ({0}) и введенное при редактировании ({1})", _siteEditModel.GetComments, _comments));
                    WasMismatch = true;
                }
                if (_siteEditModel.GetAddTeasersToSubdomains) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Добавлять тизеры на поддомены' и выбранное при редактировании"); }
                else
                {
                    LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Добавлять тизеры на поддомены' и выбранное при редактировании");
                    WasMismatch = true;
                }
                if (_siteEditModel.GetAllowAddSiteOtherClients) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Разрешить добавлять сайт другим клиентам' и выбранное при редактировании"); }
                else
                {
                    LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Разрешить добавлять сайт другим клиентам' и выбранное при редактировании");
                    WasMismatch = true;
                }
            #endregion

            LogTrace.WriteInLog("          " + _driver.Url);
            LogTrace.WriteInLog("");
            return WasMismatch;
        }
    }
}
