using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace spinjitzuu3
{
    class Spin
    {
        //---------- Declarations ----------

        private static bool canAttack = true;
        private static bool canAttackk = true;
        private static float nextAttack = 0;

        Form1 gui = new Form1();
        Input input = new Input();
        Cast cast = new Cast();
        consts consts = new consts();

        public static bool kindredAutoR = true;
        public static bool aaResets = true;

        public static int additionalWindup = 0;
        public static float glidingSpeed = 100;
        public static int ScanWait = 100;

        public static int offsetX = 65;
        public static int offsetY = 95;



        //---------- Functions ----------


        /// <summary>
        /// Scan for enemies available in LocalPlayer's AttackRange and attack them as often as it's possible with Target Selector.
        /// </summary>
        /// <param name="windup"></param>
        /// <param name="Earray"></param>
        /// <param name="BEarray"></param>
        /// <param name="myArray"></param>
        public void Spinjitzu(float windup, Point[] Earray, Point[] BEarray, Point[] myArray)
        {
            //Read all calculation info
            float timeBetweenAttacks = (1000.0f / float.Parse(API.readAttackSpeed()));
            float gameTime = (float.Parse(API.readGameTime()) * 1000);
            Point Lastmovepos;

            //Check if LocalPlayer is able to attack
            if (canAttackk)
            {
                Lastmovepos = Cursor.Position;
                //Check if there are available enemies to attack
                if (Earray.Length != 0 || BEarray.Length != 0)
                {
                    //Set cursor position right on the enemy
                    input.SetPosition(ChooseEnemy(Earray, BEarray, myArray).X + offsetX, ChooseEnemy(Earray, BEarray, myArray).Y + offsetY);
                    Thread.Sleep(10);

                    //Cast Attack
                    input.middleClick();
                    Thread.Sleep(10);

                    //Return cursor position from pre-attack saved position
                    input.SetPosition(Lastmovepos.X, Lastmovepos.Y);

                    //Wait for windup to not cancel and auto-attack
                    float waitsec = GetWindupTime(windup);
                    int wait = Convert.ToInt32(waitsec);

                    //Calculate the time when next attack would be casted
                    nextAttack = (gameTime + (timeBetweenAttacks * (glidingSpeed / 100)));
                    Thread.Sleep(wait + additionalWindup);
                    canAttackk = false;
                }
                else
                {
                    //Move your champion when attack is available but there are no enemies nearby
                    input.rightClick();
                }
            }
            else
            {
                //Move your champion when attack is not available
                if (gameTime > nextAttack)
                {
                    //Attack is now available
                    input.rightClick();
                    canAttackk = true;
                }
                else
                {
                    input.rightClick();
                }
            }
        }

        /// <summary>
        /// Cast attack as fast as its possible without cancelling any auto-attacks. It does not support Target Selector.
        /// </summary>
        public void simpleAttackMove(float windup)
        {
            //Read all calculation info
            float timeBetweenAttacks = (1000.0f / float.Parse(API.readAttackSpeed()));
            float gameTime = (float.Parse(API.readGameTime()) * 1000);

            //Check if LocalPlayer is able to attack
            if (canAttack)
            {
                //Cast Attack
                input.middleClick();

                //Wait for windup to not cancel and auto-attack
                float waitsec = GetWindupTime(windup) + 0.5f;
                int wait = Convert.ToInt32(waitsec - 10);
                if (float.Parse(API.readAttackSpeed()) > 2.5)
                {
                    nextAttack = (gameTime + (timeBetweenAttacks * 0.95f));
                    Thread.Sleep(wait);
                }
                else
                {
                    nextAttack = (gameTime + (timeBetweenAttacks * 1.05f));
                    Thread.Sleep(wait);
                }
                Thread.Sleep(wait / 3);
                canAttack = false;
            }
            else
            {
                //Move your champion when attack is not available
                if (gameTime > nextAttack)
                {
                    //Attack is now available
                    input.rightClick();
                    canAttack = true;
                }
                else
                {
                    input.rightClick();
                }
            }
        }

        public Point ChooseEnemy(Point[] enArray, Point[] enBArray, Point[] myArray)
        {
            if (enArray.Length != 0 && myArray.Length != 0)
            {
                double[] lengthArray = new double[enArray.Length];
                int i = 0;
                foreach (Point p in enArray)
                {
                    double length = Math.Sqrt((Math.Abs((p.X + offsetX) - (myArray[0].X + offsetX)) ^ 2) + (Math.Abs((p.Y + offsetY) - (myArray[0].Y + offsetY)) ^ 2));
                    lengthArray[i] = length;
                    i++;
                }
                double minValue = lengthArray.Min();
                double minIndex = Array.IndexOf(lengthArray, minValue);
                int index = Convert.ToInt32(minIndex);
                return new Point(enArray[index].X, enArray[index].Y);
            }
            if (enBArray.Length != 0 && myArray.Length != 0)
            {
                double[] lengthArray = new double[enBArray.Length];
                int i = 0;
                foreach (Point p in enBArray)
                {
                    double length = Math.Sqrt((Math.Abs((p.X + offsetX) - (myArray[0].X + offsetX)) ^ 2) + (Math.Abs((p.Y + offsetY) - (myArray[0].Y + offsetY)) ^ 2));
                    lengthArray[i] = length;
                    i++;
                }
                double minValue = lengthArray.Min();
                double minIndex = Array.IndexOf(lengthArray, minValue);
                int index = Convert.ToInt32(minIndex);
                return new Point(enBArray[index].X, enBArray[index].Y);
            }
            else
            {
                return Cursor.Position;
            }
        }
        
        /// <summary>
        /// Scans LocalPlayer's Healthbar
        /// </summary>
        public void ScanMyHP()
        {
            PixelBot pxbot = new PixelBot();
            string MYHP = "#312C00";
            Color MYcolor = System.Drawing.ColorTranslator.FromHtml(MYHP);
            while (true)
            {
                Console.WriteLine("[SCAN] - SZUKAM MOJEGO HP");
                Point[] myHPArray = pxbot.Search(new Rectangle(0, 0, 1920, 1080), MYcolor, 0);
                consts.myHPArrayGlobal = myHPArray;
                //GUI.alliesAmount = myHPArray.Length;
                Console.WriteLine("[SCAN] MOJE HP - " + myHPArray.Length);
                Console.WriteLine("[SCAN] MOJE HP - GLOBAL - " + consts.myHPArrayGlobal.Length);
                Thread.Sleep(ScanWait);
                //Thread.CurrentThread.Join(10);
            }
        }

        /// <summary>
        /// Scans Enemies' Healthbar
        /// </summary>
        public void ScanEnemyHP()
        {
            PixelBot pxbot = new PixelBot();
            string ENEMYHP = "#3A0400";
            Color ENEMYcolor = System.Drawing.ColorTranslator.FromHtml(ENEMYHP);
            while (true)
            {
                Console.WriteLine("[SCAN] - SZUKAM ENEMY HP");
                Point[] enemyArray = pxbot.Search(new Rectangle(0, 0, 1920, 1080), ENEMYcolor, 0);
                consts.enemyHPArrayGlobal = enemyArray;
                //GUI.enemiesAmount = enemyArray.Length;
                Console.WriteLine("[SCAN] ENEMY - " + enemyArray.Length);
                Console.WriteLine("[SCAN] ENEMY - GLOBAL - " + consts.enemyHPArrayGlobal.Length);
                Thread.Sleep(ScanWait);
                //Thread.CurrentThread.Join(10);
            }
        }

        /// <summary>
        /// Multithreaded All Entities Scan
        /// </summary>
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
                    gui.UpdateGUI();
                    //Thread.Sleep(ScanWait);
                    //Thread.CurrentThread.Join(10);
                    await Task.Delay(ScanWait);
                }
            });
            var scanE = Task.Run(async () => {
                while (true)
                {
                    Point[] enemyArray = pxbot.Search(new Rectangle(0, 0, 1920, 1080), ENEMYcolor, 0);
                    consts.enemyHPArrayGlobal = enemyArray;
                    gui.UpdateGUI();
                    await Task.Delay(ScanWait);
                }
            });
            var scanBE = Task.Run(async () => {
                while (true)
                {
                    Point[] enemyBuffedArray = pxbot.Search(new Rectangle(0, 0, 1920, 1080), ENEMYBUFFEDcolor, 0);
                    consts.buffedenemyHPArrayGlobal = enemyBuffedArray;
                    gui.UpdateGUI();
                    await Task.Delay(ScanWait);
                }
            });
        }

        /// <summary>
        /// Scan all Purple Healthbars (Buffed Enemies)
        /// </summary>
        public void ScanBuffedEnemyHP()
        {
            PixelBot pxbot = new PixelBot();
            string BUFFEDENEMYHP = "#6B3A73";
            Color ENEMYBUFFEDcolor = System.Drawing.ColorTranslator.FromHtml(BUFFEDENEMYHP);
            while (true)
            {
                Point[] enemyBuffedArray = pxbot.Search(new Rectangle(0, 0, 1920, 1080), ENEMYBUFFEDcolor, 0);
                consts.buffedenemyHPArrayGlobal = enemyBuffedArray;
                Thread.Sleep(ScanWait);

                //Thread.CurrentThread.Join(10);
            }
        }

        /// <summary>
        /// Get a Windup Time needed to cast a full Auto-Attack
        /// </summary>
        /// <param name="windupPerc"></param>
        /// <returns></returns>
        internal float GetWindupTime(float windupPerc)
        {
            return (1 / float.Parse(API.readAttackSpeed()) * 1000) * windupPerc;
        }

        /// <summary>
        /// Get an Attack Delay time after casting a full Auto-Attack
        /// </summary>
        /// <returns></returns>
        internal float GetAttackDelay()
        {
            return (int)(1000.0f / float.Parse(API.readAttackSpeed()));
        }
    }
}
