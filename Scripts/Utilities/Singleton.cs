using Godot;

public abstract class Singleton<T> : Node where T : Node, new()
{
    static T instance;
    public static T Instance { get { return instance; } }

    public Singleton() {
        instance = this as T;
    }
}