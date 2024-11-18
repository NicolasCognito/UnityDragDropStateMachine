using UnityEngine;

namespace App.Utils
{
    // Base interface providing common GameObject properties
    public interface IGameObject
    {
        GameObject gameObject { get; }
        Transform transform { get; }
        string name { get; }
    }
    
}