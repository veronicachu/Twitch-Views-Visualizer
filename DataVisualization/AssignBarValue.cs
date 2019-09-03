/* AssignBarValue
 * Assign a viewer count value to visualize from the list of top 20 streams for the target game, 
 * starting from the highest current viewers first, until there are no more bar objects in the
 * target game's platform
 */
using System.Collections.Generic;
using UnityEngine;

public class AssignBarValue : MonoBehaviour
{
    // variable to reference gameobject containing GetTwitchStreams.cs
    private GameObject platformParentObject;

    // variable to reference GetTwitchStreams.cs
    private GetTwitchStreams twitchstreamsRef;

    // variables to determine frequency of viewer count updates (in seconds)
    private float timer;
    public float refreshRate;    // set (in editor) seconds until next refresh

    // variable to store assigned viewer count to visualize
    private int myViewerCount;

    #region Live calls made during runtime
    private void Start()
    {
        // access parent platform object that contains the list of viewer counts
        platformParentObject = transform.parent.parent.parent.gameObject;

        // assign a reference to GetTwitchStreams
        twitchstreamsRef = platformParentObject.GetComponent<GetTwitchStreams>();

        // set timer at refreshRate to make first viewer count visualizations once program is opened
        timer = refreshRate;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // refresh bar scale values according to new viewer count values by the number of seconds
        // specified in the 'refreshRate' variable
        if (timer > refreshRate)
        {
            // get new viewer count value
            GetBarValue();

            // scale bar object according to new viewer count value
            ScaleBar();

            // reset timer for next value change
            timer = 0f;

            Debug.Log("Update bar values");
        }
    }
    #endregion

    #region Method to assign a viewer count to the current bar object and remove from viewer count list
    private void GetBarValue()
    {
        // access list of viewer counts for target game from GetTwitchStreams.cs
        List<int> viewerCounts = twitchstreamsRef.viewerCounts;

        // check if there are any streams left in the list
        if (viewerCounts.Count > 0)
        {
            // take first viewer count value in list and remove from list to prevent duplicates
            myViewerCount = viewerCounts[0];
            viewerCounts.RemoveAt(0);
        }
    }
    #endregion

    #region Method to scale bar gameobject according to assigned viewer count value
    private void ScaleBar()
    {
        // scale the bar to visually represent the viewer count
        float scaleX = transform.localScale.x;
        float scaleY = myViewerCount / 1000f;
        float scaleZ = transform.localScale.z;
        transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

        // move the bar along the y-axis to make sure it stays on the abscissa
        float positionX = transform.localPosition.x;
        float positionY = myViewerCount / 1000f / 2f;
        float positionZ = transform.localPosition.z;
        transform.localPosition = new Vector3(positionX, positionY, positionZ);
    }
    #endregion
}
