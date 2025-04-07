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
    [SerializeField] string getFirstAddedWordDetails = "http://127.0.0.1:8080/words/getFirstAddedWordsDetails";
    [SerializeField] GameDataSave gameDataSave;
    DatabaseManager databaseManager;
    string jsonDataToBeSent;

    private delegate void OnCreatedAtUpdated();



    void Start()
    {
        // initialize database manager connection
        databaseManager = new DatabaseManager("wordsDatabase.db");
        databaseManager.DropTable();
        databaseManager.CreateTable();

        // update initial timestamps for words of length 3,4,5 then call GetWordsFromServer
        OnCreatedAtUpdated onCreatedAtUpdated = GetWordsFromServer;
        StartCoroutine(UpdateFirstCreatedAtTimestamps(onCreatedAtUpdated));

        // delete used words from database
        databaseManager.DeleteUsedWords();



        // calling method to fetch words from server
        // StartCoroutine(GetWordsFromServer());
        // GetWordsFromServer();
    }

    private IEnumerator UpdateFirstCreatedAtTimestamps(OnCreatedAtUpdated onCreatedAtUpdated)
    {

        using UnityWebRequest webRequest = new UnityWebRequest(getFirstAddedWordDetails, "POST");

        // setting download handler
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        // setting request headers
        webRequest.SetRequestHeader("Content-type", "application/json");
        // Send the web request
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            string responseJsonData = webRequest.downloadHandler.text;

            if (string.IsNullOrEmpty(responseJsonData))
            {
                Debug.Log("No words received from server");
                yield break; // Successful request but no data
            }
            else
            {
                // Debug.Log(responseJsonData);
                // object result;
                List<Word> words = JsonConvert.DeserializeObject<List<Word>>(responseJsonData);
                if (words.Count != 0)
                {
                    foreach (Word word in words)
                    {
                        // Debug.Log("word: " + word.Text + " createdAt: " + word.CreatedAt);
                        if (word.TextLength == 3)
                            gameDataSave.FirstWord3 = word;
                        else if (word.TextLength == 4)
                            gameDataSave.FirstWord4 = word;
                        else if (word.TextLength == 5)
                            gameDataSave.FirstWord5 = word;

                        // databaseManager.UpdateFirstCreatedAtTimestamp(word.wordLength, word.createdAt);
                    }
                    Debug.Log("First word of all lengths updated in GameDataSave");
                    onCreatedAtUpdated?.Invoke();

                }
                else
                {
                    Debug.LogError("Error while deserializing response");
                }
            }
        }
        else
        {
            Debug.LogError("WebReq Error: " + webRequest.error);
        }


    }

    public void GetWordsFromServer()
    {
        // check if database has enough unused words
        int[] unusedWordsCounts = databaseManager.GetCountOfUnusedWords();
        // bool hasEnoughUnusedWords = true;

        for (int i = 0; i < unusedWordsCounts.Length; i++)
        {
            if (unusedWordsCounts[i] <= minNotUsedWordCount)
            {
                Debug.Log("Fetching words from server for WordLength of " + (i + 3));
                jsonDataToBeSent = $"{{\"wordsCount\": {wordsCount},\"textLength\": {(i + 3)},\"createdAt\":\"{GetCorrectTimestamp((i + 3))}\"}}";
                StartCoroutine(SendWebRequestAndGetResponse(jsonDataToBeSent, getListOfWordsURL));
                // hasEnoughUnusedWords = false;
                // break;
            }
        }

        // if (hasEnoughUnusedWords)
        // {
        //     yield break;
        // }

        // preparaing json data to be sent to server
        // jsonDataToBeSent = $"{{\"wordsCount\": {wordsCount},\"TextLength\": {wordsCount},\"lastWordCreatedAt\":\"{lastWordCreatedAt}\"}}";
        // StartCoroutine(SendWebRequestAndGetResponse(jsonDataToBeSent, getListOfWordsURL));



        // using (UnityWebRequest webRequest = new UnityWebRequest(getListOfWordsURL, "POST"))
        // {
        //     // converting json data to raw
        //     byte[] jsonDataInRaw = System.Text.Encoding.UTF8.GetBytes(jsonDataToBeSent);
        //     // setting upload and download handlers
        //     webRequest.uploadHandler = new UploadHandlerRaw(jsonDataInRaw);
        //     webRequest.downloadHandler = new DownloadHandlerBuffer();
        //     // setting request headers
        //     webRequest.SetRequestHeader("Content-type", "application/json");

        //     // code after this line is only executed once the web request is completed. 
        //     yield return webRequest.SendWebRequest();

        //     // checking if the request is completed successfully
        //     if (webRequest.result == UnityWebRequest.Result.Success)
        //     {
        //         string responseJsonData = webRequest.downloadHandler.text;
        //         if (responseJsonData == null)
        //         {
        //             Debug.Log("No words received from server");
        //             yield break;
        //         }
        //         Debug.Log("Response: " + responseJsonData);
        //         // Deserialize list of words from json to List<Word> type 
        //         List<Word> words = JsonConvert.DeserializeObject<List<Word>>(responseJsonData);
        //         if (words != null)
        //         {
        //             databaseManager.AddWordsToDatabase(words);
        //         }
        //         else
        //         {
        //             Debug.Log("error while deserializing words from json");
        //         }
        //     }
        //     else
        //     {
        //         Debug.Log("Error: " + webRequest.error);
        //     }


        // }

    }
    private string GetCorrectTimestamp(int wordLength)
    {
        string timeStamp = databaseManager.GetLastRecordCreatedAt(wordLength);
        if (!string.IsNullOrEmpty(timeStamp))
            return timeStamp;
        else if (wordLength == 3)
            return gameDataSave.FirstWord3.CreatedAt;
        else if (wordLength == 4)
            return gameDataSave.FirstWord4.CreatedAt;
        return gameDataSave.FirstWord5.CreatedAt;
    }

    private IEnumerator SendWebRequestAndGetResponse(string jsonDataToBeSent, string url)
    {
        using UnityWebRequest webRequest = new UnityWebRequest(url, "POST");
        // Convert JSON data to raw bytes
        byte[] jsonDataInRaw = System.Text.Encoding.UTF8.GetBytes(jsonDataToBeSent);

        // Set upload and download handlers
        webRequest.uploadHandler = new UploadHandlerRaw(jsonDataInRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        // Set request headers
        webRequest.SetRequestHeader("Content-type", "application/json");

        // Send the web request
        yield return webRequest.SendWebRequest();

        // Check the result of the web request
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            string responseJsonData = webRequest.downloadHandler.text;

            if (string.IsNullOrEmpty(responseJsonData))
            {
                Debug.Log("No words received from server");
                yield break; // Successful request but no data
            }
            else
            {
                object result;
                List<Word> words = JsonConvert.DeserializeObject<List<Word>>(responseJsonData);
                result = words;
                if (result != null)
                {
                    Debug.Log("Adding words to database");
                    databaseManager.AddWordsToDatabase(words);
                    // yield return result;
                }
                else
                {
                    Debug.LogError("Error while deserializing response");
                }
            }
        }
        else
        {
            Debug.LogError("WebReq Error: " + webRequest.error);
            // yield return webRequest.error; // Return error message
        }
    }


}
