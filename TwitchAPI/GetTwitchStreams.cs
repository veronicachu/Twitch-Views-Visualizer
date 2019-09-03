/* GetTwitchStreams
 * Request information about top 20 active streams for the requested game, sorted by highest 
 * current viewers first
 * API Reference: https://dev.twitch.tv/docs/api/reference/#get-streams
 */
using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class GetTwitchStreams : MonoBehaviour
{
    // variables to use in making API requests
    private string clientID;
    private const string url = "https://api.twitch.tv/helix/streams";

    // variables to determine frequency of API requests (in seconds)
    private float timer;
    public float refreshRate;   // set (in editor) seconds until next refresh

    // variable to store assigned game id
    private string gameID;

    // variable to store retrieved stream viewer counts
    public List<int> viewerCounts = new List<int>();

    #region Live calls made during runtime
    private void Start()
    {
        // get personal client ID string
        clientID = GameObject.Find("APIConnector").GetComponent<ConnectTwitchAPI>().clientID;

        // set timer at refreshRate to make first API request once program is opened
        timer = refreshRate;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // get the target game
        gameID = this.GetComponent<AssignGameName>().myGameId;

        // refresh viewer counts list for target game by the number of seconds specified 
        // in 'refreshRate' variable
        if (gameID != "" && timer > refreshRate)
        {
            // load JSON data containing top streams for target game
            GetStreamInfo jsonStreamInfo = GetStreamData(gameID);

            // parse JSON data
            GetViewerCounts(jsonStreamInfo);

            // reset timer for next request
            timer = 0;
        }
    }
    #endregion

    #region Method to make API request for top 20 (default number) streams for target game on Twitch currently
    private GetStreamInfo GetStreamData(string gameID)
    {
        // create web request form
        var request = WebRequest.Create(string.Format("{0}?game_id={1}", url, gameID));
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
        GetStreamInfo jsonData = JsonUtility.FromJson<GetStreamInfo>(data);

        // close streams
        dataReader.Close();
        dataStream.Close();
        response.Close();

        return jsonData;
    }
    #endregion

    #region Method to parse viewer counts for top streams of target game
    private List<int> GetViewerCounts(GetStreamInfo jsonData)
    {
        // make sure previous list is cleared to prevent duplicates in when refreshing
        viewerCounts.Clear();

        // loop through serialized JSON data for viewer counts and add to list
        for (int i = 0; i < jsonData.data.Count; i++)
        {
            viewerCounts.Add(jsonData.data[i].viewer_count);
        }

        Debug.Log("Update viewer counts");
        return viewerCounts;
    }
    #endregion

}

// Serialized objects formatted according to expected JSON in API reference
[Serializable]
public class GetStreamInfo
{
    public List<IndividualStreamInfo> data;
}

[Serializable]
public class IndividualStreamInfo
{
    public string user_name;
    public string type;
    public int viewer_count;
}
