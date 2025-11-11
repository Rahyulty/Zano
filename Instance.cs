using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zano;

/// <summary>
/// Base class for all game objects that can have components attached.
/// </summary>
public class Instance
{
    private readonly Dictionary<Type, Component> _components = new();

    /// <summary>
    /// Gets a component of the specified type. Returns null if not found.
    /// </summary>
    public T GetComponent<T>() where T : Component
    {
        if (_components.TryGetValue(typeof(T), out var component))
        {
            return (T)component;
        }
        return null;
    }

    /// <summary>
    /// Checks if this instance has a component of the specified type.
    /// </summary>
    public bool HasComponent<T>() where T : Component
    {
        return _components.ContainsKey(typeof(T));
    }

    /// <summary>
    /// Adds a component to this instance.
    /// </summary>
    public T AddComponent<T>(T component) where T : Component
    {
        var type = typeof(T);
        if (_components.ContainsKey(type))
        {
            throw new InvalidOperationException($"Component of type {type.Name} already exists on this instance.");
        }

        component.Instance = this;
        _components[type] = component;
        component.OnAdded();
        return component;
    }

    /// <summary>
    /// Removes a component of the specified type from this instance.
    /// </summary>
    public bool RemoveComponent<T>() where T : Component
    {
        if (_components.TryGetValue(typeof(T), out var component))
        {
            component.OnRemoved();
            component.Instance = null;
            _components.Remove(typeof(T));
            return true;
        }
        return false;
    }

    /// <summary>
    /// Updates all components attached to this instance.
    /// </summary>
    public virtual void Update(GameTime gameTime)
    {
        foreach (var component in _components.Values)
        {
            component.Update(gameTime);
        }
    }

    /// <summary>
    /// Draws all components attached to this instance.
    /// </summary>
    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        foreach (var component in _components.Values)
        {
            component.Draw(gameTime, spriteBatch);
        }
    }
}

