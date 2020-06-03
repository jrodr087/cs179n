using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Lowscope;
using UnityEngine.SceneManagement;
 
namespace Lowscope.Saving.Core
{
    public class SaveLoad: MonoBehaviour, ISaveable
    {
        public PlayerMovement movscript;
        public int[] saves;
        public bool active = false;
        
        public string sceneName = "";

        public void SaveSlot(int slot) {
            SaveMaster.DeleteSave(slot);
            Debug.Log("Saving");
            SaveMaster.SetSlot(slot, false);
            SaveMaster.WriteActiveSaveToDisk();     //write save to disk

            Text txt = GetComponentInChildren<Text>(true);
            txt.fontSize = 13;
            txt.alignment = TextAnchor.UpperRight;
            DateTime timeSaved = SaveMaster.GetSaveCreationTime(slot);
            txt.text = timeSaved.ToString("dd MMMM yyyy hh:mm:ss tt");

            //txt.text = "Saved Slot";
        }

        public void LoadGame(int slot) {
            //var slotToLoad = 0; // Set your index here
            //SaveMaster.ClearSlot();
            
            SaveMaster.SetSlot(slot, true);

            SceneManager.LoadScene("SampleScene"); //temp until more scenes/levels are added
        }

        void Start()
        {

            Debug.Log("Getting Used Save Slots");
            saves = SaveFileUtility.GetUsedSlots();
            Debug.Log("Saves: " + saves.Length.ToString());
            Debug.Log("Active Slot: " + SaveMaster.GetActiveSlot().ToString());
            
            Button[] button = GameObject.Find("SaveContent").GetComponentsInChildren<Button>();
            int i = 0;
            foreach(Button btn in button)
            {
                Text txt = btn.GetComponentInChildren<Text>(true);
                //SaveFileUtility.LoadSave(i);
            
                if(SaveFileUtility.IsSlotUsed(i))
                {
                    txt.fontSize = 13;
                    txt.alignment = TextAnchor.UpperRight;
                    DateTime timeSaved = SaveMaster.GetSaveCreationTime(i);
                    txt.text = timeSaved.ToString("dd MMMM yyyy hh:mm:ss tt");
                    //txt.text = "Saved Slot";
                }
                else if(!SaveFileUtility.IsSlotUsed(i)){
                    txt.fontSize = 16;
                    txt.alignment = TextAnchor.MiddleCenter;
                    txt.text = "New Slot";
                }
                i++;
            }
        }
        void Update()
        {

        }

        public string OnSave()
        {
            sceneName = SceneManager.GetActiveScene().name;
            Debug.Log("Scene Name:" + sceneName);
            return JsonUtility.ToJson("sceneName");
        }

        public void OnLoad(string data)
        {
            //stats = JsonUtility.FromJson<PlayerStats>(data);
            sceneName = JsonUtility.FromJson<string>(data);
            Debug.Log("Scene Name:" + sceneName);
            //SceneManager.LoadScene("sceneName");
        }

        public bool OnSaveCondition()
        {
            return true;
        }
    }
}