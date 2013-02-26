using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Task.Model;
using Task.Controller;
using Task.Utils;
using Task.Controller.Pictograms;

namespace Task.View
{
    public partial class Goods_View : Form
    {
        public Goods_View()
        {
            InitializeComponent();
            //textBox1.Text = "Auth.csv";
        }

        //public StreamWriter sw; //запись в файл

        public static string tab1 = "\t";
        public static string tab2 = "\t\t";
        public static string tab3 = "\t\t\t";

        private const string _pathToCredentials = "Auth.csv";

        public void CreateNewClient()
        {
            listBox1.Items.Add(
                "===============================Cоздание клиента по Товарам===============================");
            LogForClickers.WriteInLog("===============================Cоздание клиента по Товарам===============================");
            LogForClickers.WriteInLog(DateTime.Now.ToString());
            LogTrace.WriteInLog("===============================Cоздание клиента по Товарам===============================");
            LogTrace.WriteInLog(DateTime.Now.ToString());

            GoodsCreateClientController clientController = new GoodsCreateClientController();
            //clientController.CreateClient(openFileDialog1.FileName, checkBox2.Checked);
            clientController.CreateClient(_pathToCredentials, checkBox1.Checked, checkBox2.Checked);

            List<string> errors = clientController.Errors;
            
            if (errors.Count != 0) //список непустой -- ошибки есть
            {
                listBox1.ForeColor = Color.Red;
                listBox1.Items.Add("");
                listBox1.Items.Add("!!! Ошибки !!!");

                LogForClickers.WriteInLog("");
                LogForClickers.WriteInLog("!!! Ошибки !!!");

                LogTrace.WriteInLog("");
                LogTrace.WriteInLog("!!! Ошибки !!!");

                for (int i = 0; i < errors.Count; i++)
                {
                    listBox1.Items.Add(errors[i]);
                    LogForClickers.WriteInLog(errors[i]);
                    LogTrace.WriteInLog(errors[i]);
                }
            }
            else
            {
                //listBox1.ForeColor = Color.Black;
                string clientId = clientController.ClientId;

                LogTrace.WriteInLog("");
                LogForClickers.WriteInLog("ID клиента: " + clientId);
                LogTrace.WriteInLog("ID клиента: " + clientId);
                listBox1.Items.Add("ID клиента: " + clientId);

                LogForClickers.WriteInLog("Логин:      " + clientController.Login);
                LogTrace.WriteInLog("Логин:      " + clientController.Login);
                listBox1.Items.Add("Логин:         " + clientController.Login);

                LogForClickers.WriteInLog("Пароль:     " + clientController.Password);
                LogTrace.WriteInLog("Пароль:     " + clientController.Password);
                listBox1.Items.Add("Пароль:         " + clientController.Password);
            }

            //listBox1.Items.Add("");
            LogForClickers.WriteInLog("");
            LogTrace.WriteInLog("");
        }

        public void CreateNewSite()
        {
            listBox1.Items.Add(
                tab1 + "===============================Cоздание сайта по Товарам===============================");
            LogForClickers.WriteInLog(tab1 + "===============================Cоздание сайта по Товарам===============================");
            LogForClickers.WriteInLog(tab1 + DateTime.Now.ToString());
            LogTrace.WriteInLog(tab1 + "===============================Cоздание сайта по Товарам===============================");
            LogTrace.WriteInLog(tab1 + DateTime.Now.ToString());

            GoodsCreateSiteController siteController = new GoodsCreateSiteController();
            siteController.CreateSite(checkBox1.Checked, checkBox2.Checked);

            List<string> errors = siteController.Errors; //парсим со страницы список ошибок

            if (errors.Count != 0) //список непустой -- ошибки есть
            {
                listBox1.ForeColor = Color.Red;
                listBox1.Items.Add("");
                listBox1.Items.Add(tab1 + "!!! Ошибки !!!");

                LogForClickers.WriteInLog("");
                LogForClickers.WriteInLog(tab1 + "!!! Ошибки !!!");

                LogTrace.WriteInLog("");
                LogTrace.WriteInLog(tab1 + "!!! Ошибки !!!");

                for (int i = 0; i < errors.Count; i++)
                {
                    listBox1.Items.Add(errors[i]);
                    LogForClickers.WriteInLog(errors[i]);
                    LogTrace.WriteInLog(errors[i]);
                }
            }
            else
            {
                //listBox1.ForeColor = Color.Black;
                string siteId = siteController.SiteId;
                string clientId = siteController.ClientId;
                string siteDomain = siteController.SiteDomain;
                //string siteName = siteController.siteName;

                LogTrace.WriteInLog("");
                LogForClickers.WriteInLog(tab1 + "ID сайта: " + siteId);
                LogTrace.WriteInLog(tab1 + "ID сайта: " + siteId);
                listBox1.Items.Add(tab1 + "ID сайта: " + siteId);

                LogForClickers.WriteInLog(tab1 + "ID клиента: " + clientId);
                LogTrace.WriteInLog(tab1 + "ID клиента: " + clientId);
                listBox1.Items.Add(tab1 + "ID клиента: " + clientId);

                LogForClickers.WriteInLog(tab1 + "Домен:    " + siteDomain);
                LogTrace.WriteInLog(tab1 + "Домен:    " + siteDomain);
                listBox1.Items.Add(tab1 + "Домен:     " + siteDomain);
            }

            //listBox1.Items.Add("");
            LogForClickers.WriteInLog("");
            LogTrace.WriteInLog("");
        }

        public void CreateNewPk()
        {
            listBox1.Items.Add(
                tab1 + "===============================Cоздание РК по Товарам===============================");
            LogForClickers.WriteInLog(tab1 + "===============================Cоздание РК по Товарам===============================");
            LogForClickers.WriteInLog(tab1 + DateTime.Now.ToString());
            LogTrace.WriteInLog(tab1 + "===============================Cоздание РК по Товарам===============================");
            LogTrace.WriteInLog(tab1 + DateTime.Now.ToString());

            GoodsCreatePkController pkController = new GoodsCreatePkController();
            pkController.CreatePk(checkBox1.Checked, checkBox2.Checked);

            List<string> errors = pkController.Errors; //парсим со страницы список ошибок

            if (errors.Count != 0) //список непустой -- ошибки есть
            {
                listBox1.ForeColor = Color.Red;
                listBox1.Items.Add("");
                listBox1.Items.Add(tab1 + "!!! Ошибки !!!");

                LogForClickers.WriteInLog("");
                LogForClickers.WriteInLog(tab1 + "!!! Ошибки !!!");

                LogTrace.WriteInLog("");
                LogTrace.WriteInLog(tab1 + "!!! Ошибки !!!");

                for (int i = 0; i < errors.Count; i++)
                {
                    listBox1.Items.Add(errors[i]);
                    LogForClickers.WriteInLog(errors[i]);
                    LogTrace.WriteInLog(errors[i]);
                }
            }
            else
            {
                //listBox1.ForeColor = Color.Black;
                string pkId = pkController.PkId;
                string clientId = pkController.ClientId;
                string pkName = pkController.PkName;

                LogTrace.WriteInLog("");
                LogForClickers.WriteInLog(tab1 + "ID РК:       " + pkId);
                LogTrace.WriteInLog(tab1 + "ID РК:       " + pkId);
                listBox1.Items.Add(tab1 + "ID РК: " + pkId);

                LogForClickers.WriteInLog(tab1 + "ID клиента: " + clientId);
                LogTrace.WriteInLog(tab1 + "ID клиента: " + clientId);
                listBox1.Items.Add(tab1 + "ID клиента: " + clientId);

                LogForClickers.WriteInLog(tab1 + "Название РК: " + pkName);
                LogTrace.WriteInLog(tab1 + "Название РК: " + pkName);
                listBox1.Items.Add(tab1 + "Название РК:     " + pkName);
            }

            //listBox1.Items.Add("");
            LogForClickers.WriteInLog("");
            LogTrace.WriteInLog("");
        }

        public void CreateNewTeaser()
        {
            listBox1.Items.Add(
                tab1 + "===============================Cоздание тизера по Товарам===============================");
            LogForClickers.WriteInLog(tab1 + "===============================Cоздание тизера по Товарам===============================");
            LogForClickers.WriteInLog(tab1 + DateTime.Now.ToString());
            LogTrace.WriteInLog(tab1 + "===============================Cоздание тизера по Товарам===============================");
            LogTrace.WriteInLog(tab1 + DateTime.Now.ToString());

            GoodsCreateTeaserController teaserController = new GoodsCreateTeaserController();
            teaserController.CreateTeaser(checkBox1.Checked, checkBox2.Checked);

            List<string> errors = teaserController.Errors; //парсим со страницы список ошибок

            if (errors.Count != 0) //список непустой -- ошибки есть
            {
                listBox1.ForeColor = Color.Red;
                listBox1.Items.Add("");
                listBox1.Items.Add(tab1 + "!!! Ошибки !!!");

                LogForClickers.WriteInLog("");
                LogForClickers.WriteInLog(tab1 + "!!! Ошибки !!!");

                LogTrace.WriteInLog("");
                LogTrace.WriteInLog(tab1 + "!!! Ошибки !!!");

                for (int i = 0; i < errors.Count; i++)
                {
                    listBox1.Items.Add(errors[i]);
                    LogForClickers.WriteInLog(errors[i]);
                    LogTrace.WriteInLog(errors[i]);
                }
            }
            else
            {
                //listBox1.ForeColor = Color.Black;
                string teaserId = teaserController.TeaserId;
                string pkId = teaserController.PkId;
                string clientId = teaserController.ClientId;
                string domain = teaserController.AllowedDomain;

                LogTrace.WriteInLog("");
                LogForClickers.WriteInLog(tab1 + "ID тизера: " + teaserId);
                LogTrace.WriteInLog(tab1 + "ID тизера: " + teaserId);
                listBox1.Items.Add(tab1 + "ID тизера: " + teaserId);

                LogForClickers.WriteInLog(tab1 + "ID клиента: " + clientId);
                LogTrace.WriteInLog(tab1 + "ID клиента: " + clientId);
                listBox1.Items.Add(tab1 + "ID клиента:     " + clientId);

                LogForClickers.WriteInLog(tab1 + "ID РК:      " + pkId);
                LogTrace.WriteInLog(tab1 + "ID РК:      " + pkId);
                listBox1.Items.Add(tab1 + "ID РК:     " + pkId);

                LogForClickers.WriteInLog(tab1 + "Домен:      " + domain);
                LogTrace.WriteInLog(tab1 + "Домен:      " + domain);
                listBox1.Items.Add(tab1 + "Домен: " + domain);
            }

            //listBox1.Items.Add("");
            LogForClickers.WriteInLog("");
            LogTrace.WriteInLog("");
        }

        public void EditAndCheckClient()
        {
            listBox1.Items.Add(
                tab2 + "===============================Редактирование клиента====================================");
            LogForClickers.WriteInLog(tab2 + "===============================Редактирование клиента====================================");
            LogForClickers.WriteInLog(tab2 + DateTime.Now.ToString());
            LogTrace.WriteInLog(tab2 + "===============================Редактирование клиента====================================");
            LogTrace.WriteInLog(tab2 + DateTime.Now.ToString());

            listBox1.Items.Add(tab2 + "ID клиента: " + Registry.hashTable["clientId"]);
            LogForClickers.WriteInLog(tab2 + "ID клиента: " + Registry.hashTable["clientId"]);
            LogTrace.WriteInLog(tab2 + "ID клиента: " + Registry.hashTable["clientId"]);

            GoodsEditClientController editClientController = new GoodsEditClientController();

            editClientController.EditClient();
            
            List<string> errors = editClientController.Errors; //парсим со страницы список ошибок

            if (errors.Count != 0) //список непустой -- ошибки есть
            {
                listBox1.ForeColor = Color.Red;
                listBox1.Items.Add("");
                listBox1.Items.Add(tab3 + "!!! Ошибки !!!");

                LogForClickers.WriteInLog("");
                LogForClickers.WriteInLog(tab3 + "!!! Ошибки !!!");

                LogTrace.WriteInLog("");
                LogTrace.WriteInLog(tab3 + "!!! Ошибки !!!");

                for (int i = 0; i < errors.Count; i++)
                {
                    listBox1.Items.Add(errors[i]);
                    LogForClickers.WriteInLog(errors[i]);
                    LogTrace.WriteInLog(errors[i]);
                }
            }
            else
            {
                //listBox1.ForeColor = Color.Black;
                listBox1.Items.Add(tab2 + "Клиент успешно отредактирован");

                listBox1.Items.Add(
                tab2 + "===============================Проверка редактирования клиента===========================");
                LogForClickers.WriteInLog(tab2 + "===============================Проверка редактирования клиента===========================");
                LogTrace.WriteInLog(tab2 + "===============================Проверка редактирования клиента===========================");

                listBox1.Items.Add(tab2 + "ID клиента: " + Registry.hashTable["clientId"]);
                LogForClickers.WriteInLog(tab2 + "ID клиента: " + Registry.hashTable["clientId"]);
                LogTrace.WriteInLog(tab2 + "ID клиента: " + Registry.hashTable["clientId"]);

                editClientController.CheckEditingClient();

                if (editClientController.WasMismatch)
                {
                    listBox1.ForeColor = Color.Red;
                    listBox1.Items.Add("");
                    listBox1.Items.Add(tab2 + "!!!   Обнаружены несовпадения. См. лог");
                    listBox1.Items.Add("");
                    LogForClickers.WriteInLog("");
                    LogForClickers.WriteInLog(tab2 + "!!!   Обнаружены несовпадения. См. лог");
                    LogForClickers.WriteInLog("");
                    LogTrace.WriteInLog("");
                    LogTrace.WriteInLog(tab2 + "!!!   Обнаружены несовпадения");
                    LogTrace.WriteInLog("");
                }
                else
                {
                    listBox1.Items.Add(tab2 + "ОК, всё ранее введенное совпадает с текущими значениями");
                }
            }

            LogForClickers.WriteInLog("");
            LogTrace.WriteInLog("");
        }

        public void EditAndCheckSite()
        {
            listBox1.Items.Add(
                tab2 + "===============================Редактирование сайта====================================");
            LogForClickers.WriteInLog(tab2 + "===============================Редактирование сайта====================================");
            LogForClickers.WriteInLog(tab2 + DateTime.Now.ToString());
            LogTrace.WriteInLog(tab2 + "===============================Редактирование сайта====================================");
            LogTrace.WriteInLog(tab2 + DateTime.Now.ToString());

            listBox1.Items.Add(tab2 + "ID сайта: " + Registry.hashTable["siteId"]);
            LogForClickers.WriteInLog(tab2 + "ID сайта: " + Registry.hashTable["siteId"]);
            LogTrace.WriteInLog(tab2 + "ID сайта: " + Registry.hashTable["siteId"]);

            listBox1.Items.Add(tab2 + "ID клиента: " + Registry.hashTable["clientId"]);
            LogForClickers.WriteInLog(tab2 + "ID клиента: " + Registry.hashTable["clientId"]);
            LogTrace.WriteInLog(tab2 + "ID клиента: " + Registry.hashTable["clientId"]);

            GoodsEditSiteController editSiteController = new GoodsEditSiteController();

            editSiteController.EditSite();

            List<string> errors = editSiteController.Errors;

            if (errors.Count != 0) //список непустой -- ошибки есть
            {
                listBox1.ForeColor = Color.Red;
                listBox1.Items.Add("");
                listBox1.Items.Add(tab3 + "!!! Ошибки !!!");

                LogForClickers.WriteInLog("");
                LogForClickers.WriteInLog(tab3 + "!!! Ошибки !!!");

                LogTrace.WriteInLog("");
                LogTrace.WriteInLog(tab3 + "!!! Ошибки !!!");

                for (int i = 0; i < errors.Count; i++)
                {
                    listBox1.Items.Add(errors[i]);
                    LogForClickers.WriteInLog(errors[i]);
                    LogTrace.WriteInLog(errors[i]);
                }
            }
            else
            {
                //listBox1.ForeColor = Color.Black;
                listBox1.Items.Add(tab2 + "Сайт успешно отредактирован");

                listBox1.Items.Add(
                tab2 + "===============================Проверка редактирования сайта===========================");
                LogForClickers.WriteInLog(tab2 + "===============================Проверка редактирования сайта===========================");
                LogTrace.WriteInLog(tab2 + "===============================Проверка редактирования сайта===========================");

                listBox1.Items.Add(tab2 + "ID сайта: " + Registry.hashTable["siteId"]);
                LogForClickers.WriteInLog(tab2 + "ID сайта: " + Registry.hashTable["siteId"]);
                LogTrace.WriteInLog(tab2 + "ID сайта: " + Registry.hashTable["siteId"]);

                listBox1.Items.Add(tab2 + "ID клиента: " + Registry.hashTable["clientId"]);
                LogForClickers.WriteInLog(tab2 + "ID клиента: " + Registry.hashTable["clientId"]);
                LogTrace.WriteInLog(tab2 + "ID клиента: " + Registry.hashTable["clientId"]);
                
                editSiteController.CheckEditingSite();

                if (editSiteController.WasMismatch)
                {
                    listBox1.ForeColor = Color.Red;
                    listBox1.Items.Add("");
                    listBox1.Items.Add(tab2 + "!!!   Обнаружены несовпадения. См. лог");
                    listBox1.Items.Add("");
                    LogForClickers.WriteInLog("");
                    LogForClickers.WriteInLog(tab2 + "!!!   Обнаружены несовпадения. См. лог");
                    LogForClickers.WriteInLog("");
                }
                else
                {
                    listBox1.Items.Add(tab2 + "ОК, всё ранее введенное совпадает с текущими значениями");
                }
            }

            LogForClickers.WriteInLog("");
            LogTrace.WriteInLog("");
        }

        public void EditAndCheckPk()
        {
            listBox1.Items.Add(
                tab2 + "===============================Редактирование РК====================================");
            LogForClickers.WriteInLog(tab2 + "===============================Редактирование РК====================================");
            LogForClickers.WriteInLog(tab2 + DateTime.Now.ToString());
            LogTrace.WriteInLog(tab2 + "===============================Редактирование РК====================================");
            LogTrace.WriteInLog(tab2 + DateTime.Now.ToString());

            listBox1.Items.Add(tab2 + "ID РК: " + Registry.hashTable["pkId"]);
            LogForClickers.WriteInLog(tab2 + "ID РК: " + Registry.hashTable["pkId"]);
            LogTrace.WriteInLog(tab2 + "ID РК: " + Registry.hashTable["pkId"]);

            listBox1.Items.Add(tab2 + "ID клиента: " + Registry.hashTable["clientId"]);
            LogForClickers.WriteInLog(tab2 + "ID клиента: " + Registry.hashTable["clientId"]);
            LogTrace.WriteInLog(tab2 + "ID клиента: " + Registry.hashTable["clientId"]);

            GoodsEditPkController editPkController = new GoodsEditPkController();

            editPkController.EditPk();

            List<string> errors = editPkController.Errors;

            if (errors.Count != 0) //список непустой -- ошибки есть
            {
                listBox1.ForeColor = Color.Red;
                listBox1.Items.Add("");
                listBox1.Items.Add(tab3 + "!!! Ошибки !!!");

                LogForClickers.WriteInLog("");
                LogForClickers.WriteInLog(tab3 + "!!! Ошибки !!!");

                LogTrace.WriteInLog("");
                LogTrace.WriteInLog(tab3 + "!!! Ошибки !!!");

                for (int i = 0; i < errors.Count; i++)
                {
                    listBox1.Items.Add(errors[i]);
                    LogForClickers.WriteInLog(errors[i]);
                    LogTrace.WriteInLog(errors[i]);
                }
            }
            else
            {
                //listBox1.ForeColor = Color.Black;
                listBox1.Items.Add(tab2 + "РК успешно отредактирована");

                listBox1.Items.Add(
                tab2 + "===============================Проверка редактирования РК===========================");
                LogForClickers.WriteInLog(tab2 + "===============================Проверка редактирования РК===========================");
                LogTrace.WriteInLog(tab2 + "===============================Проверка редактирования РК===========================");

                listBox1.Items.Add(tab2 + "ID РК: " + Registry.hashTable["pkId"]);
                LogForClickers.WriteInLog(tab2 + "ID РК: " + Registry.hashTable["pkId"]);
                LogTrace.WriteInLog(tab2 + "ID РК: " + Registry.hashTable["pkId"]);

                listBox1.Items.Add(tab2 + "ID клиента: " + Registry.hashTable["clientId"]);
                LogForClickers.WriteInLog(tab2 + "ID клиента: " + Registry.hashTable["clientId"]);
                LogTrace.WriteInLog(tab2 + "ID клиента: " + Registry.hashTable["clientId"]);
                
                editPkController.CheckEditingPk();

                if (editPkController.WasMismatch)
                {
                    listBox1.ForeColor = Color.Red;
                    listBox1.Items.Add("");
                    listBox1.Items.Add(tab2 + "!!!   Обнаружены несовпадения. См. лог");
                    listBox1.Items.Add("");
                    LogForClickers.WriteInLog("");
                    LogForClickers.WriteInLog(tab2 + "!!!   Обнаружены несовпадения. См. лог");
                    LogForClickers.WriteInLog("");
                }
                else
                {
                    listBox1.Items.Add(tab2 + "ОК, всё ранее введенное совпадает с текущими значениями");
                }
            }

            LogForClickers.WriteInLog("");
            LogTrace.WriteInLog("");
        }

        public void EditAndCheckTeaser()
        {
            listBox1.Items.Add(
                tab2 + "===============================Редактирование тизера====================================");
            LogForClickers.WriteInLog(tab2 + "===============================Редактирование тизера====================================");
            LogForClickers.WriteInLog(tab2 + DateTime.Now.ToString());
            LogTrace.WriteInLog(tab2 + "===============================Редактирование тизера====================================");
            LogTrace.WriteInLog(tab2 + DateTime.Now.ToString());

            listBox1.Items.Add(tab2 + "ID тизера: " + Registry.hashTable["teaserId"]);
            LogForClickers.WriteInLog(tab2 + "ID тизера: " + Registry.hashTable["teaserId"]);
            LogTrace.WriteInLog(tab2 + "ID тизера: " + Registry.hashTable["teaserId"]);

            listBox1.Items.Add(tab2 + "ID клиента: " + Registry.hashTable["clientId"]);
            LogForClickers.WriteInLog(tab2 + "ID клиента: " + Registry.hashTable["clientId"]);
            LogTrace.WriteInLog(tab2 + "ID клиента: " + Registry.hashTable["clientId"]);

            listBox1.Items.Add(tab2 + "ID РК: " + Registry.hashTable["pkId"]);
            LogForClickers.WriteInLog(tab2 + "ID РК: " + Registry.hashTable["pkId"]);
            LogTrace.WriteInLog(tab2 + "ID РК: " + Registry.hashTable["pkId"]);

            GoodsEditTeaser_Controller editTeaserController = new GoodsEditTeaser_Controller();

            editTeaserController.EditTeaser();

            List<string> errors = editTeaserController.errors;

            if (errors.Count != 0) //список непустой -- ошибки есть
            {
                listBox1.ForeColor = Color.Red;
                listBox1.Items.Add("");
                listBox1.Items.Add(tab2 + "!!! Ошибки !!!");

                LogForClickers.WriteInLog("");
                LogForClickers.WriteInLog(tab2 + "!!! Ошибки !!!");

                LogTrace.WriteInLog("");
                LogTrace.WriteInLog(tab2 + "!!! Ошибки !!!");

                for (int i = 0; i < errors.Count; i++)
                {
                    listBox1.Items.Add(errors[i]);
                    LogForClickers.WriteInLog(errors[i]);
                    LogTrace.WriteInLog(errors[i]);
                }
            }
            else
            {
                listBox1.Items.Add(tab2 + "Тизер успешно отредактирован");

                listBox1.Items.Add(
                tab2 + "===============================Проверка редактирования тизера===========================");
                LogForClickers.WriteInLog(tab2 + "===============================Проверка редактирования тизера===========================");
                LogTrace.WriteInLog(tab2 + "===============================Проверка редактирования тизера===========================");

                listBox1.Items.Add(tab2 + "ID тизера: " + Registry.hashTable["teaserId"]);
                LogForClickers.WriteInLog(tab2 + "ID тизера: " + Registry.hashTable["teaserId"]);
                LogTrace.WriteInLog(tab2 + "ID тизера: " + Registry.hashTable["teaserId"]);

                listBox1.Items.Add(tab2 + "ID клиента: " + Registry.hashTable["clientId"]);
                LogForClickers.WriteInLog(tab2 + "ID клиента: " + Registry.hashTable["clientId"]);
                LogTrace.WriteInLog(tab2 + "ID клиента: " + Registry.hashTable["clientId"]);

                listBox1.Items.Add(tab2 + "ID РК: " + Registry.hashTable["pkId"]);
                LogForClickers.WriteInLog(tab2 + "ID РК: " + Registry.hashTable["pkId"]);
                LogTrace.WriteInLog(tab2 + "ID РК: " + Registry.hashTable["pkId"]);
                
                editTeaserController.CheckEditingTeaser();

                if (editTeaserController.wasMismatch)
                {
                    listBox1.ForeColor = Color.Red;
                    listBox1.Items.Add("");
                    listBox1.Items.Add(tab2 + "!!!   Обнаружены несовпадения. См. лог");
                    listBox1.Items.Add("");
                    LogForClickers.WriteInLog("");
                    LogForClickers.WriteInLog(tab2 + "!!!   Обнаружены несовпадения. См. лог");
                    LogForClickers.WriteInLog("");
                }
                else
                {
                    listBox1.Items.Add(tab2 + "ОК, всё ранее введенное совпадает с текущими значениями");
                }
            }

            LogForClickers.WriteInLog("");
            LogTrace.WriteInLog("");
        }

        public void PicClaimForTeaser()
        {
            listBox1.Items.Add("===============================Заявка на создание тизеров====================================");
            LogForClickers.WriteInLog("===============================Заявка на создание тизеров====================================");
            LogForClickers.WriteInLog(DateTime.Now.ToString());
            LogTrace.WriteInLog("===============================Заявка на создание тизеров====================================");
            LogTrace.WriteInLog(DateTime.Now.ToString());

            PicClaimForTeaserController claimForTeaserController = new PicClaimForTeaserController();

            claimForTeaserController.ApplyForTeaser();

            List<string> errors = claimForTeaserController.Errors;

            if (errors.Count != 0) //список непустой -- ошибки есть
            {
                listBox1.ForeColor = Color.Red;
                listBox1.Items.Add("");
                listBox1.Items.Add(tab2 + "!!! Ошибки !!!");

                LogForClickers.WriteInLog("");
                LogForClickers.WriteInLog(tab2 + "!!! Ошибки !!!");

                LogTrace.WriteInLog("");
                LogTrace.WriteInLog(tab2 + "!!! Ошибки !!!");

                for (int i = 0; i < errors.Count; i++)
                {
                    listBox1.Items.Add(errors[i]);
                    LogForClickers.WriteInLog(errors[i]);
                    LogTrace.WriteInLog(errors[i]);
                }
            }
            else
            {
                listBox1.Items.Add(string.Format("\nID заявки: {0}", claimForTeaserController.ClaimId));
                LogForClickers.WriteInLog(string.Format("ID заявки: {0}", claimForTeaserController.ClaimId));
                LogTrace.WriteInLog(string.Format("\nID заявки: {0}", claimForTeaserController.ClaimId));

                listBox1.Items.Add("ID клиента: " + Registry.hashTable["clientId"]);
                LogForClickers.WriteInLog("ID клиента: " + Registry.hashTable["clientId"]);
                LogTrace.WriteInLog("ID клиента: " + Registry.hashTable["clientId"]);

                listBox1.Items.Add("ID РК: " + Registry.hashTable["pkId"]);
                LogForClickers.WriteInLog("ID РК: " + Registry.hashTable["pkId"]);
                LogTrace.WriteInLog("ID РК: " + Registry.hashTable["pkId"]);
            }
            LogForClickers.WriteInLog("");
            LogTrace.WriteInLog("");
        }

        public void PicStatistics()
        {
            listBox1.Items.Add("===============================Заявка на создание тизеров====================================");
            LogForClickers.WriteInLog("===============================Заявка на создание тизеров====================================");
            LogForClickers.WriteInLog(DateTime.Now.ToString());
            LogTrace.WriteInLog("===============================Заявка на создание тизеров====================================");
            LogTrace.WriteInLog(DateTime.Now.ToString());

            PicStatisticsController statisticsController = new PicStatisticsController();

            if(statisticsController.ViewStatistics())
                listBox1.ForeColor = Color.Red;
            
        }

        public string NewOrExist;

        private void Button1Click(object sender, EventArgs e)
        {
            if(!newClientCheckbox.Checked)
            {
                newClientCheckbox.Checked = true;
                MessageBox.Show("Не был выбран клиент. Теперь он выбран");
                return;
            }

            if(newTeaserCheckbox.Checked & (!newPKCheckbox.Checked || !newSiteCheckbox.Checked))
            {
                newPKCheckbox.Checked = true;
                newSiteCheckbox.Checked = true;
                MessageBox.Show("Невозможно создать тизер, не создав РК и сайт для него. Теперь РК и сайт выбраны");
                return;
            }

            listBox1.ForeColor = Color.Black;
            int loopClient; //сколько создать клиентов
            int loopSite;//сколько создать сайтов
            int loopPk;//сколько создать РК
            int loopTeaser;//сколько создать тизеров

            if (newClientCheckbox.Checked) NewOrExist = "new";
            else if (existClientCheckbox.Checked) NewOrExist = "exist";

            int result;
            bool isSuccessConversion;
            switch (NewOrExist)
            {
                case "new":
                {
                    isSuccessConversion = int.TryParse(newClientTextBox.Text, out result); //сколько создать клиентов
                    if (!isSuccessConversion)
                    {
                        MessageBox.Show("Поле ввода имеет некорректное значение");
                        return;
                    }
                    loopClient = result;


                    isSuccessConversion = int.TryParse(newSiteTextBox.Text, out result);//сколько создать сайтов
                    if (!isSuccessConversion)
                    {
                        MessageBox.Show("Поле ввода имеет некорректное значение");
                        return;
                    }
                    loopSite = result;

                    isSuccessConversion = int.TryParse(newPKTextBox.Text, out result);//сколько создать РК
                    if (!isSuccessConversion)
                    {
                        MessageBox.Show("Поле ввода имеет некорректное значение");
                        return;
                    }
                    loopPk = result;

                    isSuccessConversion = int.TryParse(newTeaserTextBox.Text, out result);//сколько создать тизеров
                    if (!isSuccessConversion)
                    {
                        MessageBox.Show("Поле ввода имеет некорректное значение");
                        return;
                    }
                    loopTeaser = result;

                    break;
                }
                case "exist":
                {
                    isSuccessConversion = int.TryParse(existClientTextBox.Text, out result); //сколько создать клиентов
                    if (!isSuccessConversion)
                    {
                        MessageBox.Show("Поле ввода имеет некорректное значение");
                        return;
                    }
                    loopClient = result;


                    isSuccessConversion = int.TryParse(existSiteTextBox.Text, out result);//сколько создать сайтов
                    if (!isSuccessConversion)
                    {
                        MessageBox.Show("Поле ввода имеет некорректное значение");
                        return;
                    }
                    loopSite = result;

                    isSuccessConversion = int.TryParse(existPKTextBox.Text, out result);//сколько создать РК
                    if (!isSuccessConversion)
                    {
                        MessageBox.Show("Поле ввода имеет некорректное значение");
                        return;
                    }
                    loopPk = result;

                    isSuccessConversion = int.TryParse(existTeaserTextBox.Text, out result);//сколько создать тизеров
                    if (!isSuccessConversion)
                    {
                        MessageBox.Show("Поле ввода имеет некорректное значение");
                        return;
                    }
                    loopTeaser = result;
                    break;
                }
                default:
                {
                    loopClient = 1; //сколько создать клиентов
                    loopSite = 1;//сколько создать сайтов
                    loopPk = 1;//сколько создать РК
                    loopTeaser = 1;//сколько создать тизеров
                    break;
                }
            }
            
            try //пробуем создать цепочку согласно выбранным на форме настройкам
            {
                if (openFileDialog1.FileName != null)
                {
                    for (int i = loopClient; i > 0 ; i--)
                    {
                        CreateNewClient();
                        
                        if(editClientCheckBox.Checked)
                            EditAndCheckClient();
                        
                        if (newSiteCheckbox.Checked)
                        {
                            for (int j = loopSite; j > 0; j--)
                            {
                                CreateNewSite();
                                if(editSiteCheckBox.Checked)
                                    EditAndCheckSite();
                            }
                        }
                        if (newPKCheckbox.Checked)
                        {
                            for (int x = loopPk; x > 0; x--)
                            {
                                CreateNewPk();
                                if (editPkCheckBox.Checked)
                                    EditAndCheckPk();
                                if (newTeaserCheckbox.Checked)
                                {
                                    for (int z = loopTeaser; z > 0; z--)
                                    {
                                        CreateNewTeaser();
                                        if (editTeaserCheckBox.Checked)
                                            EditAndCheckTeaser();
                                    }
                                }
                                if (claimForTeaserCheckBox.Checked)
                                    PicClaimForTeaser();

                                if (statisticsCheckBox.Checked)
                                    PicStatistics();
                            }
                        }
                    }
                }
            }
            
            catch(Exception ex)
            {
                MessageBox.Show("Ой, что-то пошло не так!");
            }

            finally
            {
                LogForClickers.CloseLogFile();
                LogTrace.CloseLogFile();
            }
            LogForClickers.WriteInLog("");
            LogTrace.WriteInLog("");
        }

        private void Button2Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            //textBox1.Text = openFileDialog1.FileName;
        }

        private void checkBox3_Click(object sender, EventArgs e) //выбран Новый клиент
        {
            //NewOrExist = "new"; //когда выбран Новый клиент - стягиваем кол-во из левых инпутов

            existClientCheckbox.Checked = false; //снимаем выбор с Существующего клиента

            existSiteCheckbox.Checked = false; //снимаем выбор с Сайта
            existPKCheckbox.Checked = false; //снимаем выбор с РК
            existTeaserCheckbox.Checked = false; //снимаем выбор с Тизера

            //newSiteCheckbox.Checked = true; //делаем выбор Сайта
            //newPKCheckbox.Checked = true; //делаем выбор РК
            //newTeaserCheckbox.Checked = true; //делаем выбор Тизера
        }

        private void checkBox10_Click(object sender, EventArgs e) //выбран Существующий клиент
        {
            //NewOrExist = "exist"; //когда выбран Существующий клиент - стягиваем кол-во из правых инпутов

            newClientCheckbox.Checked = false; //снимаем выбор с Нового клиента

            newSiteCheckbox.Checked = false; //снимаем выбор с Сайта
            newPKCheckbox.Checked = false; //снимаем выбор с РК
            newTeaserCheckbox.Checked = false; //снимаем выбор с Тизера

            //existSiteCheckbox.Checked = true; //делаем выбор Сайта
            //existPKCheckbox.Checked = true; //делаем выбор РК
            //existTeaserCheckbox.Checked = true; //делаем выбор Тизера
        }

        private void checkBox6_Click(object sender, EventArgs e) //выбраны Тизеры для Нового клиента
        {
            //newPKCheckbox.Checked = true; //выбираем также РК
        }

        private void checkBox7_Click(object sender, EventArgs e)//выбраны Тизеры для Сущ. клиента
        {
            //existPKCheckbox.Checked = true; //выбираем также РК
        }

        private void button3_Click(object sender, EventArgs e) //очистить listbox
        {
            listBox1.Items.Clear();
            listBox1.ForeColor = Color.Black;
        }

        private void claimForTeaserPic_Click_1(object sender, EventArgs e)
        {
            if (claimForTeaserPic.BackColor == SystemColors.Control)
            {
                claimForTeaserPic.BackColor = Color.OrangeRed;
                claimForTeaserCheckBox.Checked = true;
            }

            else
            {
                claimForTeaserPic.BackColor = SystemColors.Control;
                claimForTeaserCheckBox.Checked = false;
            }
        }

       private void newTeaserCheckbox_Click(object sender, EventArgs e)
        {
            if(newTeaserCheckbox.Checked & (!newSiteCheckbox.Checked || !newPKCheckbox.Checked))
            {
                newSiteCheckbox.Checked = true;
                newPKCheckbox.Checked = true;
            }
        }

       private void statisticsPic_Click(object sender, EventArgs e)
       {
           if (statisticsPic.BackColor == SystemColors.Control)
           {
               statisticsPic.BackColor = Color.OrangeRed;
               statisticsCheckBox.Checked = true;
           }

           else
           {
               statisticsPic.BackColor = SystemColors.Control;
               statisticsCheckBox.Checked = false;
           }
       }
    }
}
