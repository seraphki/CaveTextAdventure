using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public enum Direction { North, East, South, West }
public class GameController : MonoSingleton<GameController>
{
	private float _loopTime = 1f;
	private float _loopCountdown = 0f;

	private Queue<UnityAction> ActionQueue;

	void Awake()
    {
        ItemLibrary itemLibrary = new ItemLibrary();
        EnemyLibrary enemyLibrary = new EnemyLibrary();
		ActionQueue = new Queue<UnityAction>();
    }

	void Update()
	{
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
