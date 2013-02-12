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
        IWebDriver driver;
        public string baseUrl = "https://admin.dt00.net/cab/goodhits/sites-edit/id/" + Registry.hashTable["siteId"] + "/filters/%252Fid%252F" + Registry.hashTable["siteId"];
        Randoms randoms = new Randoms();//класс генерации случайных строк
        public List<string> errors = new List<string>(); //список ошибок

        protected string siteName;
        protected string comments;

        GoodsEditSite_Model siteEditModel = new GoodsEditSite_Model();

        public void EditSite()
        {
            driver = (IWebDriver)Registry.hashTable["driver"]; //забираем из хештаблицы сохраненный ранее драйвер
            driver.Navigate().GoToUrl(baseUrl); //заходим по ссылке
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));

            siteEditModel.driver = driver;

            LogTrace.WriteInLog("          " + driver.Url);

            
            #region Заполнение
                siteName = randoms.RandomString(8) + ".ru";
                siteEditModel.SiteName = siteName;
                LogTrace.WriteInLog("          Заполняю поле Название. Было введено: " + siteEditModel.SiteName);

                comments = randoms.RandomString(30);
                siteEditModel.Comments = comments;
                LogTrace.WriteInLog("          Заполняю поле Комментарии. Было введено: " + siteEditModel.Comments);

                siteEditModel.AddTeasersToSubdomains = true;
                LogTrace.WriteInLog("          Выбран checkbox 'Добавлять тизеры на поддомены'");
                siteEditModel.AllowAddSiteOtherClients = true;
                LogTrace.WriteInLog("          Выбран checkbox 'Разрешить добавлять сайт другим клиентам'");

                string editSiteUrl = driver.Url; //запоминаем url страницы

                siteEditModel.Submit(); //пытаемся сохранить форму
                LogTrace.WriteInLog("          Нажал кнопку Сохранить");
                LogTrace.WriteInLog("          " + driver.Url);
                LogTrace.WriteInLog("");

                string isEditedClientUrl = driver.Url; //запоминаем url страницы, открывшейся после нажатия "Завершить"
                //если editSiteUrl и isEditedClientUrl совпали - мы никуда не перешли и значит есть ошибки заполнения полей
                //если editSiteUrl и isEditedClientUrl не совпали - клиент отредактировался и ошибки искать не надо
                if (editSiteUrl == isEditedClientUrl)
                {
                    LogTrace.WriteInLog("          Не удалось покинуть страницу редактирования сайта");
                    errors.Add("          Не удалось покинуть страницу редактирования сайта");
                }
                else
                {
                    LogTrace.WriteInLog("          Сайт успешно отредактирован");
                    LogForClickers.WriteInLog("          Сайт успешно отредактирован");
                }
                //Registry.hashTable.Add("driver", driver); //записываем в хештаблицу driver и его состояние, чтобы потом извлечь и использовать его при создании сайта/РК
                Registry.hashTable["driver"] = driver;
            #endregion
        }

        public bool wasMismatch = false;

        public void CheckEditingSite()
        {
            driver = (IWebDriver) Registry.hashTable["driver"]; //забираем из хештаблицы сохраненный ранее драйвер
            driver.Navigate().GoToUrl(baseUrl); //заходим по ссылке
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            LogTrace.WriteInLog("          " + driver.Url);

            #region Проверка заполнения
                LogTrace.WriteInLog("          ...Проверка: Название...");

                if (siteName == siteEditModel.GetSiteName) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля Название ({0}) и введенное при редактировании ({1})", siteEditModel.GetSiteName, siteName)); }
                else
                {
                    LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Название ({0}) и введенное при редактировании ({1})", siteEditModel.GetSiteName, siteName));
                    wasMismatch = true;
                }
                if (comments == siteEditModel.GetComments) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля Комментарии ({0}) и введенное при редактировании ({1})", siteEditModel.GetComments, comments)); }
                else
                {
                    LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Комментарии ({0}) и введенное при редактировании ({1})", siteEditModel.GetComments, comments));
                    wasMismatch = true;
                }
                if (siteEditModel.GetAddTeasersToSubdomains) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Добавлять тизеры на поддомены' и выбранное при редактировании"); }
                else
                {
                    LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Добавлять тизеры на поддомены' и выбранное при редактировании");
                    wasMismatch = true;
                }
                if (siteEditModel.GetAllowAddSiteOtherClients) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Разрешить добавлять сайт другим клиентам' и выбранное при редактировании"); }
                else
                {
                    LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Разрешить добавлять сайт другим клиентам' и выбранное при редактировании");
                    wasMismatch = true;
                }
            #endregion

            LogTrace.WriteInLog("          " + driver.Url);
            LogTrace.WriteInLog("");
            if (!wasMismatch)
            {
                LogTrace.WriteInLog("          ОК, всё ранее введенное совпадает с текущими значениями");
                LogForClickers.WriteInLog("          ОК, всё ранее введенное совпадает с текущими значениями");
            }
        }
    }
}
