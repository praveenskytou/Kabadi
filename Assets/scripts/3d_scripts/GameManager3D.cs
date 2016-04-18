using UnityEngine;
using System.Collections;

public class GameManager3D : MonoBehaviour {

    public GameManager3D instRef;

    //Game time vars
    public float raidTimeLimit=30;
    public bool hasBaulkLineTouched;
    public bool hasBonusLineTouched;
    public bool hasCrossedEndLine;

    //Kabbadi Field vars

    public static float field_EndLineBack_Limit = 7.6f;
    public static float field_EndLineLeft_Limit = -5.8f;
    public static float field_EndLineRight_Limit = 6f;

    public static float field_MidLine_Limit = 1f;
    public static float field_LobbyLeft_Limit = -4.6f;
    public static float field_LobbyRight_Limit =  4.6f;

	// Use this for initialization
	void Start () {
        instRef = this;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

}
