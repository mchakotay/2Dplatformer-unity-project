using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UI package namespace again.

public class Sign : MonoBehaviour
{
    //declare our variables, we have an image object and a text field we can customize from inside Unity
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private Text dialogText;

    public string dialog; //the actual text that will be in the dialog text
    public bool playerInRange; //a bool to check if player is in range

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Interact") && playerInRange)
        { //if the player is in range presses the interact button (e on keyboard, x on controller), then
            if(dialogBox.activeInHierarchy)
            { //if the player is already reading the sign, then make the dialog box disappear
                dialogBox.SetActive(false);
            }
            else
            { //if the player is not already reading the sign, then make the dialog box appear, and set the text = to the chosen string 
                dialogBox.SetActive(true);
                dialogText.text = dialog; //This seems redundant, unless we are just doing it for convenience of being able to edit it all in one object (maybe because its a prefab?)
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { //if the player enters range, set playerInRange to true
        if(collision.gameObject.name == "player1")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    { //if the player moves out of range, set playerInRange to false, make dialog box go away if it is showing
        if (collision.gameObject.name == "player1")
        {
            playerInRange = false;
            dialogBox.SetActive(false);
        }
    }
}
