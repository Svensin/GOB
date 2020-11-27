using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// BEGIN 2d_mainmenu
using UnityEngine.SceneManagement;

// Manages the main menu.
public class MainMenu : MonoBehaviour {

	public GameObject mainMenu;
	public GameObject settingsMenu;
	public GameObject chooseLevelMenu;
	public GameObject exitWarningMenu;

	public GameObject musicButton;
	public Sprite musicButtonOnSprite;
	public Sprite musicButtonOffSprite;

	public GameObject musicIcon;
	public Sprite musicIconOnSprite;
	public Sprite musicIconOffSprite;

	public GameObject adsButton;
	public Sprite adsButtonOnSprite;
	public Sprite adsButtonOffSprite;

	public GameObject adsIcon;
	public Sprite adsIconOnSprite;
	public Sprite adsIconOffSprite;


	GameObject currentMenu;

	bool isMusicOn = true;
	bool isAdsOn = true;

	Image musicButtonImage;
	Image musicIconImage;
	Image adsButtonImage;
	Image adsIconImage;

	public void Start() {
		currentMenu = mainMenu;
		currentMenu.SetActive(true);
		exitWarningMenu.SetActive(false);
		settingsMenu.SetActive(false);
		chooseLevelMenu.SetActive(false);

	musicButtonImage = musicButton.GetComponent<Image>();
		musicIconImage = musicIcon.GetComponent<Image>();
		adsButtonImage = adsButton.GetComponent<Image>();
		adsIconImage = adsIcon.GetComponent<Image>();
	}

	private void Update()
	{
		if (mainMenu.activeInHierarchy == true && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Menu)))
		{
			ActivateExitWarningMenu();
		}
	}

    public void BackToMainMenu()
    {
		currentMenu.SetActive(false);
		mainMenu.SetActive(true);
		currentMenu = mainMenu;
    }

	public void ToSettingsMenu()
	{
		currentMenu.SetActive(false);
		settingsMenu.SetActive(true);
		currentMenu = settingsMenu;
	}

	public void ToChooseLevelMenu()
	{
		currentMenu.SetActive(false);
		chooseLevelMenu.SetActive(true);
		currentMenu = chooseLevelMenu;
	}

	public void ChangeMusicState()
    {
		isMusicOn = !isMusicOn;

		if (isMusicOn == true)
        {
			musicButtonImage.sprite = musicButtonOnSprite;
			musicIconImage.sprite = musicIconOnSprite;
        }
        else
        {
			musicButtonImage.sprite = musicButtonOffSprite;
			musicIconImage.sprite = musicIconOffSprite;
		}
    }

	public void ChangeAdsState()
	{
		isAdsOn = !isAdsOn;

		if (isAdsOn == true)
		{
			adsButtonImage.sprite = adsButtonOnSprite;
			adsIconImage.sprite = adsIconOnSprite;
		}
		else
		{
			adsButtonImage.sprite = adsButtonOffSprite;
			adsIconImage.sprite = adsIconOffSprite;
		}
	}

	public void LoadLevel(int id)
    {
		SceneManager.LoadScene(id);
    }


	public void ActivateExitWarningMenu()
    {
		exitWarningMenu.SetActive(true);
    }

	public void DisactivateExitWarningMenu()
    {
		exitWarningMenu.SetActive(false);
	}

	public void QuitApplication()
    {
		Application.Quit();
    }
}
// END 2d_mainmenu
