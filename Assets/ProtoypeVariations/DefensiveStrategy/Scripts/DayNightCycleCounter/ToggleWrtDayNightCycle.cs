using UnityEngine;

public class ToggleWrtDayNightCycle : MonoBehaviour
{
    [SerializeField] bool HideAtDay;
    [SerializeField] bool HideAtNight;

    void Start()
    {
        if(DayNightCycleCounter.Instance != null)
        {
            DayNightCycleCounter.Instance.OnTimeOfDayChange += OnDayTimeChange;
            OnDayTimeChange(DayNightCycleCounter.Instance.CurrentTime);
        }
    }
    private void OnDestroy()
    {
        if (DayNightCycleCounter.Instance != null)
        {
            DayNightCycleCounter.Instance.OnTimeOfDayChange -= OnDayTimeChange;
        }
    }

    void OnDayTimeChange(TimeOfDay timeOfDay)
    {
        if (timeOfDay == TimeOfDay.Day)
        {
            gameObject.SetActive(!HideAtDay);        
        }
        else if(timeOfDay == TimeOfDay.Night) 
        {
            gameObject.SetActive(!HideAtNight);        
        }
    }
}
