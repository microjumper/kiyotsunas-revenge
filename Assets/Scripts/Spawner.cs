using UnityEngine;

public class Spawner : MonoBehaviour
{
    public ObjectPool pool;
    public Transform[] spawnPoints;
    public float time;
    public float repeatRate;

    void Start()
    {
        InvokeRepeating(nameof(Spawn), time, repeatRate);
    }

    private void Spawn()
    {
        int index = Random.Range(0, spawnPoints.Length);
        pool.Spawn(spawnPoints[index].transform.position, Quaternion.identity);
    }
}