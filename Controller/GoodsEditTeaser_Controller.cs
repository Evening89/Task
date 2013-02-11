using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Task.Utils;
using Task.Model;
using Task.View;

namespace Task.Controller
{
    public class GoodsEditTeaser_Controller
    {
        IWebDriver driver;
        
        public string baseUrl = "https://" + Registry.hashTable["Login"] + ":" + Registry.hashTable["Password"] + "@" + "admin.dt00.net/cab/goodhits/ghits-edit/id/" + Registry.hashTable["teaserId"] + "/filters/%252Fcampaign_id%252F" + Registry.hashTable["pkId"];

        public List<string> errors = new List<string>(); //список ошибок (в каждой строке - спарсенное со страницы описание ошибки)
        Randoms randoms = new Randoms();//класс генерации случайных строк

        public string link;
        public string title;
        public string advertText;
        public string attachFile;
        public string priceForGoodsService;
        
        public string allowedDomain;

        GoodsEditTeaser_Model teaserEditModel = new GoodsEditTeaser_Model();

        public void EditTeaser()
        {
            driver = (IWebDriver)Registry.hashTable["driver"]; //забираем из хештаблицы сохраненный ранее драйвер
            allowedDomain = GetAnyAllowedDomain();//сначала выбираем 1 из разрешенных доменов
            driver.Navigate().GoToUrl(baseUrl); //заходим по ссылке
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));

            teaserEditModel.driver = driver;

            LogTrace.WriteInLog(Goods_View.tab3 + driver.Url);

            #region Редактирование полей
                #region Required fields
                    Thread.Sleep(5000);

                    LogTrace.WriteInLog(Goods_View.tab3 + "...Заполняю обязательные поля...");

                    link = teaserEditModel.Link = allowedDomain;
                    LogTrace.WriteInLog(Goods_View.tab3 + "Заполняю поле Ссылка. Было введено: " + teaserEditModel.Link);

                    title = teaserEditModel.Title = randoms.RandomString(10);
                    LogTrace.WriteInLog(Goods_View.tab3 + "Заполняю поле Заголовок. Было введено: " + teaserEditModel.Title);

                    Random rnd = new Random();
                    int category = rnd.Next(1, 31);
                    teaserEditModel.Category = category;
                    LogTrace.WriteInLog(Goods_View.tab3 + "Работаю с выпадающим списком Категория. Было выбрано: " + teaserEditModel.chosenCategory);

                    advertText = teaserEditModel.AdvertText = randoms.RandomString(20);
                    LogTrace.WriteInLog(Goods_View.tab3 + "Заполняю поле Рекламный текст. Было введено: " + teaserEditModel.AdvertText);

                    string dir = Directory.GetCurrentDirectory();
                    teaserEditModel.AttachFile = dir + @"\zaj.jpg";
                    Thread.Sleep(5000);
                    LogTrace.WriteInLog(Goods_View.tab3 + "Работаю с полем загрузки Фото. Было загружено: " + teaserEditModel.AttachFile);
                    attachFile = driver.FindElement(By.Id("imageLink")).GetAttribute("value");
                #endregion

                #region Unrequired fields
                    LogTrace.WriteInLog(Goods_View.tab3 + "...Заполняю необязательные поля...");

                    //teaserEditModel.TeaserWomen = true;
                    //LogTrace.WriteInLog(Goods_View.tab2 + "Выбран checkbox Тизер женской тематики (если таковая есть для выбранной Категории)");
                   
                    rnd = new Random();
                    int currency = rnd.Next(0, 6);
                    teaserEditModel.Currency = currency;
                    LogTrace.WriteInLog(Goods_View.tab3 + "Работаю с выпадающим списком Валюта. Было выбрано: " + teaserEditModel.chosenCurrency);
                    
                    rnd = new Random();
                    int price = rnd.Next(1, 11);
                    priceForGoodsService = price.ToString();
                    if (priceForGoodsService.StartsWith("0"))
                    {
                        Regex regex = new Regex(@"^[0]*");
                        Match match = regex.Match(priceForGoodsService);
                        if (match.Success) priceForGoodsService = regex.Replace(priceForGoodsService, "");
                    }
                    teaserEditModel.PriceForGoodsService = priceForGoodsService;
                    LogTrace.WriteInLog(Goods_View.tab3 + "Заполняю поле Цена товара/услуги. Было введено: " + teaserEditModel.PriceForGoodsService);
                #endregion
            #endregion

            string createTeaserUrl = driver.Url; //запоминаем url страницы "Добавление тизера"
            //Thread.Sleep(5000);
            teaserEditModel.Submit(); //пытаемся сохранить форму
            //Thread.Sleep(5000);
            LogTrace.WriteInLog(Goods_View.tab3 + "Нажал кнопку Сохранить");
            string isCreatedTeaserUrl = driver.Url; //запоминаем url страницы, открывшейся после нажатия "Cохранить"
            //если createSitetUrl и isCreatedSiteUrl совпали - мы никуда не перешли и значит есть ошибки заполнения полей
            //если createSitetUrl и isCreatedSiteUrl не совпали - сайт создался и ошибки искать не надо
            if (createTeaserUrl == isCreatedTeaserUrl)
            {
                errors = teaserEditModel.GetErrors(); //проверяем, появились ли на форме ошибки заполнения полей
            }
            else
            {
                LogTrace.WriteInLog(Goods_View.tab3 + "Тизер успешно отредактирован");
                LogForClickers.WriteInLog(Goods_View.tab3 + "Тизер успешно отредактирован");
            }
            Registry.hashTable["driver"] = driver;
        }

        public bool wasMismatch = false;

        public void CheckEditingTeaser()
        {
            driver = (IWebDriver) Registry.hashTable["driver"];
                //забираем из хештаблицы сохраненный при создании клиента драйвер
            driver.Navigate().GoToUrl(baseUrl); //заходим по ссылке
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            LogTrace.WriteInLog(Goods_View.tab3 + driver.Url);

            #region Проверка заполнения

                #region Проверить обязательные поля
                    LogTrace.WriteInLog(Goods_View.tab3 + "...Проверка: Обязательные поля...");

                    if (link == teaserEditModel.GetLink) { LogTrace.WriteInLog(Goods_View.tab3 + "Совпадают: содержимое поля Ссылка (" + teaserEditModel.GetLink + ") и введенное при редактировании (" + link + ")" ); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: содержимое поля Ссылка (" + teaserEditModel.GetLink + ") и введенное при редактировании (" + link + ")");
                        wasMismatch = true;
                    }

                    if (title == teaserEditModel.GetTitle) { LogTrace.WriteInLog(Goods_View.tab3 + "Совпадают: содержимое поля Заголовок (" + teaserEditModel.GetTitle + ") и введенное при редактировании (" + title + ")"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: содержимое поля Заголовок (" + teaserEditModel.GetTitle + ") и введенное при редактировании (" + title + ")");
                        wasMismatch = true;
                    }

                    if (teaserEditModel.chosenCategory == teaserEditModel.GetCategory) { LogTrace.WriteInLog(Goods_View.tab3 + "Совпадают: содержимое поля Категория (" + teaserEditModel.GetCategory + ") и введенное при редактировании (" + teaserEditModel.chosenCategory + ")"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: содержимое поля Категория (" + teaserEditModel.GetCategory + ") и введенное при редактировании (" + teaserEditModel.chosenCategory + ")");
                        wasMismatch = true;
                    }

                    if (advertText == teaserEditModel.GetAdvertText) { LogTrace.WriteInLog(Goods_View.tab3 + "Совпадают: содержимое поля Рекламный текст (" + teaserEditModel.GetAdvertText + ") и введенное при редактировании (" + advertText + ")"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: содержимое поля Рекламный текст (" + teaserEditModel.GetAdvertText + ") и введенное при редактировании (" + advertText + ")");
                        wasMismatch = true;
                    }

                    if (attachFile == teaserEditModel.GetAttachFile) { LogTrace.WriteInLog(Goods_View.tab3 + "Совпадают: содержимое поля Фото (" + teaserEditModel.GetAttachFile + ") и введенное при редактировании (" + attachFile + ")"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: содержимое поля Фото (" + teaserEditModel.GetAttachFile + ") и введенное при редактировании (" + advertText + ")");
                        wasMismatch = true;
                    }
                #endregion

                #region Проверить необязательные поля
                    LogTrace.WriteInLog(Goods_View.tab3 + "...Проверка: Необязательные поля...");

                    if (teaserEditModel.chosenCurrency == teaserEditModel.GetCurrency) { LogTrace.WriteInLog(Goods_View.tab3 + "Совпадают: содержимое поля Валюта (" + teaserEditModel.GetCurrency + ") и введенное при редактировании (" + teaserEditModel.chosenCurrency + ")"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: содержимое поля Валюта (" + teaserEditModel.GetCurrency + ") и введенное при редактировании (" + teaserEditModel.chosenCurrency + ")");
                        wasMismatch = true;
                    }

                    if (priceForGoodsService == teaserEditModel.GetPriceForGoodsService) { LogTrace.WriteInLog(Goods_View.tab3 + "Совпадают: содержимое поля Цена товара/услуги (" + teaserEditModel.GetPriceForGoodsService + ") и введенное при редактировании (" + priceForGoodsService + ")"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: содержимое поля Цена товара/услуги (" + teaserEditModel.GetPriceForGoodsService + ") и введенное при редактировании (" + priceForGoodsService + ")");
                        wasMismatch = true;
                    }
                #endregion
            #endregion

            LogTrace.WriteInLog(Goods_View.tab3 + driver.Url);
            LogTrace.WriteInLog("");
            if (!wasMismatch)
            {
                LogTrace.WriteInLog(Goods_View.tab3 + "ОК, всё ранее введенное совпадает с текущими значениями");
                LogForClickers.WriteInLog(Goods_View.tab3 + "ОК, всё ранее введенное совпадает с текущими значениями");
            }
        }

        protected string GetAnyAllowedDomain() //функция, возвращающая 1 из разрешенных доменов для данного клиента
        {
            string domain;
            driver.Navigate().GoToUrl("https://admin.dt00.net/cab/goodhits/sites/client_id/" + Registry.hashTable["clientId"]); //идем на страницу с сайтами для данного клиента
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            List<IWebElement> listDomains = driver.FindElements(By.CssSelector("td[nowrap='nowrap']>a")).ToList(); //получаем список элементов-доменов, из которых нужно извлечь ссылку

            Random rnd = new Random();
            int randomIndex = rnd.Next(0, listDomains.Count); //генерируем индекс для списка элементов-доменов
            domain = listDomains[randomIndex].GetAttribute("href"); //извлекаем ссылку
            return domain;
        }
    }
}
