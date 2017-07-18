using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class MainMenuController : MonoBehaviour
{
    [Header("Load Game")]
    public Dropdown GameDropdown;
    public Button PlayButton;
    public Button EditButton;

    [Header("Create New Game")]
    public InputField NewGameInput;
    public Button CreateNewButton;

    [Header("Other")]
    public Text ErrorMessage;

    private List<string> _existingGames;

    private void Awake()
    {
        _existingGames = new List<string>();

        //Populate Directories Path
        string path = GameInfo.GamesPath;
        List<string> directories = Directory.GetDirectories(path).ToList();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        for (int i = 0; i < directories.Count; i++)
        {
            string option = Path.GetFileName(directories[i]);
            _existingGames.Add(option);
            if (option != "Unity")
            {
                options.Add(new Dropdown.OptionData(Path.GetFileName(option)));
            }
        }
        GameDropdown.AddOptions(options);

        //If there are games, enable play and edit buttons
        if (options.Count > 0)
        {
            PlayButton.interactable = true;
            EditButton.interactable = true;
        }      
    }

    public void PlayButtonClicked()
    {
        GameInfo.SetCurrentGame(GameDropdown.options[GameDropdown.value].text);
        SceneManager.LoadScene("Game");
    }

    public void EditButtonClicked()
    {
        GameInfo.SetCurrentGame(GameDropdown.options[GameDropdown.value].text);
        SceneManager.LoadScene("CartridgeCreator");
    }
    
    public void CreateNewGame()
    {
        string gameName = NewGameInput.text;

        //TODO: Much better string validation than this
        gameName = gameName.Replace(@"\", string.Empty);
        gameName = gameName.Replace(@"/", string.Empty);

        if (!string.IsNullOrEmpty(gameName) && gameName != "Unity")
        {
            if (!_existingGames.Contains(gameName))
            {
                GameInfo.SetCurrentGame(gameName);
                Helpers.CreateAllGameFiles();
                SceneManager.LoadScene("CartridgeCreator");
            }
            else
            {
                ErrorMessage.text = "Game Already Exists";
            }
        }
        else
        {
            ErrorMessage.text = "Invalid Game Name";
        }
    }
}
