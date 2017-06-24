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
    public Button CreateNewButton;

    private List<string> _directories;

    private void Awake()
    {
        string path = GameInfo.GamesPath;
        _directories = Directory.GetDirectories(path).ToList();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        for (int i = 0; i < _directories.Count; i++)
        {
            Debug.Log(Path.GetFileName(_directories[i]));
            options.Add(new Dropdown.OptionData(Path.GetFileName(_directories[i])));
        }
        GameDropdown.AddOptions(options);

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
}
