﻿using Unity.VisualScripting;
using UnityEngine;

namespace InputSystem
{
    public interface IDraggable
    {
        virtual void OnDrag()
        {
            
        }

        virtual void OnDrop()
        {
            
        }

        virtual bool CanBePinnedTo(IPin pin)
        {
            return true;
        }
    }
}