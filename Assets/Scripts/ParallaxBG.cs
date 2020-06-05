using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBG : MonoBehaviour
{
	public float speed;
	private float length, startPos;
	public GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        //Debug.Log("Start " + startPos);
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        //Debug.Log("Length: " + length);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate((new Vector3(-1,0,0)) * speed * Time.deltaTime);

		if (transform.position.x <= (camera.transform.position.x - length)) {
			Vector3 pos = new Vector3(camera.transform.position.x + length, transform.position.y, transform.position.z);
			transform.position = pos;
		}

    }
}

