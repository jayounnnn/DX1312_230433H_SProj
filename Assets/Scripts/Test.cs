using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Test : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI datetimeText;

	void Update()
	{
		if (Input.GetMouseButtonUp(0) && WorldTimeAPI.Instance.IsTimeLodaed)
		{
			DateTime currentDateTime = WorldTimeAPI.Instance.GetCurrentDateTime();

			datetimeText.text = currentDateTime.ToString();
		}
	}
}