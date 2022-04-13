using UnityEngine;
using System.Collections;

public class SpawnItem : MonoBehaviour
{

    public GameObject item;
    public GameObject spawnPoint;
    public bool destroyAfterSpawn;
    public bool isMulti;
    public int numberOfItemsToSpawn;
    public float delay;

    private int spawnCount;
    private float accumulatedDelay;
    private bool spawned;
    // Use this for initialization
    void Start()
    {
        accumulatedDelay = 0;
        spawnCount = numberOfItemsToSpawn;
        Debug.Log($"spawnCount starting at: {spawnCount}");
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyAfterSpawn && spawnCount <= 0)
        {
            Destroy(gameObject, 0.1f);
        }
    }

    void OnTriggerEnter2D(Collider2D obj)
    {
        if (item && spawnPoint)
        {
            if (obj.gameObject.tag.Equals("Player"))
            {
                if (!spawned)
                {
                    if (!isMulti)
                    {
                        spawned = true;
                        accumulatedDelay = delay;
                        StartCoroutine("SpawnItems");
                    }
                    else
                    {
                        for (int i = 0; i < numberOfItemsToSpawn; i++)
                        {
                            StartCoroutine("SpawnItems_Random");
                            accumulatedDelay += delay;
                        }
                        spawned = true;
                    }
                    // if (destroyAfterSpawn)
                    // {
                    //     Destroy(gameObject, 0.1f);
                    // }
                }
            }
        }
    }

    IEnumerator SpawnItems_Random()
    {
        yield return new WaitForSeconds(accumulatedDelay);
        GameObject newItem = (GameObject)Instantiate(item, spawnPoint.transform.position, Quaternion.identity);
        float x = Random.Range(-200, 200);
        float y = Random.Range(-200, 200);
        newItem.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(x, y));
        spawnCount--;
        Debug.Log($"spawnCount now at: {spawnCount}");
    }

    IEnumerator SpawnItems()
    {
        yield return new WaitForSeconds(accumulatedDelay);
        GameObject newItem = (GameObject)Instantiate(item, spawnPoint.transform.position, Quaternion.identity);
        spawnCount--;
        Debug.Log($"spawnCount now at: {spawnCount}");
    }
}
