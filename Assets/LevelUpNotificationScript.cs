using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelUpNotificationScript : MonoBehaviour
{
    // Start is called before the first frame update
    bool sweeping = false;
    float time = -math.PI / 2 * 0.95f;
    RectTransform rt;
    void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sweeping)
        {
            time += Time.deltaTime;
            if (time >=  math.PI/2 * .95f) { sweeping = false; time = -math.PI / 2 * 0.95f; }
            rt.anchoredPosition = new Vector2(Mathf.Tan(time)*32, 0);
        }
        else
        {
            rt.anchoredPosition = new Vector2(-999, 0);
        }
    }

    public void StartNotification()
    {
        time = -math.PI/2 * 0.95f;
        sweeping = true;
    }
}
