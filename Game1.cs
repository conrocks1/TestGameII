using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TiledSharp;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        TmxMap map;
        Texture2D tileset;

        int tileWidth;
        int tileHeight;
        int tileSpacing;
        int tilesetTilesWide;
        int tilesetTilesHigh;
       

        Rectangle bounds;

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
            // TODO: Add your initialization logic here

            base.Initialize();
            IsMouseVisible = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            map = new TmxMap("Content/exampleMap.tmx");
            tileset = Content.Load<Texture2D>(map.Tilesets[0].Name.ToString());

            tileWidth = map.Tilesets[0].TileWidth;
            tileHeight = map.Tilesets[0].TileHeight;
            tileSpacing = map.Tilesets[0].Spacing;

            tilesetTilesWide = tileset.Width / tileWidth;
            tilesetTilesHigh = tileset.Height / tileHeight;
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            KeyboardState keyState = Keyboard.GetState();

            int scrollx = 0, scrolly = 0;

            if (keyState.IsKeyDown(Keys.Left))
                scrollx = 10;
            if (keyState.IsKeyDown(Keys.Right))
                scrollx = -10;
            if (keyState.IsKeyDown(Keys.Up))
                scrolly = 10;
            if (keyState.IsKeyDown(Keys.Down))
                scrolly = -10;

            bounds.X = bounds.X + scrollx;
            bounds.Y = bounds.Y + scrolly;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            for (var i = 0; i < map.Layers[0].Tiles.Count; i++)
            {
                int gid = map.Layers[0].Tiles[i].Gid;

                // Empty tile, do nothing
                if (gid == 0)
                {

                }
                else
                {
                    int tileFrame = gid - 1;
                    int column = tileFrame % tilesetTilesWide - tileSpacing;
                    int row = (int)Math.Floor((double)tileFrame / (double)tilesetTilesWide - tileSpacing);

                    float x = (i % map.Width) * map.TileWidth - tileSpacing;
                    float y = (float)Math.Floor(i / (double)map.Width) * map.TileHeight - tileSpacing;

                    Rectangle tilesetRec = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);
                    Rectangle newView = new Rectangle((int)x + bounds.X, (int)y + bounds.Y, tileWidth, tileHeight);
                    spriteBatch.Draw(tileset, newView, tilesetRec, Color.White);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
