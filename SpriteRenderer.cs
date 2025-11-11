using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zano;

/// <summary>
/// Component that renders a sprite using a texture.
/// </summary>
public class SpriteRenderer : Component
{
    /// <summary>
    /// The texture to render.
    /// </summary>
    public Texture2D Texture { get; set; }

    /// <summary>
    /// The source rectangle within the texture to render. If null, renders the entire texture.
    /// </summary>
    public Rectangle? SourceRect { get; set; }

    /// <summary>
    /// The color tint to apply to the sprite.
    /// </summary>
    public Color Color { get; set; } = Color.White;

    /// <summary>
    /// The layer depth for rendering order (0 = front, 1 = back).
    /// </summary>
    public float Layer { get; set; }

    /// <summary>
    /// Creates a new SpriteRenderer component with the specified texture.
    /// </summary>
    public SpriteRenderer(Texture2D texture)
    {
        Texture = texture;
    }

    /// <summary>
    /// Creates a new SpriteRenderer component with the specified texture and source rectangle.
    /// </summary>
    public SpriteRenderer(Texture2D texture, Rectangle sourceRect) : this(texture)
    {
        SourceRect = sourceRect;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (Texture == null)
            return;

        var transform = Instance?.GetComponent<Transform2D>();
        if (transform == null)
            return;

        var sourceRect = SourceRect ?? new Rectangle(0, 0, Texture.Width, Texture.Height);
        var origin = transform.Origin;

        spriteBatch.Draw(
            Texture,
            transform.Position,
            sourceRect,
            Color,
            transform.Rotation,
            origin,
            transform.Scale,
            SpriteEffects.None,
            Layer
        );
    }
}

