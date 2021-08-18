using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public class SpawnPrefab
    {
        public SpawnPrefab(GameObject _prefab, float _timer, float _lifetime, Vector3 _prevSpawnPos)
        {
            prefab = _prefab;
            timer = _timer;
            lifetime = _lifetime;
            prevSpawnPos = _prevSpawnPos;
        }

        internal GameObject prefab;
        internal float timer;
        internal float lifetime;
        internal Vector3 prevSpawnPos;
    }


    [SerializeField] private GameObject fruitPrefab;

    [SerializeField] private float levelBound = 24f;

    [SerializeField] private float fruitCounter;
    [SerializeField] private float fruitTimer = 3f;
    [SerializeField] private float fruitLifetime = 4f;

    [SerializeField] private Vector3 prevFruitSpawnPos;
    // Start is called before the first frame update
    void Start()
    {
        //spawn first fruit
        fruitCounter = fruitTimer;
    }

    // Update is called once per frame
    void Update()
    {
        fruitCounter += Time.deltaTime;
        if (fruitCounter >= fruitLifetime)
        {
            SpawnPrefab fruit = new SpawnPrefab(fruitPrefab, fruitTimer, fruitLifetime, prevFruitSpawnPos);
            fruitCounter = 0f;
            Spawn(fruit);
        }
    }

    private void Spawn(SpawnPrefab prefab)
    {
        //calculate position to spawn fruit
        Vector3 spawnPos;
        do
        {
            spawnPos = new Vector3
            (Random.Range(-levelBound, levelBound),
            0.5f, Random.Range(-levelBound, levelBound));

        } while (prefab.prevSpawnPos == spawnPos);

        //spawn fruit
        GameObject newObject = Instantiate(prefab.prefab, spawnPos, Quaternion.identity);
        //destroy fruit after some time
        Destroy(newObject, prefab.lifetime);

        //store spawn position 
        prefab.prevSpawnPos = spawnPos;

    }
}
