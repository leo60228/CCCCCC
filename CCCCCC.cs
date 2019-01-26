using Celeste.Mod;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Mod.CCCCCC
{
    public class CCCCCCModule : EverestModule
    {

        public static CCCCCCModule Instance;

        public override Type SettingsType => typeof(CCCCCCModuleSettings);
        public static CCCCCCModuleSettings Settings => (CCCCCCModuleSettings)Instance._Settings;

        private static bool FlipGravity = false;
        private static bool FlipLanded = false;

        public CCCCCCModule()
        {
            Instance = this;
        }

        public override void Load()
        {
            On.Celeste.Player.Update += Update;
            On.Celeste.Player.Jump += Jump;
            On.Celeste.Player.Render += Render;
        }

        public override void Unload()
        {
            On.Celeste.Player.Update -= Update;
            On.Celeste.Player.Jump -= Jump;
            On.Celeste.Player.Render -= Render;
        }

        public static void Update(On.Celeste.Player.orig_Update orig_Update, Player self)
        {
            // Console.WriteLine("PlayerUpdate");
            orig_Update(self);

            if (!Settings.Enabled) return;

            if (Input.Jump.Pressed && (FlipLanded || self.OnGround()))
            {
                FlipGravity = !FlipGravity;
                FlipLanded = false;
            }
            else if (!FlipLanded)
            {
                if (FlipGravity && self.GetPreviousPosition().Y == self.GetPosition().Y)
                {
                    self.Dashes = self.MaxDashes;
                    FlipLanded = true;
                }
            }

            //self.Test(new Vector2(2, 5));

            if (FlipGravity) self.Speed = new Vector2(self.GetSpeed().X, -220f);
        }

        public static void Jump(On.Celeste.Player.orig_Jump orig_Jump, Player self, bool particles = true, bool playSfx = true)
        {
            // Console.WriteLine("PlayerJump");
            if (!Settings.Enabled) orig_Jump(self, particles, playSfx);
        }

        public static void Render(On.Celeste.Player.orig_Render orig_Render, Player self)
        {
            // Console.WriteLine("PlayerRender");
            orig_Render(self);
        }

    }
}
