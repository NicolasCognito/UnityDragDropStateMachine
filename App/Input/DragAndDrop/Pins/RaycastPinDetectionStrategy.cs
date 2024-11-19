// IPin.cs
using UnityEngine;
using App.Utils;

// RaycastPinDetectionStrategy.cs

namespace InputSystem
{
    public class RaycastPinDetectionStrategy : IPinDetectionStrategy
    {
        public IPin FindPinNearPosition(Vector3 position, LayerMask pinLayer)
        {
            var hitObject = Raycaster.GetObjectAtPosition2D(position, pinLayer);
            if (hitObject != null && hitObject.TryFindParentWithInterface<IPin>(out IPin pin))
            {
                return pin;
            }
            return null; // or some other appropriate default value
        }

        public bool IsInPinRange(Vector3 position, IPin pin, LayerMask pinLayer)
        {
            if (pin == null) return false;

            // First perform a basic distance check to avoid unnecessary raycasts
            if (Vector3.Distance(position, pin.PinPoint.position) > 0.5f)
            {
                return false;
            }

            // Perform raycast to validate we're still hitting the same pin
            var hitObject = Raycaster.GetObjectAtPosition2D(position, pinLayer);
            if (hitObject == null || !hitObject.TryFindParentWithInterface<IPin>(out IPin detectedPin))
            {
                return false;
            }

            // Verify that the detected pin is the same as the provided pin
            return ReferenceEquals(detectedPin, pin);
        }
    }
}
