using UnityEngine;
using System.Collections.Generic;

public class MilestoneManager : MonoBehaviour
{
    [SerializeField]
    MilestoneCollection collection;
    List<MilestoneTracker> trackers = new();
    [SerializeField] ToastUI toastUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (MilestoneData data in collection.milestones)
        {
            trackers.Add(new MilestoneTracker(data, ShowToast));
        }
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        foreach (MilestoneTracker tracker in trackers)
        {
            tracker.UpdateTimer(deltaTime);
        }
    }

    public void HandleCubePlaced()
    {
        trackers.ForEach(t => t.CubePlaced());
    }
    public void HandleDamageTaken()
    {
        trackers.ForEach(t => t.TakeDamage());
    }

    public void HandleWordCompleted()
    {
        trackers.ForEach(t => t.WordCompleted());
    }

    public void HandleIncorrectWord()
    {
        trackers.ForEach(t => t.Reset());
    }

    public void ShowToast(string text)
    {
        Debug.Log("Called for " + text);
        toastUI.ShowToast(text, "");
    }
}
