using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;

public class WorldTimeAPI : MonoBehaviour
{
    public static WorldTimeAPI Instance;
    private DateTime _realDateTime;
    public bool IsTimeLoaded = false;

    private const string API_KEY = "354de871f5454c72bcebe2ab783d0721";
    private const string API_URL = "https://api.ipgeolocation.io/timezone?apiKey=" + API_KEY;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(GetDateTimeFromAPI());
    }

    public DateTime GetRealDateTime()
    {
        return _realDateTime.AddSeconds(Time.realtimeSinceStartup);
    }

    IEnumerator GetDateTimeFromAPI()
    {
        UnityWebRequest request = UnityWebRequest.Get(API_URL);
        request.timeout = 10;
        yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
        if (request.result != UnityWebRequest.Result.Success)
#else
        if (request.isNetworkError || request.isHttpError)
#endif
        {
            Debug.LogError("API error: " + request.error);
        }
        else
        {
            string json = request.downloadHandler.text;
            TimezoneData timezoneData = JsonUtility.FromJson<TimezoneData>(json);

            if (!string.IsNullOrEmpty(timezoneData.date_time))
            {
                _realDateTime = DateTime.Parse(timezoneData.date_time);
                IsTimeLoaded = true;
                Debug.Log("Real time: " + _realDateTime.ToString());
            }
            else
            {
                Debug.LogError("Failed to parse date_time.");
            }
        }
    }

    [Serializable]
    public class TimezoneData
    {
        public string date_time;
    }
}