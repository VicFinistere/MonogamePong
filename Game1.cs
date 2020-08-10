using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        // Textures                 // ■ texture
        Texture2D playerBar;        // Texture player bar
        Texture2D opponentBar;      // Texture opponent bar
        Texture2D ball;             // Texture ball
    
        // Game objects 
        private Rectangle playerRect;
        private Rectangle ballRect;
        private Rectangle opponentRect;


        // Position                 // ▼ pos
        Vector2 playerPosition;     // Player position
        Vector2 opponentPosition;   // Opponent position
        Vector2 ballPosition;       // Ball position

        // Ball directions
        private float x_direction = 0; 
        private float y_direction = 0;

        // Speed                    // ((═ ■ speed
        float playerSpeed;          // Player speed
        float opponentSpeed;       // Opponent speed
        float ballSpeed;            // Ball speed
        private string debug_text;

        // Font
        private SpriteFont font;

        // Score
        private int player_score = 0;
        private int opponent_score = 0;
       
        // Random
        private Random random = new Random();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
             /// <summary>method <c>Initialize</c> Initialize the game.</summary>

            // Initialization logic here
            
            // SHAPES
            playerPosition = new Vector2(_graphics.PreferredBackBufferWidth - 70, _graphics.PreferredBackBufferHeight / 2); // Player shapes
            opponentPosition = new Vector2(70, _graphics.PreferredBackBufferHeight / 2);                                    // Opponent shapes
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);    // Ball shapes

            // SPEED
            playerSpeed = 400f;                                                                                             // Player speed
            opponentSpeed = 1000f;                                                                                           // Opponent speed
            ballSpeed = 200f;         
                                                                                            // Ball speed

            base.Initialize();
        }

        protected override void LoadContent()
        {
            /// <summary>method <c>LoadContent</c> load the game content.</summary>

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TEXTURES
            playerBar = new Texture2D(GraphicsDevice, 10, 180);   // Player texture
            opponentBar = new Texture2D(GraphicsDevice, 10, 180); // Opponent texture
            ball = new Texture2D(GraphicsDevice, 5, 5);           // Ball texture

            // FONT
            // Here we use the Score.sprintefont file name
            font = Content.Load<SpriteFont>("Score");             // Score

        }

        protected override void Update(GameTime gameTime)
        {
            /// <summary>method <c>Update</c> update the game.</summary>

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Update logic here
            
            // Game Objects
            playerRect = playerBar.Bounds;                  // Player 
            playerRect.X = (int)playerPosition.X;           // Player.x
            playerRect.Y = (int)playerPosition.Y;           // Player.y

            opponentRect = opponentBar.Bounds;              // Opponent
            opponentRect.X = (int)opponentPosition.X;       // Opponent.x
            opponentRect.Y = (int)opponentPosition.Y;       // Opponent.y    
            
            ballRect = ball.Bounds;                         // Ball
            ballRect.X = (int)ballPosition.X;               // Ball .x
            ballRect.Y = (int)ballPosition.Y;               // Ball.y

            // KEY EVENT HANDLER 
            var kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.Up))                   // Up event
                if(playerPosition.Y > 0)
                playerPosition.Y -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(kstate.IsKeyDown(Keys.Down))                 // Down event
                if((playerPosition.Y + playerRect.Height) < _graphics.PreferredBackBufferHeight)
                playerPosition.Y += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;


            // Ball launcher
            if(x_direction == 0 && y_direction == 0){
                int direction = random.Next(0, 3);
                if(direction == 0){
                    x_direction = ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    y_direction = ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                } else if(direction == 1) {
                    x_direction = (ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds) * -1;
                    y_direction = (ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds) * -1;
                } else if(direction == 2){
                    x_direction = ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    y_direction = (ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds) * -1;
                } else if(direction == 3){
                    x_direction = (ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds) * -1;
                    y_direction = ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }

            // Ball event
            if(ballPosition.X <= ((_graphics.PreferredBackBufferWidth/ 2) - 50)){
                
                if(ballPosition.X <= ((_graphics.PreferredBackBufferWidth/ 2) - 50)){
                
                    // Up
                    if (opponentPosition.Y < ballPosition.Y )                  
                        opponentPosition.Y += opponentSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    // Down
                    if ((opponentPosition.Y + (opponentRect.Height / 2)) > ballPosition.Y + ballRect.Height)                  
                        opponentPosition.Y -= opponentSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    
                } else {
                    
                     // Up
                    if (ballPosition.Y < opponentPosition.Y)                  
                        opponentPosition.Y -= opponentSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    // Down
                    if (ballPosition.Y > opponentPosition.Y)                  
                        opponentPosition.Y += opponentSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

            } else {
                if((opponentPosition.Y + opponentRect.Height) > (_graphics.PreferredBackBufferHeight / 2 )){
                    if((opponentPosition.Y + opponentRect.Height)  > (_graphics.PreferredBackBufferHeight))
                        opponentPosition.Y -= (opponentSpeed/10) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                } else if((opponentPosition.Y - opponentRect.Height)  < (_graphics.PreferredBackBufferHeight/ 2)) {
                    if( (opponentPosition.Y - opponentRect.Height) < (_graphics.PreferredBackBufferHeight))
                        opponentPosition.Y += (opponentSpeed/10) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            
            // Ball collisions
            if(ballRect.Intersects(playerRect)){                                            // Right event
                // playerBar.Height = 120
                x_direction = (ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds) * -1;
                ballSpeed = 400;
            }
            else if(ballRect.Intersects(opponentRect)){
                x_direction = ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                ballSpeed = 400;
            }
            else if(ballRect.Y < 0){
                y_direction = ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if(ballRect.Y  > _graphics.PreferredBackBufferHeight){
                y_direction = (ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds) * -1;
            }



            ballPosition.X = ballPosition.X + x_direction;
            ballPosition.Y = ballPosition.Y + y_direction;
            
            // Detect out of screen for the ball and gives score
            if(ballPosition.X > (_graphics.PreferredBackBufferWidth - 10)){
                
                // Opponent scores
                opponent_score += 1;
                
                // Reset ball position
                ballSpeed = 200;
                x_direction = 0; 
                y_direction = 0;
                ballPosition.X = _graphics.PreferredBackBufferWidth / 2;
                ballPosition.Y = _graphics.PreferredBackBufferHeight / 2;

                // Pause
                System.Threading.Thread.Sleep(2000);
            }
                    
            else if(ballPosition.X < 0){
                
                // Player scores
                player_score += 1;

                // Reset ball position
                ballSpeed = 200;
                x_direction = 0; 
                y_direction = 0;
                ballPosition.X = _graphics.PreferredBackBufferWidth / 2;
                ballPosition.Y = _graphics.PreferredBackBufferHeight / 2;

                // Pause
                System.Threading.Thread.Sleep(2000);
            }

            // Game object size
            Color[] player_data = new Color[10*180];                                    // Player size
            Color[] opponent_data = new Color[10*180];                                     // Opponent size
            Color[] ball_data = new Color[5*5];                                         // Ball size

            // Game object color (for each item in game object Color[] set it to a specific color)
            for(int i=0; i < player_data.Length; ++i) player_data[i] = Color.White;     // Player color                     
            for(int i=0; i < opponent_data.Length; ++i) opponent_data[i] = Color.White;       // Opponent color                     
            for(int i=0; i < ball_data.Length; ++i) ball_data[i] = Color.White;         // Ball color
            
            // Define game objects data
            playerBar.SetData(player_data);                                             // Define player
            opponentBar.SetData(opponent_data);                                               // Define opponent
            ball.SetData(ball_data);                                                    // Define ball

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            /// <summary>method <c>Draw</c> renders the game.</summary>

            GraphicsDevice.Clear(Color.Black);

            // Drawing code here
            _spriteBatch.Begin();

            // Player
            _spriteBatch.Draw(playerBar, playerPosition, Color.White);                      // Draw player
            
            // Opponent
            _spriteBatch.Draw(opponentBar, opponentPosition, Color.White);                  // Draw opponent
            
            // Ball
            _spriteBatch.Draw(ball, ballPosition, Color.White);                             // Draw Ball
           
            // Score
            _spriteBatch.DrawString(font, $"{opponent_score}", new Vector2(180, 70), Color.White);     
            _spriteBatch.DrawString(font, $"{player_score}", new Vector2(_graphics.PreferredBackBufferWidth - 180, 70), Color.White);     
            
            // Separator 
            string separator = "|";
            _spriteBatch.DrawString(font, separator, new Vector2(_graphics.PreferredBackBufferWidth/2, 95), Color.White);     
            _spriteBatch.DrawString(font, separator, new Vector2(_graphics.PreferredBackBufferWidth/2, 175), Color.White);     
            _spriteBatch.DrawString(font, separator, new Vector2(_graphics.PreferredBackBufferWidth/2, 325), Color.White);     
            _spriteBatch.DrawString(font, separator, new Vector2(_graphics.PreferredBackBufferWidth/2, 405), Color.White);    

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
