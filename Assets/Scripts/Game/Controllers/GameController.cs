using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public enum Direction { north, east, south, west }
public class GameController : MonoSingleton<GameController>
{
    public static WorldManager WorldManagerInst;
    public static InventoryManager InventoryManagerInst;

	private float _loopTime = 1f;
	private float _loopCountdown = 0f;

    private WorldManager _worldManager;

	private Queue<UnityAction> ActionQueue;

	private void Awake()
    {
        ActionQueue = new Queue<UnityAction>();
        WorldManagerInst = new WorldManager();
        InventoryManagerInst = new InventoryManager();
    }

    private void Start()
    {
        //TODO: This shouldn't be neccesary once I've got the cartridge loader scene up and running
        //Will check that all data exists before even entering the game scene
        //Load all Data, Make sure none are missing
        if (!WorldDatabase.LoadWorldData())
            Debug.LogWarning("World Data is Missing!");
        else if (!LevelDatabase.LoadLevelData())
            Debug.LogWarning("Level Data is Missing!");
        else if (!RoomDatabase.LoadRoomData())
            Debug.LogWarning("Room Data is Missing!");
        else if (!InteractableDatabase.LoadInteractableData())
            Debug.LogWarning("Interaction Data is Missing!");
        else if (!ObstacleDatabase.LoadObstacleData())
            Debug.LogWarning("Obstacle Data is Missing!");
        else if (!ItemDatabase.LoadItemData())
            Debug.LogWarning("Item Data is Missing!");
        else
        {
            WorldManagerInst.EnterWorld();
            UIController.Instance.EnableUI();
        }
    }

    private void Update()
	{
        //TODO: Revisit
        //Queue Created to pace out actions, and give the game more of a sense of time
        //Things were happening too quickly, some effects weren't happening in time with messages
        //May replace with an event system
		if (_loopCountdown > 0)
		{
			_loopCountdown -= Time.deltaTime;
		}
		else
		{
			if (ActionQueue.Count > 0)
			{
				Debug.Log("Executing Action");
				UnityAction action = ActionQueue.Dequeue();
				action.Invoke();

				_loopCountdown = _loopTime;
			}
		}
	}

	public void AddActionToQueue(UnityAction action)
	{
		ActionQueue.Enqueue(action);
	}
}
