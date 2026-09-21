using Assets.Lib.Entities.AIAgent;
using System.Xml;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class AIAgent : MonoBehaviour
{
    public AIAgentBehaviour behaviour;

    public MajorAiOrderType majorOrder;
    private IMajorOrderExecutor majorExecutor;

    private AIOrder priorityOrder;
    private AIOrder currentOrder;

    public AIOrder CurrentPriorityOrder
    {
        get
        {
            if(priorityOrder == null)
            {
                return currentOrder;
            }
            return priorityOrder;
        }
    }

    private AISpaceShipController controller;

    private GameManager manager;

    private SpaceEntity entity;
    void Start()
    {
        entity = GetComponent<SpaceEntity>();
        manager = FindAnyObjectByType<GameManager>();
        controller = GetComponent<AISpaceShipController>();
        SetMajorOrder(this.majorOrder);
    }

    // Update is called once per frame
    void Update()
    {
        if(majorOrder != MajorAiOrderType.None)
        {
            majorExecutor.Execute();
        }
        ExecuteCurrentOrder();
    }

    public void SetMajorOrder(MajorAiOrderType order)
    {
        if (MajorAiOrderType.None == order) {
            this.majorExecutor = null;
            
        }
        else if (MajorAiOrderType.MoveStations == order)
        {
            this.majorExecutor = new ExecuteMoveStationsOrder
            {
                controller = controller,
                manager = manager,
                entity = entity,
                agent = this
            };
        }
        this.majorOrder = order;
    }


    public AIOrder CreateMoveToLocation(Transform location)
    {
       var order = AIOrder.CreateMoveOrder(location);
       currentOrder = order;
        return order;
    }


    private bool ExecuteCurrentOrder()
    {
        var order = CurrentPriorityOrder;
        if (order == null)
        {
            return true;
        }
        if(order.type == AIOrderType.Move)
        {
            return ExecuteMoveOrder(currentOrder);
        }
        return true;
    }

    private bool ExecuteMoveOrder(AIOrder order)
    {
        Transform location = currentOrder.args[0] as Transform;

        var completed = controller.MoveToLocation(location, 2f);
        order.completed = completed;
        return completed;
        
    }

    private void SetIdle()
    {
        this.currentOrder = null;
    }

    private class ExecuteMoveStationsOrder : IMajorOrderExecutor
    {
        private SpaceEntity currentLocation;
        private SpaceEntity destination;

        public AISpaceShipController controller;

        public GameManager manager;

        public SpaceEntity entity;

        public AIAgent agent;

        private bool IsMoving = false;

        private AIOrder order;
        public void Execute()
        {
            if(!IsMoving)
            {

                
                if(currentLocation == null)
                {
                    destination = manager.FindRandomStationDestination(entity);
                }
                else
                {
                    destination = manager.FindRandomStationDestination(entity, currentLocation);
                    Debug.Log(destination);

                }
               
                
                order = agent.CreateMoveToLocation(destination.transform);
                IsMoving = true;
            }
            else
            {
                //var reached = controller.MoveToLocation(entity.transform, 2f);
                var reached = order.completed;
                
                if (reached)
                {
                    currentLocation = destination;
                    destination = null;
                    IsMoving = false;
                }
            }

        }

    }
}
