/**************************************************************
 * Datamaskingrafikk - Obligatorisk innlevering 3 - Solsystem
 * Lisa Marie Sørensen(500973) og Mikael Bendiksen(500694)
 * Solsystem.cs
 * 
 * 
 * Controls:
 *      Up = Zoom In
 *      Down = Zoom Out
 *      A = Move Camera Left
 *      D = Move Camera Right
 *      W = Move Camera Up
 *      S = Move Camera Down
 *      1 = Toggle Backgound
 *      2 = Toggle Camera Position Info
 *      
 * 
 * 
 * Background texture   : http://bgfons.com/download/2942
 * Sun texture	        : http://www.xml3d.org/xml3d/demos/17_SolarSystem/Textures/Sun.png
 * Mercury texture	    : http://www.shatters.net/~t00fri/images/SAlbers_Mercury_t00fri_col2.jpg
 * Venus texture	    : http://www.celestiamotherlode.net/catalog/images/screenshots/venus/venus_clouds__NASA_JPL_Seal_Mariner10_Oct21_2001.jpg
 * Earth texture	    : http://naturalearth.springercarto.com/ne3_data/8192/textures/2_no_clouds_8k.jpg
 * Mars texture	        : http://2.bp.blogspot.com/-2aLH6cYiaKs/TdOsBtnpRqI/AAAAAAAAAP4/bnMOdD9OMjk/s1600/mars+texture.jpg
 * Jupiter texture      : http://textures.forrest.cz/textures/library/maps/Jupiter.jpg
 * Saturn texture       : http://carpecaelum.com/planetary/celestia/textures/medres/saturn.jpg
 * Uranus texture       : http://textures.forrest.cz/textures/library/maps/Uranus.jpg
 * Neptune texture      : http://www.planetaryvisions.com/images_new/37.jpg
 * Moon texture         : http://www.planetaryvisions.com/images_new/213.jpg
 * 
 **************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Solsystemet
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Solsystem : Microsoft.Xna.Framework.Game
    {

            private const float SUN_MASS = 296342;
            private GraphicsDeviceManager graphics;
            private GraphicsDevice device;
            private Camera camera;
            private InputHandler input;
            private SpriteFont font;
            private BasicEffect effect;
            private Matrix world, view, projection;
            private SpriteBatch spriteBatch;
            private Stack<Matrix> matrixStrack = new Stack<Matrix>();
            private float sunRotY = 0.0f;
            bool showBackground = true;
            bool showCamPos = true;
            private Model mSun, mMoon;
            private Texture2D[] Textures = new Texture2D[10];
            Texture2D backgroundTexture;

            // Planets
            private Planet[] planetArray = new Planet[8];

            private float[] scaleMercury = { (2439.7f / 0.6f), 2439.7f / 0.6f };          //radius for mercury
            private float[] scaleVenus = { 6051.8f / 0.6f, 6051.8f / 0.6f };              //radius for venus
            private float[] scaleTerra = { 6378.1f / 0.6f, 6356.8f / 0.6f };              //radius for earth
            private float[] scaleMars = { 3396.2f / 0.6f, 3376.2f / 0.6f };               //radius for mars
            private float[] scaleJupiter = { 71492.0f / 0.6f, 66854.0f / 0.6f };          //radius for jupiter
            private float[] scaleSaturn = { 60268.0f / 0.6f, 54364.0f / 0.6f };           //radius for saturn
            private float[] scaleUranus = { 25559.0f / 0.6f, 24973.0f / 0.6f };           //radius for uranus
            private float[] scaleNeptune = { 24764.0f / 0.6f, 24341.0f / 0.6f };          //radius for neptune
        
            private float dc_mercury = (6.98f) * (float)Math.Pow(10, 7), df_mercury = (4.60f) * (float)Math.Pow(10, 7);     //distance from sun at closest and furthest
            private float dc_venus = 1.075f * (float)Math.Pow(10, 8), df_venus = 1.098f * (float)Math.Pow(10, 8);           //distance from sun at closest and furthest
            private float dc_terra = 1.471f * (float)Math.Pow(10, 8), df_terra = 1.521f * (float)Math.Pow(10, 8);           //distance from sun at closest and furthest
            private float dc_mars = 2.067f * (float)Math.Pow(10, 8), df_mars = 2.491f * (float)Math.Pow(10, 8);             //distance from sun at closest and furthest
            private float dc_jupiter = 7.409f * (float)Math.Pow(10, 8), df_jupiter = 8.157f * (float)Math.Pow(10, 8);       //distance from sun at closest and furthest
            private float dc_saturn = 1.384f * (float)Math.Pow(10, 9), df_saturn = 1.503f * (float)Math.Pow(10, 9);         //distance from sun at closest and furthest
            private float dc_uranus = 2.739f * (float)Math.Pow(10, 9), df_uranus = 3.003f * (float)Math.Pow(10, 9);         //distance from sun at closest and furthest
            private float dc_neptune = 4.456f * (float)Math.Pow(10, 9), df_neptune = 4.546f * (float)Math.Pow(10, 9);       //distance from sun at closest and furthest

            private bool isFullScreen = false;

            public Solsystem()
            {
                graphics = new GraphicsDeviceManager(this);
                Content.RootDirectory = "Content";

                input = new InputHandler(this);
                this.Components.Add(input);

                camera = new Camera(this);
                this.Components.Add(camera);

                //                          Name,     Model,  Scale,       DistanceFromSun,                 RS,      OS,    moonCount
                planetArray[0] = new Planet("Mercury", null, scaleMercury, new Vector3(20.0f, 0.0f, 0.0f), 0.0001f, 1.0000f, 0);
                planetArray[1] = new Planet("Venus", null, scaleVenus, new Vector3(40.0f, 0.0f, 0.0f), 0.4f, 0.9f, 0);

                planetArray[2] = new Planet("Earth", null, scaleTerra, new Vector3(60.0f, 0.0f, 0.0f), 0.06f, 0.8f, 1);
                planetArray[2].MoonArray[0] = new Moon("The Moon", 0.2f, new Vector3(4.0f, 0.0f, 0.0f), 0.1f, 0.1f);

                planetArray[3] = new Planet("Mars", null, scaleMars, new Vector3(80.0f, 0.0f, 0.0f), 0.4f, 0.7f, 2);
                planetArray[3].MoonArray[0] = new Moon("Deimos", 0.2f, new Vector3(4.0f, 0.0f, 0.0f), 0.3f, 2.0f);
                planetArray[3].MoonArray[1] = new Moon("Phobos", 0.2f, new Vector3(8.0f, 0.0f, 0.0f), 0.1f, 0.1f);

                planetArray[4] = new Planet("Jupiter", null, scaleJupiter, new Vector3(100.0f, 0.0f, 0.0f), 0.4f, 0.6f, 4);
                planetArray[4].MoonArray[0] = new Moon("Lo", 0.2f, new Vector3(4.0f, 0.0f, 0.0f), 0.2f, 1.0f);
                planetArray[4].MoonArray[1] = new Moon("Europa", 0.2f, new Vector3(8.0f, 0.0f, 0.0f), 0.9f, 0.2f);
                planetArray[4].MoonArray[2] = new Moon("Genymede", 0.2f, new Vector3(12.0f, 0.0f, 0.0f), 0.5f, 0.7f);
                planetArray[4].MoonArray[3] = new Moon("Callisto", 0.2f, new Vector3(16.0f, 0.0f, 0.0f), 1.2f, 0.1f);

                planetArray[5] = new Planet("Saturn", null, scaleSaturn, new Vector3(120.0f, 0.0f, 0.0f), 0.4f, 0.5f, 9);
                planetArray[5].MoonArray[0] = new Moon("Mimas", 0.2f, new Vector3(4.0f, 0.0f, 0.0f), 0.1f, 0.1f);
                planetArray[5].MoonArray[1] = new Moon("Enceladus", 0.2f, new Vector3(8.0f, 0.0f, 0.0f), 0.2f, 0.2f);
                planetArray[5].MoonArray[2] = new Moon("Tethys", 0.2f, new Vector3(12.0f, 0.0f, 0.0f), 0.4f, 0.3f);
                planetArray[5].MoonArray[3] = new Moon("Dione", 0.2f, new Vector3(16.0f, 0.0f, 0.0f), 0.6f, 0.4f);
                planetArray[5].MoonArray[4] = new Moon("Rhea", 0.2f, new Vector3(20.0f, 0.0f, 0.0f), 0.8f, 0.5f);
                planetArray[5].MoonArray[5] = new Moon("Titan", 0.2f, new Vector3(24.0f, 0.0f, 0.0f), 1.0f, 0.6f);
                planetArray[5].MoonArray[6] = new Moon("Hyperion", 0.2f, new Vector3(28.0f, 0.0f, 0.0f), 1.2f, 0.7f);
                planetArray[5].MoonArray[7] = new Moon("Lapetus", 0.2f, new Vector3(32.0f, 0.0f, 0.0f), 1.4f, 0.8f);
                planetArray[5].MoonArray[8] = new Moon("Phoebe", 0.2f, new Vector3(36.0f, 0.0f, 0.0f), 1.6f, 0.9f);

                planetArray[6] = new Planet("Uranus", null, scaleUranus, new Vector3(140.0f, 0.0f, 0.0f), 0.4f, 0.4f, 5);
                planetArray[6].MoonArray[0] = new Moon("Miranda", 0.2f, new Vector3(8.0f, 0.0f, 0.0f), 0.4f, 0.5f);
                planetArray[6].MoonArray[1] = new Moon("Ariel", 0.2f, new Vector3(12.0f, 0.0f, 0.0f), 0.6f, 0.3f);
                planetArray[6].MoonArray[2] = new Moon("Umbriel", 0.2f, new Vector3(16.0f, 0.0f, 0.0f), 0.8f, 0.6f);
                planetArray[6].MoonArray[3] = new Moon("Titania", 0.2f, new Vector3(20.0f, 0.0f, 0.0f), 1.0f, 0.1f);
                planetArray[6].MoonArray[4] = new Moon("Oberon", 0.2f, new Vector3(24.0f, 0.0f, 0.0f), 1.2f, 0.4f);

                planetArray[7] = new Planet("Neptune", null, scaleNeptune, new Vector3(160.0f, 0.0f, 0.0f), 0.4f, 0.3f, 3);
                planetArray[7].MoonArray[0] = new Moon("Proteus", 0.2f, new Vector3(4.0f, 0.0f, 0.0f), 0.2f, 0.5f);
                planetArray[7].MoonArray[1] = new Moon("Triton", 0.2f, new Vector3(8.0f, 0.0f, 0.0f), 0.4f, 0.2f);
                planetArray[7].MoonArray[2] = new Moon("Nereid", 0.2f, new Vector3(12.0f, 0.0f, 0.0f), 0.6f, 1.0f);

                this.IsFixedTimeStep = true;
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
                initDevice();
                this.IsMouseVisible = true;
            }

            private void initDevice()
            {
                device = graphics.GraphicsDevice;
                graphics.PreferredBackBufferWidth = 1400;
                graphics.PreferredBackBufferHeight = 800;
                graphics.IsFullScreen = isFullScreen;
                graphics.ApplyChanges();

                Window.Title = "Solarsystem - by Lisa & Mikael";

                //Initialiserer Effect-objektet:
                effect = new BasicEffect(graphics.GraphicsDevice);
                effect.VertexColorEnabled = false;
                effect.TextureEnabled = true;
            }


            /// <summary>
            /// LoadContent will be called once per game and is the place to load
            /// all of your content.
            /// </summary>
            protected override void LoadContent()
            {
                // Create a new SpriteBatch, which can be used to draw textures.
                spriteBatch = new SpriteBatch(GraphicsDevice);

                // texture for background
                backgroundTexture = Content.Load<Texture2D>(@"textures\stars2");

                // loading of font used for camera info
                font = Content.Load<SpriteFont>("font");

                loadSpaceObjects(); // load space objects/textures
            }

            /// <summary>
            /// Loads all space object models and textures.
            /// </summary>
            private void loadSpaceObjects()
            {

                // TextureArray for texture of the object
                Textures[0] = Content.Load<Texture2D>("Textures/mercury_texture");    // mercury
                Textures[1] = Content.Load<Texture2D>("Textures/venus_texture");      // venus
                Textures[2] = Content.Load<Texture2D>("Textures/earth_texture");      // earth
                Textures[3] = Content.Load<Texture2D>("Textures/mars_texture");       // mars
                Textures[4] = Content.Load<Texture2D>("Textures/jupiter_texture");    // jupiter
                Textures[5] = Content.Load<Texture2D>("Textures/saturn_texture");     // saturn
                Textures[6] = Content.Load<Texture2D>("Textures/uranus_texture");     // uranus
                Textures[7] = Content.Load<Texture2D>("Textures/neptune_texture");    // neptune
                Textures[8] = Content.Load<Texture2D>("Textures/moon_texture");       // moon
                Textures[9] = Content.Load<Texture2D>("Textures/sun_texture");        // sun

                // array with the objects
                planetArray[0].PlanetModel = Content.Load<Model>("Models/mercury"); // mercury
                planetArray[1].PlanetModel = Content.Load<Model>("Models/venus");   // venus
                planetArray[2].PlanetModel = Content.Load<Model>("Models/earth");   // earth
                planetArray[3].PlanetModel = Content.Load<Model>("Models/mars");    // mars
                planetArray[4].PlanetModel = Content.Load<Model>("Models/jupiter"); // jupiter
                planetArray[5].PlanetModel = Content.Load<Model>("Models/saturn");  // saturn
                planetArray[6].PlanetModel = Content.Load<Model>("Models/uranus");  // uranus
                planetArray[7].PlanetModel = Content.Load<Model>("Models/neptune"); // neptune
                

                for (int i = 0; i < 8; i++)
                {
                    (planetArray[i].PlanetModel.Meshes[0].Effects[0] as BasicEffect).EnableDefaultLighting();
                    (planetArray[i].PlanetModel.Meshes[0].Effects[0] as BasicEffect).Texture = Textures[i];
                    (planetArray[i].PlanetModel.Meshes[0].Effects[0] as BasicEffect).TextureEnabled = true;
                }

                
                mSun = Content.Load<Model>("Models/sun");  // sun
                (mSun.Meshes[0].Effects[0] as BasicEffect).Texture = Textures[9];
                (mSun.Meshes[0].Effects[0] as BasicEffect).TextureEnabled = true;
                

                mMoon = Content.Load<Model>("Models/moon"); // moon
                (mMoon.Meshes[0].Effects[0] as BasicEffect).Texture = Textures[8];
                (mMoon.Meshes[0].Effects[0] as BasicEffect).TextureEnabled = true;

            }

            /// <summary>
            /// UnloadContent will be called once per game and is the place to unload
            /// all content.
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
                // Allows the game to exit
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    this.Exit();
                
                    
                
                // Poll the current controller state for player one
                GamePadState padState1 = GamePad.GetState(PlayerIndex.One);
                // make sure the controller is connected
                if (padState1.IsConnected)
                {
                      if (padState1.Buttons.A == ButtonState.Pressed)
                      {
                         GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
                      }
                      else
                      {
                          // otherwise, disable the motors
                          GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
                      }

                      if (padState1.Buttons.X == ButtonState.Pressed)
                      {
                          if (showBackground)
                          { showBackground = false; }
                          else showBackground = true;
                      }

                      if (padState1.Buttons.Y == ButtonState.Pressed)
                      {
                          if (showCamPos)
                          { showCamPos = false; }
                          else showCamPos = true;
                      }
                }


                // key inputs
                if (input.KeyboardState.IsKeyDown(Keys.D1))
                {
                    if (showBackground)
                    { showBackground = false; }
                    else showBackground = true;
                    
                }
                if (input.KeyboardState.IsKeyDown(Keys.D2))
                {
                    if (showCamPos)
                    { showCamPos = false; }
                    else showCamPos = true;

                }

                base.Update(gameTime);
            }

            /// <summary>
            /// This is called when the game should draw itself.
            /// </summary>
            /// <param name="gameTime">Provides a snapshot of timing values.</param>
            protected override void Draw(GameTime gameTime)
            {
                GraphicsDevice.Clear(Color.Black);

                // fixes bug with planets overlapping / not going behind the sun in a 3D perspective 
                GraphicsDevice.BlendState = BlendState.Opaque;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                DepthStencilState depthBufferState = new DepthStencilState();
                depthBufferState.DepthBufferEnable = true;
                GraphicsDevice.DepthStencilState = depthBufferState;

                float y = 0.0f;
                spriteBatch.Begin();
                if (showBackground)
                {
                    spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                }
                if (showCamPos) 
                {
                    spriteBatch.DrawString(font, "Camera Info: " + camera.CamPos, new Vector2(0.0f, y += 20), Color.WhiteSmoke);
                }
                spriteBatch.End();

                // execute fix again
                GraphicsDevice.BlendState = BlendState.Opaque;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.DepthStencilState = depthBufferState;

                view = camera.View;
                projection = camera.Projection;

                effect.World = Matrix.Identity;
                effect.Projection = projection;
                effect.View = view;

                effect.LightingEnabled = true;
                effect.DirectionalLight0.Enabled = true;
                effect.DirectionalLight0.DiffuseColor = Color.Yellow.ToVector3();
                effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(-1.0f, -1.5f, 0.0f));
                effect.EmissiveColor = Color.Red.ToVector3();

                this.DrawStar(gameTime);

                effect.DirectionalLight0.Enabled = false;

                foreach (Planet p in planetArray)
                {
                    this.DrawPlanet(gameTime, p);

                    if (p.MoonArray.Length > 0)
                    {
                        foreach (Moon m in p.MoonArray)
                        {
                            this.DrawMoon(gameTime, m);
                            matrixStrack.Pop();
                        }
                    }

                    matrixStrack.Pop();
                }

                base.Draw(gameTime);
            }

            public void DrawStar(GameTime gameTime)
            {
                Matrix matScale, matRotateY, matTrans;

                matScale = Matrix.CreateScale(5.0f);
                matTrans = Matrix.CreateTranslation(1.0f, 0.1f, 1.0f);

                matRotateY = Matrix.CreateRotationY(sunRotY);
                sunRotY += (float)gameTime.ElapsedGameTime.Milliseconds / 5000.0f;
                sunRotY = sunRotY % (float)(2 * Math.PI);

                world = matScale * matRotateY * matTrans;
                matrixStrack.Push(world);

                effect.World = world;
                mSun.Draw(world, camera.View, camera.Projection);
            }

            private void DrawPlanet(GameTime gameTime, Planet planet)
            {
                Matrix matRotateY, matScale, matOrbTranslation, matOrbRotation;
                Matrix _world = matrixStrack.Peek();

                matScale = Matrix.CreateScale(planet.PlanetScale[0] / SUN_MASS, planet.PlanetScale[1] / SUN_MASS, planet.PlanetScale[0] / SUN_MASS);

                matRotateY = Matrix.CreateRotationY(planet.PlanetRotY);
                planet.PlanetRotY += (float)gameTime.ElapsedGameTime.Milliseconds / 5000.0f;
                planet.PlanetRotY = planet.PlanetRotY % (float)(2 * Math.PI);

                matOrbTranslation = Matrix.CreateTranslation(planet.PlanetDistanceToSun);
                planet.PlanetOrbitY += (planet.PlanetOrbitSpeed / 60); 
                planet.PlanetOrbitY = planet.PlanetOrbitY % (float)(2 * Math.PI);
                matOrbRotation = Matrix.CreateRotationY(planet.PlanetOrbitY);

                world = matScale * matRotateY * matOrbTranslation * matOrbRotation * _world;
                matrixStrack.Push(world);

                effect.World = world;

                planet.PlanetPosition = Matrix.Invert(world).Translation;
                planet.PlanetModel.Draw(world, camera.View, camera.Projection);

            }

            private void DrawMoon(GameTime gameTime, Moon moon)
            {
                Matrix matRotateY, matScale, matOrbTranslation, matOrbRotation;
                Matrix _world = matrixStrack.Peek();

                matScale = Matrix.CreateScale(moon.MoonScale);

                matRotateY = Matrix.CreateRotationY(moon.MoonRotY);
                moon.MoonRotY += (float)gameTime.ElapsedGameTime.Milliseconds / 5000.0f;
                moon.MoonRotY = moon.MoonRotY % (float)(2 * Math.PI);

                matOrbTranslation = Matrix.CreateTranslation(moon.MoonDistanceToSun);
                moon.MoonOrbitY += (moon.MoonOrbitSpeed / 60);
                moon.MoonOrbitY = moon.MoonOrbitY % (float)(2 * Math.PI);
                matOrbRotation = Matrix.CreateRotationY(moon.MoonOrbitY);

                world = matScale * matRotateY * matOrbTranslation * matOrbRotation * _world;
                matrixStrack.Push(world);

                effect.World = world;
                mMoon.Draw(world, camera.View, camera.Projection);
            }
        }
    }
