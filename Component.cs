using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zano;

/// <summary>
/// Base class for all components that can be attached to instances.
/// </summary>
public abstract class Component
{
    /// <summary>
    /// The instance this component is attached to.
    /// </summary>
    public Instance Instance { get; internal set; }

    /// <summary>
    /// Called when the component is added to an instance.
    /// </summary>
    public virtual void OnAdded() { }

    /// <summary>
    /// Called when the component is removed from an instance.
    /// </summary>
    public virtual void OnRemoved() { }

    /// <summary>
    /// Called every frame to update the component.
    /// </summary>
    public virtual void Update(GameTime gameTime) { }

    /// <summary>
    /// Called every frame to draw the component.
    /// </summary>
    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }
}

