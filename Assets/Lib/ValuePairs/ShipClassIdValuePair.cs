using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Lib.ValuePairs
{
    [Serializable]
    public struct ShipClassIdValuePair
    {

        public SpaceEntityClassId key;

        public GameObject value;
    }
}
