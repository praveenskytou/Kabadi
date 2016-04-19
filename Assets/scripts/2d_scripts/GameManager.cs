using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instRef;

    //Game time vars
    public int raidTimeLimit = 30;
    public int roundIntervalTimeLimit = 2;
    public bool hasBaulkLineTouched;
    public bool hasBonusLineTouched;
    public bool hasCrossedEndLine;

    //Kabbadi Field vars

    public static float field_EndLineBack_Limit = 8.3f;
    public static float field_EndLineLeft_Limit = -6.3f;
    public static float field_EndLineRight_Limit = 6.3f;

    public static float field_MidLine_Limit = 2f;
    public static float field_LobbyLeft_Limit = -5f;
    public static float field_LobbyRight_Limit = 5f;

    //Timer
    private int currentRaidTimeCount;
    private float elapsedRoundInterval;
    private float elapsedRaidTime;
    public Text t_timer;

    //Game state vars
    public bool isRaidOver, isRoundStarted;

	// Use this for initialization
	void Start () {
        instRef = this;
        elapsedRaidTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {

        if (isRoundStarted)
        {
            raidTimeCheck();

        }
        else
        {
            //start the next round in N seconds
            if(Time.time - elapsedRoundInterval > roundIntervalTimeLimit)
            {

                //reset all vars
                currentRaidTimeCount = raidTimeLimit;

                isRoundStarted = true;
                isRaidOver = false;
                Debug.Log("Next round started");
            }
        }
    }

    void raidTimeCheck()
    {

        if (currentRaidTimeCount <= 0)
        {
            isRaidOver = true;
            isRoundStarted = false;
            elapsedRoundInterval = Time.time;
        }

        if (Time.time - elapsedRaidTime > 1)
        {
            currentRaidTimeCount--;
            elapsedRaidTime = Time.time;
        }

        if (currentRaidTimeCount < 0)
            currentRaidTimeCount = 0;

        t_timer.text = "Timer : " + currentRaidTimeCount;
    }


}
