using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{

    //private float thrustUp = 4.0f;
    //private float thrustLeft = 1.0f;

    private Vector3 angle = new Vector3(0, 0, 45);


    [SerializeField]
    private Transform spawningPoint;

    [SerializeField]
    private Rigidbody2D rb2D;


    void Start()
    {
        transform.position = spawningPoint.position;

        ShootBall();
    }

    private void Update()
    {
        if (transform.position.y <= -10.0)
        {
            Destroy(this.gameObject);
        }
    }

    public void ShootBall() {
        float maxUp = 5.0f;
        float min = 1.0f;
        float maxLeft = 2.0f;


        float forceUp = Random.Range(min, maxUp);
        float forceLeft = Random.Range(min, maxLeft);
      
        rb2D.AddForce(transform.up * forceUp, ForceMode2D.Impulse);
        rb2D.AddForce(-transform.right * forceLeft, ForceMode2D.Impulse);


    }

}

