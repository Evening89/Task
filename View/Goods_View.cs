using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Task.Model;
using Task.Controller;
using Task.Utils;

namespace Task.View
{
    public partial class Goods_View : Form
    {
        public Goods_View()
        {
            InitializeComponent();
            textBox1.Text = "Auth.csv";
        }

        //public StreamWriter sw; //запись в файл
       
        public void CreateNewClient()
        {
            listBox1.Items.Add(
                "===============================Cоздание клиента по Товарам===============================");
            LogForClickers.WriteInLog("===============================Cоздание клиента по Товарам===============================");
            LogForClickers.WriteInLog(DateTime.Now.ToString());
            LogTrace.WriteInLog("===============================Cоздание клиента по Товарам===============================");
            LogTrace.WriteInLog(DateTime.Now.ToString());

            GoodsCreateClient_Controller clientController = new GoodsCreateClient_Controller();
            //clientController.CreateClient(openFileDialog1.FileName, checkBox2.Checked);
            clientController.CreateClient("Auth.csv", checkBox1.Checked, checkBox2.Checked);

            List<string> errors = clientController.errors;
            
            if (errors.Count != 0) //список непустой -- ошибки есть
            {
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
                string clientId = clientController.clientId;

                LogTrace.WriteInLog("");
                LogForClickers.WriteInLog("ID клиента: " + clientId);
                LogTrace.WriteInLog("ID клиента: " + clientId);
                listBox1.Items.Add("ID клиента: " + clientId);

                LogForClickers.WriteInLog("Логин:      " + clientController.login);
                LogTrace.WriteInLog("Логин:      " + clientController.login);
                listBox1.Items.Add("Логин:         " + clientController.login);

                LogForClickers.WriteInLog("Пароль:     " + clientController.password);
                LogTrace.WriteInLog("Пароль:     " + clientController.password);
                listBox1.Items.Add("Пароль:         " + clientController.password);
            }

            //listBox1.Items.Add("");
            LogForClickers.WriteInLog("");
            LogTrace.WriteInLog("");
        }

        public void CreateNewSite()
        {
            listBox1.Items.Add(
                "     ===============================Cоздание сайта по Товарам===============================");
            LogForClickers.WriteInLog("     ===============================Cоздание сайта по Товарам===============================");
            LogForClickers.WriteInLog("     " + DateTime.Now.ToString());
            LogTrace.WriteInLog("     ===============================Cоздание сайта по Товарам===============================");
            LogTrace.WriteInLog("     " + DateTime.Now.ToString());

            GoodsCreateSite_Controller siteController = new GoodsCreateSite_Controller();
            siteController.CreateSite(checkBox1.Checked, checkBox2.Checked);

            List<string> errors = siteController.errors; //парсим со страницы список ошибок

            if (errors.Count != 0) //список непустой -- ошибки есть
            {
                listBox1.Items.Add("");
                listBox1.Items.Add("     !!! Ошибки !!!");

                LogForClickers.WriteInLog("");
                LogForClickers.WriteInLog("     !!! Ошибки !!!");

                LogTrace.WriteInLog("");
                LogTrace.WriteInLog("     !!! Ошибки !!!");

                for (int i = 0; i < errors.Count; i++)
                {
                    listBox1.Items.Add(errors[i]);
                    LogForClickers.WriteInLog(errors[i]);
                    LogTrace.WriteInLog(errors[i]);
                }
            }
            else
            {
                string siteId = siteController.siteId;
                string clientId = siteController.clientId;
                string siteDomain = siteController.siteDomain;
                //string siteName = siteController.siteName;

                LogTrace.WriteInLog("");
                LogForClickers.WriteInLog("     ID сайта: " + siteId);
                LogTrace.WriteInLog("     ID сайта: " + siteId);
                listBox1.Items.Add("     ID сайта: " + siteId);

                LogForClickers.WriteInLog("     ID клиента: " + clientId);
                LogTrace.WriteInLog("     ID клиента: " + clientId);
                listBox1.Items.Add("     ID клиента: " + clientId);

                LogForClickers.WriteInLog("     Домен:    " + siteDomain);
                LogTrace.WriteInLog("     Домен:    " + siteDomain);
                listBox1.Items.Add("     Домен:     " + siteDomain);
            }

            //listBox1.Items.Add("");
            LogForClickers.WriteInLog("");
            LogTrace.WriteInLog("");
        }

        public void CreateNewPk()
        {
            listBox1.Items.Add(
                "     ===============================Cоздание РК по Товарам===============================");
            LogForClickers.WriteInLog("     ===============================Cоздание РК по Товарам===============================");
            LogForClickers.WriteInLog("     " + DateTime.Now.ToString());
            LogTrace.WriteInLog("     ===============================Cоздание РК по Товарам===============================");
            LogTrace.WriteInLog("     " + DateTime.Now.ToString());

            GoodsCreatePk_Controller pkController = new GoodsCreatePk_Controller();
            pkController.CreatePk(checkBox1.Checked, checkBox2.Checked);

            List<string> errors = pkController.errors; //парсим со страницы список ошибок

            if (errors.Count != 0) //список непустой -- ошибки есть
            {
                listBox1.Items.Add("");
                listBox1.Items.Add("     !!! Ошибки !!!");

                LogForClickers.WriteInLog("");
                LogForClickers.WriteInLog("     !!! Ошибки !!!");

                LogTrace.WriteInLog("");
                LogTrace.WriteInLog("     !!! Ошибки !!!");

                for (int i = 0; i < errors.Count; i++)
                {
                    listBox1.Items.Add(errors[i]);
                    LogForClickers.WriteInLog(errors[i]);
                    LogTrace.WriteInLog(errors[i]);
                }
            }
            else
            {
                string pkId = pkController.pkId;
                string clientId = pkController.clientId;
                string pkName = pkController.pkName;

                LogTrace.WriteInLog("");
                LogForClickers.WriteInLog("     ID РК:       " + pkId);
                LogTrace.WriteInLog("     ID РК:       " + pkId);
                listBox1.Items.Add("     ID РК: " + pkId);

                LogForClickers.WriteInLog("     ID клиента: " + clientId);
                LogTrace.WriteInLog("     ID клиента: " + clientId);
                listBox1.Items.Add("     ID клиента: " + clientId);

                LogForClickers.WriteInLog("     Название РК: " + pkName);
                LogTrace.WriteInLog("     Название РК: " + pkName);
                listBox1.Items.Add("     Название РК:     " + pkName);
            }

            //listBox1.Items.Add("");
            LogForClickers.WriteInLog("");
            LogTrace.WriteInLog("");
        }

        public void CreateNewTeaser()
        {
            listBox1.Items.Add(
                "          ===============================Cоздание тизера по Товарам===============================");
            LogForClickers.WriteInLog("     ===============================Cоздание тизера по Товарам===============================");
            LogForClickers.WriteInLog("     " + DateTime.Now.ToString());
            LogTrace.WriteInLog("     ===============================Cоздание тизера по Товарам===============================");
            LogTrace.WriteInLog("     " + DateTime.Now.ToString());

            GoodsCreateTeaser_Controller teaserController = new GoodsCreateTeaser_Controller();
            teaserController.CreateTeaser(checkBox1.Checked, checkBox2.Checked);

            List<string> errors = teaserController.errors; //парсим со страницы список ошибок

            if (errors.Count != 0) //список непустой -- ошибки есть
            {
                listBox1.Items.Add("");
                listBox1.Items.Add("     !!! Ошибки !!!");

                LogForClickers.WriteInLog("");
                LogForClickers.WriteInLog("     !!! Ошибки !!!");

                LogTrace.WriteInLog("");
                LogTrace.WriteInLog("     !!! Ошибки !!!");

                for (int i = 0; i < errors.Count; i++)
                {
                    listBox1.Items.Add(errors[i]);
                    LogForClickers.WriteInLog(errors[i]);
                    LogTrace.WriteInLog(errors[i]);
                }
            }
            else
            {
                string teaserId = teaserController.teaserId;
                string pkId = teaserController.pkId;
                string clientId = teaserController.clientId;
                string domain = teaserController.allowedDomain;

                LogTrace.WriteInLog("");
                LogForClickers.WriteInLog("     ID тизера: " + teaserId);
                LogTrace.WriteInLog("     ID тизера: " + teaserId);
                listBox1.Items.Add("          ID тизера: " + teaserId);

                LogForClickers.WriteInLog("     ID клиента: " + clientId);
                LogTrace.WriteInLog("     ID клиента: " + clientId);
                listBox1.Items.Add("          ID клиента:     " + clientId);
                
                LogForClickers.WriteInLog("     ID РК:      " + pkId);
                LogTrace.WriteInLog("     ID РК:      " + pkId);
                listBox1.Items.Add("          ID РК:     " + pkId);

                LogForClickers.WriteInLog("     Домен:      " + domain);
                LogTrace.WriteInLog("     Домен:      " + domain);
                listBox1.Items.Add("          Домен: " + domain);
            }

            //listBox1.Items.Add("");
            LogForClickers.WriteInLog("");
            LogTrace.WriteInLog("");
        }

        public void EditAndCheckClient()
        {
            listBox1.Items.Add(
                "          ===============================Редактирование клиента====================================");
            LogForClickers.WriteInLog("          ===============================Редактирование клиента====================================");
            LogForClickers.WriteInLog("          " + DateTime.Now.ToString());
            LogTrace.WriteInLog("          ===============================Редактирование клиента====================================");
            LogTrace.WriteInLog("          " + DateTime.Now.ToString());

            listBox1.Items.Add("          ID клиента: " + Registry.hashTable["clientId"]);
            LogForClickers.WriteInLog("          ID клиента: " + Registry.hashTable["clientId"]);
            LogTrace.WriteInLog("          ID клиента: " + Registry.hashTable["clientId"]);

            GoodsEditClient_Controller editClientController = new GoodsEditClient_Controller();

            editClientController.EditClient();
            
            List<string> errors = editClientController.errors; //парсим со страницы список ошибок

            if (errors.Count != 0) //список непустой -- ошибки есть
            {
                listBox1.Items.Add("");
                listBox1.Items.Add("               !!! Ошибки !!!");

                LogForClickers.WriteInLog("");
                LogForClickers.WriteInLog("               !!! Ошибки !!!");

                LogTrace.WriteInLog("");
                LogTrace.WriteInLog("               !!! Ошибки !!!");

                for (int i = 0; i < errors.Count; i++)
                {
                    listBox1.Items.Add(errors[i]);
                    LogForClickers.WriteInLog(errors[i]);
                    LogTrace.WriteInLog(errors[i]);
                }
            }
            else
            {
                listBox1.Items.Add("          Клиент успешно отредактирован");

                listBox1.Items.Add(
                "          ===============================Проверка редактирования клиента===========================");
                LogForClickers.WriteInLog("          ===============================Проверка редактирования клиента===========================");
                LogTrace.WriteInLog("          ===============================Проверка редактирования клиента===========================");

                listBox1.Items.Add("          ID клиента: " + Registry.hashTable["clientId"]);
                LogForClickers.WriteInLog("          ID клиента: " + Registry.hashTable["clientId"]);
                LogTrace.WriteInLog("          ID клиента: " + Registry.hashTable["clientId"]);

                editClientController.CheckEditingClient();

                if (editClientController.wasMismatch)
                {
                    listBox1.Items.Add("");
                    listBox1.Items.Add("          !!!   Обнаружены несовпадения. См. лог");
                    listBox1.Items.Add("");
                    LogForClickers.WriteInLog("");
                    LogForClickers.WriteInLog("          !!!   Обнаружены несовпадения. См. лог");
                    LogForClickers.WriteInLog("");
                    LogTrace.WriteInLog("");
                    LogTrace.WriteInLog("          !!!   Обнаружены несовпадения");
                    LogTrace.WriteInLog("");
                }
                else
                {
                    listBox1.Items.Add("          ОК, всё ранее введенное совпадает с текущими значениями");
                }
            }

            LogForClickers.WriteInLog("");
            LogTrace.WriteInLog("");
        }

        public void EditAndCheckSite()
        {
            listBox1.Items.Add(
                "          ===============================Редактирование сайта====================================");
            LogForClickers.WriteInLog("          ===============================Редактирование сайта====================================");
            LogForClickers.WriteInLog("          " + DateTime.Now.ToString());
            LogTrace.WriteInLog("          ===============================Редактирование сайта====================================");
            LogTrace.WriteInLog("          " + DateTime.Now.ToString());

            listBox1.Items.Add("          ID клиента: " + Registry.hashTable["clientId"]);
            LogForClickers.WriteInLog("          ID клиента: " + Registry.hashTable["clientId"]);
            LogTrace.WriteInLog("          ID клиента: " + Registry.hashTable["clientId"]);
            listBox1.Items.Add("          ID сайта: " + Registry.hashTable["siteId"]);
            LogForClickers.WriteInLog("          ID сайта: " + Registry.hashTable["siteId"]);
            LogTrace.WriteInLog("          ID сайта: " + Registry.hashTable["siteId"]);

            GoodsEditSite_Controller editSiteController = new GoodsEditSite_Controller();

            editSiteController.EditSite();

            List<string> errors = editSiteController.errors;

            if (errors.Count != 0) //список непустой -- ошибки есть
            {
                listBox1.Items.Add("");
                listBox1.Items.Add("               !!! Ошибки !!!");

                LogForClickers.WriteInLog("");
                LogForClickers.WriteInLog("               !!! Ошибки !!!");

                LogTrace.WriteInLog("");
                LogTrace.WriteInLog("               !!! Ошибки !!!");

                for (int i = 0; i < errors.Count; i++)
                {
                    listBox1.Items.Add(errors[i]);
                    LogForClickers.WriteInLog(errors[i]);
                    LogTrace.WriteInLog(errors[i]);
                }
            }
            else
            {
                listBox1.Items.Add("          Сайт успешно отредактирован");

                listBox1.Items.Add(
                "          ===============================Проверка редактирования сайта===========================");
                LogForClickers.WriteInLog("          ===============================Проверка редактирования сайта===========================");
                LogTrace.WriteInLog("          ===============================Проверка редактирования сайта===========================");

                listBox1.Items.Add("          ID клиента: " + Registry.hashTable["clientId"]);
                LogForClickers.WriteInLog("          ID клиента: " + Registry.hashTable["clientId"]);
                LogTrace.WriteInLog("          ID клиента: " + Registry.hashTable["clientId"]);
                listBox1.Items.Add("          ID сайта: " + Registry.hashTable["siteId"]);
                LogForClickers.WriteInLog("          ID сайта: " + Registry.hashTable["siteId"]);
                LogTrace.WriteInLog("          ID сайта: " + Registry.hashTable["siteId"]);

                editSiteController.CheckEditingSite();

                if (editSiteController.wasMismatch)
                {
                    listBox1.Items.Add("");
                    listBox1.Items.Add("          !!!   Обнаружены несовпадения. См. лог");
                    listBox1.Items.Add("");
                    LogForClickers.WriteInLog("");
                    LogForClickers.WriteInLog("          !!!   Обнаружены несовпадения. См. лог");
                    LogForClickers.WriteInLog("");
                }
                else
                {
                    listBox1.Items.Add("          ОК, всё ранее введенное совпадает с текущими значениями");
                }
            }

            LogForClickers.WriteInLog("");
            LogTrace.WriteInLog("");
        }

        public void EditAndCheckPk()
        {
            listBox1.Items.Add(
                "          ===============================Редактирование РК====================================");
            LogForClickers.WriteInLog("          ===============================Редактирование РК====================================");
            LogForClickers.WriteInLog("          " + DateTime.Now.ToString());
            LogTrace.WriteInLog("          ===============================Редактирование РК====================================");
            LogTrace.WriteInLog("          " + DateTime.Now.ToString());

            listBox1.Items.Add("          ID клиента: " + Registry.hashTable["clientId"]);
            LogForClickers.WriteInLog("          ID клиента: " + Registry.hashTable["clientId"]);
            LogTrace.WriteInLog("          ID клиента: " + Registry.hashTable["clientId"]);
            listBox1.Items.Add("          ID РК: " + Registry.hashTable["pkId"]);
            LogForClickers.WriteInLog("          ID РК: " + Registry.hashTable["pkId"]);
            LogTrace.WriteInLog("          ID РК: " + Registry.hashTable["pkId"]);

            GoodsEditPk_Controller editPkController = new GoodsEditPk_Controller();

            editPkController.EditPk();

            List<string> errors = editPkController.errors;

            if (errors.Count != 0) //список непустой -- ошибки есть
            {
                listBox1.Items.Add("");
                listBox1.Items.Add("               !!! Ошибки !!!");

                LogForClickers.WriteInLog("");
                LogForClickers.WriteInLog("               !!! Ошибки !!!");

                LogTrace.WriteInLog("");
                LogTrace.WriteInLog("               !!! Ошибки !!!");

                for (int i = 0; i < errors.Count; i++)
                {
                    listBox1.Items.Add(errors[i]);
                    LogForClickers.WriteInLog(errors[i]);
                    LogTrace.WriteInLog(errors[i]);
                }
            }
            else
            {
                listBox1.Items.Add("          РК успешно отредактирована");

                listBox1.Items.Add(
                "          ===============================Проверка редактирования РК===========================");
                LogForClickers.WriteInLog("          ===============================Проверка редактирования РК===========================");
                LogTrace.WriteInLog("          ===============================Проверка редактирования РК===========================");

                listBox1.Items.Add("          ID клиента: " + Registry.hashTable["clientId"]);
                LogForClickers.WriteInLog("          ID клиента: " + Registry.hashTable["clientId"]);
                LogTrace.WriteInLog("          ID клиента: " + Registry.hashTable["clientId"]);
                listBox1.Items.Add("          ID РК: " + Registry.hashTable["pkId"]);
                LogForClickers.WriteInLog("          ID РК: " + Registry.hashTable["pkId"]);
                LogTrace.WriteInLog("          ID РК: " + Registry.hashTable["pkId"]);

                editPkController.CheckEditingPk();

                if (editPkController.wasMismatch)
                {
                    listBox1.Items.Add("");
                    listBox1.Items.Add("          !!!   Обнаружены несовпадения. См. лог");
                    listBox1.Items.Add("");
                    LogForClickers.WriteInLog("");
                    LogForClickers.WriteInLog("          !!!   Обнаружены несовпадения. См. лог");
                    LogForClickers.WriteInLog("");
                }
                else
                {
                    listBox1.Items.Add("          ОК, всё ранее введенное совпадает с текущими значениями");
                }
            }

            LogForClickers.WriteInLog("");
            LogTrace.WriteInLog("");
        }


        private void Button1Click(object sender, EventArgs e)
        {
            int loopClient; //сколько создать клиентов
            int loopSite;//сколько создать сайтов
            int loopPK;//сколько создать РК
            int loopTeaser;//сколько создать тизеров

            switch (NewOrExist)
            {
                case "new":
                {
                    loopClient = int.Parse(newClientTextBox.Text); //сколько создать клиентов
                    loopSite = int.Parse(newSiteTextBox.Text);//сколько создать сайтов
                    loopPK = int.Parse(newPKTextBox.Text);//сколько создать РК
                    loopTeaser = int.Parse(newTeaserTextBox.Text);//сколько создать тизеров
                    break;
                }
                case "exist":
                {
                    loopClient = int.Parse(existClientTextBox.Text); //сколько создать клиентов
                    loopSite = int.Parse(existSiteTextBox.Text);//сколько создать сайтов
                    loopPK = int.Parse(existPKTextBox.Text);//сколько создать РК
                    loopTeaser = int.Parse(existTeaserTextBox.Text);//сколько создать тизеров
                    break;
                }
                default:
                {
                    loopClient = 1; //сколько создать клиентов
                    loopSite = 1;//сколько создать сайтов
                    loopPK = 1;//сколько создать РК
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
                            for (int x = loopPK; x > 0; x--)
                            {
                                CreateNewPk();
                                if (editPkCheckBox.Checked)
                                    EditAndCheckPk();
                                if (newTeaserCheckbox.Checked)
                                {
                                    for (int z = loopTeaser; z > 0; z--)
                                        CreateNewTeaser();
                                }
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
        }

        private void Button2Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            //textBox1.Text = openFileDialog1.FileName;
        }

        public string NewOrExist;

        private void checkBox3_Click(object sender, EventArgs e) //выбран Новый клиент
        {
            NewOrExist = "new"; //когда выбран Новый клиент - стягиваем кол-во из левых инпутов

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
            NewOrExist = "exist"; //когда выбран Существующий клиент - стягиваем кол-во из правых инпутов

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

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }
    }
}
