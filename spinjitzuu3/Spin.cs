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
        private static bool canAttack = true;
        private static bool canAttackk = true;
        private static float nextAttack = 0;
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

        //public Point[] myHPArrayGlobal = { };
        //public Point[] enemyHPArrayGlobal = { };
        //public Point[] buffedenemyHPArrayGlobal = { };

        public void simpleAttackMove(float windup)
        {
            float timeBetweenAttacks = (1000.0f / float.Parse(API.readAttackSpeed()));
            float gameTime = (float.Parse(API.readGameTime()) * 1000);

            if (canAttack)
            {
                input.middleClick();
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
                if (gameTime > nextAttack)
                {
                    input.rightClick();
                    canAttack = true;
                }
                else
                {
                    input.rightClick();
                }
            }
        }

        public void Spinjitzu(float windup, Point[] Earray, Point[] BEarray, Point[] myArray)
        {
            //Point[] myHPArrayLocal = consts.myHPArrayGlobal;
            //Point[] enemyHPArrayLocal = consts.enemyHPArrayGlobal;
            //Point[] buffedenemyHPArraylocal = consts.buffedenemyHPArrayGlobal;
            float timeBetweenAttacks = (1000.0f / float.Parse(API.readAttackSpeed()));
            //if (kindredAutoR)
            //    cast.kindredAutoR();
            float gameTime = (float.Parse(API.readGameTime()) * 1000);
            Point Lastmovepos;
            //Console.WriteLine("waiting for attack");
            if (canAttackk)
            {
                Lastmovepos = Cursor.Position;
                //Console.WriteLine("looking for enemy");
                if (Earray.Length != 0 || BEarray.Length != 0)
                {
                    input.SetPosition(ChooseEnemy(Earray, BEarray, myArray).X + offsetX, ChooseEnemy(Earray, BEarray, myArray).Y + offsetY);
                    //input.SetPosition(0, 0);
                    //Console.WriteLine("found enemy");
                    Thread.Sleep(10);
                    input.middleClick();
                    //Console.WriteLine("attacked");
                    Thread.Sleep(10);

                    input.SetPosition(Lastmovepos.X, Lastmovepos.Y);
                    //float waitsec = GetWindupTime(windup) + 0.5f;
                    //int wait = Convert.ToInt32(waitsec - 10);
                    float waitsec = GetWindupTime(windup);
                    int wait = Convert.ToInt32(waitsec);
                    //if (float.Parse(API.readAttackSpeed()) > 2.5)
                    //{
                    //    nextAttack = (gameTime + (timeBetweenAttacks * 0.95f));
                    //    Thread.Sleep(wait);
                    //}
                    //else
                    //{
                    //    nextAttack = (gameTime + (timeBetweenAttacks * 1.05f));
                    //    Thread.Sleep(wait);
                    //}
                    nextAttack = (gameTime + (timeBetweenAttacks * (glidingSpeed/100)));

                    //nextAttack = (gameTime + (timeBetweenAttacks*1.05f));
                    
                    Thread.Sleep(wait+additionalWindup);
                    //targetedW();
                    canAttackk = false;
                }
                else
                {
                    //input.SetPosition(consts.enemyHPArrayGlobal.First().X, consts.enemyHPArrayGlobal.First().Y);
                    input.rightClick();
                }
            }
            else
            {
                if (gameTime > nextAttack)
                {
                    Console.WriteLine("waiting for aa");
                    input.rightClick();
                    canAttackk = true;
                }
                else
                {
                    //if (API.readChampionName() == "Kindred" && aaResets)
                    //{
                    //    if (cast.isKindredQReady())
                    //        cast.Q();
                    //    targetedE();
                    //}
                    //if (API.readChampionName() == "Twitch" && aaResets)
                    //{
                    //    targetedW();
                    //}
                    Console.WriteLine("spinning");
                    input.rightClick();
                }
            }
        }


        public void targetedW()
        {
            if (cast.isTwitchWReady())
            {
                Point Lastmovepos;
                Lastmovepos = Cursor.Position;
                input.SetPosition(ChooseEnemy(consts.enemyHPArrayGlobal, consts.buffedenemyHPArrayGlobal, consts.myHPArrayGlobal).X + 65, ChooseEnemy(consts.enemyHPArrayGlobal, consts.buffedenemyHPArrayGlobal, consts.myHPArrayGlobal).Y + 95);
                Thread.Sleep(10);
                cast.W();
                Thread.Sleep(10);
                input.SetPosition(Lastmovepos.X, Lastmovepos.Y);
            }
        }
        public void targetedE()
        {
            if (cast.isKindredEReady())
            {
                Point Lastmovepos;
                Lastmovepos = Cursor.Position;
                Spin spin = new Spin();
                input.SetPosition(ChooseEnemy(consts.enemyHPArrayGlobal,consts.buffedenemyHPArrayGlobal,consts.myHPArrayGlobal).X + 65, ChooseEnemy(consts.enemyHPArrayGlobal, consts.buffedenemyHPArrayGlobal, consts.myHPArrayGlobal).Y + 95);
                Thread.Sleep(10);
                cast.E();
                Thread.Sleep(10);
                input.SetPosition(Lastmovepos.X, Lastmovepos.Y);
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

        Form1 gui = new Form1();
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


        internal float GetWindupTime(float windupPerc)
        {
            return (1 / float.Parse(API.readAttackSpeed()) * 1000) * windupPerc;
        }
        internal float GetAttackDelay()
        {
            return (int)(1000.0f / float.Parse(API.readAttackSpeed()));
        }
    }
}
