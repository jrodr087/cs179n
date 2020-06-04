using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    private List<string> MusicLocations;
    private List<int> MusicPriorities;
    private List<bool> MusicLoop;
    private string currsong;
    private bool loop = false;
    private int currPri;
    private AudioSource mus;
    void Start()
    {
        MusicLocations = new List<string>();
        MusicPriorities = new List<int>();
        MusicLoop = new List<bool>();
        mus = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!loop)
        {
            if (!mus.isPlaying)
            {
                currPri = -1;
                currsong = "";
                UpdateCurrentSong();
            }
        }
    }
    void UpdateCurrentSong()
    {
        if (MusicLocations.Count == 0) { return; }
        int highestPriIndex = 0;
        int highestPri = MusicPriorities[0];
        for (int i = 0; i < MusicLocations.Count; i++)
        {
            if (MusicPriorities[i] > highestPri)
            {
                highestPri = MusicPriorities[i];
                highestPriIndex = i;
            }
        }
        if (currPri >= highestPri)
        {
            return;
        }
        if (currsong != "")
        {
            MusicLocations.Add(currsong);
            MusicPriorities.Add(currPri);
            MusicLoop.Add(loop);
        }
        currsong = MusicLocations[highestPriIndex];
        currPri = highestPri;
        loop = MusicLoop[highestPriIndex];
        MusicLocations.RemoveAt(highestPriIndex);
        MusicLoop.RemoveAt(highestPriIndex);
        MusicPriorities.RemoveAt(highestPriIndex);
        mus.Stop();
        mus.clip = (AudioClip)Resources.Load(currsong);
        mus.loop = loop;
        mus.Play();
    }
    public void PlaySong(string songloc, int priority, bool loop)
    {
        MusicLocations.Add(songloc);
        MusicPriorities.Add(priority);
        MusicLoop.Add(loop);
        UpdateCurrentSong();
    }
    public void StopSong(string songloc)
    {
        if (currsong == songloc)
        {
            currsong = "";
            currPri = -1;
            loop = false;
            mus.Stop();
            mus.clip = null;
            UpdateCurrentSong();
        }
        else
        {
            for (int i = 0; i < MusicLocations.Count; i++)
            {
                if (songloc == MusicLocations[i])
                {
                    MusicLocations.RemoveAt(i);
                    MusicLoop.RemoveAt(i);
                    MusicPriorities.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
