
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
    public int wordLength = 3;
    public bool repeatable;
    public int noOfTimesCompleted = 0;
    public int rewardCoins;

}
