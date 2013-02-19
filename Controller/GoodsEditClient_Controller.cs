using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using Task.Model;
using Task.Utils;

namespace Task.Controller
{
    public class GoodsEditClientController
    {
        private IWebDriver _driver;
        private GoodsEditClient_Model _clientEditModel;
        private readonly string _baseUrl = "https://admin.dt00.net/cab/goodhits/clients-edit/id/" + Registry.hashTable["clientId"] + "/filters/%252Fclient_id%252F" + Registry.hashTable["clientId"] + "%252Fsubnet%252FFall";
        readonly Randoms _randoms = new Randoms();//класс генерации случайных строк
        private string _emailByDirection;
        private string _emailAdditionalByDirection;
        private string _phoneByDirection;
        private string _skypeByDirection;
        private string _icqByDirection;
        private string _nameByDirection;
        private string _emailCommon;
        private string _emailAdditionalCommon;
        private string _phoneCommon;
        private string _skypeCommon;
        private string _icqCommon;
        private string _nameCommon;
        private string _aliasCommon;
        private int _startSectionCommon;
        private string _requisitesOfPayment;
        private int _sendPassword;
        private int _newCuratorExternal;
        private int _newCuratorInternal;
        private int _attractorClient;
        private string _limitPkQuantity;
        private string _limitTeasersQuantity;
        private string _comments;

        public List<string> Errors = new List<string>(); //список ошибок
        public bool WasMismatch = false; //наличие/отсутствие несовпадений

        public void EditClient()
        {
            GetDriver();
            SetUpFields();
            EditingIsSuccessful();
        }

        private void GetDriver()
        {
            _driver = (IWebDriver)Registry.hashTable["driver"]; //забираем из хештаблицы сохраненный ранее драйвер
            _driver.Navigate().GoToUrl(_baseUrl); //заходим по ссылке
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
        }

        private void SetUpFields()
        {
            _clientEditModel = new GoodsEditClient_Model();
            _clientEditModel.driver = _driver;
            LogTrace.WriteInLog("          " + _driver.Url);

            #region Редактирование полей
                #region Контакты по направлению
                    LogTrace.WriteInLog("          ...Контакты по направлению...");

                    _emailByDirection = _randoms.RandomString(5) + "@" + _randoms.RandomString(5) + "." + "com";
                    _clientEditModel.ContactsByDirectionEmail = _emailByDirection;
                    LogTrace.WriteInLog("               Заполняю поле E-Mail по направлению. Было введено: " + _clientEditModel.ContactsByDirectionEmail);

                    _emailAdditionalByDirection = _randoms.RandomString(5) + "@" + _randoms.RandomString(5) + "." + "com";
                    _clientEditModel.ContactsByDirectionEmailAdditional = _emailAdditionalByDirection;
                    LogTrace.WriteInLog("               Заполняю поле Дополнительный E-Mail по направлению. Было введено: " + _clientEditModel.ContactsByDirectionEmailAdditional);

                    _phoneByDirection = _randoms.RandomNumber(10);
                    _clientEditModel.ContactsByDirectionPhone = _phoneByDirection;
                    LogTrace.WriteInLog("               Заполняю поле Телефон по направлению. Было введено: " + _clientEditModel.ContactsByDirectionPhone);

                    _skypeByDirection = _randoms.RandomString(5) + _randoms.RandomNumber(5);
                    _clientEditModel.ContactsByDirectionSkype = _skypeByDirection;
                    LogTrace.WriteInLog("               Заполняю поле Skype по направлению. Было введено: " + _clientEditModel.ContactsByDirectionSkype);

                    _icqByDirection = _randoms.RandomNumber(5);
                    _clientEditModel.ContactsByDirectionIcq = _icqByDirection;
                    LogTrace.WriteInLog("               Заполняю поле ICQ по направлению. Было введено: " + _clientEditModel.ContactsByDirectionIcq);

                    _nameByDirection = _randoms.RandomString(10);
                    _clientEditModel.ContactsByDirectionName = _nameByDirection;
                    LogTrace.WriteInLog("               Заполняю поле ФИО по направлению. Было введено: " + _clientEditModel.ContactsByDirectionName);
                #endregion

                #region Основные контакты
                    LogTrace.WriteInLog("          ...Основные контакты...");

                    _emailCommon = _randoms.RandomString(5) + "@" + _randoms.RandomString(5) + "." + "com";
                    _clientEditModel.ContactsCommonEmail = _emailCommon;
                    LogTrace.WriteInLog("               Заполняю поле E-Mail по основным контактам. Было введено: " + _clientEditModel.ContactsCommonEmail);

                    _emailAdditionalCommon = _randoms.RandomString(5) + "@" + _randoms.RandomString(5) + "." + "com";
                    _clientEditModel.ContactsCommonEmailAdditional = _emailAdditionalCommon;
                    LogTrace.WriteInLog("               Заполняю поле Дополнительный E-Mail по основным контактам. Было введено: " + _clientEditModel.ContactsCommonEmailAdditional);

                    _phoneCommon = _randoms.RandomNumber(10);
                    _clientEditModel.ContactsCommonPhone = _phoneCommon;
                    LogTrace.WriteInLog("               Заполняю поле Телефон по основным контактам. Было введено: " + _clientEditModel.ContactsCommonPhone);

                    _skypeCommon = _randoms.RandomString(5) + _randoms.RandomNumber(5);
                    _clientEditModel.ContactsCommonSkype = _skypeCommon;
                    LogTrace.WriteInLog("               Заполняю поле Skype по основным контактам. Было введено: " + _clientEditModel.ContactsCommonSkype);

                    _icqCommon = _randoms.RandomNumber(5);
                    _clientEditModel.ContactsCommonIcq = _icqCommon;
                    LogTrace.WriteInLog("               Заполняю поле ICQ по основным контактам. Было введено: " + _clientEditModel.ContactsCommonIcq);

                    _nameCommon = _randoms.RandomString(10);
                    _clientEditModel.ContactsCommonName = _nameCommon;
                    LogTrace.WriteInLog("               Заполняю поле ФИО по основным контактам. Было введено: " + _clientEditModel.ContactsCommonName);

                    _aliasCommon = _randoms.RandomString(10);
                    _clientEditModel.ContactsCommonAlias = _aliasCommon;
                    LogTrace.WriteInLog("               Заполняю поле 'Псевдоним для креатива' по основным контактам. Было введено: " + _clientEditModel.ContactsCommonAlias);

                    int countElementsInList = _clientEditModel.QuantityItemsInList(_clientEditModel.locatorStartSection);
                    Random rnd = new Random();
                    _startSectionCommon = rnd.Next(0, countElementsInList);
                    _clientEditModel.StartSectionProfile = _startSectionCommon;
                    LogTrace.WriteInLog("               Выбираю в выпадающем списке 'Стартовый раздел кабинета' по основным контактам: " + _clientEditModel.chosenStartSectionProfile);
                #endregion

                #region Платёжные реквизиты
                    LogTrace.WriteInLog("          ...Платёжные реквизиты...");

                    _clientEditModel.KindOfPayment = true;
                    LogTrace.WriteInLog("               Выбираю в выпадающем списке 'Форма оплаты' по платежным реквизитам: " + _clientEditModel.chosenKindOfPayment);

                    _requisitesOfPayment = _randoms.RandomNumber(16);
                    _clientEditModel.Requisites = _requisitesOfPayment;
                    LogTrace.WriteInLog("               Заполняю поле Реквизиты по платежным реквизитам. Было введено: " + _clientEditModel.Requisites);

                    _clientEditModel.ButtonAdd = true;
                    LogTrace.WriteInLog("               Нажимаю кнопку 'Добавить' по платежным реквизитам");
                #endregion

                #region Управление паролем
                    LogTrace.WriteInLog("          ...Управление паролем...");

                    _clientEditModel.Password = true;
                    LogTrace.WriteInLog("               Нажимаю кнопку генерации пароля");

                    rnd = new Random();
                    _sendPassword = rnd.Next(0, 3);
                    _clientEditModel.SendPassword = _sendPassword;
                    LogTrace.WriteInLog("               Выбираю radiobutton 'Отправить пароль?'. Выбрано: " + _clientEditModel.chosenSendPassword);
                #endregion

                #region Взаимодействие
                    LogTrace.WriteInLog("          ...Взаимодействие...");

                    countElementsInList = _clientEditModel.QuantityItemsInList(_clientEditModel.locatorExtCurator);
                    rnd = new Random();
                    _newCuratorExternal = rnd.Next(0, countElementsInList);
                    _clientEditModel.NewCuratorExternal = _newCuratorExternal;
                    LogTrace.WriteInLog("               Выбираю в выпадающем списке 'Выбрать нового куратора' (внешний): " + _clientEditModel.chosenNewCuratorExternal);

                    countElementsInList = _clientEditModel.QuantityItemsInList(_clientEditModel.locatorIntCurator);
                    rnd = new Random();
                    _newCuratorInternal = rnd.Next(0, countElementsInList);
                    _clientEditModel.NewCuratorInternal = _newCuratorInternal;
                    LogTrace.WriteInLog("               Выбираю в выпадающем списке 'Выбрать нового куратора' (внутренний): " + _clientEditModel.chosenNewCuratorInternal);

                    countElementsInList = _clientEditModel.QuantityItemsInList(_clientEditModel.locatorAttractor);
                    rnd = new Random();
                    _attractorClient = rnd.Next(0, countElementsInList);
                    _clientEditModel.AttractorClient = _attractorClient;
                    LogTrace.WriteInLog("               Выбираю в выпадающем списке 'Привлекатель клиента': " + _clientEditModel.chosenAttractorClient);
                #endregion

                #region Настройки
                    LogTrace.WriteInLog("          ...Настройки...");

                    _clientEditModel.ExchangeInProfile = true;
                    LogTrace.WriteInLog("               Выбран checkbox Обмен в кабинете");

                    _clientEditModel.NewsInProfile = true;
                    LogTrace.WriteInLog("               Выбран checkbox Новости в кабинете");

                    _limitPkQuantity = _randoms.RandomNumber(3);
                    if (_limitPkQuantity.StartsWith("0"))
                    {
                        Regex regex = new Regex(@"^[0]*");
                        Match match = regex.Match(_limitPkQuantity);
                        if (match.Success) _limitPkQuantity = regex.Replace(_limitPkQuantity, "");
                    }
                    _clientEditModel.LimitPkQuantity = _limitPkQuantity;
                    LogTrace.WriteInLog("               Заполняю поле 'Ограничение по количеству кампаний'. Было введено: " + _clientEditModel.LimitPkQuantity);

                    _clientEditModel.AllowViewFilterByPlatforms = true;
                    LogTrace.WriteInLog("               Выбран checkbox Разрешен просмотр фильтра по площадкам");

                    _limitTeasersQuantity = _randoms.RandomNumber(3);
                    if (_limitTeasersQuantity.StartsWith("0"))
                    {
                        Regex regex = new Regex(@"^[0]*");
                        Match match = regex.Match(_limitTeasersQuantity);
                        if (match.Success) _limitTeasersQuantity = regex.Replace(_limitTeasersQuantity, "");
                    }
                    _clientEditModel.LimitTeasersQuantityPerDay = _limitTeasersQuantity;
                    LogTrace.WriteInLog("               Заполняю поле 'Ограничение на кол-во создаваемых тизеров в сутки каждой РК'. Было введено: " + _clientEditModel.LimitTeasersQuantityPerDay);

                    _clientEditModel.SwitchOffTabAdvBlocks = true;
                    LogTrace.WriteInLog("               Выбран checkbox 'Отключить вкладку Рекламные блоки'");

                    //clientEditModel.ReflectionStatisticsSpending = true;
                    //LogTrace.WriteInLog("     Выбран checkbox 'Отображение сводной статистики трат'");

                    _comments = _randoms.RandomString(30);
                    _clientEditModel.Comments = _comments;
                    LogTrace.WriteInLog("               Заполняю поле Комментарий. Было введено: " + _clientEditModel.Comments);
                #endregion
            #endregion
        }

        private void EditingIsSuccessful()
        {
            string editClientUrl = _driver.Url; //запоминаем url страницы

            _clientEditModel.Submit(); //пытаемся сохранить форму
            LogTrace.WriteInLog("          Нажал кнопку Применить");
            LogTrace.WriteInLog("          " + _driver.Url);
            LogTrace.WriteInLog("");

            //если editClientUrl и текущий url совпали - мы никуда не перешли и значит есть ошибки заполнения полей
            if (_driver.Url == editClientUrl)
            {
                Errors = _clientEditModel.GetErrors(); //проверяем, появились ли на форме ошибки заполнения полей
            }
            else
            {
                LogTrace.WriteInLog("          Клиент успешно отредактирован");
                LogForClickers.WriteInLog("          Клиент успешно отредактирован");
            }
            Registry.hashTable["driver"] = _driver;
        }
        
        public void CheckEditingClient()
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
            LogTrace.WriteInLog("          " + _driver.Url);

            #region Проверка редактирования
                #region Контакты по направлению
                    LogTrace.WriteInLog("          ...Проверка: Контакты по направлению...");

                    if (_emailByDirection == _clientEditModel.GetContactsByDirectionEmail) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля E-Mail ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsByDirectionEmail, _emailByDirection)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля E-Mail ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsByDirectionEmail, _emailByDirection));
                        WasMismatch = true;
                    }

                    if (_emailAdditionalByDirection == _clientEditModel.GetContactsByDirectionEmailAdditional) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля Дополнительный E-Mail ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsByDirectionEmailAdditional, _emailAdditionalByDirection)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Дополнительный E-Mail ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsByDirectionEmailAdditional, _emailAdditionalByDirection));
                        WasMismatch = true;
                    }

                    if (_phoneByDirection == _clientEditModel.GetContactsByDirectionPhone) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля Телефон ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsByDirectionPhone, _phoneByDirection)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Телефон ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsByDirectionPhone, _phoneByDirection));
                        WasMismatch = true;
                    }

                    if (_skypeByDirection == _clientEditModel.GetContactsByDirectionSkype) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля Skype ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsByDirectionSkype, _skypeByDirection)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Skype ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsByDirectionSkype, _skypeByDirection));
                        WasMismatch = true;
                    }

                    if (_icqByDirection == _clientEditModel.GetContactsByDirectionIcq) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля ICQ ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsByDirectionIcq, _icqByDirection)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля ICQ ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsByDirectionIcq, _icqByDirection));
                        WasMismatch = true;
                    }

                    if (_nameByDirection == _clientEditModel.GetContactsByDirectionName) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля ФИО ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsByDirectionName, _nameByDirection)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля ФИО ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsByDirectionName, _nameByDirection));
                        WasMismatch = true;
                    }
                #endregion

                #region Основные контакты
                    LogTrace.WriteInLog("          ...Проверка: Основные контакты...");

                    if (_emailCommon == _clientEditModel.GetContactsCommonEmail) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля E-Mail ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsCommonEmail, _emailCommon)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля E-Mail ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsCommonEmail, _emailCommon));
                        WasMismatch = true;
                    }

                    if (_emailAdditionalCommon == _clientEditModel.GetContactsCommonEmailAdditional) { LogTrace.WriteInLog("          Совпадают: содержимое поля Дополнительный E-Mail (" + _clientEditModel.GetContactsCommonEmailAdditional + ") и введенное при редактировании (" + _emailAdditionalCommon + ")"); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Дополнительный E-Mail ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsCommonEmailAdditional, _emailAdditionalCommon));
                        WasMismatch = true;
                    }

                    if (_phoneCommon == _clientEditModel.GetContactsCommonPhone) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля Телефон ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsCommonPhone, _phoneCommon)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Телефон ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsCommonPhone, _phoneCommon));
                        WasMismatch = true;
                    }

                    if (_skypeCommon == _clientEditModel.GetContactsCommonSkype) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля Skype ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsCommonSkype, _skypeCommon)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Skype ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsCommonSkype, _skypeCommon));
                        WasMismatch = true;
                    }

                    if (_icqCommon == _clientEditModel.GetContactsCommonIcq) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля ICQ ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsCommonIcq, _icqCommon)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля ICQ ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsCommonIcq, _icqCommon));
                        WasMismatch = true;
                    }

                    if (_nameCommon == _clientEditModel.GetContactsCommonName) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля ФИО ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsCommonName, _nameCommon)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля ФИО ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsCommonName, _nameCommon));
                        WasMismatch = true;
                    }

                    if (_aliasCommon == _clientEditModel.GetContactsCommonAlias) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля 'Псевдоним для креатива' ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsCommonAlias, _aliasCommon)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля 'Псевдоним для креатива' ({0}) и введенное при редактировании ({1})", _clientEditModel.GetContactsCommonAlias, _aliasCommon));
                        WasMismatch = true;
                    }

                    if (_clientEditModel.chosenStartSectionProfile == _clientEditModel.GetStartSectionProfile) { LogTrace.WriteInLog(string.Format("          Совпадают: пункт списка 'Стартовый раздел кабинета' ({0}) и введенное при редактировании ({1})", _clientEditModel.GetStartSectionProfile, _clientEditModel.chosenStartSectionProfile)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: пункт списка 'Стартовый раздел кабинета' ({0}) и введенное при редактировании ({1})", _clientEditModel.GetStartSectionProfile, _clientEditModel.chosenStartSectionProfile));
                        WasMismatch = true;
                    }
                #endregion

                #region Платёжные реквизиты
                    LogTrace.WriteInLog("          ...Проверка: Платёжные реквизиты...");

                    string concatenated = _clientEditModel.chosenKindOfPayment + " " + _requisitesOfPayment;
                    if (concatenated == _clientEditModel.GetRequisites) { LogTrace.WriteInLog(string.Format("          Совпадают: форма оплаты + реквизиты ({0}) и введенное при редактировании ({1})", _clientEditModel.GetRequisites, concatenated)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: форма оплаты + реквизиты ({0}) и введенное при редактировании ({1})", _clientEditModel.GetRequisites, concatenated));
                        WasMismatch = true;
                    }
                #endregion

                #region Взаимодействие
                    LogTrace.WriteInLog("          ...Проверка: Взаимодействие...");

                    if (_clientEditModel.GetCurrentCurator) { LogTrace.WriteInLog(string.Format("          Совпадают: текущий куратор ({0}) и введенное при редактировании ({1})", _clientEditModel.GetCurrentCurator, _clientEditModel.chosenNewCuratorInternal)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: текущий куратор ({0}) и введенное при редактировании ({1})", _clientEditModel.GetCurrentCurator, _clientEditModel.chosenNewCuratorInternal));
                        WasMismatch = true;
                    }

                    if (_clientEditModel.chosenAttractorClient == _clientEditModel.GetAttractorClient) { LogTrace.WriteInLog(string.Format("          Совпадают: пункт списка 'Привлекатель клиента' ({0}) и введенное при редактировании ({1})", _clientEditModel.GetAttractorClient, _clientEditModel.chosenAttractorClient)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: пункт списка 'Привлекатель клиента' ({0}) и введенное при редактировании ({1})", _clientEditModel.GetAttractorClient, _clientEditModel.chosenAttractorClient));
                        WasMismatch = true;
                    }
                #endregion

                #region Настройки
                    LogTrace.WriteInLog("          ...Проверка: Настройки...");

                    if (_clientEditModel.GetClientIsActive) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Клиент активен' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Клиент активен' и выбранное при редактировании");
                        WasMismatch = true;
                    }

                    if (_clientEditModel.GetExchangeInProfile) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Обмен в кабинете' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Обмен в кабинете' и выбранное при редактировании");
                        WasMismatch = true;
                    }

                    if (_clientEditModel.GetNewsInProfile) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Новости в кабинете' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Новости в кабинете' и выбранное при редактировании");
                        WasMismatch = true;
                    }

                    if (_clientEditModel.GetTestClient) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Тестовый клиент' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Тестовый клиент' и выбранное при редактировании");
                        WasMismatch = true;
                    }

                    if (_limitPkQuantity == _clientEditModel.GetLimitPkQuantity) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля 'Ограничение по количеству кампаний' ({0}) и введенное при редактировании ({1})", _clientEditModel.GetLimitPkQuantity, _limitPkQuantity)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля 'Ограничение по количеству кампаний' ({0}) и введенное при редактировании ({1})", _clientEditModel.GetLimitPkQuantity, _limitPkQuantity));
                        WasMismatch = true;
                    }

                    if (_clientEditModel.GetAllowViewFilterByPlatforms) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Разрешен просмотр фильтра по площадкам' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Разрешен просмотр фильтра по площадкам' и выбранное при редактировании");
                        WasMismatch = true;
                    }

                    if (_limitTeasersQuantity == _clientEditModel.GetLimitTeasersQuantityPerDay) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля 'Ограничение на кол-во создаваемых тизеров в сутки каждой РК' ({0}) и введенное при редактировании ({1})", _clientEditModel.GetLimitTeasersQuantityPerDay, _limitTeasersQuantity)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля 'Ограничение на кол-во создаваемых тизеров в сутки каждой РК' ({0}) и введенное при редактировании ({1})", _clientEditModel.GetLimitTeasersQuantityPerDay, _limitTeasersQuantity));
                        WasMismatch = true;
                    }

                    if (_clientEditModel.GetSwitchOffTabAdvBlocks) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Отключить вкладку Рекламные блоки' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Отключить вкладку Рекламные блоки' и выбранное при редактировании");
                        WasMismatch = true;
                    }

                    if (_clientEditModel.GetReflectionStatisticsSpending) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Отображение сводной статистики трат' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Отображение сводной статистики трат' и выбранное при редактировании");
                        WasMismatch = true;
                    }

                    if (_comments == _clientEditModel.GetComments) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля Комментарий ({0}) и введенное при редактировании ({1})", _clientEditModel.GetComments, _comments)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Комментарий ({0}) и введенное при редактировании ({1})", _clientEditModel.GetComments, _comments));
                        WasMismatch = true;
                    }
                #endregion
            #endregion

            LogTrace.WriteInLog("          " + _driver.Url);
            LogTrace.WriteInLog("");
            return WasMismatch;
        }
    }
}
