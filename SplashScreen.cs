using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace mymonogame
{
    public class SplashScreen:GameScreen
    {
        public Image Image;
        public override void LoadContent()
        {
            base.LoadContent();
            Image.LoadContent();
        }
        public override void UnloadContent()
        {
            base.UnloadContent();
            Image.UnloadContent();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Image.Update(gameTime);
            if(InputManager.Instance.KeyPressed(Keys.Enter,Keys.Z))
            {
                ScreenManager.Instance.ChangeScreens("SplashScreen");
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Image.Draw(spriteBatch);
        }
    }
}
