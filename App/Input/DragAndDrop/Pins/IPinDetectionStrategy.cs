// IPin.cs
using UnityEngine;

// IPinDetectionStrategy.cs

namespace InputSystem
{
    public interface IPinDetectionStrategy
    {
        IPin FindPinNearPosition(Vector3 position, LayerMask pinLayer);
        bool IsInPinRange(Vector3 position, IPin pin, LayerMask pinLayer);
    }
}
