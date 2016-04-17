using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameManager instRef;

    //Game time vars
    public float raidTimeLimit=30;
    public bool hasBaulkLineTouched;
    public bool hasBonusLineTouched;
    public bool hasCrossedEndLine;

    //Kabbadi Field vars

    public static float field_EndLineBack_Limit = 8.3f;
    public static float field_EndLineLeft_Limit = -6.3f;
    public static float field_EndLineRight_Limit = 6.3f;

    public static float field_MidLine_Limit = 1.5f;
    public static float field_LobbyLeft_Limit = -5f;
    public static float field_LobbyRight_Limit = 5f;

	// Use this for initialization
	void Start () {
        instRef = this;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

}
