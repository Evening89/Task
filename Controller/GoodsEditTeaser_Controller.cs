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
        private IWebDriver _driver;
        private int _countElementsInList;
        private int _newCategory;
        private int _newCurrency;
        private string _allowedDomain;
        private GoodsEditTeaser_Model _teaserEditModel = new GoodsEditTeaser_Model();

        public List<string> Errors = new List<string>(); //список ошибок (в каждой строке - спарсенное со страницы описание ошибки)
        readonly Randoms _randoms = new Randoms();//класс генерации случайных строк

        public string Link;
        public string Title;
        public string AdvertText;
        public string AttachFile;
        public string PriceForGoodsService;
        public bool WasMismatch = false;

        public void EditTeaser()
        {
            GetDriver();
            SetUpFields();
            EditingIsSuccessful();
        }

        private void GetDriver()
        {
            _driver = (IWebDriver)Registry.hashTable["driver"]; //забираем из хештаблицы сохраненный ранее драйвер
            _allowedDomain = GetAnyAllowedDomain();//сначала выбираем 1 из разрешенных доменов
            _driver.Navigate().GoToUrl(Paths.UrlEditTeaser); //заходим по ссылке
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
        }

        private void SetUpFields()
        {
            _teaserEditModel.driver = _driver;

            LogTrace.WriteInLog(Goods_View.tab3 + _driver.Url);

            #region Редактирование полей
                #region Required fields
                    Thread.Sleep(5000);

                    LogTrace.WriteInLog(Goods_View.tab3 + "...Заполняю обязательные поля...");

                    Link = _teaserEditModel.Link = _allowedDomain;
                    LogTrace.WriteInLog(Goods_View.tab3 + "Заполняю поле Ссылка. Было введено: " + _teaserEditModel.Link);

                    Title = _teaserEditModel.Title = _randoms.RandomString(10);
                    LogTrace.WriteInLog(Goods_View.tab3 + "Заполняю поле Заголовок. Было введено: " + _teaserEditModel.Title);

                    _countElementsInList = _teaserEditModel.QuantityItemsInList(_teaserEditModel.locatorCategory);
                    Random rand = new Random();
                    _newCategory = rand.Next(1, _countElementsInList);
                    _teaserEditModel.Category = _newCategory;
                    LogTrace.WriteInLog(Goods_View.tab3 + "Работаю с выпадающим списком Категория. Было выбрано: " + _teaserEditModel.chosenCategory);

                    AdvertText = _teaserEditModel.AdvertText = _randoms.RandomString(20);
                    LogTrace.WriteInLog(Goods_View.tab3 + "Заполняю поле Рекламный текст. Было введено: " + _teaserEditModel.AdvertText);

                    //string dir = Directory.GetCurrentDirectory();
                    //teaserEditModel.AttachFile = dir + @"\zaj.jpg";
                    //Thread.Sleep(5000);
                    //LogTrace.WriteInLog(Goods_View.tab3 + "Работаю с полем загрузки Фото. Было загружено: " + teaserEditModel.AttachFile);
                    //attachFile = driver.FindElement(By.Id("imageLink")).GetAttribute("value");
                #endregion

                #region Unrequired fields
                    LogTrace.WriteInLog(Goods_View.tab3 + "...Заполняю необязательные поля...");

                    //teaserEditModel.TeaserWomen = true;
                    //LogTrace.WriteInLog(Goods_View.tab2 + "Выбран checkbox Тизер женской тематики (если таковая есть для выбранной Категории)");

                    _countElementsInList = _teaserEditModel.QuantityItemsInList(_teaserEditModel.locatorCurrency);
                    Random rnd = new Random();
                    _newCurrency = rand.Next(0, _countElementsInList);
                    _teaserEditModel.Currency = _newCurrency;
                    LogTrace.WriteInLog(Goods_View.tab3 + "Работаю с выпадающим списком Валюта. Было выбрано: " + _teaserEditModel.chosenCurrency);

                    rnd = new Random();
                    int price = rnd.Next(1, 11);
                    PriceForGoodsService = price.ToString();
                    if (PriceForGoodsService.StartsWith("0"))
                    {
                        Regex regex = new Regex(@"^[0]*");
                        Match match = regex.Match(PriceForGoodsService);
                        if (match.Success) PriceForGoodsService = regex.Replace(PriceForGoodsService, "");
                    }
                    _teaserEditModel.PriceForGoodsService = PriceForGoodsService;
                    LogTrace.WriteInLog(Goods_View.tab3 + "Заполняю поле Цена товара/услуги. Было введено: " + _teaserEditModel.PriceForGoodsService);
                #endregion
            #endregion
        }

        private void EditingIsSuccessful()
        {
            string editTeaserUrl = _driver.Url; //запоминаем url страницы "Добавление тизера"
            _teaserEditModel.Submit(); //пытаемся сохранить форму
            LogTrace.WriteInLog(Goods_View.tab3 + "Нажал кнопку Сохранить");

            //если editTeaserUrl и текущий url совпали - мы никуда не перешли и значит есть ошибки заполнения полей
            if (_driver.Url == editTeaserUrl)
            {
                Errors = _teaserEditModel.GetErrors(); //проверяем, появились ли на форме ошибки заполнения полей
            }
            else
            {
                LogTrace.WriteInLog(Goods_View.tab3 + "Тизер успешно отредактирован");
                LogForClickers.WriteInLog(Goods_View.tab3 + "Тизер успешно отредактирован");
            }
            Registry.hashTable["driver"] = _driver;
        }

        public void CheckEditingTeaser()
        {
            GetDriver();

            if (!CheckFields())
            {
                LogTrace.WriteInLog("          ОК, всё ранее введенное совпадает с текущими значениями");
                LogForClickers.WriteInLog("          ОК, всё ранее введенное совпадает с текущими значениями");
            }
        }

        private bool CheckFields()
        {
            LogTrace.WriteInLog(Goods_View.tab3 + _driver.Url);

            #region Проверка редактирования

                #region Проверить обязательные поля
                    LogTrace.WriteInLog(Goods_View.tab3 + "...Проверка: Обязательные поля...");

                    if (Link == _teaserEditModel.GetLink) { LogTrace.WriteInLog(string.Format("{0}Совпадают: содержимое поля Ссылка ({1}) и введенное при редактировании ({2})", Goods_View.tab3, _teaserEditModel.GetLink, Link)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Ссылка ({0}) и введенное при редактировании ({1})", _teaserEditModel.GetLink, Link));
                        WasMismatch = true;
                    }

                    if (Title == _teaserEditModel.GetTitle) { LogTrace.WriteInLog(string.Format("{0}Совпадают: содержимое поля Заголовок ({1}) и введенное при редактировании ({2})", Goods_View.tab3, _teaserEditModel.GetTitle, Title)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Заголовок ({0}) и введенное при редактировании ({1})", _teaserEditModel.GetTitle, Title));
                        WasMismatch = true;
                    }

                    if (_teaserEditModel.chosenCategory == _teaserEditModel.GetCategory) { LogTrace.WriteInLog(string.Format("{0}Совпадают: содержимое поля Категория ({1}) и введенное при редактировании ({2})", Goods_View.tab3, _teaserEditModel.GetCategory, _teaserEditModel.chosenCategory)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Категория ({0}) и введенное при редактировании ({1})", _teaserEditModel.GetCategory, _teaserEditModel.chosenCategory));
                        WasMismatch = true;
                    }

                    if (AdvertText == _teaserEditModel.GetAdvertText) { LogTrace.WriteInLog(string.Format("{0}Совпадают: содержимое поля Рекламный текст ({1}) и введенное при редактировании ({2})", Goods_View.tab3, _teaserEditModel.GetAdvertText, AdvertText)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Рекламный текст ({0}) и введенное при редактировании ({1})", _teaserEditModel.GetAdvertText, AdvertText));
                        WasMismatch = true;
                    }

                    //if (attachFile == teaserEditModel.GetAttachFile) { LogTrace.WriteInLog(string.Format("{0}Совпадают: содержимое поля Фото ({1}) и введенное при редактировании ({2})", Goods_View.tab3, teaserEditModel.GetAttachFile, attachFile)); }
                    //else
                    //{
                    //    LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Фото ({0}) и введенное при редактировании ({1})", teaserEditModel.GetAttachFile, advertText));
                    //    wasMismatch = true;
                    //}
                #endregion

                #region Проверить необязательные поля
                    LogTrace.WriteInLog(Goods_View.tab3 + "...Проверка: Необязательные поля...");

                    if (_teaserEditModel.chosenCurrency == _teaserEditModel.GetCurrency) { LogTrace.WriteInLog(string.Format("{0}Совпадают: содержимое поля Валюта ({1}) и введенное при редактировании ({2})", Goods_View.tab3, _teaserEditModel.GetCurrency, _teaserEditModel.chosenCurrency)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Валюта ({0}) и введенное при редактировании ({1})", _teaserEditModel.GetCurrency, _teaserEditModel.chosenCurrency));
                        WasMismatch = true;
                    }

                    if (PriceForGoodsService == _teaserEditModel.GetPriceForGoodsService) { LogTrace.WriteInLog(string.Format("{0}Совпадают: содержимое поля Цена товара/услуги ({1}) и введенное при редактировании ({2})", Goods_View.tab3, _teaserEditModel.GetPriceForGoodsService, PriceForGoodsService)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Цена товара/услуги ({0}) и введенное при редактировании ({1})", _teaserEditModel.GetPriceForGoodsService, PriceForGoodsService));
                        WasMismatch = true;
                    }
                #endregion
            #endregion

            LogTrace.WriteInLog(Goods_View.tab3 + _driver.Url);
            LogTrace.WriteInLog("");
            return WasMismatch;
        }

        protected string GetAnyAllowedDomain() //функция, возвращающая 1 из разрешенных доменов для данного клиента
        {
            string domain;
            _driver.Navigate().GoToUrl(Paths.GetAnyAllowedDomain); //идем на страницу с сайтами для данного клиента
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            List<IWebElement> listDomains = _driver.FindElements(By.CssSelector("td[nowrap='nowrap']>a")).ToList(); //получаем список элементов-доменов, из которых нужно извлечь ссылку

            Random rnd = new Random();
            int randomIndex = rnd.Next(0, listDomains.Count); //генерируем индекс для списка элементов-доменов
            domain = listDomains[randomIndex].GetAttribute("href"); //извлекаем ссылку
            return domain;
        }

        
    }
}
