using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong;

class Paddle : GameObject
{
    Keys up;
    Keys down;
    bool useLeftThumbstick;

    public int Score { get; set; }

    public Paddle(Texture2D tex, Vector2 pos, Keys upKey, Keys downKey, bool leftThumbstick) : base(tex, pos)
    {
        up = upKey;
        down = downKey;
        useLeftThumbstick = leftThumbstick;

        velocity.Y = 500;
    }

    public override void Update(GameTime gameTime, InputState inputState, GraphicsDevice graphicsDevice)
    {
        // Get delta time for accurate movement
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        float movement = velocity.Y * deltaTime;

        // Move based on thumbsticks
        position.Y -= ((useLeftThumbstick) ? inputState.CurrentGamePadState.ThumbSticks.Left.Y : inputState.CurrentGamePadState.ThumbSticks.Right.Y) * movement;
        
        // Move based on keys
        if (inputState.CurrentKeyboardState.IsKeyDown(up))
        {
            position.Y -= movement;
        }

        if (inputState.CurrentKeyboardState.IsKeyDown(down))
        {
            position.Y += movement;
        }

        // Don't let the paddle get out of bounds
        position.Y = MathHelper.Clamp(position.Y, texture.Height / 2, graphicsDevice.Viewport.Height - texture.Height / 2);
    }
}
