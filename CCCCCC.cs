using Celeste.Mod;
using Microsoft.Xna.Framework;
using MonoMod.Detour;
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

        private readonly static MethodInfo m_Update = typeof(Player).GetMethod("Update", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        private readonly static MethodInfo m_Jump = typeof(Player).GetMethod("Jump", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        private readonly static MethodInfo m_Render = typeof(Player).GetMethod("Render", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        
        private static bool FlipGravity = false;
        private static bool FlipLanded = false;

        public CCCCCCModule()
        {
            Instance = this;
        }

        public override void Load()
        {
            Type t_CCCCCCModule = GetType();
            orig_Update = m_Update.Detour<d_Update>(t_CCCCCCModule.GetMethod("Update"));
            orig_Jump = m_Jump.Detour<d_Jump>(t_CCCCCCModule.GetMethod("Jump"));
            orig_Render = m_Render.Detour<d_Render>(t_CCCCCCModule.GetMethod("Render"));
        }

        public override void Unload()
        {
            RuntimeDetour.Undetour(m_Update);
            RuntimeDetour.Undetour(m_Jump);
            RuntimeDetour.Undetour(m_Render);
        }

        public delegate void d_Update(Player self);
        public static d_Update orig_Update;
        public static void Update(Player self)
        {
            Console.WriteLine("PlayerUpdate");
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
                    self.UseRefill();
                    FlipLanded = true;
                }
            }

            //self.Test(new Vector2(2, 5));

            if (FlipGravity) self.SetSpeed(new Vector2(self.GetSpeed().X, -220f));
        }

        public delegate void d_Jump(Player self, bool particles = true, bool playSfx = true);
        public static d_Jump orig_Jump;
        public static void Jump(Player self, bool particles = true, bool playSfx = true)
        {
            Console.WriteLine("PlayerJump");
            if (!Settings.Enabled) orig_Jump(self, particles, playSfx);
        }

        public delegate void d_Render(Player self);
        public static d_Render orig_Render;
        public static void Render(Player self)
        {
            Console.WriteLine("PlayerRender");
            orig_Render(self);
        }

    }
}