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
        IWebDriver driver;
        public string baseUrl = "https://admin.dt00.net/cab/goodhits/clients-edit/id/" + Registry.hashTable["clientId"] + "/filters/%252Fclient_id%252F" + Registry.hashTable["clientId"] + "%252Fsubnet%252FFall";
        Randoms randoms = new Randoms();//класс генерации случайных строк
        public List<string> errors = new List<string>(); //список ошибок

        public string emailByDirection;
        public string emailAdditionalByDirection;
        public string phoneByDirection;
        public string skypeByDirection;
        public string icqByDirection;
        public string nameByDirection;

        public string emailCommon;
        public string emailAdditionalCommon;
        public string phoneCommon;
        public string skypeCommon;
        public string icqCommon;
        public string nameCommon;
        public string aliasCommon;
        public int startSectionCommon;

        public string requisitesOfPayment;

        public int sendPassword;

        private int newCuratorExternal;
        private int newCuratorInternal;
        private int attractorClient;

        public string limitPkQuantity;
        public string limitTeasersQuantity;
        public string comments;
        
        GoodsEditClient_Model clientEditModel = new GoodsEditClient_Model();

        public void EditClient()
        {
            driver = (IWebDriver)Registry.hashTable["driver"]; //забираем из хештаблицы сохраненный ранее драйвер
            driver.Navigate().GoToUrl(baseUrl); //заходим по ссылке
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            
            clientEditModel.driver = driver;

            LogTrace.WriteInLog("          " + driver.Url);

            #region Редактирование полей
                #region Контакты по направлению

                    LogTrace.WriteInLog("          ...Контакты по направлению...");

                    emailByDirection = randoms.RandomString(5) +"@" + randoms.RandomString(5) + "." + "com";
                    clientEditModel.ContactsByDirectionEmail = emailByDirection;
                    LogTrace.WriteInLog("               Заполняю поле E-Mail по направлению. Было введено: " + clientEditModel.ContactsByDirectionEmail);

                    emailAdditionalByDirection = randoms.RandomString(5) + "@" + randoms.RandomString(5) + "." + "com";
                    clientEditModel.ContactsByDirectionEmailAdditional = emailAdditionalByDirection;
                    LogTrace.WriteInLog("               Заполняю поле Дополнительный E-Mail по направлению. Было введено: " + clientEditModel.ContactsByDirectionEmailAdditional);

                    phoneByDirection = randoms.RandomNumber(10);
                    clientEditModel.ContactsByDirectionPhone = phoneByDirection;
                    LogTrace.WriteInLog("               Заполняю поле Телефон по направлению. Было введено: " + clientEditModel.ContactsByDirectionPhone);

                    skypeByDirection = randoms.RandomString(5) + randoms.RandomNumber(5);
                    clientEditModel.ContactsByDirectionSkype = skypeByDirection;
                    LogTrace.WriteInLog("               Заполняю поле Skype по направлению. Было введено: " + clientEditModel.ContactsByDirectionSkype);

                    icqByDirection = randoms.RandomNumber(5);
                    clientEditModel.ContactsByDirectionIcq = icqByDirection;
                    LogTrace.WriteInLog("               Заполняю поле ICQ по направлению. Было введено: " + clientEditModel.ContactsByDirectionIcq);

                    nameByDirection = randoms.RandomString(10);
                    clientEditModel.ContactsByDirectionName = nameByDirection;
                    LogTrace.WriteInLog("               Заполняю поле ФИО по направлению. Было введено: " + clientEditModel.ContactsByDirectionName);
                #endregion

                #region Основные контакты

                    LogTrace.WriteInLog("          ...Основные контакты...");

                    emailCommon = randoms.RandomString(5) + "@" + randoms.RandomString(5) + "." + "com";
                    clientEditModel.ContactsCommonEmail = emailCommon;
                    LogTrace.WriteInLog("               Заполняю поле E-Mail по основным контактам. Было введено: " + clientEditModel.ContactsCommonEmail);

                    emailAdditionalCommon = randoms.RandomString(5) + "@" + randoms.RandomString(5) + "." + "com";
                    clientEditModel.ContactsCommonEmailAdditional = emailAdditionalCommon;
                    LogTrace.WriteInLog("               Заполняю поле Дополнительный E-Mail по основным контактам. Было введено: " + clientEditModel.ContactsCommonEmailAdditional);

                    phoneCommon = randoms.RandomNumber(10);
                    clientEditModel.ContactsCommonPhone = phoneCommon;
                    LogTrace.WriteInLog("               Заполняю поле Телефон по основным контактам. Было введено: " + clientEditModel.ContactsCommonPhone);

                    skypeCommon = randoms.RandomString(5) + randoms.RandomNumber(5);
                    clientEditModel.ContactsCommonSkype = skypeCommon;
                    LogTrace.WriteInLog("               Заполняю поле Skype по основным контактам. Было введено: " + clientEditModel.ContactsCommonSkype);

                    icqCommon = randoms.RandomNumber(5);
                    clientEditModel.ContactsCommonIcq = icqCommon;
                    LogTrace.WriteInLog("               Заполняю поле ICQ по основным контактам. Было введено: " + clientEditModel.ContactsCommonIcq);

                    nameCommon = randoms.RandomString(10);
                    clientEditModel.ContactsCommonName = nameCommon;
                    LogTrace.WriteInLog("               Заполняю поле ФИО по основным контактам. Было введено: " + clientEditModel.ContactsCommonName);

                    aliasCommon = randoms.RandomString(10);
                    clientEditModel.ContactsCommonAlias = aliasCommon;
                    LogTrace.WriteInLog("               Заполняю поле 'Псевдоним для креатива' по основным контактам. Было введено: " + clientEditModel.ContactsCommonAlias);

                    int countElementsInList = clientEditModel.QuantityItemsInList(clientEditModel.locatorStartSection);
                    Random rnd = new Random();
                    startSectionCommon = rnd.Next(0, countElementsInList);
                    clientEditModel.StartSectionProfile = startSectionCommon;
                    LogTrace.WriteInLog("               Выбираю в выпадающем списке 'Стартовый раздел кабинета' по основным контактам: " + clientEditModel.chosenStartSectionProfile);
                #endregion

                #region Платёжные реквизиты

                    LogTrace.WriteInLog("          ...Платёжные реквизиты...");

                    clientEditModel.KindOfPayment = true;
                    LogTrace.WriteInLog("               Выбираю в выпадающем списке 'Форма оплаты' по платежным реквизитам: " + clientEditModel.chosenKindOfPayment);

                    requisitesOfPayment = randoms.RandomNumber(16);
                    clientEditModel.Requisites = requisitesOfPayment;
                    LogTrace.WriteInLog("               Заполняю поле Реквизиты по платежным реквизитам. Было введено: " + clientEditModel.Requisites);

                    clientEditModel.ButtonAdd = true;
                    LogTrace.WriteInLog("               Нажимаю кнопку 'Добавить' по платежным реквизитам");
                #endregion

                #region Управление паролем

                    LogTrace.WriteInLog("          ...Управление паролем...");

                    clientEditModel.Password = true;
                    LogTrace.WriteInLog("               Нажимаю кнопку генерации пароля");

                    rnd = new Random();
                    sendPassword = rnd.Next(0, 3);
                    clientEditModel.SendPassword = sendPassword;
                    LogTrace.WriteInLog("               Выбираю radiobutton 'Отправить пароль?'. Выбрано: " + clientEditModel.chosenSendPassword);
                #endregion

                #region Взаимодействие

                    LogTrace.WriteInLog("          ...Взаимодействие...");

                    countElementsInList = clientEditModel.QuantityItemsInList(clientEditModel.locatorExtCurator);
                    rnd = new Random();
                    newCuratorExternal = rnd.Next(0, countElementsInList);
                    clientEditModel.NewCuratorExternal = newCuratorExternal;
                    LogTrace.WriteInLog("               Выбираю в выпадающем списке 'Выбрать нового куратора' (внешний): " + clientEditModel.chosenNewCuratorExternal);

                    countElementsInList = clientEditModel.QuantityItemsInList(clientEditModel.locatorIntCurator);
                    rnd = new Random();
                    newCuratorInternal = rnd.Next(0, countElementsInList);
                    clientEditModel.NewCuratorInternal = newCuratorInternal;
                    LogTrace.WriteInLog("               Выбираю в выпадающем списке 'Выбрать нового куратора' (внутренний): " + clientEditModel.chosenNewCuratorInternal);

                    countElementsInList = clientEditModel.QuantityItemsInList(clientEditModel.locatorAttractor);
                    rnd = new Random();
                    attractorClient = rnd.Next(0, countElementsInList);
                    clientEditModel.AttractorClient = attractorClient;
                    LogTrace.WriteInLog("               Выбираю в выпадающем списке 'Привлекатель клиента': " + clientEditModel.chosenAttractorClient);
                #endregion

                #region Настройки

                    LogTrace.WriteInLog("          ...Настройки...");

                    clientEditModel.ExchangeInProfile = true;
                    LogTrace.WriteInLog("               Выбран checkbox Обмен в кабинете");

                    clientEditModel.NewsInProfile = true;
                    LogTrace.WriteInLog("               Выбран checkbox Новости в кабинете");

                    limitPkQuantity = randoms.RandomNumber(3);
                    if(limitPkQuantity.StartsWith("0"))
                    {
                        Regex regex = new Regex(@"^[0]*");
                        Match match = regex.Match(limitPkQuantity);
                        if (match.Success) limitPkQuantity = regex.Replace(limitPkQuantity, "");
                    }
                    clientEditModel.LimitPkQuantity = limitPkQuantity;
                    LogTrace.WriteInLog("               Заполняю поле 'Ограничение по количеству кампаний'. Было введено: " + clientEditModel.LimitPkQuantity);

                    clientEditModel.AllowViewFilterByPlatforms = true;
                    LogTrace.WriteInLog("               Выбран checkbox Разрешен просмотр фильтра по площадкам");

                    limitTeasersQuantity = randoms.RandomNumber(3);
                    if (limitTeasersQuantity.StartsWith("0"))
                    {
                        Regex regex = new Regex(@"^[0]*");
                        Match match = regex.Match(limitTeasersQuantity);
                        if (match.Success) limitTeasersQuantity = regex.Replace(limitTeasersQuantity, "");
                    }
                    clientEditModel.LimitTeasersQuantityPerDay = limitTeasersQuantity;
                    LogTrace.WriteInLog("               Заполняю поле 'Ограничение на кол-во создаваемых тизеров в сутки каждой РК'. Было введено: " + clientEditModel.LimitTeasersQuantityPerDay);

                    clientEditModel.SwitchOffTabAdvBlocks = true;
                    LogTrace.WriteInLog("               Выбран checkbox 'Отключить вкладку Рекламные блоки'");
                    
                    //clientEditModel.ReflectionStatisticsSpending = true;
                    //LogTrace.WriteInLog("     Выбран checkbox 'Отображение сводной статистики трат'");

                    comments = randoms.RandomString(30);
                    clientEditModel.Comments = comments;
                    LogTrace.WriteInLog("               Заполняю поле Комментарий. Было введено: " + clientEditModel.Comments);
                #endregion
            #endregion
            
            string editClientUrl = driver.Url; //запоминаем url страницы

            clientEditModel.Submit(); //пытаемся сохранить форму
            LogTrace.WriteInLog("          Нажал кнопку Применить");
            LogTrace.WriteInLog("          " + driver.Url);
            LogTrace.WriteInLog("");
            
            string isEditedClientUrl = driver.Url; //запоминаем url страницы, открывшейся после нажатия "Завершить"
            //если createClientUrl и isCreatedClientUrl совпали - мы никуда не перешли и значит есть ошибки заполнения полей
            //если createClientUrl и isCreatedClientUrl не совпали - клиент отредактировался и ошибки искать не надо
            if (editClientUrl == isEditedClientUrl)
            {
                errors = clientEditModel.GetErrors(); //проверяем, появились ли на форме ошибки заполнения полей
            }
            else
            {
                LogTrace.WriteInLog("          Клиент успешно отредактирован");
                LogForClickers.WriteInLog("          Клиент успешно отредактирован");
            }
            //Registry.hashTable.Add("driver", driver); //записываем в хештаблицу driver и его состояние, чтобы потом извлечь и использовать его при создании сайта/РК
            Registry.hashTable["driver"] = driver;
            //LogTrace.WriteInLog(driver.Url);
        }

        public bool wasMismatch = false;

        public void CheckEditingClient()
        {
            driver = (IWebDriver)Registry.hashTable["driver"]; //забираем из хештаблицы сохраненный при создании клиента драйвер
            driver.Navigate().GoToUrl(baseUrl); //заходим по ссылке
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            LogTrace.WriteInLog("          " + driver.Url);
            
            #region Проверка заполнения
               
                #region Контакты по направлению
                    LogTrace.WriteInLog("          ...Проверка: Контакты по направлению...");

                    if (emailByDirection == clientEditModel.GetContactsByDirectionEmail) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля E-Mail ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsByDirectionEmail, emailByDirection)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля E-Mail ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsByDirectionEmail, emailByDirection));
                        wasMismatch = true;
                    }

                    if (emailAdditionalByDirection == clientEditModel.GetContactsByDirectionEmailAdditional) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля Дополнительный E-Mail ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsByDirectionEmailAdditional, emailAdditionalByDirection)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Дополнительный E-Mail ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsByDirectionEmailAdditional, emailAdditionalByDirection));
                        wasMismatch = true;
                    }

                    if (phoneByDirection == clientEditModel.GetContactsByDirectionPhone) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля Телефон ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsByDirectionPhone, phoneByDirection)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Телефон ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsByDirectionPhone, phoneByDirection));
                        wasMismatch = true;
                    }

                    if (skypeByDirection == clientEditModel.GetContactsByDirectionSkype) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля Skype ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsByDirectionSkype, skypeByDirection)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Skype ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsByDirectionSkype, skypeByDirection));
                        wasMismatch = true;
                    }

                    if (icqByDirection == clientEditModel.GetContactsByDirectionIcq) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля ICQ ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsByDirectionIcq, icqByDirection)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля ICQ ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsByDirectionIcq, icqByDirection));
                        wasMismatch = true;
                    }

                    if (nameByDirection == clientEditModel.GetContactsByDirectionName) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля ФИО ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsByDirectionName, nameByDirection)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля ФИО ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsByDirectionName, nameByDirection));
                        wasMismatch = true;
                    }
                #endregion

                #region Основные контакты
                    LogTrace.WriteInLog("          ...Проверка: Основные контакты...");

                    if (emailCommon == clientEditModel.GetContactsCommonEmail) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля E-Mail ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsCommonEmail, emailCommon)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля E-Mail ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsCommonEmail, emailCommon));
                        wasMismatch = true;
                    }

                    if (emailAdditionalCommon == clientEditModel.GetContactsCommonEmailAdditional) { LogTrace.WriteInLog("          Совпадают: содержимое поля Дополнительный E-Mail (" + clientEditModel.GetContactsCommonEmailAdditional + ") и введенное при редактировании (" + emailAdditionalCommon + ")"); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Дополнительный E-Mail ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsCommonEmailAdditional, emailAdditionalCommon));
                        wasMismatch = true;
                    }

                    if (phoneCommon == clientEditModel.GetContactsCommonPhone) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля Телефон ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsCommonPhone, phoneCommon)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Телефон ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsCommonPhone, phoneCommon));
                        wasMismatch = true;
                    }

                    if (skypeCommon == clientEditModel.GetContactsCommonSkype) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля Skype ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsCommonSkype, skypeCommon)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Skype ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsCommonSkype, skypeCommon));
                        wasMismatch = true;
                    }

                    if (icqCommon == clientEditModel.GetContactsCommonIcq) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля ICQ ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsCommonIcq, icqCommon)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля ICQ ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsCommonIcq, icqCommon));
                        wasMismatch = true;
                    }

                    if (nameCommon == clientEditModel.GetContactsCommonName) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля ФИО ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsCommonName, nameCommon)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля ФИО ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsCommonName, nameCommon));
                        wasMismatch = true;
                    }

                    if (aliasCommon == clientEditModel.GetContactsCommonAlias) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля 'Псевдоним для креатива' ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsCommonAlias, aliasCommon)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля 'Псевдоним для креатива' ({0}) и введенное при редактировании ({1})", clientEditModel.GetContactsCommonAlias, aliasCommon));
                        wasMismatch = true;
                    }

                    if (clientEditModel.chosenStartSectionProfile == clientEditModel.GetStartSectionProfile) { LogTrace.WriteInLog(string.Format("          Совпадают: пункт списка 'Стартовый раздел кабинета' ({0}) и введенное при редактировании ({1})", clientEditModel.GetStartSectionProfile, clientEditModel.chosenStartSectionProfile)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: пункт списка 'Стартовый раздел кабинета' ({0}) и введенное при редактировании ({1})", clientEditModel.GetStartSectionProfile, clientEditModel.chosenStartSectionProfile));
                        wasMismatch = true;
                    }       
                #endregion

                #region Платёжные реквизиты
                    LogTrace.WriteInLog("          ...Проверка: Платёжные реквизиты...");

                    string concatenated = clientEditModel.chosenKindOfPayment + " " + requisitesOfPayment;
                    if (concatenated == clientEditModel.GetRequisites) { LogTrace.WriteInLog(string.Format("          Совпадают: форма оплаты + реквизиты ({0}) и введенное при редактировании ({1})", clientEditModel.GetRequisites, concatenated)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: форма оплаты + реквизиты ({0}) и введенное при редактировании ({1})", clientEditModel.GetRequisites, concatenated));
                        wasMismatch = true;
                    }
                #endregion

                #region Взаимодействие
                    LogTrace.WriteInLog("          ...Проверка: Взаимодействие...");

                    if (clientEditModel.GetCurrentCurator) { LogTrace.WriteInLog(string.Format("          Совпадают: текущий куратор ({0}) и введенное при редактировании ({1})", clientEditModel.GetCurrentCurator, clientEditModel.chosenNewCuratorInternal)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: текущий куратор ({0}) и введенное при редактировании ({1})", clientEditModel.GetCurrentCurator, clientEditModel.chosenNewCuratorInternal));
                        wasMismatch = true;
                    }

                    if (clientEditModel.chosenAttractorClient == clientEditModel.GetAttractorClient) { LogTrace.WriteInLog(string.Format("          Совпадают: пункт списка 'Привлекатель клиента' ({0}) и введенное при редактировании ({1})", clientEditModel.GetAttractorClient, clientEditModel.chosenAttractorClient)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: пункт списка 'Привлекатель клиента' ({0}) и введенное при редактировании ({1})", clientEditModel.GetAttractorClient, clientEditModel.chosenAttractorClient));
                        wasMismatch = true;
                    }
                #endregion

                #region Настройки
                    LogTrace.WriteInLog("          ...Проверка: Настройки...");

                    if (clientEditModel.GetClientIsActive) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Клиент активен' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Клиент активен' и выбранное при редактировании");
                        wasMismatch = true;
                    }

                    if (clientEditModel.GetExchangeInProfile) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Обмен в кабинете' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Обмен в кабинете' и выбранное при редактировании");
                        wasMismatch = true;
                    }

                    if (clientEditModel.GetNewsInProfile) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Новости в кабинете' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Новости в кабинете' и выбранное при редактировании");
                        wasMismatch = true;
                    }

                    if (clientEditModel.GetTestClient) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Тестовый клиент' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Тестовый клиент' и выбранное при редактировании");
                        wasMismatch = true;
                    }

                    if (limitPkQuantity == clientEditModel.GetLimitPkQuantity) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля 'Ограничение по количеству кампаний' ({0}) и введенное при редактировании ({1})", clientEditModel.GetLimitPkQuantity, limitPkQuantity)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля 'Ограничение по количеству кампаний' ({0}) и введенное при редактировании ({1})", clientEditModel.GetLimitPkQuantity, limitPkQuantity));
                        wasMismatch = true;
                    }

                    if (clientEditModel.GetAllowViewFilterByPlatforms) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Разрешен просмотр фильтра по площадкам' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Разрешен просмотр фильтра по площадкам' и выбранное при редактировании");
                        wasMismatch = true;
                    }

                    if (limitTeasersQuantity == clientEditModel.GetLimitTeasersQuantityPerDay) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля 'Ограничение на кол-во создаваемых тизеров в сутки каждой РК' ({0}) и введенное при редактировании ({1})", clientEditModel.GetLimitTeasersQuantityPerDay, limitTeasersQuantity)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля 'Ограничение на кол-во создаваемых тизеров в сутки каждой РК' ({0}) и введенное при редактировании ({1})", clientEditModel.GetLimitTeasersQuantityPerDay, limitTeasersQuantity));
                        wasMismatch = true;
                    }

                    if (clientEditModel.GetSwitchOffTabAdvBlocks) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Отключить вкладку Рекламные блоки' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Отключить вкладку Рекламные блоки' и выбранное при редактировании");
                        wasMismatch = true;
                    }

                    if (clientEditModel.GetReflectionStatisticsSpending) { LogTrace.WriteInLog("          Совпадают: состояние checkbox 'Отображение сводной статистики трат' и выбранное при редактировании"); }
                    else
                    {
                        LogTrace.WriteInLog("НЕ СОВПАДАЮТ: состояние checkbox 'Отображение сводной статистики трат' и выбранное при редактировании");
                        wasMismatch = true;
                    }

                    if (comments == clientEditModel.GetComments) { LogTrace.WriteInLog(string.Format("          Совпадают: содержимое поля Комментарий ({0}) и введенное при редактировании ({1})", clientEditModel.GetComments, comments)); }
                    else
                    {
                        LogTrace.WriteInLog(string.Format("НЕ СОВПАДАЮТ: содержимое поля Комментарий ({0}) и введенное при редактировании ({1})", clientEditModel.GetComments, comments));
                        wasMismatch = true;
                    } 
                #endregion
            #endregion

            LogTrace.WriteInLog("          " + driver.Url);
            LogTrace.WriteInLog("");
            if(!wasMismatch)
            {
                LogTrace.WriteInLog("          ОК, всё ранее введенное совпадает с текущими значениями");
                LogForClickers.WriteInLog("          ОК, всё ранее введенное совпадает с текущими значениями");
            }
        }
    }
}
