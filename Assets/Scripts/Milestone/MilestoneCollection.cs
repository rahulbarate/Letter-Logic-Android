using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MilestoneCollection", menuName = "Milestone/Milestone Collection")]
public class MilestoneCollection : ScriptableObject
{
    public List<MilestoneData> milestones;
}
