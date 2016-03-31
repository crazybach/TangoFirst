using UnityEngine;
using System.Collections;

public class UIEventHandler : MonoBehaviour {

    [HideInInspector]
    public GameManager manager;
    
	// Use this for initialization
	void Start () {
        manager = GetComponent<GameManager>();
        if (manager)
        {
            Debug.Log("GameManager Found");
        }
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    // Event handler for button click event
    public void onButtonClickEvent(GameObject obj){
        manager.CreateWorld();
    }
}
