using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankTest.Properties;

namespace TankTest
{
    //敌人
    internal class EnemyTank : Movething
    {
        private int AttackSpeed { get; set; }//攻击速度 每60帧攻击一次(1秒一次)
        private int AttackCount = 0;
        private Random random = new Random();

        private int ChangeCount = 0;//计数器
        public EnemyTank(double x, double y, double speed, Bitmap bmpUp, Bitmap bmpDown, Bitmap bmpLeft, Bitmap bmpRight)
        {
            this.X = x;
            this.Y = y;
            this.Speed = speed;
            BitmapUp = bmpUp;
            BitmapDown = bmpDown;
            BitmapLeft = bmpLeft;
            BitmapRight = bmpRight;
            this.Dir = Direction.Down;
            AttackSpeed = 60;
        }

        //移动 重写虚方法
        public override void Update()
        {
            MoveCheck();//检查
            Move();
            AttackCheck();
            base.Update();
        }

        public async void MoveCheck()
        {
            //超出边界
            if (Dir == Direction.Up)
            {
                if (Y - Speed < 0)
                {
                    ChangeDirection();
                    return;
                }
            }
            else if (Dir == Direction.Down)
            {
                if (Y + Speed + Height > 450)
                {
                    ChangeDirection();
                    return;
                }
            }
            else if (Dir == Direction.Left)
            {
                if (X - Speed < 0)
                {
                    ChangeDirection();
                    return;
                }
            }
            else if (Dir == Direction.Right)
            {
                if (X + Speed + Height > 450)
                {
                    ChangeDirection();
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
                Speed = 0;//当撞到破坏墙时速度为0
                await Task.Delay(2000); // 延迟二秒 异步操作
                ChangeDirection();
                Speed = 1;
                return;
            }
            if (GameObjectManager.IsCollidedStee(rect) != null)
            {
                ChangeDirection();
                return;
            }
            if (GameObjectManager.IsCollidedBoss(rect))
            {
                ChangeDirection();
                return;
            }

            //每200帧随机朝向
            if (ChangeCount == 200)
            {
                ChangeDirection();
                ChangeCount = 0;
            }
            else
            {
                ChangeCount++;
            }
        }

        //随机朝向
        private void ChangeDirection()
        {
            int maxAttempts = 100; // 设置最大尝试次数
            int attempts = 0; // 记录当前尝试次数

            while (true)
            {
                Direction dir = (Direction)random.Next(0, 4);
                if (dir != Dir)
                {
                    Dir = dir;
                    break;
                }

                attempts++;
                if (attempts >= maxAttempts)
                {
                    // 达到最大尝试次数，采取其他处理方式
                    Dir = 0;
                    break;
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

        private void AttackCheck()
        {
            AttackCount++;
            if (AttackCount < AttackSpeed) return;
            Attack();
            AttackCount = 0;
        }
        //攻击
        private void Attack()
        {
            double x = this.X;
            double y = this.Y;
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
            GameObjectManager.CreateBUllet(x, y, Dir, Tag.EnemyTank);
        }
    }
}
