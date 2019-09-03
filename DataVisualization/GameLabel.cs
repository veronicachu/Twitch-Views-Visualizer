/* GameLabel
 * Display game title above the respective platform gameobject
 */
using UnityEngine;
using UnityEngine.UI;

public class GameLabel : MonoBehaviour
{
    private GameObject platformParentObject;
    private string myGameName;

    void Start()
    {
        // access respective parent platform object
        platformParentObject = transform.parent.parent.gameObject;

        // adjust location of text to be above parent object
        float xCoord = platformParentObject.transform.position.x;
        float yCoord = platformParentObject.transform.position.y + 40f;
        float zCoord = platformParentObject.transform.position.z;
        this.transform.position = new Vector3(xCoord, yCoord, zCoord);

        // access game name variable
        myGameName = platformParentObject.GetComponent<AssignGameName>().myGameName;
    }
    
    void Update()
    {
        // change text to the platform's game name
        this.GetComponent<Text>().text = myGameName;
    }
}
