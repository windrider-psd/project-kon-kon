using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class AsteroidAreaSpawner : MonoBehaviour
{

    public float spawnTime;
    public int spawnCount;

    private GameManager manager;
    private BoxCollider2D boxCollider;
    private SpaceEntity[] Asteroids 
    {
        get
        {
            return this.GetComponentsInChildren<SpaceEntity>();
        }
    }
    void Start()
    {
       boxCollider = GetComponent<BoxCollider2D>();
        manager = FindAnyObjectByType<GameManager>();
        StartCoroutine(SpawnTimer());
    }

    IEnumerator SpawnTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
            if(Asteroids.Length < spawnCount)
            {
                manager.SpawnAsteroidWithinArea(boxCollider);
            }
        }
        

    }
}
