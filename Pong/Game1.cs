using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        InputState inputState;
        SpriteFont defaultFont;

        Paddle paddleOne;
        Paddle paddleTwo;
        Ball ball;

        Rectangle topWall;
        Rectangle bottomWall;
        Rectangle leftWall;
        Rectangle rightWall;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            inputState = new InputState();

            int wallHeight = 40;
            Vector2 center = GraphicsDevice.Viewport.TitleSafeArea.Center.ToVector2();
            topWall = new Rectangle(0, -wallHeight / 2, GraphicsDevice.Viewport.TitleSafeArea.Width, wallHeight);
            bottomWall = new Rectangle(0, (int)(center.Y*2 + wallHeight / 2), GraphicsDevice.Viewport.TitleSafeArea.Width, wallHeight);
            leftWall = new Rectangle(-wallHeight / 2, 0, wallHeight, GraphicsDevice.Viewport.TitleSafeArea.Height);
            rightWall = new Rectangle(GraphicsDevice.Viewport.Width + wallHeight / 2, 0, wallHeight, GraphicsDevice.Viewport.TitleSafeArea.Height);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Get font
            defaultFont = Content.Load<SpriteFont>("DefaultFont");

            // Get textures
            Texture2D paddleTexture = Content.Load<Texture2D>("Paddle");
            Texture2D ballTexture = Content.Load<Texture2D>("Ball");

            // Calculate positions
            Vector2 paddleOnePosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + paddleTexture.Width, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            Vector2 paddleTwoPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width - paddleTexture.Width, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);

            // Initialize GameObjects
            paddleOne = new Paddle(paddleTexture, paddleOnePosition, Keys.W, Keys.S, true);
            paddleTwo = new Paddle(paddleTexture, paddleTwoPosition, Keys.Up, Keys.Down, false);
            ball = new Ball(ballTexture, GraphicsDevice.Viewport.TitleSafeArea.Center.ToVector2(), new Vector2(400, 400));
            ball.Reset(GraphicsDevice);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            inputState.Update();

            // Exit game
            if (inputState.CurrentGamePadState.Buttons.Back == ButtonState.Pressed || inputState.CurrentKeyboardState.IsKeyDown(Keys.Escape))
                Exit();

            // Update GameObjects
            paddleOne.Update(gameTime, inputState, GraphicsDevice);
            paddleTwo.Update(gameTime, inputState, GraphicsDevice);
            ball.Update(gameTime);

            // Check collisions
            if (ball.BoundingBox.Intersects(paddleOne.BoundingBox) || ball.BoundingBox.Intersects(paddleTwo.BoundingBox))
                ball.Bounce(true);

            if (ball.BoundingBox.Intersects(topWall) || ball.BoundingBox.Intersects(bottomWall))
                ball.Bounce(false);

            // Winning conditions
            if (ball.BoundingBox.Intersects(leftWall))
            {
                paddleTwo.Score++;
                ball.Reset(GraphicsDevice);
            }

            if (ball.BoundingBox.Intersects(rightWall))
            {
                paddleOne.Score++;
                ball.Reset(GraphicsDevice);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            spriteBatch.Begin();

            // Draw GameObjects
            paddleOne.Draw(spriteBatch);
            paddleTwo.Draw(spriteBatch);
            ball.Draw(spriteBatch);

            // Write score
            string score = paddleOne.Score + ":" + paddleTwo.Score;
            spriteBatch.DrawString(defaultFont, score, new Vector2((GraphicsDevice.Viewport.Width / 2) - (defaultFont.MeasureString(score).X / 2), 0), Color.Black);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }

    /// <summary>
    /// Struct for storing current and previous input states
    /// For Keyboard, GamePad and Mouse
    /// </summary>
    struct InputState
    {
        public KeyboardState CurrentKeyboardState { get; set; }
        public KeyboardState PreviousKeyboardState { get; set; }
        public GamePadState CurrentGamePadState { get; set; }
        public GamePadState PreviousGamePadState { get; set; }
        public MouseState CurrentMouseState { get; set; }
        public MouseState PreviousMouseState { get; set; }

        public void Update()
        {
            // Set previous
            if (CurrentKeyboardState != null)
                PreviousKeyboardState = CurrentKeyboardState;
            if (CurrentGamePadState != null)
                PreviousGamePadState = CurrentGamePadState;
            if (CurrentMouseState != null)
                PreviousMouseState = CurrentMouseState;

            // Set current
            CurrentKeyboardState = Keyboard.GetState();
            CurrentGamePadState = GamePad.GetState(PlayerIndex.One);
            CurrentMouseState = Mouse.GetState();
        }
    }
}
