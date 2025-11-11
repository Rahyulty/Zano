using System;
using System.IO;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;

namespace Zano;

/// <summary>
/// Component that allows Lua scripting for instances.
/// </summary>
public class LuaScript : Component
{
    private Script _script;
    private bool _initialized = false;

    /// <summary>
    /// The Lua script code to execute.
    /// </summary>
    public string ScriptCode { get; set; }

    /// <summary>
    /// Creates a new LuaScript component with the specified script code.
    /// </summary>
    public LuaScript(string scriptCode)
    {
        ScriptCode = scriptCode;
        _script = new Script();
    }

    /// <summary>
    /// Creates a new LuaScript component by loading a script from a file.
    /// Tries to find the file relative to the executable directory first, then the current directory.
    /// </summary>
    public static LuaScript FromFile(string filePath)
    {
        // Try relative to executable directory first
        string executableDir = AppDomain.CurrentDomain.BaseDirectory;
        string fullPath = Path.Combine(executableDir, filePath);
        
        if (!File.Exists(fullPath))
        {
            // Try the path as-is
            if (File.Exists(filePath))
            {
                fullPath = filePath;
            }
            else
            {
                throw new FileNotFoundException($"Lua script file not found: {filePath} (also tried: {fullPath})");
            }
        }

        string scriptCode = File.ReadAllText(fullPath);
        return new LuaScript(scriptCode);
    }

    public override void OnAdded()
    {
        base.OnAdded();
        SetupLuaAPI();
        ExecuteScript();
    }

    private void SetupLuaAPI()
    {
        // Register types for Lua with user data
        UserData.RegisterType<Transform2D>();
        UserData.RegisterType<Vector2>();
        UserData.RegisterType<LuaVector2>();
        UserData.RegisterType<LuaTransform2D>();
        UserData.RegisterType<LuaInstanceWrapper>();

        // Create a table to expose the instance to Lua
        _script.Globals["instance"] = DynValue.FromObject(_script, new LuaInstanceWrapper(Instance));
        
        // Helper functions for Lua
        _script.Globals["print"] = (Action<string>)((msg) => Console.WriteLine($"[Lua] {msg}"));
        _script.Globals["vec2"] = (Func<double, double, LuaVector2>)((x, y) => new LuaVector2((float)x, (float)y));
    }

    private void ExecuteScript()
    {
        try
        {
            _script.DoString(ScriptCode);
            
            // Call init function if it exists
            var initFunc = _script.Globals.Get("init");
            if (initFunc.Type == DataType.Function)
            {
                _script.Call(initFunc);
                _initialized = true;
            }
            else
            {
                _initialized = true;
            }
        }
        catch (ScriptRuntimeException ex)
        {
            Console.WriteLine($"[Lua Error] {ex.Message}\n{ex.DecoratedMessage}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Lua Error] {ex.Message}");
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (!_initialized) return;

        try
        {
            var updateFunc = _script.Globals.Get("update");
            if (updateFunc.Type == DataType.Function)
            {
                _script.Call(updateFunc, gameTime.TotalGameTime.TotalSeconds);
            }
        }
        catch (ScriptRuntimeException ex)
        {
            Console.WriteLine($"[Lua Update Error] {ex.Message}");
        }
    }

    /// <summary>
    /// Calls a Lua function by name with optional arguments.
    /// </summary>
    public DynValue CallFunction(string functionName, params object[] args)
    {
        try
        {
            var func = _script.Globals.Get(functionName);
            if (func.Type == DataType.Function)
            {
                return _script.Call(func, args);
            }
        }
        catch (ScriptRuntimeException ex)
        {
            Console.WriteLine($"[Lua Call Error] {ex.Message}");
        }
        return DynValue.Nil;
    }
}

/// <summary>
/// Wrapper class to expose Instance functionality to Lua.
/// </summary>
[MoonSharpUserData]
public class LuaInstanceWrapper
{
    private Instance _instance;

    public LuaInstanceWrapper(Instance instance)
    {
        _instance = instance;
    }

    public LuaTransform2D GetTransform()
    {
        var transform = _instance.GetComponent<Transform2D>();
        return transform != null ? new LuaTransform2D(transform) : null;
    }

    public SpriteRenderer GetSprite()
    {
        return _instance.GetComponent<SpriteRenderer>();
    }
}

/// <summary>
/// Lua-friendly wrapper for Transform2D.
/// </summary>
[MoonSharpUserData]
public class LuaTransform2D
{
    private Transform2D _transform;

    public LuaTransform2D(Transform2D transform)
    {
        _transform = transform;
    }

    public LuaVector2 Position
    {
        get => new LuaVector2(_transform.Position.X, _transform.Position.Y);
        set => _transform.Position = new Vector2(value.X, value.Y);
    }

    public double Rotation
    {
        get => _transform.Rotation;
        set => _transform.Rotation = (float)value;
    }

    public LuaVector2 Scale
    {
        get => new LuaVector2(_transform.Scale.X, _transform.Scale.Y);
        set => _transform.Scale = new Vector2(value.X, value.Y);
    }

    public LuaVector2 Origin
    {
        get => new LuaVector2(_transform.Origin.X, _transform.Origin.Y);
        set => _transform.Origin = new Vector2(value.X, value.Y);
    }
}

/// <summary>
/// Lua-friendly Vector2 wrapper.
/// </summary>
[MoonSharpUserData]
public class LuaVector2
{
    public float X { get; set; }
    public float Y { get; set; }

    public LuaVector2(float x, float y)
    {
        X = x;
        Y = y;
    }

    public LuaVector2 Add(LuaVector2 other)
    {
        return new LuaVector2(X + other.X, Y + other.Y);
    }

    public LuaVector2 Subtract(LuaVector2 other)
    {
        return new LuaVector2(X - other.X, Y - other.Y);
    }

    public LuaVector2 Multiply(double scalar)
    {
        return new LuaVector2(X * (float)scalar, Y * (float)scalar);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}

