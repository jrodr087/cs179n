using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraSubject { player,battle};
public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerChar;
    public GameObject cam;
    public GameObject Battle;
    public CameraSubject sub = CameraSubject.player;
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
        if (sub == CameraSubject.player)
        {
            camtransform.position = new Vector3(playertransform.position.x, playertransform.position.y + 1, -10);
        }
        else if (sub == CameraSubject.battle)
        {
            camtransform.position = new Vector3(Battle.transform.position.x, Battle.transform.position.y + 1, -10);
        }
    }
}
