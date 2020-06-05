﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Lowscope;
using Lowscope.Saving;


public class EnemyScript : MonoBehaviour, ISaveable
{
    public PlayerMovement movscript;
    private CameraShader cs;
    private Texture inwipe;
    private Texture outwipe;
    public EnemyFactory.EnemyType type;
    public MusicPlayerScript mps;

    public bool active=true;
    
    [System.Serializable]
    public struct EData
    {
        public bool isActive;
    }

    // Start is called before the first frame update
    void Start()
    {

        cs = GameObject.Find("Main Camera").GetComponent<CameraShader>();
        movscript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        mps = GameObject.Find("Main Camera/MusicPlayer").GetComponent<MusicPlayerScript>();
        inwipe = Resources.Load<Texture>("Textures/weirdspiralwipe");
        outwipe = Resources.Load<Texture>("Textures/screenwipeouttex");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void InitializeBattle()
    {
        GameObject btl = (GameObject)Instantiate(Resources.Load("Prefabs/BattlePrefab"));
        btl.transform.position = new Vector3(1000, 1000, 0);
        CameraScript cs = GameObject.Find("Main Camera").GetComponent<CameraScript>();
        cs.sub = CameraSubject.battle;
        cs.Battle = btl;
        movscript.battle = btl;
        BattleMasterScript bm;
        bm = GameObject.Find("BattleMaster").GetComponent<BattleMasterScript>();
        bm.InitializeBattle(type);
        bm.songloc = "Sounds/Music/Fight_Mood";
        mps.PlaySong("Sounds/Music/Fight_Mood", 2, true);
        //Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" && !movscript.GetMovementLock())
        {
            mps.PlaySong("Sounds/Music/O_SHIT_I_ENCOUNTERED_AN_ENEMY_BUT_ITS_SHORTER", 3, false);
            Debug.Log("Enemy touched player");
            cs.StartWipe(inwipe, outwipe, InitializeBattle, null,1.0f,3.0f);
            movscript.LockMovement();
            //SceneManager.LoadScene("BattleScene");
            active = false;
            gameObject.SetActive(active);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" && !movscript.GetMovementLock())
        {
        }
    }

    [SerializeField]
    private EData enemyData;
    public string OnSave()
        {
            return JsonUtility.ToJson(new EData() {isActive = active});
        }

        public void OnLoad(string data)
        {
            //stats = JsonUtility.FromJson<PlayerStats>(data);
            enemyData = JsonUtility.FromJson<EData>(data);
            active = enemyData.isActive;
            gameObject.SetActive(active);
        }

        public bool OnSaveCondition()
        {
            return true;
        }
}
