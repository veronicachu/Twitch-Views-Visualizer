/* SetupBars
 * Setup bar gameobjects as a grid in relation to the platform objects previous set up, with 
 * width and length specified by inputs in the Unity Editor
 */
using UnityEngine;

public class SetupBars : MonoBehaviour
{
    void Awake()
    {
        MakeWaypoints();
        MakeBars();
    }

    #region Create waypoint locations for bar gameobjects in a grid structure within platform gameobject
    public int width;   // set (in editor) number of bars along the width
    public int length;  // set (in editor) number of bars along the length
    public float spacing;   //set (in editor) spacing between bars gameobjects

    void MakeWaypoints()
    {
        int newwidth = width + 1;   // adjust for origin (0,0,0)
        int newlength = length + 1;    // adjust for origin (0,0,0)
        float arrayWidth = (newwidth * spacing) / 2;    // used to calculate x location with adjustment for spacing
        float arrayLength = (newlength * spacing) / 2;    // used to calculate z location with adjustment for spacing

        // loop along number of objects for width
        for (int i = 1; i < newwidth; i++)
        {
            // loop along number of objects for length
            for (int j = 1; j < newlength; j++)
            {
                // setup new gameobject's properties
                GameObject waypoint = new GameObject("Waypoint" + i + j);   // assign gameobject name
                waypoint.tag = "Waypoint";  // assign gameobject tag
                waypoint.transform.parent = this.transform; // place gameobject under current gameobject in editor's gameobject heirarchy

                // calculate location (x,y,z) to place new gameobject
                float xCoord = (spacing * i) - arrayWidth + this.transform.position.x;
                float yCoord = this.transform.position.y;
                float zCoord = (spacing * j) - arrayLength + this.transform.position.z;
                waypoint.transform.position = new Vector3(xCoord, yCoord, zCoord);
            }
        }
    }
    #endregion

    #region Place bar gameobjects into each of the waypoint locations
    // variable to place (in editor) the desired bar gameobject to be copied into the waypoint locations
    public GameObject barObject;

    void MakeBars()
    {
        // get transform info for all children gameobjects (bar waypoints)
        Transform[] waypointLoc = GetComponentsInChildren<Transform>();

        // loop through each waypoint object to fill with the bar object
        // skip first index because extra object under parent object in current scene setup
        for (int i = 1; i < waypointLoc.Length; i++)
        {
            // instantiate a copy of the bar gameobject at waypoint location
            GameObject temp = Instantiate(barObject, waypointLoc[i]) as GameObject;
        }
    }
    #endregion
}

