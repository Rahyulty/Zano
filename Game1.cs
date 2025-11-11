using System;
using System.Text.Encodings.Web;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MoonSharp.Interpreter;

namespace Zano;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Instance _testInstance;
    private Texture2D _whiteTexture;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Create a simple white texture for rendering (50x50 square)
        _whiteTexture = new Texture2D(GraphicsDevice, 50, 50);
        var textureData = new Color[50 * 50];
        for (int i = 0; i < textureData.Length; i++)
            textureData[i] = Color.White;
        _whiteTexture.SetData(textureData);

        // Warm up MoonSharp
        Script.WarmUp();

        // Create a test instance with Lua scripting
        _testInstance = new Instance();
        
        // Add transform component - start at center of screen
        var transform = new Transform2D(new Vector2(400, 300));
        transform.Scale = Vector2.One; // Full size
        _testInstance.AddComponent(transform);

        // Add sprite renderer (will create a simple colored square)
        var sprite = new SpriteRenderer(_whiteTexture);
        sprite.Color = Color.Red;
        _testInstance.AddComponent(sprite);

        // Load and add Lua script from file
        var luaScript = LuaScript.FromFile("Scripts/sprite_controller.lua");
        _testInstance.AddComponent(luaScript);

        Console.WriteLine("Lua scripting demo initialized!");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Update the instance (which will call Lua update function)
        _testInstance?.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Draw the instance (which will render the sprite)
        _spriteBatch.Begin();
        _testInstance?.Draw(gameTime, _spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
