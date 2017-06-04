using UnityEngine;
using System.Collections;

public class GameController : MonoSingleton<GameController>
{
    public enum GameState { Explore, Battle }

    private GameState _currentState;

	void Awake()
    {
        ItemLibrary itemLibrary = new ItemLibrary();
        EnemyLibrary enemyLibrary = new EnemyLibrary();

        _currentState = GameState.Explore;
    }

    void Start()
    {
        StartGame();
    }

    #region Private Methods

    private void StartGame()
    {

    }

    #endregion
}
