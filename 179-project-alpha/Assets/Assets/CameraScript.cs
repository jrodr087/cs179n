using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerChar;
    public GameObject cam;
    private Transform playertransform;
    private Transform camtransform;
    void Start()
    {
        playertransform = playerChar.GetComponent<Transform>();
        camtransform = cam.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        camtransform.position = new Vector3(playertransform.position.x, playertransform.position.y + 1,-10);
    }
}
