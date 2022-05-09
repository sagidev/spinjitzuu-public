using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using MetroSet_UI;
using MetroSet_UI.Forms;
using MySql.Data.MySqlClient;

namespace spinjitzuu3
{
    public partial class Form1 : MetroSetForm
    {
        public static string[] configArray = new string[4] { "", "500", "", "" };
        public static bool isLogged = false;
        public static bool inGame = false;
        public static Key spacegliderKey = Key.C;
        public static Key attackMoveKey = Key.N;
        public static Key flashKey = Key.F;

        public static short fKey = 33;
        public static short dKey = 32;

        public static float windupPercentage = 0.2f;

        public static string savedLogin = "";
        public static string savedPass = "";

        static Cast cast = new Cast();
        static Spin spin = new Spin();
        static Input input = new Input();
        consts consts = new consts();

        public Form1()
        {
            InitializeComponent();
        }

        
        public void ReadConfig()
        {
            int i = 0;
            foreach (string line in System.IO.File.ReadLines(@"config.txt"))
            {

                //for(int i = 0; i<3; i++)
                //{
                //    configArray[i] = line;
                //}
                configArray[i] = line;
                i++;
            }
            if (configArray[0] == "C")
            {
                spacegliderKey = Key.C;
                //Console.WriteLine("Spaceglider key set to C");
                
                BeginInvoke(new Action(() =>
                {
                    spacegliderkeyLabel.Text = "C";
                }));
            }
            if (configArray[0] == "Space")
            {
                spacegliderKey = Key.Space;
                //Console.WriteLine("Spaceglider key set to Space");
                BeginInvoke(new Action(() =>
                {
                    spacegliderkeyLabel.Text = "Space";
                }));
            }
            //string scanconfig = configArray[2];
            //ScanSleeper = Convert.ToInt32(configArray[1]);
            if (configArray[2] != "")
            {
                savedLogin = configArray[2];
            }
            if (configArray[3] != "")
            {
                savedPass = configArray[3];
            }
            //if(configArray[0]==1)
            //{
            //    MessageBox.Show("spacja");
            //}
            //if (configArray[0] == 2)
            //{
            //    MessageBox.Show("C");
            //}
        }

        public void UpdateGUI()
        {
            BeginInvoke(new Action(() =>
            {
                enemiesLbl.Text = consts.enemyHPArrayGlobal.Length.ToString();
                //input.SetPosition(consts.enemyHPArrayGlobal.First().X, consts.enemyHPArrayGlobal.First().Y);
                alliesLbl.Text = consts.myHPArrayGlobal.Length.ToString();
            }));
        }

        private void flashDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (flashDropdown.SelectedIndex == 0)
            {
                flashKey = Key.D;
            }
            if (flashDropdown.SelectedIndex == 1)
            {
                flashKey = Key.F;
            }
        }

        private void windupBar_Scroll(object sender)
        {
            Spin.additionalWindup = windupBar.Value-50;
            additionalWindupValueLbl.Text = Spin.additionalWindup.ToString();
            windupLbl.Text = windupBar.Value.ToString() + "ms";
        }

        private void scanningBar_Scroll(object sender)
        {
            Spin.ScanWait = scanningBar.Value;
            scanningValueLbl.Text = scanningBar.Value.ToString();
            scanningLbl.Text = scanningBar.Value.ToString() + "ms";
        }

        private void glidingSpeedBar_Scroll(object sender)
        {
            Spin.glidingSpeed = glidingSpeedBar.Value;
            glidingSpeedValueLbl.Text = glidingSpeedBar.Value.ToString();
            speedLbl.Text = (glidingSpeedBar.Value.ToString() + "%");
        }

        private void spaceglideKeyDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (spaceglideKeyDropdown.SelectedIndex == 0)
            {
                spacegliderKey = Key.Space;
                BeginInvoke(new Action(() =>
                {
                    spacegliderkeyLabel.Text = "Space";
                }));
            }
            if (spaceglideKeyDropdown.SelectedIndex == 1)
            {
                spacegliderKey = Key.Z;
                BeginInvoke(new Action(() =>
                {
                    spacegliderkeyLabel.Text = "Z";
                }));
            }
            if (spaceglideKeyDropdown.SelectedIndex == 2)
            {
                spacegliderKey = Key.X;
                BeginInvoke(new Action(() =>
                {
                    spacegliderkeyLabel.Text = "X";
                }));
            }
            if (spaceglideKeyDropdown.SelectedIndex == 3)
            {
                spacegliderKey = Key.C;
                BeginInvoke(new Action(() =>
                {
                    spacegliderkeyLabel.Text = "C";
                }));
            }
            if (spaceglideKeyDropdown.SelectedIndex == 4)
            {
                spacegliderKey = Key.V;
                BeginInvoke(new Action(() =>
                {
                    spacegliderkeyLabel.Text = "V";
                }));
            }
        }

        private void attackmoveKeyDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (attackmoveKeyDropdown.SelectedIndex == 0)
            {
                attackMoveKey = Key.Space;
                BeginInvoke(new Action(() =>
                {
                    attackmovekeyLabel.Text = "Space";
                }));
            }
            if (attackmoveKeyDropdown.SelectedIndex == 1)
            {
                attackMoveKey = Key.Z;
                BeginInvoke(new Action(() =>
                {
                    attackmovekeyLabel.Text = "Z";
                }));
            }
            if (attackmoveKeyDropdown.SelectedIndex == 2)
            {
                attackMoveKey = Key.X;
                BeginInvoke(new Action(() =>
                {
                    attackmovekeyLabel.Text = "X";
                }));
            }
            if (attackmoveKeyDropdown.SelectedIndex == 3)
            {
                attackMoveKey = Key.C;
                BeginInvoke(new Action(() =>
                {
                    attackmovekeyLabel.Text = "C";
                }));
            }
            if (attackmoveKeyDropdown.SelectedIndex == 4)
            {
                attackMoveKey = Key.V;
                BeginInvoke(new Action(() =>
                {
                    attackmovekeyLabel.Text = "V";
                }));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            scanningLbl.Text = scanningBar.Value.ToString() + "ms";
            speedLbl.Text = (glidingSpeedBar.Value.ToString() + "%");
            windupLbl.Text = windupBar.Value.ToString() + "ms";

            Thread TH = new Thread(Script);
            TH.SetApartmentState(ApartmentState.STA);
            TH.Start();
        }

        static bool CheckIfInGame()
        {
            try
            {
                if (float.Parse(API.readAttackSpeed()) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public void ConstScanner()
        {
            //cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            while (true)
            {
                if (CheckIfInGame() == true)
                {
                    inGame = true;
                }
                if (CheckIfInGame() == false)
                {
                    inGame = false;
                }

                if (inGame)
                {
                    if ((Keyboard.GetKeyStates(spacegliderKey) & KeyStates.Down) > 0)
                    {
                        spin.Spinjitzu(windupPercentage, consts.enemyHPArrayGlobal, consts.buffedenemyHPArrayGlobal, consts.myHPArrayGlobal);
                        UpdateGUI();
                        //if(consts.enemyHPArrayGlobal.Length != 0 && consts.myHPArrayGlobal.Length != 0)
                        //    input.SetPosition(spin.ChooseEnemy().X + Spin.offsetX, spin.ChooseEnemy().Y + Spin.offsetY);
                        //input.SetPosition(0, 0);
                        BeginInvoke(new Action(() => {
                            scriptActiveCheck.Text = "Active";
                        }));
                    }
                    if ((Keyboard.GetKeyStates(attackMoveKey) & KeyStates.Down) > 0)
                    {
                        spin.simpleAttackMove(windupPercentage);
                    }
                    else
                    {
                        BeginInvoke(new Action(() => {
                            scriptActiveCheck.Text = "Not Active";
                        }));
                    }
                }
                Thread.Sleep(1);
            }
        }



        public void SetWindup()
        {
            
            if (inGame)
            {
                string champname = API.readChampionName();
                if (champname == "Twitch")
                {
                    windupPercentage = 0.2019f;
                    BeginInvoke(new Action(() =>
                    {
                        championLabel.Text = "Twitch";
                    }));
                }
                if (champname == "Kog'Maw")
                {
                    windupPercentage = 0.1662f;
                    BeginInvoke(new Action(() =>
                    {
                        championLabel.Text = "Kog'Maw";
                    }));
                }
                if (champname == "Ashe")
                {
                    windupPercentage = 0.2193f;
                    BeginInvoke(new Action(() =>
                    {
                        championLabel.Text = "Ashe";
                    }));
                }
                    
                if (champname == "Jinx")
                {
                    windupPercentage = 0.1688f;
                    BeginInvoke(new Action(() =>
                    {
                        championLabel.Text = "Jinx";
                    }));
                }
                if (champname == "Kai'Sa")
                {
                    windupPercentage = 0.1611f;
                    BeginInvoke(new Action(() =>
                    {
                        championLabel.Text = "Kai'Sa";
                    }));
                }
                if (champname == "Twisted Fate")
                {
                    windupPercentage = 0.244f;
                    BeginInvoke(new Action(() =>
                    {
                        championLabel.Text = "Twisted Fate";
                    }));
                }
                     //championLabel.Text = "Twisted Fate";
                if (champname == "Varus")
                {
                    windupPercentage = 0.1754f;
                    BeginInvoke(new Action(() =>
                    {
                        championLabel.Text = "Varus";
                    }));
                }
                     //championLabel.Text = "Varus";
                if (champname == "Kayle")
                {
                    windupPercentage = 0.1936f;
                    BeginInvoke(new Action(() =>
                    {
                        championLabel.Text = "Kayle";
                    }));
                }

                     //championLabel.Text = "Kayle";
                if (champname == "Caitlyn")
                {
                    windupPercentage = 0.1771f;
                    BeginInvoke(new Action(() =>
                    {
                        championLabel.Text = "Caitlyn";
                    }));
                }
                     //championLabel.Text = "Caitlyn";
                if (champname == "Lucian")
                {
                    windupPercentage = 0.15f;
                    BeginInvoke(new Action(() =>
                    {
                        championLabel.Text = "Lucian";
                    }));
                }
                     //championLabel.Text = "Lucian";
                if (champname == "Miss Fortune")
                {
                    windupPercentage = 0.148f;
                    BeginInvoke(new Action(() =>
                    {
                        championLabel.Text = "Miss Fortune";
                    }));
                }
                     //championLabel.Text = "Miss Fortune";
                if (champname == "Sivir")
                {
                    windupPercentage = 0.12f;
                    BeginInvoke(new Action(() =>
                    {
                        championLabel.Text = "Sivir";
                    }));

                }
//championLabel.Text = "Sivir";
                if (champname == "Tristana")
                {
                    windupPercentage = 0.148f;
                    BeginInvoke(new Action(() =>
                    {
                        championLabel.Text = "Tristana";
                    }));
                }
                     //championLabel.Text = "Tristana";
                if (champname == "Vayne")
                {
                    windupPercentage = 0.1754f;
                    BeginInvoke(new Action(() =>
                    {
                        championLabel.Text = "Vayne";
                    }));
                }
                     //championLabel.Text = "Vayne";
                if (champname == "Xayah")
                {
                    windupPercentage = 0.1769f;
                    BeginInvoke(new Action(() =>
                    {
                        championLabel.Text = "Xayah";
                    }));
                }
                     //championLabel.Text = "Xayah";
                if (champname == "Kindred")
                {
                    windupPercentage = 0.1754f;
                    BeginInvoke(new Action(() =>
                    {
                        championLabel.Text = "Kindred";
                    }));
                }
                    //windupPercentage = 0.1754f; //championLabel.Text = "Kindred";
            }
        }

        public void scan()
        {
            PixelBot pxbot = new PixelBot();
            string ENEMYHP = "#3A0400";
            Color ENEMYcolor = System.Drawing.ColorTranslator.FromHtml(ENEMYHP);
            string MYHP = "#312C00";
            Color MYcolor = System.Drawing.ColorTranslator.FromHtml(MYHP);
            string BUFFEDENEMYHP = "#6B3A73";
            Color ENEMYBUFFEDcolor = System.Drawing.ColorTranslator.FromHtml(BUFFEDENEMYHP);
            var scanHP = Task.Run(async () => {
                while (true)
                {
                    Point[] myHPArray = pxbot.Search(new Rectangle(0, 0, 1920, 1080), MYcolor, 0);
                    consts.myHPArrayGlobal = myHPArray;
                    UpdateGUI();
                    //Thread.Sleep(ScanWait);
                    //Thread.CurrentThread.Join(10);
                    await Task.Delay(Spin.ScanWait);
                }
            });
            var scanE = Task.Run(async () => {
                while (true)
                {
                    Point[] enemyArray = pxbot.Search(new Rectangle(0, 0, 1920, 1080), ENEMYcolor, 0);
                    consts.enemyHPArrayGlobal = enemyArray;
                    UpdateGUI();
                    await Task.Delay(Spin.ScanWait);
                }
            });
            var scanBE = Task.Run(async () => {
                while (true)
                {
                    Point[] enemyBuffedArray = pxbot.Search(new Rectangle(0, 0, 1920, 1080), ENEMYBUFFEDcolor, 0);
                    consts.buffedenemyHPArrayGlobal = enemyBuffedArray;
                    UpdateGUI();
                    await Task.Delay(Spin.ScanWait);
                }
            });
        }

        public void Script()
        {
            //ReadConfig();
            //Login();
            //ReadConfig();
            //PatchNotes23();

            while (true)
            {
                Spin spin = new Spin();

                ReadConfig();
                //Login();

                //inicjalizacja wszystkiego
                Thread Scanner = new Thread(ConstScanner);
                Scanner.SetApartmentState(ApartmentState.STA);
                Scanner.Start();

                //Thread ScanMyHP = new Thread(spin.ScanMyHP);
                ////ScanMyHP.IsBackground = true;
                //ScanMyHP.SetApartmentState(ApartmentState.STA);
                //ScanMyHP.Start();

                //Thread ScanEnemyHP = new Thread(spin.ScanEnemyHP);
                //ScanEnemyHP.SetApartmentState(ApartmentState.STA);
                ////ScanEnemyHP.IsBackground = true;
                //ScanEnemyHP.Start();

                //Thread ScanBuffedEnemyHP = new Thread(spin.ScanBuffedEnemyHP);
                //ScanBuffedEnemyHP.SetApartmentState(ApartmentState.STA);
                ////ScanBuffedEnemyHP.IsBackground = true;
                //ScanBuffedEnemyHP.Start();



                //Thread Scanner = new Thread(ConstScanner);
                //Scanner.SetApartmentState(ApartmentState.STA);
                //Scanner.Start();

                //Thread ScanMyHP = new Thread(spin.ScanMyHP);
                //ScanMyHP.SetApartmentState(ApartmentState.STA);
                //ScanMyHP.Start();

                //Thread ScanEnemyHP = new Thread(spin.ScanEnemyHP);
                //ScanEnemyHP.SetApartmentState(ApartmentState.STA);
                //ScanEnemyHP.Start();

                //Thread ScanBuffedEnemyHP = new Thread(spin.ScanBuffedEnemyHP);
                //ScanBuffedEnemyHP.SetApartmentState(ApartmentState.STA);
                //ScanBuffedEnemyHP.Start();

                scan();


                //Task.Run(() => spin.ScanMyHPAsync());
                //Task.Run(() => spin.ScanEnemyHPAsync());
                //Task.Run(() => spin.ScanBuffedEnemyHPAsync());

                //Thread ScanSkills = new Thread(spin.ScanSkills);
                //ScanSkills.SetApartmentState(ApartmentState.STA);
                //ScanSkills.Start();


                //overlay
                //overlay over = new overlay();
                //over.Show();

                while (true)//petla zeby sie nie zamknela apka
                {

                    //Console.WriteLine("[LOG] - GAME NOT FOUND");

                    if (inGame == false)
                    {
                        while (inGame == false)
                        {
                            //Console.WriteLine("[GAME] - Waiting for game");
                            BeginInvoke(new Action(() =>
                            {
                                statusLabel.Text = "Not in Game";
                            }));
                            //statusLabel.Text = "Not in Game";
                            Thread.Sleep(2000);
                        }
                    }
                    if (inGame == true)
                    {
                        BeginInvoke(new Action(() =>
                        {
                            statusLabel.Text = "in Game";
                        }));
                        
                        SetWindup();
                        string ENEMYHP = "#3A0400";
                        Color ENEMYcolor = System.Drawing.ColorTranslator.FromHtml(ENEMYHP);
                        string MYHP = "#312C00";
                        Color MYcolor = System.Drawing.ColorTranslator.FromHtml(MYHP);
                        string BUFFEDENEMYHP = "#6B3A73";
                        Color ENEMYBUFFEDcolor = System.Drawing.ColorTranslator.FromHtml(BUFFEDENEMYHP);
                        PixelBot pxbot = new PixelBot();
                        if (configArray[0] == "C")
                        {
                            spacegliderKey = Key.C;
                            //Console.WriteLine("[CONFIG] - Spaceglider key set to C");
                        }
                        if (configArray[0] == "Space")
                        {
                            spacegliderKey = Key.Space;
                            //Console.WriteLine("[CONFIG] - Spaceglider key set to Space");
                        }
                        while (inGame == true)
                        {
                            //Point[] myHPArray = pxbot.Search(new Rectangle(0, 0, 1920, 1080), MYcolor, 0);
                            //consts.myHPArrayGlobal = myHPArray;
                            //Point[] enemyArray = pxbot.Search(new Rectangle(0, 0, 1920, 1080), ENEMYcolor, 0);
                            //consts.enemyHPArrayGlobal = enemyArray;
                            //UpdateGUI();
                            //Thread.Sleep(Spin.ScanWait);

                            //Thread.CurrentThread.Join(10);
                            //await Task.Delay(ScanWait);
                        }
                    }

                }

            }
        }
    }
}
