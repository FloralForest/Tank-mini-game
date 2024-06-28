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
    enum Tag
    {
        MyTank,
        EnemyTank
    }

    //子弹
    internal class Bullet : Movething
    {
        private Random random = new Random();
        public Tag Tag { get; set; }

        public Bullet(double x, double y, double speed, Direction dir, Tag tag)
        {
            this.X = x;
            this.Y = y;
            this.Speed = speed;
            BitmapUp = Resources.BulletUp;
            BitmapDown = Resources.BulletDown;
            BitmapLeft = Resources.BulletLeft;
            BitmapRight = Resources.BulletRight;
            this.Dir = dir;
            this.Tag = tag;

            //减去子弹图片自身的宽高
            this.X -= Width / 2;
            this.Y -= Height / 2;
        }

        //移动 重写虚方法
        public override void Update()
        {
            MoveCheck();//检查
            Move();
            base.Update();
        }

        public void MoveCheck()
        {
            //超出边界
            if (Dir == Direction.Up)
            {
                if (Y < 0)
                {
                    GameObjectManager.DestroyBullet(this);
                    return;
                }
            }
            else if (Dir == Direction.Down)
            {
                if (Y > 450)
                {
                    GameObjectManager.DestroyBullet(this);
                    return;
                }
            }
            else if (Dir == Direction.Left)
            {
                if (X < 0)
                {
                    GameObjectManager.DestroyBullet(this);
                    return;
                }
            }
            else if (Dir == Direction.Right)
            {
                if (X > 450)
                {
                    GameObjectManager.DestroyBullet(this);
                    return;
                }
            }
            //得到当前位置
            Rectangle rect = GetRectangle();
            //得到子弹
            rect.X = (int)Math.Floor(X);
            rect.Y = (int)Math.Floor(Y);
            //爆炸中心
            double xExplosion = this.X + Width / 2;
            double yExplosion = this.Y + Height / 2;


            //墙体碰撞 使用内置的Rectangle判断矩形碰撞
            NotMovething wall;
            if ((wall = GameObjectManager.IsCollidedWall(rect)) != null)
            {
                GameObjectManager.DestroyWall(wall);
                GameObjectManager.DestroyBullet(this);
                GameObjectManager.CreateExplosion(xExplosion, yExplosion);//爆炸效果
                SoundManager.PlayBlast();//音效
                return;
            }
            if (GameObjectManager.IsCollidedStee(rect) != null)
            {
                GameObjectManager.DestroyBullet(this);
                GameObjectManager.CreateExplosion(xExplosion, yExplosion);//爆炸效果
                SoundManager.PlayBlast();//音效
                return;
            }
            if (GameObjectManager.IsCollidedBoss(rect))
            {
                GameObjectManager.DestroyBullet(this);
                GameObjectManager.CreateExplosion(xExplosion, yExplosion);//爆炸效果
                SoundManager.PlayBlast();//音效
                MessageBox.Show("Boss被击中游戏结束！", "提示", MessageBoxButtons.OK);
                GameFramework.ChangeGameOver();
                return;
            }

            //攻击敌人
            if (Tag == Tag.MyTank)
            {
                EnemyTank tank;
                if ((tank = GameObjectManager.IsCollidedEnemyTank(rect)) != null)
                {
                    GameObjectManager.DestroyEnemyTank(tank);
                    GameObjectManager.DestroyBullet(this);
                    GameObjectManager.CreateExplosion(xExplosion, yExplosion);//爆炸效果
                    SoundManager.PlayBlast();//音效
                    return;
                }
            }
            else if (Tag == Tag.EnemyTank)//当子弹为敌人发出时
            {
                MyTank myTank;
                if ((myTank = GameObjectManager.IsCollidedMyTank(rect)) != null)
                {
                    GameObjectManager.DestroyBullet(this);//子弹销毁
                    GameObjectManager.CreateExplosion(xExplosion, yExplosion);//爆炸效果
                    SoundManager.PlayHit();//受击音效
                    myTank.TankDamage();//减少血量

                    return;
                }
            }
        }

        public void Move()
        {
            switch (Dir)
            {
                case Direction.Up:
                    Y -= Speed;
                    break;
                case Direction.Down:
                    Y += Speed;
                    break;
                case Direction.Left:
                    X -= Speed;
                    break;
                case Direction.Right:
                    X += Speed;
                    break;
            }
        }
    }
}
