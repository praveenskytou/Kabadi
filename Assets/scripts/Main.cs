using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void load2DScene()
    {
        SceneManager.LoadScene("2dscene");
    }

    public void load3DScene()
    {
        SceneManager.LoadScene("3dscene");
    }
}
