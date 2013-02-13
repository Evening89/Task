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
    public class PicClaimForTeaserController
    {
        private IWebDriver _driver;
        private PicClaimForTeaserModel _claimForTeaserModel;
        private readonly string _baseUrl = "https://admin.dt00.net/cab/goodhits/creative-add/id/" + Registry.hashTable["pkId"] + "/filters/%252Fclient_id%252F" + Registry.hashTable["clientId"];
        private readonly Randoms _randoms = new Randoms();//класс генерации случайных строк

        public List<string> Errors = new List<string>(); //список ошибок
        private int _siteInDropDownList;
        private int _priorityInDropDownList;
        private string _terms;
        private string _numberOfTeasers;
        private string _pricePerClick;
        private int _actionWhenSaveClaim;
        private string _numberOfTeasersPopUpWindow;

        public void ApplyForTeaser()
        {
            _driver = (IWebDriver)Registry.hashTable["driver"]; //забираем из хештаблицы сохраненный ранее драйвер
            _driver.Navigate().GoToUrl(_baseUrl); //заходим по ссылке
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));

            _claimForTeaserModel = new PicClaimForTeaserModel();
            _claimForTeaserModel.driver = _driver;

            LogTrace.WriteInLog("          " + _driver.Url);

            #region Заполнение полей
                int countElementsInList = _claimForTeaserModel.QuantityItemsInList(_claimForTeaserModel.LocatorSites);
                Random rnd = new Random();
                _siteInDropDownList = rnd.Next(0, countElementsInList);
                _claimForTeaserModel.Site = _siteInDropDownList;
                LogTrace.WriteInLog("Выбираю в выпадающем списке Сайт. Выбрано: " + _claimForTeaserModel.ChosenSite);

                _terms = "тест. не креативить"; //_randoms.RandomString(20);
                _claimForTeaserModel.Terms = _terms;

                _numberOfTeasers = rnd.Next(1, 11).ToString();
                _claimForTeaserModel.NumberOfTeasers = _numberOfTeasers;

                //countElementsInList = _claimForTeaserModel.QuantityItemsInList(_claimForTeaserModel.LocatorPriority);
                //rnd = new Random();
                _priorityInDropDownList = 0; // rnd.Next(0, countElementsInList);
                _claimForTeaserModel.Priority = _priorityInDropDownList;

                _pricePerClick = rnd.Next(1, 11).ToString();
                _claimForTeaserModel.PricePerClick = _pricePerClick;

                //rnd = new Random();
                _actionWhenSaveClaim = rnd.Next(0, 3);
                _claimForTeaserModel.ActionWhenSaveClaim = _actionWhenSaveClaim;
                LogTrace.WriteInLog("Выбираю radiobutton 'Какое действие выполнить при сохранении заявки?'. Выбрано: " + _claimForTeaserModel.chosenActionWhenSaveClaim);

                if (_actionWhenSaveClaim == 1)
                {
                    _claimForTeaserModel.TestCreativist = true;
                    _numberOfTeasersPopUpWindow = rnd.Next(1, 11).ToString();
                    _claimForTeaserModel.NumberOfTeasersPopUpWindow = _numberOfTeasersPopUpWindow;
                    _claimForTeaserModel.ButtonApply = true;
                }

                _claimForTeaserModel.OnlyForEditorsWagers = true;
            #endregion

            string claimedForTeaserUrl = _driver.Url; 
            _claimForTeaserModel.Submit();
            if (_driver.Url == claimedForTeaserUrl)
            {
                Errors = _claimForTeaserModel.GetErrors(); //проверяем, появились ли на форме ошибки заполнения полей
            }
            else
            {
                LogTrace.WriteInLog("Заявка на создание тизеров успешно отправлена");
                LogForClickers.WriteInLog("Заявка на создание тизеров успешно отправлена");
            }
            Registry.hashTable["driver"] = _driver;
        }
    }
}
