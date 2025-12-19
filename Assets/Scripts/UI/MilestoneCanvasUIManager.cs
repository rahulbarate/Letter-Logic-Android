using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MilestoneCanvasUIManager : MonoBehaviour
{

    [SerializeField] private GameObject alphabetMilestonesScrollContent;
    [SerializeField] private TextMeshProUGUI alphabetMilestoneButtonImage;
    [SerializeField] private GameObject numbersMilestonesScrollContent;
    [SerializeField] private TextMeshProUGUI numbersMilestoneButtonImage;
    [SerializeField] private GameObject wordsMilestonesScrollContent;
    [SerializeField] private TextMeshProUGUI wordsMilestoneButtonImage;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color defaultColor;
    public void ToggleMilestoneContent(int no)
    {
        alphabetMilestonesScrollContent.SetActive(false);
        alphabetMilestoneButtonImage.color = defaultColor;
        numbersMilestonesScrollContent.SetActive(false);
        numbersMilestoneButtonImage.color = defaultColor;
        wordsMilestonesScrollContent.SetActive(false);
        wordsMilestoneButtonImage.color = defaultColor;
        if (no == 1)
        {
            alphabetMilestonesScrollContent.SetActive(true);
            scrollRect.content = alphabetMilestonesScrollContent.GetComponent<RectTransform>();
            alphabetMilestoneButtonImage.color = selectedColor;

        }

        else if (no == 2)
        {
            numbersMilestonesScrollContent.SetActive(true);
            scrollRect.content = numbersMilestonesScrollContent.GetComponent<RectTransform>();
            numbersMilestoneButtonImage.color = selectedColor;

        }
        else if (no == 3)
        {
            wordsMilestonesScrollContent.SetActive(true);
            scrollRect.content = wordsMilestonesScrollContent.GetComponent<RectTransform>();
            wordsMilestoneButtonImage.color = selectedColor;


        }

    }
}
