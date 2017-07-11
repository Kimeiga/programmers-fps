using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextInput : MonoBehaviour {

    private MouseLook mouseLookScript;

    public InputField textInput;
            
    public bool communicating = false;

    public CommunicationManager comManScript;

	// Use this for initialization
	void Start () {
        
        
        mouseLookScript = GetComponent<MouseLook>();
        mouseLookScript.canRotate = true;

	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!communicating)
            {
                communicating = true;
                mouseLookScript.canRotate = false;
                textInput.ActivateInputField();
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (communicating)
            {
                if(textInput.text != "")
                {
                    
                    comManScript.SendMessage("ReceiveTextMessage", textInput.text);

                    textInput.text = "";
                    textInput.DeactivateInputField();
                    communicating = false;
                    mouseLookScript.canRotate = true;
                }
                else
                {
                    textInput.DeactivateInputField();
                    communicating = false;
                    mouseLookScript.canRotate = true;
                }
                
            }
        }
        

	}



}
