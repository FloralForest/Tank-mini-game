using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankTest.Properties;

namespace TankTest
{
    //控制爆炸效果
    internal class Explosion : GameObject
    {
        //用于控制图片销毁
        public bool IsNeedDestroy { get; set; }

        //定义爆炸动画帧数
        private int playSpeed = 2;
        private int playCount = 0;
        private int index = 0;

        //爆炸图片加入集合
        private Bitmap[] bitmap = new Bitmap[]{
            Resources.EXP1,
            Resources.EXP2,
            Resources.EXP3,
            Resources.EXP4,
            Resources.EXP5
        };
        //设置透明和获取中心点
        public Explosion(double x, double y)
        {
            foreach (Bitmap bmp in bitmap)
            {
                bmp.MakeTransparent(Color.Black);
            }
            this.X = x - bitmap[0].Width / 2;
            this.Y = y - bitmap[0].Height / 2;
            IsNeedDestroy = false;
        }

        protected override Image GetImage()
        {
            if (index > 4) return bitmap[4];
            return bitmap[index];
        }

        public override void Update()
        {
            playCount++;
            index = (playCount - 1) / playSpeed; //每张图片停留2帧
            /*
            1 - 1 / 2 = 0
            2 - 1 / 2 = 0
            3 - 1 / 2 = 1
            4 - 1 / 2 = 1
            5 - 1 / 2 = 2
            6 - 1 / 2 = 2
             */
            if (index > 4)
            {
                IsNeedDestroy = true;
            }
            base.Update();//绘制
        }
    }
}
