using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;



public class AIOrder{
    public AIOrderType type;
    public object[] args;

    public bool completed;

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
    }
}

