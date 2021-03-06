using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace mymonogame
{
    public class GameScreen
    {
        protected ContentManager content;
        [XmlIgnore]
        public Type Type;
        public string XmlPath;
        public GameScreen()
        {
            Type = this.GetType();
            XmlPath ="Load/"+ Type.ToString().Replace("mymonogame.","") + ".xml";
        }
        public virtual void LoadContent() 
        {
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");
        }
        public virtual void UnloadContent() 
        {
            content.Unload();
        }
        public virtual void Update(GameTime gameTime) 
        {
            InputManager.Instance.Update();
        }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
