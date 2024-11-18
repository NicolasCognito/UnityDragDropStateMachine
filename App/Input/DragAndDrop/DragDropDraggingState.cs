using UnityEngine;
using App.Utility.StateMachine;

namespace InputSystem
{
    public class DragDropDraggingState : BaseState
    {
        private readonly DragAndDropManager dragDropManager;

        public DragDropDraggingState(DragAndDropManager manager, IStateMachine stateMachine) 
            : base(stateMachine)
        {
            this.dragDropManager = manager;
        }

        public override void Update()
        {
            if (dragDropManager.CurrentDraggedObject != null)
            {
                MoveDraggedObject(dragDropManager.CurrentDraggedObject, Input.mousePosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (dragDropManager.CurrentDraggedObject != null)
                {
                    dragDropManager.NotifyObjectDropped(dragDropManager.CurrentDraggedObject);
                }
                (stateMachine as DragDropStateMachine)?.HandleTrigger(DragDropTrigger.EndDrag);
            }
        }

        private void MoveDraggedObject(GameObject draggedObject, Vector3 mousePosition)
        {
            Vector3 worldPosition = dragDropManager.MainCamera.ScreenToWorldPoint(new Vector3(
                mousePosition.x,
                mousePosition.y,
                dragDropManager.MainCamera.WorldToScreenPoint(draggedObject.transform.position).z
            ));

            draggedObject.transform.position = Vector3.Lerp(
                draggedObject.transform.position,
                worldPosition,
                Time.deltaTime * dragDropManager.DragSpeed
            );
        }
    }
}