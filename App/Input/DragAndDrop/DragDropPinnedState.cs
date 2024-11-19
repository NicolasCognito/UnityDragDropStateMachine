using UnityEngine;
using App.Utility.StateMachine;

namespace InputSystem
{
    public class DragDropPinnedState : BaseState
    {
        private readonly DragAndDropManager dragDropManager;

        public DragDropPinnedState(DragAndDropManager manager, IStateMachine stateMachine) 
            : base(stateMachine)
        {
            this.dragDropManager = manager;
        }

        public override void Enter()
        {
            if (dragDropManager.CurrentDraggedObject != null && dragDropManager.CurrentPin != null)
            {
                // Snap the object to pin position
                dragDropManager.CurrentDraggedObject.transform.position = dragDropManager.CurrentPin.PinPoint.position;
            }
        }

        public override void Update()
        {
            if (dragDropManager.CurrentDraggedObject == null || dragDropManager.CurrentPin == null)
            {
                (stateMachine as DragDropStateMachine)?.HandleTrigger(DragDropTrigger.EndDrag);
                return;
            }

            // Check if mouse is still in range
            if (!dragDropManager.PinDetector.IsInPinRange(Input.mousePosition, dragDropManager.CurrentPin))
            {
                (stateMachine as DragDropStateMachine)?.HandleTrigger(DragDropTrigger.LeavePin);
                return;
            }

            if (Input.GetMouseButtonUp(0))
            {
                dragDropManager.NotifyObjectDropped(dragDropManager.CurrentDraggedObject);
                (stateMachine as DragDropStateMachine)?.HandleTrigger(DragDropTrigger.EndDrag);
            }
        }
    }
}