using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TankTest
{
    //游戏逻辑
    enum GameState
    {
        Running,
        GameOver
    }

    internal class GameFramework
    {

        public static Graphics gs;
        private static GameState gameState = GameState.Running;
        //游戏开始时
        public static void Start()
        {
            GameObjectManager.CreateMap();//绘制墙
            GameObjectManager.CreateMyTank();//我的坦克位置
            GameObjectManager.Start();//生成敌人位置

        }

        //游戏运行时
        public static void Update()
        {
            if (gameState == GameState.Running)
            {
                GameObjectManager.Update();
            }
            else
            {
                GameOverUpdate();
            }
        }
        private static void GameOverUpdate()
        {
            Properties.Resources.GameOver.MakeTransparent(Color.Black);
            int x = 450 / 2 - Properties.Resources.GameOver.Width / 2;
            int y = 450 / 2 - Properties.Resources.GameOver.Height / 2;
            gs.DrawImage(Properties.Resources.GameOver, x, y);

        }

        public static void ChangeGameOver()
        {
            gameState = GameState.GameOver;
        }
    }
}
