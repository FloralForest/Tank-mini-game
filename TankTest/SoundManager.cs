using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using TankTest.Properties;

namespace TankTest
{
    //音效
    internal class SoundManager
    {
        //游戏音效
        private static SoundPlayer soundPlayer = new SoundPlayer(Resources.start);//开始
        private static SoundPlayer addPlayer = new SoundPlayer(Resources.add);//增加敌人
        private static SoundPlayer blastPlayer = new SoundPlayer(Resources.blast);//爆炸
        private static SoundPlayer firePlayer = new SoundPlayer(Resources.fire);//发射子弹
        private static SoundPlayer hitPlayer = new SoundPlayer(Resources.hit);//受击

        public static void PlayStart()
        {
            soundPlayer.Play();//开始
        }
        public static void PlayAdd()
        {
            addPlayer.Play();//开始
        }
        public static void PlayBlast()
        {
            blastPlayer.Play();//开始
        }
        public static void PlayFire()
        {
            firePlayer.Play();//开始
        }
        public static void PlayHit()
        {
            hitPlayer.Play();//开始
        }
    }
}
