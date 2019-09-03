/* SetupPlatform
 * Setup platform gameobjects as a grid in relation to origin (0,0,0) with width and length
 * specified by inputs in the the Unity Editor
 */
using UnityEngine;

public class SetupPlatform : MonoBehaviour
{
    void Awake()
    {
        MakeWaypoints();
        MakePlatforms();
    }

    #region Create waypoint locations for platforms gameobjects in a grid structure
    public int width;   // set (in editor) number of bars along the width
    public int length;  // set (in editor) number of bars along the length
    public float spacing;   //set (in editor) spacing between platform gameobjects
    void MakeWaypoints()
    {
        int newwidth = width + 1;   // adjust for origin (0,0,0)
        int newlength = length + 1; // adjust for origin (0,0,0)
        float arrayWidth = (newwidth * spacing) / 2;    // used to calculate x location with adjustment for spacing
        float arrayLength = (newlength * spacing) / 2;  // used calculate z location with adjustment for spacing

        // loop along number of objects for width
        for (int i = 1; i < newwidth; i++)
        {
            // loop along number of objects for length
            for (int j = 1; j < newlength; j++)
            {
                // setup new gameobject's properties
                GameObject waypoint = new GameObject("Waypoint" + j + i);   // assign gameobject name
                waypoint.tag = "PlatformWaypoint";  // assign gameobject tag
                waypoint.transform.parent = this.transform; // place gameobject under current gameobject in editor's gameobject heirarchy

                // calculate location (x,y,z) to place new gameobject
                float xCoord = (spacing * j) - arrayWidth;
                float yCoord = this.transform.position.y + 0.5f;
                float zCoord = (spacing * i) - arrayLength + this.transform.position.z;
                waypoint.transform.position = new Vector3(xCoord, yCoord, zCoord);
            }
        }
    }
    #endregion

    #region Place platform gameobjects into each of the waypoint locations
    // variable to place (in editor) the desired platform gameobject to be copied into the waypoint locations
    public GameObject platformObject;

    void MakePlatforms()
    {
        // get all gameobjects with "PlatformWaypoint" tag
        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag("PlatformWaypoint");

        // loop through each waypoint object to fill with the platform object
        for (int i = 0; i < waypointObjects.Length; i++)
        {
            // get waypoint's transform info (x,y,z coordinates)
            Transform waypointLoc = waypointObjects[i].transform;

            // instantiate a copy of the platform gameobject at waypoint location
            GameObject temp = Instantiate(platformObject, waypointLoc) as GameObject;
            temp.tag = "Platform";
        }
    }
    #endregion
}

