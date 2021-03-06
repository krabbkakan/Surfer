﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FivehundredPointsController : MonoBehaviour {


    [SerializeField]
    private float _speed = 8.0f;

    UIManager _uiManager;

    [SerializeField]
    private Transform spawningPoint;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        transform.position = spawningPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * _speed * Time.deltaTime);

        if (transform.position.x >= 20.0)
        {
            Destroy(this.gameObject);
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Colided with: " + other.name + " Tag: " + other.tag);


        //kolla om det powerup coliderar med är player
        if (other.tag == "Surfer")
        {
            _uiManager.UpdateScore(500);


            // destroy powerup
            Destroy(this.gameObject);
        }
    }

}
