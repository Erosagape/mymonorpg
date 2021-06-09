using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace mymonogame
{
    public class ImageEffect
    {
        protected Image Image;
        public bool IsActive;
        public ImageEffect()
        {
            IsActive = false;
        }
        public virtual void LoadContent(ref Image Image)
        {
            this.Image = Image;
        }
        public virtual void UnloadContent()
        {

        }
        public virtual void Update(GameTime gameTime)
        {

        }
    }
}
