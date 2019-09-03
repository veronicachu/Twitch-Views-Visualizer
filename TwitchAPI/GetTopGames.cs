/* GetTopGames
 * Request the current top 20 games on Twitch, sorted by highest current viewers first
 * Store the top 20 games' ids and names in local variables for use
 * API Reference: https://dev.twitch.tv/docs/api/reference/#get-games
 */
using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GetTopGames : MonoBehaviour
{
    // variables to use in making API requests
    private string clientID;
    private const string url = "https://api.twitch.tv/helix/games/top";
    
    // variables to determine frequency of API requests (in seconds)
    private float timer;
    public float refreshRate;    // set (in editor) seconds until next refresh

    // variables to store retrieved top games' ids and names for use
    [HideInInspector]
    public List<string> gameIds = new List<string>();
    [HideInInspector]
    public List<string> gameNames = new List<string>();
    public Dictionary<string, string> gameDictionary = new Dictionary<string, string>();

    // **setup event system in future**
    // unity event that signals refreshed top games data
    //public UnityEvent topGamesRefreshed = new UnityEvent();
    
    #region Live calls made during runtime
    private void Start()
    {
        // get personal client ID string from ConnectTwitchAPI.cs
        clientID = this.GetComponent<ConnectTwitchAPI>().clientID;

        // make an API request when first open program
        LoadTopGames();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        
        // make new API request for top game data when hit 'spacebar'
        // request rate limited according to setting for 'refreshRate' variable
        if (Input.GetKeyDown(KeyCode.Space) && timer > refreshRate)
        {
            // make an API request
            LoadTopGames();

            // reset timer for next request
            timer = 0;
        }
    }
    #endregion

    #region Method to make API request for top 20 (default number) games on Twitch currently
    private TopGamesInfo GetTopGamesData()
    {
        // create web request form
        var request = WebRequest.Create(url);
        request.Method = "Get";
        request.Timeout = 12000;
        request.Headers.Add("Client-ID", clientID);

        // get response from API
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        // get data stream from API
        Stream dataStream = response.GetResponseStream();

        // open data stream
        StreamReader dataReader = new StreamReader(dataStream);

        // read data stream
        string data = dataReader.ReadToEnd();
        //Debug.Log(data);

        // transform JSON into a serialized object
        TopGamesInfo jsonData = JsonUtility.FromJson<TopGamesInfo>(data);
        //Debug.Log(jsonData.data[0].name);

        // close streams
        dataReader.Close();
        dataStream.Close();
        response.Close();

        return jsonData;
    }
    #endregion

    #region Method to parse top game IDs from JSON
    private List<string> GetGameIDs(TopGamesInfo jsonData)
    {
        // make sure previous list is cleared to prevent duplicates when refreshing
        gameIds.Clear(); 

        // loop through serialized JSON data for game IDs and add to list
        for (int i = 0; i < jsonData.data.Count; i++)
        {
            gameIds.Add(jsonData.data[i].id);
        }

        return gameIds;
    }
    #endregion

    #region Method to parse top game names from JSON
    private List<string> GetGameNames(TopGamesInfo jsonData)
    {
        // make sure previous list is cleared to prevent duplicates when refreshing
        gameNames.Clear();

        // loop through serialized JSON data for game names and add to list
        for (int i = 0; i < jsonData.data.Count; i++)
        {
            gameNames.Add(jsonData.data[i].name);
        }

        return gameNames;
    }
    #endregion

    #region Method to create dictionary to refer to game by game ID
    private Dictionary<string, string> GetGameDictionary(TopGamesInfo jsonData)
    {
        // make sure previous dictionary is cleared to prevent duplicates when refreshing
        gameDictionary.Clear();
        gameDictionary.Add("0", "Null"); // add entry for null (for use in future builds)

        // loop through serialized JSON data for game ids and names and add to dictionary
        for (int i = 0; i < jsonData.data.Count; i++)
        {
            gameDictionary.Add(jsonData.data[i].id, jsonData.data[i].name);
        }

        return gameDictionary;
    }
    #endregion

    #region Method to load and parse serialized JSON data of current top games on Twitch currently
    private void LoadTopGames()
    {
        // load serialized JSON data
        TopGamesInfo jsonTopGames = GetTopGamesData();

        // method calls to parse serialized JSON data
        GetGameIDs(jsonTopGames);
        GetGameNames(jsonTopGames);
        GetGameDictionary(jsonTopGames);

        // **setup event system in future**
        // send out event signal that top games have been refreshed
        //topGamesRefreshed.Invoke();
        
        Debug.Log("Update top games");
    }
    #endregion
}

// Serialized objects formatted according to expected JSON in API reference
[Serializable]
public class TopGamesInfo
{
    public List<IndividualGameInfo> data;
}

[Serializable]
public class IndividualGameInfo
{
    public string id;
    public string name;
}