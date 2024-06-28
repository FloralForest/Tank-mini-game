using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TankTest
{
    public partial class Form1 : Form
    {
        private Thread thread;
        private static Graphics graphics;
        private static Bitmap bmp;

        public Form1()
        {
            InitializeComponent();
            //窗体生成于中间
            this.StartPosition = FormStartPosition.CenterScreen;

            //绘图画布 直接画于窗体会有闪烁,因为这个方法会刷新以用于监控物体移动的帧数
            graphics = this.CreateGraphics();
            //解决闪烁问题，先把元素绘制在临时图片上再一起放入画布
            bmp = new Bitmap(515,490);
            Graphics bmpG = Graphics.FromImage(bmp);
            //赋值给GameFramework 在其中编写逻辑
            GameFramework.gs = bmpG;

            //线程
            thread = new Thread(new ThreadStart(GameMinThread));
            thread.Start();//开始
        }

        //控制游戏运行的逻辑
        private static void GameMinThread()
        {
            SoundManager.PlayStart();//播放音乐
            GameFramework.Start();
            
            while (true)
            {
                //画布涂黑
                GameFramework.gs.Clear(Color.Black);
                GameFramework.Update();
                //绘制画布 从0，0坐标开始画
                graphics.DrawImage(bmp, 0, 0);

                Thread.Sleep(1000 / 60);//休息1/60秒
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //当窗体关闭时关闭线程
            thread.Abort();
        }

        //监听键盘按下和抬起
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            GameObjectManager.KeyUp(e);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            GameObjectManager.KeyDown(e);
        }
    }
}
