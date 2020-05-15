using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{

    private float length, startPos;
    public GameObject camera;
    public float parallaxValue;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = (camera.transform.position.x * (1 - parallaxValue));
        float distance = (camera.transform.position.x *parallaxValue);
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (x > startPos + length){
            startPos += length;
        }
        else if (x < startPos - length){
            startPos -= length;
        }
    }
}
