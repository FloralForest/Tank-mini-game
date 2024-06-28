using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankTest
{
    //设置坐标轴
    internal abstract class GameObject
    {
        public double X { get; set; }
        public double Y { get; set; }

        //定义矩形
        public Rectangle GetRectangle()
        {
            return new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
        }

        //定义高宽
        public double Width { get; set; } 
        public double Height { get; set; }

        //绘制自身{绘制游戏}
        protected abstract Image GetImage();
        //添加虚方法方便子类重写
        public virtual void DrawSelf()
        {
            Graphics graphics = GameFramework.gs;
            graphics.DrawImage(GetImage(), (float)X, (float)Y);
        }

        //添加虚方法方便子类重写
        public virtual void Update()
        {
            DrawSelf();
        }
    }
}
