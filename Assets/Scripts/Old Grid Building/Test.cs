using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Test : MonoBehaviour
{
	private bool inProgress;
	private DateTime TimerStart;
	private DateTime TimerEnd;

	[Header("Production time")]
	public int Days;
	public int Hours;
	public int Minutes;
	public int Seconds;

	[Header("UI")]
	[SerializeField] private GameObject window;
	[SerializeField] private TextMeshProUGUI startTimeText;
	[SerializeField] private TextMeshProUGUI endTimeText;
	[SerializeField] private GameObject timeLeftObj;
	[SerializeField] private TextMeshProUGUI timeLeftText;
	[SerializeField] private Slider timeLeftSlider;
	[SerializeField] private Button skipButton;
	[SerializeField] private Button startButton;

    #region Unity Methods

    private void Start()
    {
		startButton.onClick.AddListener(StartTimer);
    }

    #endregion

    #region UI Methods

	private void InitializeWindow()
    {
		startTimeText.text = "Start Time: \n" + TimerStart;
		endTimeText.text = "End Time: \n" + TimerEnd;
	}

    #endregion

    #region Timed Event

    private void StartTimer()
    {
		TimerStart = DateTime.Now;
		TimeSpan time = new TimeSpan(Days, Hours, Minutes, Seconds);
		TimerEnd = TimerStart.Add(time);
		inProgress = true;

		InitializeWindow();
    }

	#endregion
}