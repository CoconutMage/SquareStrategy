using UnityEngine;
using TMPro;

public class MainMenus : MonoBehaviour
{
	public RectTransform mainMenu;
	public RectTransform multiplayerMenu;
	public RectTransform multiplayerJoinGameLobbyMenu;
	public RectTransform multiplayerJoinGameLobbyMenuCreateCountryMenu;

	public TMP_Text countryName;

	Data data;

	void Start()
	{
		data = Data.Instance;
	}

	public void MainMenuSinglePlayerButton()
	{
		//Lmao imagine having AI
	}

	public void MainMenuMultiplayerButton()
	{
		mainMenu.gameObject.SetActive(false);
		multiplayerMenu.gameObject.SetActive(true);
	}

	public void MainMenuOptionsButton()
	{
		mainMenu.gameObject.SetActive(false);
	}

	public void MultiplayerMenuReturnToMainMenuButton()
	{
		multiplayerMenu.gameObject.SetActive(false);
		mainMenu.gameObject.SetActive(true);
	}

	public void MultiplayerMenuDebugJoinGameButton()
	{
		multiplayerMenu.gameObject.SetActive(false);
		multiplayerJoinGameLobbyMenu.gameObject.SetActive(true);
	}

	public void MultiplayerJoinGameLobbyMenuReturnToMultiplayerMenuButton()
	{
		multiplayerJoinGameLobbyMenu.gameObject.SetActive(false);
		multiplayerMenu.gameObject.SetActive(true);
	}

	public void MultiplayerJoinGameLobbyMenuCreateCountryButton()
	{
		multiplayerJoinGameLobbyMenu.gameObject.SetActive(false);
		multiplayerJoinGameLobbyMenuCreateCountryMenu.gameObject.SetActive(true);
	}

	public void MultiplayerJoinGameLobbyMenuCreateCountryMenuReturnToJoinGameLobbyMenuButton()
	{
		multiplayerJoinGameLobbyMenuCreateCountryMenu.gameObject.SetActive(false);
		multiplayerJoinGameLobbyMenu.gameObject.SetActive(true);
	}

	public void FinalizeCountryCreationButton()
	{
		if (CountryCreationValuesNullChecker())
		{
			Debug.Log(countryName.text);
			//data.CreatePlayerCountry(countryName.text);
		}
	}

	public bool CountryCreationValuesNullChecker()
	{
		//this if statement doesnt work, probably gotta make sure it contains characters rather than compare to null strings
		if (countryName.text == null || countryName.text == "" || countryName.text == " ")
		{
			Debug.Log("FALSE");
			return false;
		}
		return true;
	}
}
