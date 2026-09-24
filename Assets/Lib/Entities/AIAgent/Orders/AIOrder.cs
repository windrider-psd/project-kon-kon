using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;



public abstract class AIOrder {
    public AIOrderType type;

    public AIAgent agent;
    public object[] args;


    public bool completed;

    public abstract void Execute();

    protected AIOrder(AIOrderType type, AIAgent agent)
    {
        this.agent = agent;
        this.type = type;
    }
    /*
    public static AIOrder CreateMoveOrder(Transform location)
    {
        return new AIOrder()
        {
            type = AIOrderType.Move,
            args = new object[] { location }
        };
    }

    public static AIOrder CreateDestroyOrder(Transform target)
    {
        return new AIOrder()
        {
            type = AIOrderType.Destroy,
            args = new object[] { target }
        };
    }*/
}

