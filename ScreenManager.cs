using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace mymonogame
{
    public class ScreenManager
    {
        private static ScreenManager instance;
        GameScreen currentScreen,newScreen;
        [XmlIgnore]
        XmlManager<GameScreen> xmlGameScreenManager;
        [XmlIgnore]
        public Vector2 Dimensions { get; set; }
        [XmlIgnore]
        public ContentManager Content { get; set; }
        [XmlIgnore]
        public GraphicsDevice GraphicsDevice;
        [XmlIgnore]
        public SpriteBatch SpriteBatch;
        public Image Image;
        [XmlIgnore]
        public bool IsTransitioning { get; set; }
        public ScreenManager()
        {
            Dimensions = new Vector2(640, 480);
            xmlGameScreenManager = new XmlManager<GameScreen>();

            LoadSplashScreen();
        }
        public void ChangeScreen(string screenName)
        {
            newScreen = (GameScreen)Activator.CreateInstance(Type.GetType("mymonogame." + screenName));
            Image.IsActive = true;
            Image.FadeEffect.Increase = true;
            Image.Alpha = 0.0f;
            IsTransitioning = true;
        }
        void Transition(GameTime gameTime)
        {
            if (IsTransitioning)
            {
                Image.Update(gameTime);
                if (Image.Alpha == 1.0f)
                {
                    currentScreen.UnloadContent();
                    currentScreen = newScreen;
                    xmlGameScreenManager.Type = currentScreen.Type;
                    if (File.Exists(currentScreen.XmlPath))
                       xmlGameScreenManager.Load(currentScreen.XmlPath);
                    currentScreen.LoadContent();
                } else if (Image.Alpha == 0.0f)
                {
                    Image.IsActive = false;
                    IsTransitioning = false;
                }
            }
        }
        private void LoadSplashScreen()
        {
            currentScreen = new SplashScreen();
            xmlGameScreenManager.Type = currentScreen.Type;
            currentScreen = xmlGameScreenManager.Load("Load/SplashScreen.xml");
        }
        public static ScreenManager Instance 
        { 
            get
            {
                if (instance == null)
                {
                    XmlManager<ScreenManager> xml = new XmlManager<ScreenManager>();
                    instance = xml.Load("Load/ScreenManager.xml");
                }
                return instance;
            }
        }
        public static void Setup(GraphicsDevice device,SpriteBatch spriteBatch,ContentManager content)
        {
            instance.GraphicsDevice = device;
            instance.SpriteBatch = spriteBatch;
            instance.LoadContent(content);
        }
        public void LoadContent(ContentManager content)
        {
            this.Content = new ContentManager(content.ServiceProvider, "Content");
            currentScreen.LoadContent();
            Image.LoadContent();
        }
        public void UnloadContent()
        {
            currentScreen.UnloadContent();
            Image.UnloadContent();
        }
        public void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);
            Transition(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
            if (IsTransitioning)
                Image.Draw(spriteBatch);
        }
    }
}
