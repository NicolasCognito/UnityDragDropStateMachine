using UnityEngine;
using System;
using App.Utility.StateMachine;
using App.Utils;

namespace InputSystem
{
    public class DragAndDropManager : MonoBehaviour
    {
        [SerializeField] private float dragSpeed = 10f;
        [SerializeField] private LayerMask draggableLayer;

        private Camera mainCamera;
        private GameObject currentDraggedObject;
        private DragDropStateMachine stateMachine;

        public event Action<GameObject> OnObjectPickedUp;
        public event Action<GameObject> OnObjectDropped;

        public float DragSpeed => dragSpeed;
        public Camera MainCamera => mainCamera;
        public GameObject CurrentDraggedObject => currentDraggedObject;

        public DragDropStateMachine StateMachine { get => StateMachine1; set => StateMachine1 = value; }
        public DragDropStateMachine StateMachine1 { get => stateMachine; set => stateMachine = value; }

        private void Start()
        {
            mainCamera = Camera.main;
            InitializeStateMachine();
        }

        private void InitializeStateMachine()
        {
            StateMachine = new DragDropStateMachine();
            
            // Create states
            var idleState = new DragDropIdleState(this, StateMachine);
            var draggingState = new DragDropDraggingState(this, StateMachine);

            // Register states
            StateMachine.RegisterState(idleState);
            StateMachine.RegisterState(draggingState);
            
            // Register all possible transitions
            StateMachine.RegisterTransition<DragDropIdleState, DragDropDraggingState>(DragDropTrigger.StartDrag);
            StateMachine.RegisterTransition<DragDropDraggingState, DragDropIdleState>(DragDropTrigger.EndDrag);

            // Initialize with idle state
            StateMachine.Initialize(idleState);
        }

        private void Update()
        {
            StateMachine.Update();
        }

        private void FixedUpdate()
        {
            StateMachine.FixedUpdate();
        }

        public GameObject GetDraggableObjectUnderMouse()
        {
            return Raycaster.GetObjectUnderMouse2D(draggableLayer)
                    .TryFindParentWithInterface<IDraggable>();
        }

        public void NotifyObjectPickedUp(GameObject obj)
        {
            currentDraggedObject = obj;
            OnObjectPickedUp?.Invoke(obj);

            var draggables = obj.GetComponents<IDraggable>();
            foreach (var draggable in draggables)
            {
                draggable.OnDrag();
            }
        }

        public void NotifyObjectDropped(GameObject obj)
        {
            var draggables = obj.GetComponents<IDraggable>();
            foreach (var draggable in draggables)
            {
                draggable.OnDrop();
            }

            OnObjectDropped?.Invoke(obj);
            currentDraggedObject = null;
        }
    }
}