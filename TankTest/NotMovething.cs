using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankTest
{
    //设置不可移动物体(墙)
    internal class NotMovething : GameObject
    {
        private static readonly object imageLock = new object();//添加锁解决资源占用异常
        private Image img;
        public Image Img
        {
            get
            {
                lock (imageLock)
                {
                    return img;
                }
            }
            set
            {
                lock (imageLock)
                {
                    img = value;
                    Width = img.Width;
                    Height = img.Height;
                }
            }
        }

        protected override Image GetImage()
        {
            return Img;
        }

        public NotMovething()
        {
        }

        public NotMovething(double x, double y, Image img)
        {
            this.X = x;
            this.Y = y;
            this.Img = img;
        }
    }
}
