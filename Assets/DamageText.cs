using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    // Start is called before the first frame update
    float t;
    float x;
    float y;
    float mult = 32;
    float drag = 1.0f;
    float newx;
    private RectTransform txt;
    public Text txt1;
    public Text txt2;
    public int dmg = 101;
    void Start()
    {
        t = 0;
        txt = gameObject.GetComponent<RectTransform>();
        Vector2 pos = txt.anchoredPosition;
        x = pos.x;
        y = pos.y;
        newx = 0;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime * 4;
        if (drag >= 0.0f)
        {
            newx += Time.deltaTime * 4 * drag;
            txt.anchoredPosition = new Vector2(x + newx * mult, y + GetY(t) * mult);
            drag -= .01f;
        }
        if (t >= 10.0f) { Destroy(gameObject); }
        txt1.text = dmg.ToString();
        txt2.text = dmg.ToString();
    }
    private float GetY(float x)
    {
        float y = Mathf.Abs(Mathf.Sin(3 * x)*3) / (x * x + 1);
        return y;
    }
}
