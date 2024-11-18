using UnityEngine;
using App.Utility.StateMachine;

namespace InputSystem
{
    public class DragDropIdleState : BaseState
    {
        private readonly DragAndDropManager dragDropManager;

        public DragDropIdleState(DragAndDropManager manager, IStateMachine stateMachine) 
            : base(stateMachine)
        {
            this.dragDropManager = manager;
        }

        public override void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject draggableObject = dragDropManager.GetDraggableObjectUnderMouse();
                if (draggableObject != null)
                {
                    dragDropManager.NotifyObjectPickedUp(draggableObject);
                    (stateMachine as DragDropStateMachine)?.HandleTrigger(DragDropTrigger.StartDrag);
                }
            }
        }
    }
}