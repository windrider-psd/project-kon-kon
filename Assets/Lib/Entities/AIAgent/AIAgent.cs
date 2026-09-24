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
                return priorityOrder;
            }
            return currentOrder;
        }
    }

    public AISpaceShipController controller;

    public GameManager manager;

    public SpaceEntity entity;
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
       
        ExecuteProvocationStatus();


    }

    public void ExecuteProvocationStatus()
    {

        if(!isProvoked)
        {
            return;
        }

        else if(priorityOrder != provokedOrder)
        {
            isProvoked = false;
            provokedOrder = null;
        }
        else if (provokedOrder.completed)
        {
            isProvoked = false;
        }

        /*
        if (isProvoked && (provokedOrder.completed || (priorityOrder != null && priorityOrder != provokedOrder)))
        {
            isProvoked = false;
            provokedOrder = null;
        }*/
        /*
        if (priorityOrder != null && priorityOrder.completed)
        {
            priorityOrder = null;
        }
        if (currentOrder != null && currentOrder.completed)
        {
            currentOrder = null;
        }*/
    }

    public bool isProvoked = false;
    private AIOrder provokedOrder;
    public void Provoke(SpaceEntity origin)
    {
        var current = priorityOrder;
        if (!isProvoked)
        {
            provokedOrder = CreateDefendYourself(true);
            isProvoked = true;
        }
        
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
        else if(MajorAiOrderType.Patrol == order)
        {
            this.majorExecutor = new ExecutePatrolOrder
            {
                controller = controller,
                manager = manager,
                entity = entity,
                agent = this,
                friendFoeManager = FindAnyObjectByType<FriendFoeManager>()
            };
        }
        this.majorOrder = order;
    }


    public AIOrder CreateMoveToLocation(Transform location, bool priority = false)
    {
        var order = new MoveOrder(this, location);
        if (priority)
            priorityOrder = order;
        else
            currentOrder = order;
        return order;
    }

    public AIOrder CreateDefendYourself(bool priority = false)
    {
        var order = new DefendYourselfOrder(this);
        if (priority)
            priorityOrder = order;
        else
            currentOrder = order;
        return order;
    }

    public AIOrder CreateDestroy(Transform target, bool priority = false)
    {
        var order = new DestroyOrder(this, target);
        if (priority)
            priorityOrder = order;
        else
            currentOrder = order;
        return order;
    }


    private void ExecuteCurrentOrder()
    {
        AIOrder orderToExecute = null;

        if (priorityOrder != null && priorityOrder.completed)
        {
            priorityOrder = null;
        }

        if (currentOrder != null && currentOrder.completed)
        {
            currentOrder = null;
        }
        
        if (priorityOrder != null)
        {
            orderToExecute = priorityOrder;
            priorityOrder.Execute();

        }
        else if (currentOrder != null)
        {
            orderToExecute = currentOrder;
            currentOrder.Execute();
        }
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

        float turnDeadZone = 10f;

        return angle > turnDeadZone == false && angle < -turnDeadZone == false;
    }

    private void SetIdle()
    {
        this.priorityOrder = null;
        this.currentOrder = null;
        controller.SetIdle();
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
                    if (!entity.inventory.IsInventoryNearFull())
                    {
                        stage = 0;
                    }
                    else
                    {
                        var destination = manager.FindRandomStationDestination(entity);
                        order = agent.CreateMoveToLocation(destination.transform);

                        stage = 5;
                    }
                    
                }
            }
            if(stage == 5)
            {
                var reached = order.completed;

                if (reached)
                {
                    entity.inventory.ClearInventory();
                    stage = 0;
                }
            }


        }
    }
    private class ExecutePatrolOrder : IMajorOrderExecutor
    {


        public AISpaceShipController controller;

        public GameManager manager;

        public SpaceEntity entity;

        public AIAgent agent;

        public FriendFoeManager friendFoeManager;

      
        private AIOrder order;

        private Timer timer;

        //0 = looking; 1 = found and attacking;
        private int stage = 0;
        public void Execute()
        {
            if(stage == 0)
            {
                SpaceEntity enemy = manager.FindNearbyEnemy(entity);
                
                if(enemy != null)
                {
                    this.order = agent.CreateDestroy(enemy.transform);
                    stage = 1;
                }
                else
                {
                    if (order == null)
                    {
                        
                        this.order = agent.CreateMoveToLocation(
                               manager.CreateCheckpoint(entity, entity.CurrentSector.GetRandomPointWithin()
                           ).transform
                        );
                    }
                    else if (order.completed == true)
                    {
                        {
                            order = null;
                        }
                    }
                }
                
            }
            else if(stage == 1)
            {
                if(order.completed)
                {
                    stage = 0;
                    order = null;
                }
            }
            
        }
    }
}
