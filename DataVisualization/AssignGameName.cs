/* AssignGameName
 * Assign a game from the list of top 20 games to each platform game object, starting from 
 * the highest current viewers first, until there are no more platform game objects in the
 * scene
 */
using System.Collections.Generic;
using UnityEngine;

public class AssignGameName : MonoBehaviour
{
    // variable to reference GetTopGames.cs
    private GetTopGames topgamesRef;

    // variables to store assigned game id and name
    public string myGameId;
    public string myGameName;

    #region Live calls made during runtime
    private void Start()
    {
        // assign a reference to GetTopGames
        topgamesRef = GameObject.Find("APIConnector").GetComponent<GetTopGames>();
        
        GetGameAssignment();
        GetGameName();

        // **setup event system in future**
        //topgamesRef.topGamesRefreshed.AddListener(GetGameAssignment);
        //topgamesRef.topGamesRefreshed.AddListener(GetGameName);
    }

    private void Update()
    {
        // refresh game assignments for refreshed top games list by hitting "spacebar"
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetGameAssignment();
            GetGameName();
        }
    }
    #endregion

    #region Method to assign a game to the current platform and remove from game list
    private void GetGameAssignment()
    {
        // access list of game ids from GetTopGames.cs
        List<string> gameIds = topgamesRef.gameIds;

        // check if there are any more games in the list
        if (gameIds.Count > 0)
        {
            // take first game id in list and remove from list to prevent duplicates
            myGameId = gameIds[0];
            gameIds.RemoveAt(0);
        }

        Debug.Log("Game assigned");
    }
    #endregion

    #region Method to use assigned game id to get game name from dictionary
    private void GetGameName()
    {
        Dictionary<string, string> gameDictionary = topgamesRef.gameDictionary;
        myGameName = gameDictionary[myGameId];
    }
    #endregion
}
