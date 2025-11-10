
[System.Serializable]
public class MilestoneData
{
    public int no;
    public enum Type { TimeBased, DamageBased, Mixed }
    public Type type;
    public int cubesToPlace;
    public float timeLimit;
    public int maxDamageAllowed;
    public int wordsToComplete;
    public bool repeatable;
    public int rewardCoins;

}
