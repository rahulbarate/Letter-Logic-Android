using UnityEngine;
using UnityEngine.SceneManagement;

public class MilestoneTracker
{
    MilestoneData data;
    int cubesPlaced;
    int wordsCompleted;
    float elapsedTime;
    bool isActive;
    bool isMilestoneCompleted = false;
    int damageTaken;


    public MilestoneTracker(MilestoneData milestone)
    {
        data = milestone;
        Reset();
    }
    public void Reset()
    {
        cubesPlaced = 0;
        wordsCompleted = 0;
        elapsedTime = 0f;
        damageTaken = 0;
        isActive = true;
        isMilestoneCompleted = false;
    }
    public void UpdateTimer(float deltaTime)
    {
        if (!isActive || data.type == MilestoneData.Type.DamageBased) return;
        elapsedTime += deltaTime;
        if (elapsedTime > data.timeLimit) Reset();
    }
    public void TakeDamage()
    {
        damageTaken += 1;
        if (damageTaken > data.maxDamageAllowed)
            Reset();
    }

    public void CubePlaced()
    {
        if (!isActive) return;
        cubesPlaced += 1;
        if (cubesPlaced >= data.cubesToPlace)
        {

            if (data.type == MilestoneData.Type.TimeBased && elapsedTime <= data.timeLimit) // for time based milestone
                ProcessMilestoneCompletion();
            if (data.type == MilestoneData.Type.DamageBased && damageTaken <= data.maxDamageAllowed) // for damage based milestones
                ProcessMilestoneCompletion();
            if (data.type == MilestoneData.Type.Mixed && damageTaken <= data.maxDamageAllowed && elapsedTime <= data.timeLimit)// for mixed milestone
                ProcessMilestoneCompletion();
            if (!isMilestoneCompleted) // for partially completed milestone
                Reset();
        }

    }
    public void WordCompleted()
    {
        if (!isActive) return;
        wordsCompleted += 1;
        if (wordsCompleted >= data.wordsToComplete)
        {
            if (data.type == MilestoneData.Type.TimeBased && elapsedTime <= data.timeLimit)
                ProcessMilestoneCompletion();
            if (data.type == MilestoneData.Type.DamageBased && damageTaken <= data.maxDamageAllowed)
                ProcessMilestoneCompletion();
            if (data.type == MilestoneData.Type.Mixed && damageTaken <= data.maxDamageAllowed && elapsedTime <= data.timeLimit)
                ProcessMilestoneCompletion();
            if (!isMilestoneCompleted)
                Reset();
        }
    }

    void ProcessMilestoneCompletion()
    {
        isMilestoneCompleted = true;
        data.noOfTimesCompleted += 1;
        if (SceneManager.GetActiveScene().buildIndex >= 3)
            CustomLogger.Log($"Milestone {data.no} Achieved. Completed {data.wordsToComplete} words in {elapsedTime} seconds with {damageTaken} damage. Rewarded {data.rewardCoins} coins.");
        else
            CustomLogger.Log($"Milestone {data.no} Achieved. Placed {data.cubesToPlace} in {elapsedTime} seconds with {damageTaken} damage. Rewarded {data.rewardCoins} coins.");

        if (!data.repeatable)
            isActive = false;
        else
            Reset();

    }
}
