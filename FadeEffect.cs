using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace mymonogame
{
    public class FadeEffect : ImageEffect
    {
        public float FadeSpeed;
        public bool Increase;
        public FadeEffect()
        {
            FadeSpeed = 200;
            Increase = false;
        }
        public override void LoadContent(ref Image Image)
        {
            base.LoadContent(ref Image);
        }
        public override void UnloadContent()
        {
            base.UnloadContent();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Image.IsActive)
            {
                if (!Increase)
                    Image.Alpha -= FadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                    Image.Alpha += FadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (Image.Alpha < 0.0f)
                {
                    Increase = true;
                    Image.Alpha = 0.0f;
                } else if (Image.Alpha > 1.0f)
                {
                    Increase = false;
                    Image.Alpha = 1.0f;
                }
            } else
            {
                Image.Alpha = 1.0f;
            }
        }
    }
}
