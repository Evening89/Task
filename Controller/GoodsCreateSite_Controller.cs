using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Task.Model;
using Task.Utils;

namespace Task.Controller
{
    public class GoodsCreateSiteController
    {
        private IWebDriver _driver;
        private GoodsCreateSite_Model _siteModel;
        //private readonly string _baseUrl = "https://admin.dt00.net/cab/goodhits/clients-new-site/client_id/" + Registry.hashTable["clientId"]; //берем сохраненный ранее 
                                                                                                                                    //(при создании клиента Task.Controller.GoodsCreateClient_Controller) ID клиента
                                                                                                                                    //и дописываем в URL
        public List<string> Errors = new List<string>(); //список ошибок (в каждой строке - спарсенное со страницы описание ошибки)
        public string SiteId; //переменная для хранения ID только что созданного сайта
        public string ClientId;
        public string SiteName; //переменная для хранения названия только что созданного сайта
        public string SiteDomain; //переменная для хранения доменного имени только что созданного сайта
        readonly Randoms _randoms = new Randoms(); //класс генерации случайных строк

        public void CreateSite(bool setNecessaryFields, bool setUnnecessaryFields)
        {
            GetDriver();
            SetUpFields(setNecessaryFields, setUnnecessaryFields);
            CreationIsSuccessful();
        }

        private void GetDriver()
        {
            _driver = (IWebDriver)Registry.hashTable["driver"]; //забираем из хештаблицы сохраненный при создании клиента драйвер
            _driver.Navigate().GoToUrl(Paths.UrlCreateSite); //заходим по ссылке
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            //ставим ожидание в 10 сек на случай, если страница медленно грузится и нужные эл-ты появятся не сразу
        }

        private void SetUpFields(bool setNecessaryFields, bool setUnnecessaryFields)
        {
            _siteModel = new GoodsCreateSite_Model();
            _siteModel.driver = _driver;

            LogTrace.WriteInLog("     " + _driver.Url);

            #region Required fields
                if (setNecessaryFields) //выбрано заполнение обязательных полей
                {
                    LogTrace.WriteInLog("     ...Заполняю обязательные поля...");
                    SiteDomain = _randoms.RandomString(7) + "." + "ru";
                    _siteModel.Domain = SiteDomain;
                    LogTrace.WriteInLog("     Заполняю поле Домен. Было введено: " + _siteModel.Domain);
                }
            #endregion

            #region Unrequired fields
                if (setUnnecessaryFields) //выбрано заполнение также необязательных полей
                {
                    LogTrace.WriteInLog("     ...Заполняю необязательные поля...");
                    if (needSet())
                    {
                        SiteName = _randoms.RandomString(10);
                        _siteModel.Name = SiteName;
                        LogTrace.WriteInLog("     Заполняю поле Название. Было введено: " + _siteModel.Name);
                    }
                    if (needSet())
                    {
                        _siteModel.Comments = _randoms.RandomString(30);
                        LogTrace.WriteInLog("     Заполняю поле Комментарии. Было введено: " + _siteModel.Comments);
                    }
                    if (needSet())
                    {
                        _siteModel.AddTeasersInSubdomains = true;
                        LogTrace.WriteInLog("     Выбран checkbox Добавлять тизеры на поддомены");
                    }
                    if (needSet())
                    {
                        _siteModel.AllowAddSiteOtherClients = true;
                        LogTrace.WriteInLog("     Выбран checkbox Разрешить добавлять сайт другим клиентам");
                    }
                }
            #endregion
        }

        private void CreationIsSuccessful()
        {
            string createSitetUrl = _driver.Url; //запоминаем url страницы "Добавление сайта"
            _siteModel.Submit(); //пытаемся сохранить форму
            LogTrace.WriteInLog("     Нажал кнопку Сохранить");

            //если текущий и createSitetUrl не совпали - сайт создался и ошибки искать не надо
            if (_driver.Url == createSitetUrl)
                Errors.Add(_siteModel.GetErrors().ToString()); //проверяем, появились ли на форме ошибки заполнения полей
            else
                GetSiteIdFromUrl();
            
            Registry.hashTable["driver"] = _driver;
            LogTrace.WriteInLog("     " + _driver.Url);
        }

        private void GetSiteIdFromUrl()
        {
            char[] slash = new char[] { '/' };
            string[] url = _driver.Url.Split(slash); //разбиваем URL по /
            SiteId = url[url.Length - 1]; //берем последний элемент массива - это id нового клиента
            ClientId = Registry.hashTable["clientId"].ToString(); //берется для вывода в listBox и логи
            Registry.hashTable["siteId"] = SiteId;
        }

        private bool needSet() //генерируем 0 или 1.  1 - заполняем необязательное поле, 0 - не заполняем
        {
            Random rnd = new Random();
            return rnd.Next(0, 2) == 1 ? true : false;
        }
    }
}
