using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestHandler : MonoBehaviour
{
    [SerializeField] int wordsCount;
    [SerializeField] int minNotUsedWordCount = 20;
    [SerializeField] int maxNotUsedWordCount = 100;

    [SerializeField] string lastWordCreatedAt;
    [SerializeField] string getListOfWordsURL = "http://127.0.0.1:8080/words/getListOfWords";
    DatabaseManager databaseManager;

    string jsonDataToBeSent;
    void Start()
    {
        // initialize database manager connection
        databaseManager = new DatabaseManager("wordsDatabase.db");
        // calling method to fetch words from server
        StartCoroutine(GetWordsFromServer());
    }

    public IEnumerator GetWordsFromServer()
    {
        // check if database has enough unused words
        if (!(databaseManager.GetCountOfUnusedWords() <= minNotUsedWordCount))
        {
            // Debug.Log("Enough unused words in database: " + databaseManager.GetCountOfUnusedWords() + ", " + minNotUsedWordCount);
            yield break;
        }

        // preparaing json data to be sent to server
        jsonDataToBeSent = $"{{\"wordsCount\": {wordsCount},\"lastWordCreatedAt\":\"{lastWordCreatedAt}\"}}";

        using (UnityWebRequest webRequest = new UnityWebRequest(getListOfWordsURL, "POST"))
        {
            // converting json data to raw
            byte[] jsonDataInRaw = System.Text.Encoding.UTF8.GetBytes(jsonDataToBeSent);
            // setting upload and download handlers
            webRequest.uploadHandler = new UploadHandlerRaw(jsonDataInRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            // setting request headers
            webRequest.SetRequestHeader("Content-type", "application/json");

            // code after this line is only executed once the web request is completed. 
            yield return webRequest.SendWebRequest();

            // checking if the request is completed successfully
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string responseJsonData = webRequest.downloadHandler.text;
                if (responseJsonData == null)
                {
                    Debug.Log("No words received from server");
                    yield break;
                }
                Debug.Log("Response: " + responseJsonData);
                // Deserialize list of words from json to List<Word> type 
                List<Word> words = JsonConvert.DeserializeObject<List<Word>>(responseJsonData);
                if (words != null)
                {
                    databaseManager.AddWordsToDatabase(words);
                }
                else
                {
                    Debug.Log("error while deserializing words from json");
                }
            }
            else
            {
                Debug.Log("Error: " + webRequest.error);
            }


        }
    }

}
