using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TankTest.Properties;

namespace TankTest
{
    //墙
    internal class GameObjectManager
    {
        private static List<NotMovething> wallList = new List<NotMovething>();//可破坏墙
        private static List<NotMovething> steelList = new List<NotMovething>();//钢铁墙
        private static NotMovething boss = null;//Boss墙
        private static MyTank myTank = null;//tank
        private static int enemyBornSpeed = 300, enemyBornCount = 300;//enemyBornSpeed 5秒生成一个敌人和enemyBornCount一个计数器
        private static Point[] points = new Point[3];//存放敌人生成的位置
        private static List<EnemyTank> tankList = new List<EnemyTank>();//存放敌人
        private static List<Bullet> bulletList = new List<Bullet>();//子弹集合
        private static List<Explosion> expList = new List<Explosion>();//爆炸图片集合

        //绘制
        public static async void Update()
        {
            foreach (NotMovething item in wallList)
            {
                item.Update();//绘制
            }
            foreach (NotMovething item in steelList)
            {
                item.Update();
            }
            foreach (EnemyTank tank in tankList)
            {
                tank.Update();
            }
            for (int i = 0; i < bulletList.Count; i++) //修改无法修改枚举的问题
            {
                Bullet bullet = bulletList[i];
                bullet.Update();
            }
            for (int i = 0; i < expList.Count; i++) //修改无法修改枚举的问题
            {
                Explosion exp = expList[i];
                exp.Update();
            }
            boss.Update();
            myTank.Update();
            await Task.Delay(4700); // 延迟4.7秒 给播放游戏音乐时间
            EnemyBorn();
            DestroyExplosion();
        }

        //判断传过来的rt参数是否碰撞墙,没有碰撞返回null,返回NotMovething明白是那堵墙用于子弹交互
        public static NotMovething IsCollidedWall(Rectangle rt)
        {
            foreach (NotMovething wall in wallList) //遍历墙
            {
                if (wall.GetRectangle().IntersectsWith(rt)) //得到墙位置和宽高 判断是否右物体与其碰撞
                {
                    return wall;
                }
            }
            return null;
        }
        // 钢铁墙
        public static NotMovething IsCollidedStee(Rectangle rt)
        {
            foreach (NotMovething stee in steelList) //遍历钢铁墙
            {
                if (stee.GetRectangle().IntersectsWith(rt)) //得到墙位置和宽高 判断是否右物体与其碰撞
                {
                    return stee;
                }
            }
            return null;

        }

        //碰撞boss
        public static bool IsCollidedBoss(Rectangle rt)
        {
            return boss.GetRectangle().IntersectsWith(rt);
        }

        //碰撞敌人
        public static EnemyTank IsCollidedEnemyTank(Rectangle rt)
        {
            foreach (EnemyTank tank in tankList)
            {
                if (tank.GetRectangle().IntersectsWith(rt)) //返回被击中的坦克敌人
                {
                    return tank;
                }
            }
            return null;
        }

        //敌人子弹攻击到我
        public static MyTank IsCollidedMyTank(Rectangle rt)
        {
            if (myTank.GetRectangle().IntersectsWith(rt)) return myTank;
            else return null;
        }

        //初始化地图
        public static void CreateMap()
        {
            CreateWall(1, 1, 5, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(3, 1, 5, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(5, 1, 4, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(7, 1, 3, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(9, 1, 4, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(11, 1, 5, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(13, 1, 5, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(7, 5, 1, Resources.steel, steelList);//x y坐标和创建墙数
            CreateWall(7, 6, 1, Resources.steel, steelList);//x y坐标和创建墙数
            CreateWall(0, 7, 1, Resources.steel, steelList);//x y坐标和创建墙数
            CreateWall(14, 7, 1, Resources.steel, steelList);//x y坐标和创建墙数

            CreateWall(2, 7, 1, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(3, 7, 1, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(4, 7, 1, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(5, 7, 1, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(9, 7, 1, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(10, 7, 1, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(11, 7, 1, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(12, 7, 1, Resources.wall, wallList);//x y坐标和创建墙数

            CreateWall(7, 7, 1, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(5, 8, 1, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(6, 8, 1, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(7, 8, 1, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(8, 8, 1, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(9, 8, 1, Resources.wall, wallList);//x y坐标和创建墙数

            CreateWall(1, 9, 6, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(3, 10, 4, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(5, 10, 2, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(6, 10, 1, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(7, 10, 1, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(8, 10, 1, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(9, 10, 2, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(11, 10, 4, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(13, 9, 6, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(7, 11, 1, Resources.steel, steelList);//x y坐标和创建墙数
            CreateWall(6, 13, 2, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(7, 13, 1, Resources.wall, wallList);//x y坐标和创建墙数
            CreateWall(8, 13, 2, Resources.wall, wallList);//x y坐标和创建墙数

            CreateBoss(6.9, 14, Resources.Boss);//x y坐标和创建墙数

        }

        private static void CreateBoss(double x, double y, Image img)
        {
            double xPosition = x * 30;
            double yPosition = y * 30;
            boss = new NotMovething(xPosition, yPosition, img);
        }

        public static void CreateMyTank()
        {
            double xPosition = 5 * 30;
            double yPosition = 14 * 30;
            myTank = new MyTank(xPosition, yPosition, 2);
        }

        //设置墙
        public static void CreateWall(double x, double y, int count, Image img, List<NotMovething> wallList)
        {
            double xPosition = x * 30;
            double yPosition = y * 30;
            for (double i = yPosition; i < yPosition + count * 30; i += 15)
            {
                NotMovething notMovething = new NotMovething(xPosition, i, img);//左墙
                NotMovething notMovethingTwo = new NotMovething(xPosition + 15, i, img);//右墙
                wallList.Add(notMovething);
                wallList.Add(notMovethingTwo);
            }
        }

        //爆炸效果
        public static void CreateExplosion(double x, double y)
        {
            Explosion explosion = new Explosion(x, y);
            expList.Add(explosion);
        }

        //键盘按下抬起时
        public static void KeyUp(KeyEventArgs args)
        {
            myTank.KeyUp(args);
        }
        public static void KeyDown(KeyEventArgs args)
        {
            myTank.KeyDown(args);
        }

        //设置生成位置
        public static void Start()
        {
            points[0].X = 0;
            points[0].Y = 0;

            points[1].X = 7 * 30;
            points[1].Y = 0;

            points[2].X = 14 * 30;
            points[2].Y = 0;
        }

        //生成敌人逻辑
        private static void EnemyBorn()
        {
            enemyBornCount++;
            if (enemyBornCount < enemyBornSpeed) return;
            SoundManager.PlayAdd();//增加音效
            //随机生成随机数 实现固定位的随机位置生成敌人
            Random random = new Random();
            int index = random.Next(0, 3);
            Point point = points[index];

            //随机选取tank
            int enemyType = random.Next(1, 4);
            switch (enemyType)
            {
                case 1:
                    CreateEnemtTank1(point.X, point.Y);
                    break;
                case 2:
                    CreateEnemtTank2(point.X, point.Y);
                    break;
                case 3:
                    CreateEnemtTank3(point.X, point.Y);
                    break;
                case 4:
                    CreateEnemtTank4(point.X, point.Y);
                    break;
            }

            enemyBornCount = 0;
        }

        private static void CreateEnemtTank1(double x, double y)
        {
            EnemyTank tank = new EnemyTank(x, y, 1, Resources.GrayUp, Resources.GrayDown, Resources.GrayLeft, Resources.GrayRight);
            tankList.Add(tank);
        }
        private static void CreateEnemtTank2(double x, double y)
        {
            EnemyTank tank = new EnemyTank(x, y, 1, Resources.GreenUp, Resources.GreenDown, Resources.GreenLeft, Resources.GreenRight);
            tankList.Add(tank);
        }
        private static void CreateEnemtTank3(double x, double y)
        {
            EnemyTank tank = new EnemyTank(x, y, 1, Resources.QuickUp, Resources.QuickDown, Resources.QuickLeft, Resources.QuickRight);
            tankList.Add(tank);
        }
        private static void CreateEnemtTank4(double x, double y)
        {
            EnemyTank tank = new EnemyTank(x, y, 1, Resources.SlowUp, Resources.SlowDown, Resources.SlowLeft, Resources.SlowRight);
            tankList.Add(tank);
        }

        //创建子弹
        public static void CreateBUllet(double x, double y, Direction dir, Tag tag)
        {
            Bullet bullet = new Bullet(x, y, 5, dir, tag); //xy坐标速度方向和子弹类型
            lock (bulletList)
            {
                bulletList.Add(bullet);
            }
        }

        //超边界移除
        public static void DestroyBullet(Bullet bullet)
        {
            lock (bulletList)
            {
                bulletList.Remove(bullet);
            }
        }

        //子弹击中移除墙
        public static void DestroyWall(NotMovething wall)
        {
            wallList.Remove(wall);
        }

        //子弹击中移除坦克
        public static void DestroyEnemyTank(EnemyTank tank)
        {
            tankList.Remove(tank);
        }

        //爆炸效果销毁
        public static void DestroyExplosion()
        {
            List<Explosion> explosions = new List<Explosion>();
            foreach (Explosion item in expList)
            {
                if (item.IsNeedDestroy == true)
                {
                    explosions.Add(item);
                }
            }
            foreach (Explosion exp in explosions)
            {
                expList.Remove(exp);
            }
        }
    }
}
