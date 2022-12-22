using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.RuntimeDetour.HookGen;
using ReLogic.Content;
using Terraria.ModLoader;

namespace nerd
{
    public sealed class nerd : Mod
    {
        private delegate void SpriteBatch_PushSprite(
            SpriteBatch self,
            Texture2D texture,
            float sourceX,
            float sourceY,
            float sourceW,
            float sourceH,
            float destinationX,
            float destinationY,
            float destinationW,
            float destinationH,
            Color color,
            float originX,
            float originY,
            float rotationSin,
            float rotationCos,
            float depth,
            byte effects
        );

        private Asset<Texture2D> nerdTexture;

        public override void Load()
        {
            base.Load();

            var sbType = typeof(SpriteBatch);
            var psMeth = sbType.GetMethod("PushSprite", BindingFlags.NonPublic | BindingFlags.Instance);

            HookEndpointManager.Add(psMeth, PushNerdSprite);
        }

        public override void PostSetupContent()
        {
            base.PostSetupContent();

            RequestAssetIfExists<Texture2D>("nerd", out nerdTexture);
        }

        public override void Unload()
        {
            base.Unload();

            nerdTexture = null;
        }

        private void PushNerdSprite(
            SpriteBatch_PushSprite orig,
            SpriteBatch self,
            Texture2D texture,
            float sourceX,
            float sourceY,
            float sourceW,
            float sourceH,
            float destinationX,
            float destinationY,
            float destinationW,
            float destinationH,
            Color color,
            float originX,
            float originY,
            float rotationSin,
            float rotationCos,
            float depth,
            byte effects)
        {
            if (nerdTexture is not null) texture = nerdTexture.Value;

            orig(self, texture, sourceX, sourceY, sourceW, sourceH, destinationX, destinationY, destinationW, destinationH, color, originX, originY, rotationSin, rotationCos, depth, effects);
        }
    }
}