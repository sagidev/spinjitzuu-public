using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace spinjitzuu3
{
    class Spin
    {
        private bool canAttack = true;
        private bool canAttackk = true;
        private float nextAttack = 0;

        private Form1 gui = new Form1();
        private Input input = new Input();
        private Consts consts = new Consts();

        public static bool kindredAutoR = true;
        public static bool aaResets = true;

        public static int additionalWindup = 0;
        public static float glidingSpeed = 100;
        public static int ScanWait = 100;

        public static int offsetX = 65;
        public static int offsetY = 95;

        public void Orbwalk(float windup, Point[] Earray, Point[] BEarray, Point[] myArray)
        {
            float timeBetweenAttacks = (1000.0f / float.Parse(API.readAttackSpeed()));
            float gameTime = (float.Parse(API.readGameTime()) * 1000);
            Point lastMovePos;

            if (canAttackk)
            {
                lastMovePos = Cursor.Position;
                if (Earray.Length != 0 || BEarray.Length != 0)
                {
                    input.SetPosition(ChooseEnemy(Earray, BEarray, myArray).X + offsetX, ChooseEnemy(Earray, BEarray, myArray).Y + offsetY);
                    Thread.Sleep(10);
                    input.middleClick();
                    Thread.Sleep(10);
                    input.SetPosition(lastMovePos.X, lastMovePos.Y);

                    float waitSec = GetWindupTime(windup);
                    int wait = Convert.ToInt32(waitSec);

                    nextAttack = (gameTime + (timeBetweenAttacks * (glidingSpeed / 100)));
                    Thread.Sleep(wait + additionalWindup);
                    canAttackk = false;
                }
                else
                {
                    input.rightClick();
                }
            }
            else
            {
                if (gameTime > nextAttack)
                {
                    input.rightClick();
                    canAttackk = true;
                }
                else
                {
                    input.rightClick();
                }
            }
        }

        public void SimpleAttackMove(float windup)
        {
            float timeBetweenAttacks = (1000.0f / float.Parse(API.readAttackSpeed()));
            float gameTime = (float.Parse(API.readGameTime()) * 1000);

            if (canAttack)
            {
                input.middleClick();

                float waitSec = GetWindupTime(windup) + 0.5f;
                int wait = Convert.ToInt32(waitSec - 10);
                nextAttack = (gameTime + (timeBetweenAttacks * (float.Parse(API.readAttackSpeed()) > 2.5 ? 0.95f : 1.05f)));
                Thread.Sleep(wait);
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

        public Point ChooseEnemy(Point[] enArray, Point[] enBArray, Point[] myArray)
        {
            if (enArray.Length != 0 && myArray.Length != 0)
            {
                double[] lengthArray = enArray.Select(p => Math.Sqrt(Math.Pow(Math.Abs((p.X + offsetX) - (myArray[0].X + offsetX)), 2) + Math.Pow(Math.Abs((p.Y + offsetY) - (myArray[0].Y + offsetY)), 2))).ToArray();
                int index = Array.IndexOf(lengthArray, lengthArray.Min());
                return new Point(enArray[index].X, enArray[index].Y);
            }
            if (enBArray.Length != 0 && myArray.Length != 0)
            {
                double[] lengthArray = enBArray.Select(p => Math.Sqrt(Math.Pow(Math.Abs((p.X + offsetX) - (myArray[0].X + offsetX)), 2) + Math.Pow(Math.Abs((p.Y + offsetY) - (myArray[0].Y + offsetY)), 2))).ToArray();
                int index = Array.IndexOf(lengthArray, lengthArray.Min());
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
            Color myColor = ColorTranslator.FromHtml("#312C00");
            while (true)
            {
                Point[] myHPArray = pxbot.Search(new Rectangle(0, 0, 1920, 1080), myColor, 0);
                consts.myHPArrayGlobal = myHPArray;
                Thread.Sleep(ScanWait);
            }
        }

        public void ScanEnemyHP()
        {
            PixelBot pxbot = new PixelBot();
            Color enemyColor = ColorTranslator.FromHtml("#3A0400");
            while (true)
            {
                Point[] enemyArray = pxbot.Search(new Rectangle(0, 0, 1920, 1080), enemyColor, 0);
                consts.enemyHPArrayGlobal = enemyArray;
                Thread.Sleep(ScanWait);
            }
        }

        public void Scan()
        {
            PixelBot pxbot = new PixelBot();
            Color enemyColor = ColorTranslator.FromHtml("#3A0400");
            Color myColor = ColorTranslator.FromHtml("#312C00");
            Color buffedEnemyColor = ColorTranslator.FromHtml("#6B3A73");

            Task.Run(async () => { while (true) { consts.myHPArrayGlobal = pxbot.Search(new Rectangle(0, 0, 1920, 1080), myColor, 0); gui.UpdateGUI(); await Task.Delay(ScanWait); } });
            Task.Run(async () => { while (true) { consts.enemyHPArrayGlobal = pxbot.Search(new Rectangle(0, 0, 1920, 1080), enemyColor, 0); gui.UpdateGUI(); await Task.Delay(ScanWait); } });
            Task.Run(async () => { while (true) { consts.buffedenemyHPArrayGlobal = pxbot.Search(new Rectangle(0, 0, 1920, 1080), buffedEnemyColor, 0); gui.UpdateGUI(); await Task.Delay(ScanWait); } });
        }

        public void ScanBuffedEnemyHP()
        {
            PixelBot pxbot = new PixelBot();
            Color buffedEnemyColor = ColorTranslator.FromHtml("#6B3A73");
            while (true)
            {
                consts.buffedenemyHPArrayGlobal = pxbot.Search(new Rectangle(0, 0, 1920, 1080), buffedEnemyColor, 0);
                Thread.Sleep(ScanWait);
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