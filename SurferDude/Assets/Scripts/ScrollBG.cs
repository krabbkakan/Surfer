using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBG : MonoBehaviour {

    [SerializeField]
    private Material material;

    public float speed;
    public float start = 0;

    Vector2 offset = new Vector2(0, 0);

    private void Start()
    {
        speed = 0.1f;
    }

    void Update()
    {  
        offset.x = start + Time.time * speed;
        material.mainTextureOffset = offset;
    }
}
                    