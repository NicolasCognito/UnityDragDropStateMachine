using UnityEngine;

namespace App.Utils
{
    public class DefaultGameObjectProvider
    {
        // Static variable to hold the reference to the default empty GameObject.
        private static GameObject _defaultGameObject;

        // Public static method to provide global access to the empty GameObject.
        public static GameObject GetDefaultGameObject()
        {
            // If the GameObject is not created yet, create a new empty GameObject.
            if (_defaultGameObject == null)
            {
                _defaultGameObject = new GameObject("DefaultGameObject");
            }

            // Return the instance of the empty GameObject.
            return _defaultGameObject;
        }
    }
}