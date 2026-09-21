using Assets.Lib.Entities.AIAgent;
using System.Xml;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static UnityEngine.GraphicsBuffer;

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
        else if (MajorAiOrderType.FarmAsteroids == order)
        {
            this.majorExecutor = new ExecuteFarmAsteroidsOrder
            {
                controller = controller,
                manager = manager,
                entity = entity,
                agent = this,
                spawner = FindAnyObjectByType<AsteroidAreaSpawner>()
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

    public AIOrder CreateDestroy(Transform target)
    {
        var order = AIOrder.CreateDestroyOrder(target);
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
        else if(order.type == AIOrderType.Destroy)
        {
            return ExecuteDestroyOrder(currentOrder);
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

    private bool ExecuteDestroyOrder(AIOrder order)
    {
        
        Transform location = currentOrder.args[0] as Transform;
        if(location == null)
        {
            order.completed = true;
            return true;
        }

        controller.MoveToLocation(location, 2f);

        if (IsLookingAtTheTarget(location))
        {
            entity.ShootCannons();
        }
        
        return false;
    }

    public bool IsLookingAtTheTarget(Transform target)
    {
        Vector2 toTarget = target.position - transform.position;
        float distance = toTarget.magnitude;

        Vector2 directionToTarget = toTarget.normalized;

        float angle = Vector2.SignedAngle(
            transform.up,
            directionToTarget
        );

        float turnDeadZone = 5f;

        return angle > turnDeadZone == false && angle < -turnDeadZone == false;
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
            if (!IsMoving)
            {


                if (currentLocation == null)
                {
                    destination = manager.FindRandomStationDestination(entity);
                }
                else
                {
                    destination = manager.FindRandomStationDestination(entity, currentLocation);
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
    private class ExecuteFarmAsteroidsOrder : IMajorOrderExecutor
    {
        private SpaceEntity currentLocation;
        private SpaceEntity destination;

        public AISpaceShipController controller;

        public GameManager manager;

        public SpaceEntity entity;

        public AIAgent agent;

        public AsteroidAreaSpawner spawner;

        private int stage = 0;

        private AIOrder order;

        private Vector3 bufferedAsteroidLocation;

        private Timer timer;
        public void Execute()
        {
            Debug.Log(stage);
            if (stage == 0)
            {
                var asteroids = spawner.Asteroids;

                if (asteroids.Length > 0)
                {
                    var asteroid = RandomUtils.GetRandomFromArray(asteroids);
                    this.bufferedAsteroidLocation = asteroid.transform.position;
                    order = agent.CreateDestroy(asteroid.transform);
                    stage = 1;
                }
            }
            else if (stage == 1)
            {
                var completed = order.completed;

                if (completed)
                {
                    order = agent.CreateMoveToLocation(manager.CreateCheckpoint(entity, bufferedAsteroidLocation).transform);
                    stage = 2;
                }
            }
            else if(stage == 2)
            {
                var completed = order.completed;

                if (completed)
                {
                    agent.SetIdle();
                    stage = 3;
                }
            }
            else if (stage == 3)
            {
                entity.GripNearbyScrape();
                stage = 4;
                timer = new Timer(3);
            }
            if (stage == 4)
            {
                timer.Update();
                if (timer.Finished)
                {
                    stage = 0;
                }
            }


        }
    }
}
