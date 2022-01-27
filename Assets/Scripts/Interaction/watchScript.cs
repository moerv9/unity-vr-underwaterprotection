using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

using UnityEngine.UI;

/***************************************************************************************************************************
 *This script provides the functionality to update the watche's score when a garbage object is collected aswell as it sets 
 *the initial value for the watch.
 ***************************************************************************************************************************/

public class watchScript : MonoBehaviour
{

    public Text thisText;
    public InputAction switchWatch;
    private int currentView;
    private static string OHIText ;
    private static string piecesText;
    private static bool incomingMessage = false;
    private MeshRenderer meshRenderer = null;
    private XRGrabInteractable grabInteractable = null;
    private Slider progressbarSlider;
    private GameObject progressbar;


    public GameObject SceneManager;

    private void OnEnable()
    {
        switchWatch.Enable();
    }

    private void Awake()
    {
        switchWatch.performed += switchWatchView;
    }

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        progressbar = GameObject.Find("Progressbar");
        progressbarSlider = progressbar.GetComponent<Slider>();
        piecesText = "Pieces: " + 0;
        OHIText = "OHI: " + 0;
        thisText.text = piecesText;
        currentView = 0;
        progressbarSlider.fillRect.GetComponent<Image>().color = new Color(0,255,0);
    }

    /***************************************************************************************************************************
    *Updates score when selected interactable is tagged as "Garbage".
    ***************************************************************************************************************************/
    public void updateScore(SelectEnterEventArgs selectEnterEventArgs)
    {
        if (selectEnterEventArgs.interactable.tag == "Garbage")
        {
            SceneManager.GetComponent<SceneManagerScript>().setOHIIndex(SceneManager.GetComponent<SceneManagerScript>().getOHIIndex() + 0.25f);
            SceneManager.GetComponent<SceneManagerScript>().setAmountGarbage(SceneManager.GetComponent<SceneManagerScript>().getAmountGarbage() + 1);
            OHIText = "OHI: " + SceneManager.GetComponent<SceneManagerScript>().getOHIIndex();
            piecesText = "Pieces: " + SceneManager.GetComponent<SceneManagerScript>().getAmountGarbage();
            updateWatch();
        };
    }

    private void switchWatchView(InputAction.CallbackContext ctx)
    {
        if (currentView == 1)
        {
            currentView = 0;

        }
        else if (currentView == 0)
        {
            currentView = 1;
        }
        updateWatch();
        Debug.Log("Watch View changed");
    }

    public void updateWatch()
    {
         if (incomingMessage == false)
        {
            if (currentView == 1)
            {
                thisText.text = OHIText;
                progressbarSlider. maxValue = 100;
                progressbarSlider.value = SceneManager.GetComponent<SceneManagerScript>().getOHIIndex();

            }
            else if (currentView == 0)
            {
                thisText.text = piecesText;
                progressbarSlider.maxValue = 150;
                progressbarSlider.value = SceneManager.GetComponent<SceneManagerScript>().getAmountGarbage();

            }
        }
    }

    public void setIncomingMessage(bool value)
    {
        incomingMessage = value;
        if (value == true)
        {
            thisText.text = "New message!";
            thisText.fontSize = 11;
            thisText.transform.localPosition = new Vector3(-0.0033f, thisText.transform.localPosition.y, thisText.transform.localPosition.z);
            progressbar.SetActive(false);

        }
        if (value == false)
        {
            thisText.fontSize = 14;
            updateWatch();
            thisText.transform.localPosition = new Vector3(0.005f, thisText.transform.localPosition.y, thisText.transform.localPosition.z);
            progressbar.SetActive(true);
        }
    }

    public void stillPlaying()
    {
        thisText.text = "Audio still playing!";
        Invoke("resetText", 1);
    }

    public void resetText()
    {
        if(thisText.text == "Audio still playing!")
        {
            thisText.text = "New message! ";
        }
    }

}
