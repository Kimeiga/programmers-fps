using UnityEngine;
using System.Collections;

public class TextBrain : MonoBehaviour {
    
    public Transform head;

    public GameObject[] players;

    public CommunicationManager comManScript;

	// Use this for initialization
	void Start () {

        players = comManScript.players;

	}
	
	// Update is called once per frame
	void Update () {
	

	}
    

    public void TakeCommand(Word verb,Word object1,Word object2)
    {
        if(verb.actualWord == "lo")
        {

            foreach(GameObject player in players)
            {
                if(player.name == object1.actualWord)
                {

                    Transform lookAtTransform = player.transform;
                    transform.LookAt(lookAtTransform);
                }
            }
        }
    }

}
