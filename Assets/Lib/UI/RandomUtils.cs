using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

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

    public static Vector2 RandomPointWithinBoxCollider(BoxCollider2D box)
    {
        return new Vector2(
            Random.Range(box.bounds.min.x, box.bounds.max.x),
            Random.Range(box.bounds.min.y, box.bounds.max.y)
        );
    }
}
