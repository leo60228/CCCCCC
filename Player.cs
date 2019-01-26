#pragma warning disable CS0626 // Method, operator, or accessor is marked external and has no attributes on it

using Celeste.Mod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monocle;
using MonoMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Celeste
{
    public static class PlayerExt
    {
        public static bool UseRefill(this Player self)
             => self.UseRefill();

        public static Vector2 GetPosition(this Player self)
            => self.Position;

        public static void SetPosition(this Player self, Vector2 position)
            => self.SetPosition(position);

        public static Vector2 GetPreviousPosition(this Player self)
            => self.PreviousPosition;

        public static Vector2 GetSpeed(this Player self)
            => self.Speed;

        public static void SetSpeed(this Player self, Vector2 speed)
            => self.SetSpeed(speed);

    }
}