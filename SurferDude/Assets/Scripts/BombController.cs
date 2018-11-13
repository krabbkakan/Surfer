using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{

    [SerializeField]
    private Transform spawningPoint;

    // Use this for initialization
    void Start()
    {
        transform.position = spawningPoint.position;

    }

    private void Update()
    {

        if (transform.position.y <= -6.0)
        {
            Destroy(this.gameObject);
        }
    }


}
