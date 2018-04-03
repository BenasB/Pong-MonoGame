using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong;
using System;

class Ball : GameObject
{
    Vector2 startVelocity;

    public Ball(Texture2D tex, Vector2 pos, Vector2 vel) : base(tex, pos, vel)
    {
        startVelocity = vel;
    }

    public override void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Move the ball constantly
        position += velocity * deltaTime;
    }

    public void Bounce(bool paddleHit)
    {
        if (!paddleHit)
            velocity.Y *= -1;

        if (paddleHit)
            velocity.X *= -1;
    }

    /// <summary>
    /// Resets the ball to starting position
    /// </summary>
    public void Reset(GraphicsDevice graphicsDevice)
    {
        // Randomize movement
        Random rnd = new Random();
        int randomNumber = rnd.Next(0, 2);
        int[] variants = { -1, 1 };

        velocity = startVelocity;
        velocity *= variants[randomNumber];
        position = graphicsDevice.Viewport.TitleSafeArea.Center.ToVector2();
    }
}
