using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankTest
{
    //设置可移动物体

    //定义枚举设置物体的朝向
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    internal class Movething : GameObject
    {
        private static readonly object dirLock = new object();//添加锁解决资源占用异常
        //物体上下左右的移动
        public Bitmap BitmapUp { get; set; }
        public Bitmap BitmapDown { get; set; }
        public Bitmap BitmapLeft { get; set; }
        public Bitmap BitmapRight { get; set; }

        //物体移动速度
        public double Speed { get; set; }

        //物体朝向  获得物体高宽
        //public Direction Dir { get; set; }

        private Direction dir;
        public Direction Dir
        {
            get { return dir; }
            set
            {
                dir = value;
                Bitmap bitmap = null;
                switch (dir)
                {
                    case Direction.Up:
                        bitmap = BitmapUp;
                        break;
                    case Direction.Down:
                        bitmap = BitmapDown;
                        break;
                    case Direction.Left:
                        bitmap = BitmapLeft;
                        break;
                    case Direction.Right:
                        bitmap = BitmapRight;
                        break;
                }
                lock (dirLock)
                {
                    Width = bitmap.Width;
                    Height = bitmap.Height;
                }
            }
        }

        protected override Image GetImage()
        {
            //设置透明属性
            Bitmap bitmap = null;

            switch (Dir)
            {
                case Direction.Up:
                    bitmap = BitmapUp;
                    break;
                case Direction.Down:
                    bitmap = BitmapDown;
                    break;
                case Direction.Left:
                    bitmap = BitmapLeft;
                    break;
                case Direction.Right:
                    bitmap = BitmapRight;
                    break;
                default:
                    bitmap = BitmapUp;//默认向上
                    break;
            }
            //设置透明
            bitmap.MakeTransparent(Color.Black);
            return bitmap;
        }

        //重写DrawSelf 加锁解决资源冲突问题
        //多线程当DrawSelf绘制的同时(调用有GetImage方法，GetImage方法中也需要使用到bitmap) bitmap可能需要对高宽进行操作从而引发资源冲突{bitmap的冲突}
        public override void DrawSelf()
        {
            lock (dirLock)
            {
                base.DrawSelf();
            }
        }
    }
}
