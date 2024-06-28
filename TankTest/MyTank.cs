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
    //我的坦克
    internal class MyTank : Movething
    {
        //是否移动坦克
        public bool IsMoving { get; set; }
        public int HP { get; set; }
        public MyTank(double x, double y, double speed)
        {
            IsMoving = false;
            this.X = x;
            this.Y = y;
            this.Speed = speed;
            BitmapUp = Resources.MyTankUp;
            BitmapDown = Resources.MyTankDown;
            BitmapLeft = Resources.MyTankLeft;
            BitmapRight = Resources.MyTankRight;
            HP = 5;//5条命
            this.Dir = Direction.Up;
        }

        //键盘按下时
        public void KeyDown(KeyEventArgs args)
        {
            //获取按键事件
            switch (args.KeyCode)
            {
                case Keys.W:
                    Dir = Direction.Up;
                    IsMoving = true;//简单移动
                    break;
                case Keys.S:
                    Dir = Direction.Down;
                    IsMoving = true;//简单移动
                    break;
                case Keys.A:
                    Dir = Direction.Left;
                    IsMoving = true;//简单移动
                    break;
                case Keys.D:
                    Dir = Direction.Right;
                    IsMoving = true;//简单移动
                    break;
                case Keys.Space:
                    Attack();//按下空格发射子弹
                    break;
            }
        }

        //键盘抬起时
        public void KeyUp(KeyEventArgs args)
        {
            //获取按键事件
            switch (args.KeyCode)
            {
                case Keys.W:
                    Dir = Direction.Up;
                    IsMoving = false;//简单移动
                    break;
                case Keys.S:
                    Dir = Direction.Down;
                    IsMoving = false;//简单移动
                    break;
                case Keys.A:
                    Dir = Direction.Left;
                    IsMoving = false;//简单移动
                    break;
                case Keys.D:
                    Dir = Direction.Right;
                    IsMoving = false;//简单移动
                    break;
            }
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
                if (Y - Speed < 0)
                {
                    IsMoving = false;
                    return;
                }
            }
            else if (Dir == Direction.Down)
            {
                if (Y + Speed + Height > 450)
                {
                    IsMoving = false;
                    return;
                }
            }
            else if (Dir == Direction.Left)
            {
                if (X - Speed < 0)
                {
                    IsMoving = false;
                    return;
                }
            }
            else if (Dir == Direction.Right)
            {
                if (X + Speed + Height > 450)
                {
                    IsMoving = false;
                    return;
                }
            }
            //得到当前位置
            Rectangle rect = GetRectangle();
            //得到将来位置(rect)（下一步的位置)用于判断方向的下一步是否为墙---可解决碰撞墙时不动的问题（可向无墙的方向走）
            switch (Dir)
            {
                case Direction.Up:
                    rect.Y -= (int)Math.Floor(Speed);
                    break;
                case Direction.Down:
                    rect.Y += (int)Math.Floor(Speed);
                    break;
                case Direction.Left:
                    rect.X -= (int)Math.Floor(Speed);
                    break;
                case Direction.Right:
                    rect.X += (int)Math.Floor(Speed);
                    break;
            }

            //墙体碰撞 使用内置的Rectangle判断矩形碰撞
            if (GameObjectManager.IsCollidedWall(rect) != null)
            {
                IsMoving = false;
                return;
            }
            if (GameObjectManager.IsCollidedStee(rect) != null)
            {
                IsMoving = false;
                return;
            }
            if (GameObjectManager.IsCollidedBoss(rect))
            {
                IsMoving = false;
                return;
            }
            //碰撞到敌人坦克敌人死亡并减少血量
            EnemyTank tank;
            if ((tank = GameObjectManager.IsCollidedEnemyTank(rect)) != null)
            {
                GameObjectManager.DestroyEnemyTank(tank);
                GameObjectManager.CreateExplosion(this.X + Width / 2, this.Y + Width / 2);//爆炸效果
                SoundManager.PlayHit();//受击音效
                TankDamage();//减少血量
                return;
            }

        }

        public void Move()
        {
            if (IsMoving == false) return;
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

        //攻击
        private void Attack()
        {
            double x = this.X;
            double y = this.Y;
            SoundManager.PlayFire();//发射音效
            //判断朝向
            switch (Dir)
            {
                case Direction.Up:
                    x = x + Width / 2;
                    break;
                case Direction.Down:
                    x = x + Width / 2;
                    y += Height;
                    break;
                case Direction.Left:
                    y = y + Height / 2;
                    break;
                case Direction.Right:
                    x += Width;
                    y = y + Height / 2;
                    break;
            }
            GameObjectManager.CreateBUllet(x, y, Dir, Tag.MyTank);
        }

        //被攻击时减少血量逻辑
        public void TankDamage()
        {
            HP--;
            if (HP <= 0)
            {
                MessageBox.Show("血量为零，游戏结束！", "提示", MessageBoxButtons.OK);
                GameFramework.ChangeGameOver();
            }
        }
    }
}
