using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    [SerializeField]
    private GameObject ballPrefab;

    [SerializeField]
    private GameObject bombPrefab;

    [SerializeField]
    private GameObject[] points;

    private UIManager _uIManager;

    // Use this for initialization
    void Start()
    {
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();


        StartCoroutine(BallSpawnRoutine());
        StartCoroutine(PointsSpawnRoutine());
        StartCoroutine(BombSpawnRoutine());

    }


    // creating a courutine for spawning an enemy every 5 secs.

    IEnumerator BallSpawnRoutine()
    {
        while (true)
        {
            Instantiate(ballPrefab);
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator BombSpawnRoutine()
    {
        while (true)
        {
            Instantiate(bombPrefab, new Vector3(Random.Range(-2.0f, 8.0f), 7, 0), Quaternion.identity);
            yield return new WaitForSeconds(10.0f);
        }
    }

    // creating a courutine for spawning a random poweup every 5 secs.
    IEnumerator PointsSpawnRoutine()
    {
        
        while (true)
        {
            int randomPoints = Random.Range(0, 2);
            Instantiate(points[randomPoints]);
            yield return new WaitForSeconds(5.0f);
        }
    }
}
