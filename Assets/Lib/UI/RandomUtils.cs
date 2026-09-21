using System.Collections;
using UnityEngine;

public class RandomUtils
{

    private static System.Random random = new();

    public static T GetRandomFromArray<T>(T[] array)
    {
        if(array.Length == 0)
        {
            return default(T);
        }
       var index =  random.Next(array.Length);
        return array[index];
    }
}
