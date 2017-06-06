using UnityEngine;
using System.Collections;

public class CameraController : MonoSingleton<CameraController>
{
	private float _shake = 0;
	private float _shakeAmount = 0.1f;
 
	void Update()
	{
		if (_shake > 0)
		{
			Camera.main.transform.localPosition = Random.insideUnitSphere * _shakeAmount;
			_shake -= Time.deltaTime;

			if (_shake < 0)
			{
				Camera.main.transform.localEulerAngles = Vector3.zero;
			}
		}
	}

	public void CameraShake()
	{
		_shake = 1;
	}
}
