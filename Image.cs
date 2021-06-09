using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace mymonogame
{
    public class Image
    {
        public float Alpha;
        public string Text, FontName, Path,Effects;
        public Vector2 Position,Scale;
        public bool IsActive;
        public FadeEffect FadeEffect;

        Texture2D texture;
        Rectangle sourceRect;
        Vector2 origin;
        ContentManager content;
        RenderTarget2D renderTarget;
        SpriteFont font;
        Dictionary<string, ImageEffect> effectList;

        public Image()
        {
            Path = Text = Effects= String.Empty;
            FontName = "Fonts/Arial";
            Alpha = 1.0f;
            Position = Vector2.Zero;
            Scale = Vector2.One;
            sourceRect = Rectangle.Empty;
            effectList = new Dictionary<string, ImageEffect>();
        }
        void SetEffect<T>(ref T effect)
        {
            if (effect == null)
                effect = (T)Activator.CreateInstance(typeof(T));
            else
            {
                (effect as ImageEffect).IsActive = true;
                var obj = this;
                (effect as ImageEffect).LoadContent(ref obj);
            }
            effectList.Add(effect.GetType().ToString().Replace("mymonogame.",""),(effect as ImageEffect));
        }
        public void ActivateEffect(string effect)
        {
            if (effectList.ContainsKey(effect))
            {
                effectList[effect].IsActive = true;
                var obj = this;
                effectList[effect].LoadContent(ref obj);
            }
        }
        public void DeactivateEffect(string effect)
        {
            if (effectList.ContainsKey(effect))
            {
                effectList[effect].IsActive = false;
                effectList[effect].UnloadContent();
            }
        }
        public void LoadContent()
        {
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");
            if (Path != String.Empty)
            {
                texture = content.Load<Texture2D>(Path);
            }
            Vector2 dimensions = Vector2.Zero;
            font = content.Load<SpriteFont>(FontName);
            dimensions.X += font.MeasureString(Text).X;
            if (texture != null)
            {
                dimensions.X += texture.Width;
                dimensions.Y = Math.Max(texture.Height, font.MeasureString(Text).Y);
            } else
            {
                dimensions.Y = font.MeasureString(Text).Y;
            }
            if (sourceRect == Rectangle.Empty)
            {
                sourceRect = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);
            }

            renderTarget = new RenderTarget2D(ScreenManager.Instance.GraphicsDevice, (int)dimensions.X, (int)dimensions.Y);
            
            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(renderTarget);
            ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);

            ScreenManager.Instance.SpriteBatch.Begin();            
            if(texture != null)
            {
                ScreenManager.Instance.SpriteBatch.Draw(texture, Vector2.Zero, Color.White);
            }
            ScreenManager.Instance.SpriteBatch.DrawString(font, Text, Vector2.Zero, Color.White);
            ScreenManager.Instance.SpriteBatch.End();
            
            texture = renderTarget;
            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null);

            SetEffect<FadeEffect>(ref FadeEffect);
            if (Effects != String.Empty)
            {
                string[] split = Effects.Split(':');
                foreach(string item in split)
                {
                    ActivateEffect(item);
                }
            }
        }
        public void UnloadContent()
        {
            content.Unload();
            foreach (var effect in effectList)
            {
                DeactivateEffect(effect.Key);
            }
        }
        public void Update(GameTime gameTime)
        {
            foreach(var effect in effectList)
            {
                if(effect.Value.IsActive)
                    effect.Value.Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
            spriteBatch.Draw(texture, Position+origin, sourceRect, Color.White * Alpha,0.0f,origin,Scale,SpriteEffects.None,0.0f);
        }
    }
}
