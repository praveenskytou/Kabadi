using UnityEngine;
using System.Collections;


public enum AIStates
{
    idle, 
    attack,
    defence
}
public class AIBehaviour : MonoBehaviour
{

    public AIBehaviour instance = null;

    public GameObject playerRef;
    public AIStates aiStates;

    void Awake()
    {
        instance = this;
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }
	// Use this for initialization
	void Start ()
    {
	
	}
	

    void Idle()
    {

    }

    void Attack()
    {

    }
	// Update is called once per frame
	void Update ()
    {
	
        switch(aiStates)
        {
            case AIStates.idle:

                Idle();
                break;

            case AIStates.attack:

                Attack();

                break;
        }
	}
}
