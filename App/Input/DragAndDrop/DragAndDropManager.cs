using UnityEngine;
using System;
using App.Utility.StateMachine;
using App.Utils;
using App.Utility.Debug;

namespace InputSystem
{
    public class DragAndDropManager : MonoBehaviour
    {
        [SerializeField] private float dragSpeed = 10f;
        [SerializeField] private LayerMask draggableLayer;
        [SerializeField] private LayerMask pinLayer;
        
        [Header("Debug")]
        [SerializeField] private StateMachineDebugger debugger;

        private Camera mainCamera;
        private GameObject currentDraggedObject;
        private DragDropStateMachine stateMachine;
        private PinDetector pinDetector;
        private IPin currentPin;

        public event Action<GameObject> OnObjectPickedUp;
        public event Action<GameObject> OnObjectDropped;
        public event Action<GameObject, IPin> OnObjectPinned;
        public event Action<GameObject, IPin> OnObjectUnpinned;

        public float DragSpeed => dragSpeed;
        public Camera MainCamera => mainCamera;
        public GameObject CurrentDraggedObject => currentDraggedObject;
        public PinDetector PinDetector => pinDetector;
        public IPin CurrentPin
        {
            get => currentPin;
            set
            {
                if (currentPin != value)
                {
                    var oldPin = currentPin;
                    currentPin = value;
                    
                    if (oldPin != null && currentDraggedObject != null)
                    {
                        OnObjectUnpinned?.Invoke(currentDraggedObject, oldPin);
                    }
                    
                    if (currentPin != null && currentDraggedObject != null)
                    {
                        OnObjectPinned?.Invoke(currentDraggedObject, currentPin);
                    }
                }
            }
        }

        private void Awake()
        {
            // Add debugger component if not already present
            if (debugger == null)
            {
                debugger = gameObject.AddComponent<StateMachineDebugger>();
            }
        }

        private void Start()
        {
            mainCamera = Camera.main;
            pinDetector = new PinDetector(new RaycastPinDetectionStrategy(), pinLayer);
            InitializeStateMachine();
        }

        private void InitializeStateMachine()
        {
            stateMachine = new DragDropStateMachine();
            
            // Create states
            var idleState = new DragDropIdleState(this, stateMachine);
            var draggingState = new DragDropDraggingState(this, stateMachine);
            var pinnedState = new DragDropPinnedState(this, stateMachine);

            // Register states
            stateMachine.RegisterState(idleState);
            stateMachine.RegisterState(draggingState);
            stateMachine.RegisterState(pinnedState);
            
            // Register all possible transitions
            stateMachine.RegisterTransition<DragDropIdleState, DragDropDraggingState>(DragDropTrigger.StartDrag);
            stateMachine.RegisterTransition<DragDropDraggingState, DragDropIdleState>(DragDropTrigger.EndDrag);
            stateMachine.RegisterTransition<DragDropDraggingState, DragDropPinnedState>(DragDropTrigger.NearPin);
            stateMachine.RegisterTransition<DragDropPinnedState, DragDropDraggingState>(DragDropTrigger.LeavePin);
            stateMachine.RegisterTransition<DragDropPinnedState, DragDropIdleState>(DragDropTrigger.EndDrag);

            // Initialize with idle state
            stateMachine.Initialize(idleState);
            
            // Initialize debugger
            debugger.Initialize(stateMachine);
        }

        private void Update()
        {
            stateMachine.Update();
        }

        private void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        public GameObject GetDraggableObjectUnderMouse()
        {
            return Raycaster.GetObjectUnderMouse2D(draggableLayer)
                    .TryFindParentWithInterface<IDraggable>(out var draggable);
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
            CurrentPin = null;
            currentDraggedObject = null;
        }
    }
}