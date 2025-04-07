using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;

public class DatabaseManager
{
    // [SerializeField] GameDataSave gameDataSave;
    private SQLiteConnection _connection;

    public DatabaseManager(string DatabaseName)
    {

#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
		
#elif UNITY_STANDALONE_OSX
		var loadDb = Application.dataPath + "/Resources/Data/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

    }

    public void AddWordsToDatabase(List<Word> words)
    {
        try
        {
            // drop table and create table will be removed later
            // _connection.DropTable<Word>();
            // _connection.CreateTable<Word>();
            _connection.InsertAll(words);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public void DropTable()
    {
        try
        {
            _connection.DropTable<Word>();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public void CreateTable()
    {
        try
        {
            _connection.CreateTable<Word>();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public int[] GetCountOfUnusedWords()
    {
        int[] unusedWordCount = new int[3];
        try
        {
            // Count unused words with TextLength of 3, 4, and 5
            unusedWordCount[0] = _connection.Table<Word>().Where(x => x.IsUsed == 0 && x.TextLength == 3).Count();
            unusedWordCount[1] = _connection.Table<Word>().Where(x => x.IsUsed == 0 && x.TextLength == 4).Count();
            unusedWordCount[2] = _connection.Table<Word>().Where(x => x.IsUsed == 0 && x.TextLength == 5).Count();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error fetching unused word counts: {e.Message}");
        }
        return unusedWordCount;
    }
    public string GetLastRecordCreatedAt(int textLength)
    {
        try
        {
            // Query the database for the last record with the given TextLength
            Word lastRecord = _connection.Table<Word>()
                .Where(x => x.TextLength == textLength)
                .OrderByDescending(x => x.CreatedAt) // Sorting works because the format is consistent
                .FirstOrDefault();

            // Return the CreatedAt property if a record is found
            if (lastRecord != null)
            {
                return lastRecord.CreatedAt;
            }
            else
            {
                // Debug.LogWarning($"No records found in the database for TextLength: {textLength}");
                return null;
            }

        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error fetching last record for TextLength {textLength}: {e.Message}");
            return null;
        }
    }

    public void DeleteUsedWords()
    {
        try
        {
            // Delete all words that are used (IsUsed = 1)
            _connection.Execute("DELETE FROM Word WHERE IsUsed = 1");
            // Debug.Log("Deleted used words from the database.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error deleting used words: {e.Message}");
        }
    }

}
