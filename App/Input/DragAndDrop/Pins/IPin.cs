// IPin.cs
using UnityEngine;

namespace InputSystem
{
    public interface IPin
    {
        Transform PinPoint { get; }
    }
}