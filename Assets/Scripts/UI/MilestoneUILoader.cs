using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System;

public class MilestoneUILoader : MonoBehaviour
{
    [Header("Milestone Collections")]
    public List<MilestoneCollection> milestoneCollections;

    [Header("Milestone Card Setup")]
    public GameObject milestoneCardTemplate;
    public string milestoneCardChildName = "milestoneCard";
    public string textComponentName = "Text (TMP)";

    public Color defaultMilestoneIconColor = new Color(1f, 1f, 0f, 1f);
    public Color completedMilestoneIconColor = new Color(0.5f, 0.5f, 0.5f, 1f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Called when the object becomes enabled and active
    void OnEnable()
    {
        LoadAndUpdateMilestoneCards();
    }

    private void LoadAndUpdateMilestoneCards()
    {
        if (milestoneCollections == null || milestoneCollections.Count == 0)
        {
            Debug.LogWarning("No milestone collections found!");
            return;
        }

        // Debug.Log($"=== Loading {milestoneCollections.Count} Milestone Collection(s) ===");

        int milestoneIndex = 0;

        for (int i = 0; i < milestoneCollections.Count; i++)
        {
            MilestoneCollection collection = milestoneCollections[i];

            if (collection == null)
            {
                Debug.LogWarning($"MilestoneCollection at index {i} is null!");
                continue;
            }

            if (collection.milestones == null || collection.milestones.Count == 0)
            {
                Debug.LogWarning($"MilestoneCollection at index {i} has no milestones!");
                continue;
            }

            // Debug.Log($"--- Milestone Collection {i + 1} ({collection.milestones.Count} milestones) ---");

            foreach (MilestoneData milestone in collection.milestones)
            {
                UpdateMilestoneCard(milestone, milestoneIndex);
                milestoneIndex++;
            }
        }
    }

    private void UpdateMilestoneCard(MilestoneData milestone, int cardIndex)
    {
        if (milestone == null)
        {
            Debug.LogWarning("MilestoneData is null!");
            return;
        }

        // Get or create milestone card
        GameObject milestoneCard = GetOrCreateMilestoneCard(cardIndex);
        if (milestoneCard == null)
        {
            Debug.LogError("Failed to get or create milestone card!");
            return;
        }

        // Find TextMeshPro component in the milestone card
        TextMeshProUGUI textComponent = FindTextComponentInCard(milestoneCard);
        if (textComponent == null)
        {
            Debug.LogError($"No TextMeshProUGUI component found in milestone card at index {cardIndex}");
            return;
        }

        // Format the milestone text
        string milestoneText = FormatMilestoneText(milestone);
        textComponent.text = milestoneText;

        // Find and change image icon color if it is completed.
        if (milestone.noOfTimesCompleted > 0)
        {
            Image iconImage = FindIconImageInCard(milestoneCard);
            if (iconImage == null)
                Debug.LogError($"No icon image found in milestone card at index {cardIndex}");
            else
                iconImage.color = completedMilestoneIconColor;
        }

        milestoneCard.SetActive(true);
        // Debug.Log($"Updated milestone card {cardIndex} with milestone #{milestone.id}, card name {milestoneCard.name}");
    }

    private Image FindIconImageInCard(GameObject milestoneCard)
    {
        if (milestoneCard.transform.childCount > 1)
        {
            Transform iconTransform = milestoneCard.transform.GetChild(0);
            return iconTransform.GetComponent<Image>();
        }

        // Fallback: try to find any TextMeshProUGUI in children
        Image[] imageComponents = milestoneCard.GetComponentsInChildren<Image>();
        if (imageComponents.Length > 0)
        {
            return imageComponents[0];
        }
        return null;
    }

    private GameObject GetOrCreateMilestoneCard(int cardIndex)
    {
        // Find existing milestone card by index
        if (cardIndex < transform.childCount)
        {
            Transform existingCard = transform.GetChild(cardIndex);
            return existingCard.gameObject;
        }

        // If no existing card found, create new one from template
        if (milestoneCardTemplate != null)
        {
            GameObject newCard = Instantiate(milestoneCardTemplate, transform);
            return newCard;
        }

        // // If no template available, create simple GameObject as fallback
        // Debug.LogWarning($"No milestone card template available, creating simple card for index {cardIndex}");
        // GameObject fallbackCard = new GameObject($"MilestoneCard_{cardIndex}");
        // fallbackCard.transform.SetParent(transform);

        // // Add TextMeshProUGUI component
        // TextMeshProUGUI textComponent = fallbackCard.AddComponent<TextMeshProUGUI>();

        return null;
    }

    private TextMeshProUGUI FindTextComponentInCard(GameObject milestoneCard)
    {
        // Find TextMeshProUGUI at index 1 (0th index is image, 1st is text)
        if (milestoneCard.transform.childCount > 1)
        {
            Transform textTransform = milestoneCard.transform.GetChild(1);
            return textTransform.GetComponent<TextMeshProUGUI>();
        }

        // Fallback: try to find any TextMeshProUGUI in children
        TextMeshProUGUI[] textComponents = milestoneCard.GetComponentsInChildren<TextMeshProUGUI>();
        if (textComponents.Length > 0)
        {
            return textComponents[0];
        }

        // Final fallback: create new TextMeshProUGUI component
        // Debug.LogWarning("No TextMeshProUGUI component found, creating new one.");
        // return milestoneCard.AddComponent<TextMeshProUGUI>();
        return null;
    }

    private string FormatMilestoneText(MilestoneData milestone)
    {
        string taskDescription = GetTaskDescription(milestone);
        return $"Milestone ID #{milestone.id} \n{taskDescription} \nReward - {milestone.rewardCoins} coins \nCompleted {milestone.noOfTimesCompleted} time{(milestone.noOfTimesCompleted != 1 ? "s" : "")} \nRepeatable - {(milestone.repeatable == true ? "Yes" : "No")}.";
    }

    private string GetTaskDescription(MilestoneData milestone)
    {
        switch (milestone.type)
        {
            case MilestoneData.Type.TimeBased:
                if (milestone.wordsToComplete > 0)
                {
                    return $"Complete {milestone.wordsToComplete} words of {milestone.wordLength} letters in {milestone.timeLimit} seconds";
                }
                return $"Place {milestone.cubesToPlace} Letter cubes in {milestone.timeLimit} seconds";

            case MilestoneData.Type.DamageBased:
                if (milestone.wordsToComplete > 0)
                {
                    return $"Complete {milestone.wordsToComplete} words of {milestone.wordLength} letters with max {milestone.maxDamageAllowed} damage";
                }
                return $"Place {milestone.cubesToPlace} Letter cubes with max {milestone.maxDamageAllowed} damage";

            case MilestoneData.Type.Mixed:
                if (milestone.wordsToComplete > 0)
                {
                    return $"Complete {milestone.wordsToComplete} words of {milestone.wordLength} letters in {milestone.timeLimit} seconds with max {milestone.maxDamageAllowed} damage";
                }
                return $"Place {milestone.cubesToPlace} Letter cubes in {milestone.timeLimit} seconds with max {milestone.maxDamageAllowed} damage";

            default:
                return $"Complete milestone #{milestone.id}";
        }
    }

    private void PrintMilestoneData(MilestoneData milestone)
    {
        if (milestone == null)
        {
            Debug.LogWarning("MilestoneData is null!");
            return;
        }

        Debug.Log($"Milestone #{milestone.id}");
        Debug.Log($"  Type: {milestone.type}");
        Debug.Log($"  Cubes to Place: {milestone.cubesToPlace}");
        Debug.Log($"  Time Limit: {milestone.timeLimit}");
        Debug.Log($"  Max Damage Allowed: {milestone.maxDamageAllowed}");
        Debug.Log($"  Words to Complete: {milestone.wordsToComplete}");
        Debug.Log($"  Repeatable: {milestone.repeatable}");
        Debug.Log($"  Reward Coins: {milestone.rewardCoins}");
        Debug.Log("  ---");
    }
}
