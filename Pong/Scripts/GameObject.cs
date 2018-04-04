using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    class GameObject
    {
        protected Texture2D texture;
        protected Vector2 position;
        protected Vector2 velocity;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value;  }
        }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)position.X + texture.Width / 2, (int)position.Y - texture.Height / 2, texture.Width, texture.Height);
            }
        }

        public GameObject(Texture2D tex, Vector2 pos)
        {
            position = pos;
            texture = tex;
        }

        public GameObject(Texture2D tex, Vector2 pos, Vector2 vel)
        {
            position = pos;
            texture = tex;
            velocity = vel;
        }

        public virtual void Update() { }

        public virtual void Update(GameTime gameTime, InputState inputState, GraphicsDevice graphicsDevice) { }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), Vector2.One, SpriteEffects.None, 0f);
        }
    }
}