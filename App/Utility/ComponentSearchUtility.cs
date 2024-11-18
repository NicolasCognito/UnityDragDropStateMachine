using App.Utils;
using UnityEngine;

namespace App.Utils
{

    public static class ComponentSearchUtility
    {
        public static GameObject FindParentWithInterface<T>(Component startComponent) where T : class
        {
            if (startComponent == null)
                return null;

            Debug.Log($"Looking for interface on object {startComponent.gameObject.name}");
            Transform current = startComponent.transform;
            while (current != null)
            {
                if (current.TryGetComponent(out T component))
                {
                    return current.gameObject;
                }

                current = current.parent;
            }

            return null;
        }

        public static GameObject FindParentWithInterface<T>(GameObject startComponent) where T : class
        {
            if (startComponent == null)
                return null;

            Debug.Log($"Looking for interface on object {startComponent.gameObject.name}");
            Transform current = startComponent.transform;
            while (current != null)
            {
                if (current.TryGetComponent(out T component))
                {
                    return current.gameObject;
                }

                current = current.parent;
            }

            return null;
        }

        public static GameObject TryFindParentWithInterface<T>(this GameObject startComponent) where T : class
        {
            if (startComponent == null)
                return DefaultGameObjectProvider.GetDefaultGameObject();

            Debug.Log($"Looking for interface on object {startComponent.gameObject.name}");
            Transform current = startComponent.transform;
            while (current != null)
            {
                if (current.TryGetComponent(out T component))
                {
                    return current.gameObject;
                }

                current = current.parent;
            }

            return DefaultGameObjectProvider.GetDefaultGameObject();
        }
    }
}