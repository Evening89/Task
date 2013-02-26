using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Task.Model.Pictograms;
using Task.Utils;
using Task.View;

namespace Task.Controller.Pictograms
{
    public class PicClaimForTeaserController
    {
        private IWebDriver _driver;
        private PicClaimForTeaserModel _claimForTeaserModel;
        private readonly string _baseUrl = "https://admin.dt00.net/cab/goodhits/creative-add/id/" + Registry.hashTable["pkId"] + "/filters/%252Fclient_id%252F" + Registry.hashTable["clientId"];
        private readonly Randoms _randoms = new Randoms();//класс генерации случайных строк

        public List<string> Errors = new List<string>(); //список ошибок
        public string ClaimId;
        public string ClientId;
        public string PkId;

        public void ApplyForTeaser()
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
            _claimForTeaserModel = new PicClaimForTeaserModel();
            _claimForTeaserModel.driver = _driver;

            LogTrace.WriteInLog(_driver.Url);

            #region Заполнение полей
                LogTrace.WriteInLog("...Заполняю обязательные поля...");

                int countElementsInList = _claimForTeaserModel.QuantityItemsInList(_claimForTeaserModel.LocatorSites);
                Random rnd = new Random();
                _claimForTeaserModel.Site = rnd.Next(0, countElementsInList);
                LogTrace.WriteInLog("Выбираю в выпадающем списке Сайт. Выбрано: " + _claimForTeaserModel.ChosenSite);

                _claimForTeaserModel.Terms = "тест. не креативить";
                LogTrace.WriteInLog("Заполняю поле Условия. Было введено: " + _claimForTeaserModel.Terms);

                _claimForTeaserModel.NumberOfTeasers = rnd.Next(1, 11).ToString();
                LogTrace.WriteInLog("Заполняю поле 'Кол-во тизеров'. Было введено: " + _claimForTeaserModel.NumberOfTeasers);

                //countElementsInList = _claimForTeaserModel.QuantityItemsInList(_claimForTeaserModel.LocatorPriority);
                // rnd.Next(0, countElementsInList);
                _claimForTeaserModel.Priority = 0;
                LogTrace.WriteInLog("Выбираю в выпадающем списке Приоритет. Выбрано: " + _claimForTeaserModel.ChosenPriority);

                List<IWebElement> inputsPricesPerClick = _driver.FindElements(By.CssSelector("fieldset#fieldset-countries input")).ToList(); //инпуты геотаргетинга
                List<IWebElement> inputsLabelsPricesPerClick = _driver.FindElements(By.CssSelector("fieldset#fieldset-countries p.hint")).ToList(); //сохраняет подписи к инпутам (для логирования)
                string str;
                if (inputsPricesPerClick.Count != 0)
                {
                    if (_driver.PageSource.Contains("Россия") || _driver.PageSource.Contains("Украина"))
                    {
                        List<IWebElement> list = _driver.FindElements(By.CssSelector("div.geo-country-collapsed")).ToList();//список того, на что надо кликнуть чтоб развернуть
                        foreach (var x in list)
                        {
                            x.Click();
                        }
                    }
                    else _claimForTeaserModel.PricePerClickExpand = true;

                    for (int i = 0; i < inputsPricesPerClick.Count; i++)
                    {
                        IWebElement webElement = _driver.FindElement(By.Id(inputsPricesPerClick[i].GetAttribute("id")));
                        str = rnd.Next(1, 11).ToString();
                        webElement.SendKeys(str);
                        LogTrace.WriteInLog(string.Format("Заполняю поле '{0}'. Было введено: {1}", inputsLabelsPricesPerClick[i].Text, str));
                    }
                }

                _claimForTeaserModel.PricePerClick = rnd.Next(1, 11).ToString();
                LogTrace.WriteInLog("Заполняю поле 'Цена за клик на все регионы'. Было введено: " + _claimForTeaserModel.PricePerClick);

                _claimForTeaserModel.ActionWhenSaveClaim = rnd.Next(0, 3);
                LogTrace.WriteInLog("Выбираю radiobutton 'Какое действие выполнить при сохранении заявки?'. Выбрано: " + _claimForTeaserModel.chosenActionWhenSaveClaim);

                if (_claimForTeaserModel.ActionWhenSaveClaim == 1)
                {
                    _claimForTeaserModel.TestCreativist = true;
                    LogTrace.WriteInLog("Выбираю Тестового креативиста");
                    _claimForTeaserModel.NumberOfTeasersPopUpWindow = _claimForTeaserModel.NumberOfTeasers;//rnd.Next(1, 11).ToString();
                    LogTrace.WriteInLog("Заполняю поле 'К-во тизеров'. Было введено: " + _claimForTeaserModel.NumberOfTeasersPopUpWindow);
                    _claimForTeaserModel.ButtonApply = true;
                    LogTrace.WriteInLog("Нажал кнопку Применить");
                }

                if(needSetCheckBox())
                {
                    _claimForTeaserModel.OnlyForEditorsWagers = true;
                    LogTrace.WriteInLog("Выбран checkbox 'Только для редакторов-ставочников'");
                }
            #endregion
        }

        private void CreationIsSuccessful()
        {
            string claimedForTeaserUrl = _driver.Url;
            _claimForTeaserModel.Submit();

            if (_driver.Url == claimedForTeaserUrl)
                Errors = _claimForTeaserModel.GetErrors(); //проверяем, появились ли на форме ошибки заполнения полей
            else
            {
                GetClaimIdFromUrl();
                RefuseClaimForTeaser();
            }
            Registry.hashTable["driver"] = _driver;
        }

        private void GetClaimIdFromUrl()
        {
            string url = _driver.Url;
            char[] slash = new char[] { '/' };
            string[] mas = url.Split(slash); //разбиваем URL по /
            ClaimId = mas[mas.Length - 1]; //берем последний элемент массива - это id новой РК
            ClientId = Registry.hashTable["clientId"].ToString(); //берется для вывода в listBox и логи
            PkId = Registry.hashTable["pkId"].ToString(); //берется для вывода в listBox и логи
            LogTrace.WriteInLog(_driver.Url);
        }

        private void RefuseClaimForTeaser()
        {
            _claimForTeaserModel.RefuseClaimPic = true;
            LogTrace.WriteInLog("Нажал пиктограмму 'Закрыть заявку'");

            _claimForTeaserModel.CauseOfRefusal = "патамушто апельсин";
            LogTrace.WriteInLog("Заполняю поле 'Причина закрытия заявки'. Было введено: " + _claimForTeaserModel.CauseOfRefusal);

            _claimForTeaserModel.RefuseButton = true;
            LogTrace.WriteInLog("Нажал кнопку 'Отклонить заявку'");
        }

        private bool needSetCheckBox() //генерируем 0 или 1.  1 - заполняем необязательное поле, 0 - не заполняем
        {
            Random rnd = new Random();
            return rnd.Next(0, 2) == 1 ? true : false;
        }
    }
}
