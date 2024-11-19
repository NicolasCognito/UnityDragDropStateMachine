// IPin.cs
using UnityEngine;

// PinDetector.cs

namespace InputSystem
{
    public class PinDetector
    {
        private readonly IPinDetectionStrategy detectionStrategy;
        private readonly LayerMask pinLayer;

        public PinDetector(IPinDetectionStrategy strategy, LayerMask pinLayer)
        {
            this.detectionStrategy = strategy;
            this.pinLayer = pinLayer;
        }

        public IPin GetPinNearPosition(Vector3 position)
        {
            return detectionStrategy.FindPinNearPosition(position, pinLayer);
        }

        public bool IsInPinRange(Vector3 position, IPin pin)
        {
            return detectionStrategy.IsInPinRange(position, pin, pinLayer);
        }
    }
}