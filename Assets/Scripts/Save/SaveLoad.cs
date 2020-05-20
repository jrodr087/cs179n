using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Lowscope;
using UnityEngine.SceneManagement;

 
namespace Lowscope.Saving.Core
{
    public class SaveLoad: MonoBehaviour
    {
        public PlayerMovement movscript;
        public int[] saves;
        public bool active = false;

        public void SaveSlot(int slot) {
            Debug.Log("Saving");
            SaveMaster.SetSlot(slot, false);
            SaveMaster.WriteActiveSaveToDisk();     //write save to disk

            Text txt = GetComponentInChildren<Text>(true);
            txt.text = "Saved Slot";
        }

        public void LoadGame(int slot) {
            //var slotToLoad = 0; // Set your index here
            //SaveMaster.ClearSlot();
            SceneManager.LoadScene("SampleScene");
            SaveMaster.SetSlot(slot, true);
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
                    txt.text = "Saved Slot";
                }
                else if(!SaveFileUtility.IsSlotUsed(i)){
                    txt.text = "New Slot";
                }
                i++;
            }
        }
        void Update()
        {
            
            // DisplaySlot(1);
            // DisplaySlot(2);
            // DisplaySlot(3);
            // DisplaySlot(4);
            // DisplaySlot(5);
            // DisplaySlot(6);
            // DisplaySlot(7);
            // DisplaySlot(8);
            // DisplaySlot(9);
        }
    }
}