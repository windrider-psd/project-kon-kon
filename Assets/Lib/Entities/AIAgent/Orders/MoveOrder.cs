using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class MoveOrder : AIOrder
{
    public Transform transform;
    public MoveOrder(AIAgent a, Transform t): base(AIOrderType.Move, a)
    {
        transform = t;
    }

    public override void Execute()
    {
        completed = agent.controller.MoveToLocation(transform, 2f, true);
    }
}

