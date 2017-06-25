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

    private List<string> _directories;

    private void Awake()
    {
        //Populate Directories Path
        string path = GameInfo.GamesPath;
        _directories = Directory.GetDirectories(path).ToList();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        for (int i = 0; i < _directories.Count; i++)
        {
            Debug.Log(Path.GetFileName(_directories[i]));
            options.Add(new Dropdown.OptionData(Path.GetFileName(_directories[i])));
        }
        GameDropdown.AddOptions(options);

        //If there are games, enable play and edit buttons
        if (_directories.Count > 0)
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

        if (!string.IsNullOrEmpty(gameName))
        {
            GameInfo.SetCurrentGame(gameName);
            Helpers.CreateAllGameFiles();
            SceneManager.LoadScene("CartridgeCreator");
        }
    }
}
