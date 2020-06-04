using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SelectorBobScript : MonoBehaviour
{
    // Start is called before the first frame update
    RectTransform rt;
    void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rt.anchoredPosition = new Vector3(0, 8, 0) * math.sin(Time.time * 2);
    }
}
