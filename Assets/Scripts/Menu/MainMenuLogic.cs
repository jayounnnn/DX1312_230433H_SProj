using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLogic : MonoBehaviour
{
    [Header("Menu Canvases")]
    [SerializeField] private Canvas mainMenu;
    [SerializeField] private Canvas settingsMenu;
    [SerializeField] private Canvas creditsMenu;

    public GameObject controls;
    public GameObject graphics;
    public GameObject Audio;
    public AudioSource buttonSound;

    void Start()
    {
        // Grab references to each Canvas by name
        //mainMenu = GameObject.Find("MainMenuCanvas");
        //settingsMenu = GameObject.Find("SettingsCanvas");
        //creditsMenu = GameObject.Find("CreditsCanvas");

        // Show Main Menu, hide Settings and Credits
        mainMenu.GetComponent<Canvas>().enabled = true;
        settingsMenu.GetComponent<Canvas>().enabled = false;
        creditsMenu.GetComponent<Canvas>().enabled = false;

        // Hide the sub-panels in Settings
        controls.SetActive(false);
        graphics.SetActive(false);
        Audio.SetActive(false);
    }

    // PLAY
    public void StartButton()
    {
        buttonSound.Play();
        // Directly load the game scene
        SceneManager.LoadScene("MainBase");
    }

    // SETTINGS
    public void SettingsButton()
    {
        buttonSound.Play();
        mainMenu.GetComponent<Canvas>().enabled = false;
        settingsMenu.GetComponent<Canvas>().enabled = true;
        creditsMenu.GetComponent<Canvas>().enabled = false;

        // Default sub-panel (e.g., Audio) or hide them all
        controls.SetActive(false);
        graphics.SetActive(false);
        Audio.SetActive(true); // example: show Audio by default
    }

    public void ControlsButton()
    {
        buttonSound.Play();
        controls.SetActive(true);
        graphics.SetActive(false);
        Audio.SetActive(false);
    }

    public void GraphicsButton()
    {
        buttonSound.Play();
        controls.SetActive(false);
        graphics.SetActive(true);
        Audio.SetActive(false);
    }

    public void AudioButton()
    {
        buttonSound.Play();
        controls.SetActive(false);
        graphics.SetActive(false);
        Audio.SetActive(true);
    }

    // CREDITS
    public void CreditsButton()
    {
        buttonSound.Play();
        mainMenu.GetComponent<Canvas>().enabled = false;
        settingsMenu.GetComponent<Canvas>().enabled = false;
        creditsMenu.GetComponent<Canvas>().enabled = true;
    }

    // EXIT
    public void ExitGameButton()
    {
        buttonSound.Play();
        Application.Quit();
        Debug.Log("App Has Exited");
    }

    // RETURN TO MAIN MENU
    public void ReturnToMainMenuButton()
    {
        buttonSound.Play();
        mainMenu.GetComponent<Canvas>().enabled = true;
        settingsMenu.GetComponent<Canvas>().enabled = false;
        creditsMenu.GetComponent<Canvas>().enabled = false;
    }

    void Update()
    {
        // Empty unless you need real-time menu logic
    }
}
