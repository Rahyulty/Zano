using Microsoft.Xna.Framework;

namespace Zano;

/// <summary>
/// Component that stores 2D transformation data (position, rotation, scale, origin).
/// </summary>
public class Transform2D : Component
{
    /// <summary>
    /// The position of the instance in 2D space.
    /// </summary>
    public Vector2 Position { get; set; }

    /// <summary>
    /// The rotation of the instance in radians.
    /// </summary>
    public float Rotation { get; set; }

    /// <summary>
    /// The scale of the instance.
    /// </summary>
    public Vector2 Scale { get; set; } = Vector2.One;

    /// <summary>
    /// The origin point for rotation and scaling (0,0 = top-left, 0.5,0.5 = center).
    /// </summary>
    public Vector2 Origin { get; set; }

    /// <summary>
    /// Creates a new Transform2D component with the specified position.
    /// </summary>
    public Transform2D(Vector2 position)
    {
        Position = position;
    }

    /// <summary>
    /// Creates a new Transform2D component at position (0, 0).
    /// </summary>
    public Transform2D() : this(Vector2.Zero)
    {
    }
}

