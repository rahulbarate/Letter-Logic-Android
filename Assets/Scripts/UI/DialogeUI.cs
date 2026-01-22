using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class DialogeUI : MonoBehaviour
{
    [SerializeField] GameObject dialogePanel;
    [SerializeField] TextMeshProUGUI heading;
    [SerializeField] TextMeshProUGUI body;
    [SerializeField] Button trueButton;
    [SerializeField] Button falseButton;

    [SerializeField] GameObject pauseMenu;
    TextMeshProUGUI trueButtonText;
    TextMeshProUGUI falseButtonText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (trueButton)
            trueButtonText = trueButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (falseButton)
            falseButtonText = falseButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowDialoge(string heading, string trueButtonText, string falseButtonText, UnityEngine.Events.UnityAction trueButtonAction, string body = null, UnityEngine.Events.UnityAction falseButtonAction = null)
    {
        // CustomLogger.Log("In Here");
        trueButton.onClick.RemoveAllListeners();
        falseButton.onClick.RemoveAllListeners();
        dialogePanel.SetActive(false);
        this.heading.text = heading;
        this.body.text = body ?? "";
        this.trueButtonText.text = trueButtonText;
        this.falseButtonText.text = falseButtonText;
        trueButton.onClick.AddListener(trueButtonAction);
        if (falseButtonAction == null)
            falseButton.onClick.AddListener(HideDialoge);
        else
            falseButton.onClick.AddListener(falseButtonAction);
        dialogePanel.SetActive(true);
    }
    public void HideDialoge()
    {
        if (dialogePanel.activeSelf == true)
        {
            if (pauseMenu != null && !pauseMenu.activeSelf)
                Time.timeScale = 1f;
            dialogePanel.SetActive(false);
        }
    }
}
