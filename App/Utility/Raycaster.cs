using UnityEngine;
using System.Collections.Generic;

namespace App.Utils
{
    public class Raycaster : MonoBehaviour
    {
        private static Raycaster _instance;
        
        private static Raycaster Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject("RaycastManager");
                    _instance = obj.AddComponent<Raycaster>();
                    DontDestroyOnLoad(obj);
                }
                return _instance;
            }
        }

        private Camera _mainCamera;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
            _mainCamera = Camera.main;
        }

        // Core 3D raycast functionality
        public static bool Cast3D(Vector3 origin, Vector3 direction, out RaycastHit hit, 
            float maxDistance = Mathf.Infinity, LayerMask layerMask = default, 
            GameObject[] excludeObjects = null)
        {
            if (excludeObjects != null && excludeObjects.Length > 0)
            {
                List<Collider> disabledColliders = new List<Collider>();
                foreach (var obj in excludeObjects)
                {
                    var colliders = obj.GetComponentsInChildren<Collider>();
                    foreach (var col in colliders)
                    {
                        if (col.enabled)
                        {
                            col.enabled = false;
                            disabledColliders.Add(col);
                        }
                    }
                }

                bool result = Physics.Raycast(origin, direction, out hit, maxDistance, layerMask);

                // Re-enable colliders
                foreach (var col in disabledColliders)
                {
                    col.enabled = true;
                }

                return result;
            }

            return Physics.Raycast(origin, direction, out hit, maxDistance, layerMask);
        }

        // Core 2D raycast functionality
        public static bool Cast2D(Vector2 origin, Vector2 direction, out RaycastHit2D hit,
            float maxDistance = Mathf.Infinity, LayerMask layerMask = default,
            GameObject[] excludeObjects = null)
        {
            if (excludeObjects != null && excludeObjects.Length > 0)
            {
                List<Collider2D> disabledColliders = new List<Collider2D>();
                foreach (var obj in excludeObjects)
                {
                    var colliders = obj.GetComponentsInChildren<Collider2D>();
                    foreach (var col in colliders)
                    {
                        if (col.enabled)
                        {
                            col.enabled = false;
                            disabledColliders.Add(col);
                        }
                    }
                }

                hit = Physics2D.Raycast(origin, direction, maxDistance, layerMask);

                // Re-enable colliders
                foreach (var col in disabledColliders)
                {
                    col.enabled = true;
                }

                return hit.collider != null;
            }

            hit = Physics2D.Raycast(origin, direction, maxDistance, layerMask);
            return hit.collider != null;
        }

        // Mouse position raycasting utilities
        public static bool MouseRaycast3D(out RaycastHit hit, float maxDistance = Mathf.Infinity, 
            LayerMask layerMask = default, GameObject[] excludeObjects = null)
        {
            Ray ray = Instance._mainCamera.ScreenPointToRay(Input.mousePosition);
            return Cast3D(ray.origin, ray.direction, out hit, maxDistance, layerMask, excludeObjects);
        }

        public static bool MouseRaycast2D(out RaycastHit2D hit, float maxDistance = Mathf.Infinity,
            LayerMask layerMask = default, GameObject[] excludeObjects = null)
        {
            Vector2 mousePosition = Instance._mainCamera.ScreenToWorldPoint(Input.mousePosition);
            return Cast2D(mousePosition, Vector2.zero, out hit, maxDistance, layerMask, excludeObjects);
        }

        // Utility method for getting object under mouse
        public static GameObject GetObjectUnderMouse2D(LayerMask layer, GameObject[] excludeObjects = null)
        {
            if (MouseRaycast2D(out RaycastHit2D hit, Mathf.Infinity, layer, excludeObjects))
            {
                return hit.collider.gameObject;
            }
            return null;
        }

        public static GameObject GetObjectUnderMouse3D(LayerMask layer, GameObject[] excludeObjects = null)
        {
            if (MouseRaycast3D(out RaycastHit hit, Mathf.Infinity, layer, excludeObjects))
            {
                return hit.collider.gameObject;
            }
            return null;
        }

        // Extension method to get specific component from raycast
        public static T GetComponentUnderMouse2D<T>(LayerMask layer, GameObject[] excludeObjects = null) where T : Component
        {
            GameObject obj = GetObjectUnderMouse2D(layer, excludeObjects);
            return obj != null ? obj.GetComponent<T>() : null;
        }

        public static T GetComponentUnderMouse3D<T>(LayerMask layer, GameObject[] excludeObjects = null) where T : Component
        {
            GameObject obj = GetObjectUnderMouse3D(layer, excludeObjects);
            return obj != null ? obj.GetComponent<T>() : null;
        }
        
        // New method for interfaces
        public static T GetInterfaceUnderMouse2D<T>(LayerMask layer, GameObject[] excludeObjects = null) where T : class
        {
            GameObject obj = GetObjectUnderMouse2D(layer, excludeObjects);
            if (obj == null) return null;

            // Try to get interface from the object itself
            T component = obj.GetComponent<T>();
            if (component != null) return component;

            // If not found, try to get from parent objects
            return obj.GetComponentInParent<T>();
        }

        public static T GetInterfaceUnderMouse3D<T>(LayerMask layer, GameObject[] excludeObjects = null) where T : class
        {
            GameObject obj = GetObjectUnderMouse3D(layer, excludeObjects);
            if (obj == null) return null;

            // Try to get interface from the object itself
            T component = obj.GetComponent<T>();
            if (component != null) return component;

            // If not found, try to get from parent objects
            return obj.GetComponentInParent<T>();
        }

        // Optional: Extension method to make interface checking more convenient
        public static GameObject TryFindParentWithInterface<T>(GameObject obj) where T : class
        {
            if (obj == null) return null;

            // Check if the current object has the interface
            if (obj.GetComponent<T>() != null)
                return obj;

            // Check parent objects
            var parent = obj.transform.parent;
            while (parent != null)
            {
                if (parent.GetComponent<T>() != null)
                    return parent.gameObject;
                parent = parent.parent;
            }

            return null;
        }
    }
}