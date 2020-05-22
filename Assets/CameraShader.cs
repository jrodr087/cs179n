using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void EmptyVoidCallback();

[ExecuteInEditMode]
public class CameraShader : MonoBehaviour
{
    public enum WipeState { wipein,wait,wipeout,dead};
    public Material material;
    private WipeState currstate = WipeState.dead;
    public float cutoff = 0.0f;
    public Texture wipein;
    public Texture wipeout;
    private float speedin = 3.0f;
    private float speedout = 3.0f;
    private float timer = 0.0f;
    private float timerVal = 0.2f;
    EmptyVoidCallback cbmid = null;
    EmptyVoidCallback cbend = null;
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }
// Start is called before the first frame update
    void Start()
    {
        cutoff = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (currstate == WipeState.wipein)
        {
            material.SetTexture("_TransitionTex", wipein);
            cutoff += (Time.deltaTime * speedin);
            if (cutoff >= 1.0f)
            {
                cutoff = 1.0f;
                currstate = WipeState.wait;
                cbmid?.Invoke();
                cbmid = null;
            }
        }
        else if (currstate == WipeState.wait)
        {
            timer += Time.deltaTime;
            if (timer >= timerVal)
            {
                currstate = WipeState.wipeout;
                timer = 0.0f;
            }
        }
        else if (currstate == WipeState.wipeout)
        {
            material.SetTexture("_TransitionTex", wipeout);
            cutoff -= (Time.deltaTime * speedout);
            if (cutoff <= 0.0f)
            {
                cutoff = 0.0f;
                currstate = WipeState.dead;
                cbend?.Invoke();
                cbend = null;
            }
        }
        material.SetFloat("_Cutoff", cutoff);
    }

    public void StartWipe(Texture wipein, Texture wipeout, EmptyVoidCallback callbackmid,EmptyVoidCallback callbackend)
    {
        this.wipein = wipein;
        this.wipeout = wipeout;
        cbmid = callbackmid;
        cbend = callbackend;
        currstate = WipeState.wipein;
        this.speedin = 3.0f;
        this.speedout = 3.0f;
    }
    public void StartWipe(EmptyVoidCallback callbackmid, EmptyVoidCallback callbackend)
    {
        this.wipein = Resources.Load<Texture>("Textures/screenwipeintex");
        this.wipeout = Resources.Load<Texture>("Textures/screenwipeouttex");
        cbmid = callbackmid;
        cbend = callbackend;
        currstate = WipeState.wipein;
        this.speedin = 3.0f;
        this.speedout = 3.0f;
    }
    public void StartWipe(Texture wipein, Texture wipeout, EmptyVoidCallback callbackmid, EmptyVoidCallback callbackend, float speedin, float speedout)
    {
        this.wipein = wipein;
        this.wipeout = wipeout;
        cbmid = callbackmid;
        cbend = callbackend;
        currstate = WipeState.wipein;
        this.speedin = speedin;
        this.speedout = speedout;
    }
}
