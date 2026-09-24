using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static UnityEngine.EventSystems.EventTrigger;


public class DestroyOrder : AIOrder
{
    public Transform transform;
    public DestroyOrder(AIAgent a, Transform t): base(AIOrderType.Destroy, a)
    {
        transform = t;
    }

    public override void Execute()
    {
     
        if (transform == null)
        {
            completed = true;
            return;
        }

        agent.controller.MoveToLocation(transform, 2f, false);

        if (agent.IsLookingAtTheTarget(transform))
        {
            agent.entity.ShootCannonsInRange(transform);
        }
    }
}

