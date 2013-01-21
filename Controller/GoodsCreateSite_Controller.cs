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
    public class GoodsCreateSite_Controller
    {
        public IWebDriver driver;
        public string baseUrl = "https://admin.dt00.net/cab/goodhits/clients-new-site/client_id/" + Registry.hashTable["clientId"]; //берем сохраненный ранее 
                                                                                                                                    //(при создании клиента Task.Controller.GoodsCreateClient_Controller) ID клиента
                                                                                                                                    //и дописываем в URL
        public List<string> errors = new List<string>(); //список ошибок (в каждой строке - спарсенное со страницы описание ошибки)
        public string siteId; //переменная для хранения ID только что созданного сайта
        public string clientId;
        public string siteName; //переменная для хранения названия только что созданного сайта
        public string siteDomain; //переменная для хранения доменного имени только что созданного сайта
        Randoms randoms = new Randoms(); //класс генерации случайных строк

        public void CreateSite(bool setNecessaryFields, bool setUnnecessaryFields)
        {
            driver = (IWebDriver)Registry.hashTable["driver"]; //забираем из хештаблицы сохраненный при создании клиента драйвер
            driver.Navigate().GoToUrl(baseUrl); //заходим по ссылке
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            //ставим ожидание в 10 сек на случай, если страница медленно грузится и нужные эл-ты появятся не сразу
            
            GoodsCreateSite_Model siteModel = new GoodsCreateSite_Model();
            siteModel.driver = driver;

            LogTrace.WriteInLog("     " + driver.Url);

            #region Necessary fields
            if (setNecessaryFields) //выбрано заполнение обязательных полей
            {
                LogTrace.WriteInLog("     ...Заполняю обязательные поля...");
                siteDomain = randoms.RandomString(7) + "." + "ru";
                siteModel.Domain = siteDomain;
                LogTrace.WriteInLog("     Заполняю поле Домен. Было введено: " + siteModel.Domain);
            }
            #endregion
            
            #region Not necessary fields
            if (setUnnecessaryFields) //выбрано заполнение также необязательных полей
            {
                LogTrace.WriteInLog("     ...Заполняю необязательные поля...");
                if (needSet())
                {
                    siteName = randoms.RandomString(10);
                    siteModel.Name = siteName;
                    LogTrace.WriteInLog("     Заполняю поле Название. Было введено: " + siteModel.Name);
                }
                if (needSet())
                {
                    siteModel.Comments = randoms.RandomString(30);
                    LogTrace.WriteInLog("     Заполняю поле Комментарии. Было введено: " + siteModel.Comments);
                }
                if (needSet())
                {
                    siteModel.AddTeasersInSubdomains = true;
                    LogTrace.WriteInLog("     Выбран checkbox Добавлять тизеры на поддомены");
                }
                if (needSet())
                {
                    siteModel.AllowAddSiteOtherClients = true;
                    LogTrace.WriteInLog("     Выбран checkbox Разрешить добавлять сайт другим клиентам");
                }
            }
            #endregion

            string createSitetUrl = driver.Url; //запоминаем url страницы "Добавление сайта"
            siteModel.Submit(); //пытаемся сохранить форму
            LogTrace.WriteInLog("     Нажал кнопку Сохранить");
            string isCreatedSiteUrl = driver.Url; //запоминаем url страницы, открывшейся после нажатия "Завершить"
            //если createSitetUrl и isCreatedSiteUrl совпали - мы никуда не перешли и значит есть ошибки заполнения полей
            //если createSitetUrl и isCreatedSiteUrl не совпали - сайт создался и ошибки искать не надо
            if (createSitetUrl == isCreatedSiteUrl)
            {
                errors.Add(siteModel.GetErrors().ToString()); //проверяем, появились ли на форме ошибки заполнения полей
            }
            else
            {
                //if (errors.Count == 0)
                //если нет ошибок - значит клиент успешно создался и мы перешли на страницу с инфо о созданном клиенте
                //{
                char[] slash = new char[] { '/' };
                string[] url = driver.Url.Split(slash); //разбиваем URL по /
                siteId = url[url.Length - 1]; //берем последний элемент массива - это id нового клиента
                clientId = Registry.hashTable["clientId"].ToString(); //берется для вывода в listBox и логи
                Registry.hashTable["siteId"] = siteId;
                //}
            }
            //Registry.hashTable.Add("driver", driver); //записываем в хештаблицу driver и его состояние, чтобы потом извлечь и использовать его при создании сайта/РК
            Registry.hashTable["driver"] = driver;
            //Registry.hashTable["siteDomain"] = siteDomain;
            LogTrace.WriteInLog("     " + driver.Url);
        }

        protected bool needSet() //генерируем 0 или 1.  1 - заполняем необязательное поле, 0 - не заполняем
        {
            Random rnd = new Random();
            return rnd.Next(0, 2) == 1 ? true : false;
        }
    }
}
