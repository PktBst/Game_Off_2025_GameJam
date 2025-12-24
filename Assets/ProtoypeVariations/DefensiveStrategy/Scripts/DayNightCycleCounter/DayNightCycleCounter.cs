using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayNightCycleCounter : MonoBehaviour
{
    [SerializeField] bool automaticCycle;
    int ticks = 0;

    [SerializeField] int lengthOfDayInTicks = 12000;
    public int LengthOfDayInTicks => automaticCycle? lengthOfDayInTicks : 100;
    private TimeOfDay currentTime;
    public TimeOfDay CurrentTime => currentTime;
    public event System.Action<TimeOfDay> OnTimeOfDayChange;
    [SerializeField] TextMeshProUGUI CycleOfTheDayTmp;
    [SerializeField] Light DirectionalLight;

    private Quaternion dayLightOrientation;
    int FullCycleLength => LengthOfDayInTicks == 0 ? 12000 : 2 * LengthOfDayInTicks;

    public static DayNightCycleCounter Instance;
    public Button SetNightButton;

    private Coroutine phaseDayCoroutine;
    public bool PayTaxesOnDay;
    public bool HideCardsAtNight;
    public bool IsAutomatic=> automaticCycle;

    private bool pause;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        if (DirectionalLight != null)
        {
            dayLightOrientation = DirectionalLight.transform.rotation;
        }
        if (SetNightButton != null)
        {
            if (IsAutomatic)
            {
                SetNightButton.gameObject.SetActive(false);
            }
            SetNightButton.onClick.AddListener(SetNight);
        }
    }

    private void Start()
    {
        GameManager.Instance.TickSystem.Subscribe(Tick);
    }

    private void OnDestroy()
    {
        GameManager.Instance.TickSystem.Unsubscribe(Tick);
    }

    public void PausePhasing()
    {
        pause = true;
    }
    public void ResumePhasing()
    {
        pause = false;
    }
    private void Tick()
    {
        if (!automaticCycle)
        { 
            return; 
        }

        if (pause)
        {
            return;
        }
        if(phaseDayCoroutine != null)
        {
            StopCoroutine(phaseDayCoroutine);
            phaseDayCoroutine = null;
        }
        ticks++;

        if (ticks >= FullCycleLength)
        {
            ticks = 0;
        }

        if (ticks == LengthOfDayInTicks)
        {
            SetNight();
        }
        else if (ticks == 0)
        {
            SetDay();
        }

        PhaseTheDayLight();
    }

    private void SetTimeOfDay(TimeOfDay newTime)
    {
        if (currentTime == newTime)
        {
            return;
        }
        currentTime = newTime;
        OnTimeOfDayChange?.Invoke(currentTime);
        Debug.Log($"Current Time: {currentTime}");
        HandleTimeOfDayChanged(currentTime);
        UpdateTimeOfDayTMP();
    }
    private void HandleTimeOfDayChanged(TimeOfDay time)
    {
        var cards = GameManager.Instance.CardSystem;

        if (time == TimeOfDay.Day)
            cards.ShowDeck();
        else
            cards.HideDeck();

        if (SetNightButton != null)
        {
            if (time == TimeOfDay.Day) 
            {
                SetNightButton.interactable = true;
            }
            else
            {
                SetNightButton.interactable = false;
            }
        }
    }

    [Button]
    public void SetDay()
    {
        if (currentTime == TimeOfDay.Day) return;
        GameManager.Instance.CardSystem.redrawDeck();
        if (automaticCycle)
        {
            SetTimeOfDay(TimeOfDay.Day);
        }
        else
        {
            PhaseDayTo(TimeOfDay.Day);
        }
    }

    [Button]
    public void SetNight()
    {
        if (currentTime == TimeOfDay.Night) return;
        if (automaticCycle)
        {
            SetTimeOfDay(TimeOfDay.Night);
        }
        else
        {
            PhaseDayTo(TimeOfDay.Night);
        }
    }


    private void UpdateTimeOfDayTMP()
    {
        if (CycleOfTheDayTmp != null)
        {
            CycleOfTheDayTmp.SetText(currentTime.ToString());
        }
    }

    private void PhaseDayTo(TimeOfDay cycleTo)
    {
        if(phaseDayCoroutine != null)
        {
            StopCoroutine(phaseDayCoroutine);
        }
        phaseDayCoroutine = StartCoroutine(PhaseDayIEnumerator(cycleTo));
    }

    System.Collections.IEnumerator PhaseDayIEnumerator(TimeOfDay cycleTo)
    {
        int tickFrom = ticks;
        int tickTill = 0;
        switch (cycleTo)
        {
            case TimeOfDay.Day:
                //SetTimeOfDay(TimeOfDay.Night);
                tickFrom = 0;
                break;
            case TimeOfDay.Night:
                //SetTimeOfDay(TimeOfDay.Day);
                tickFrom = LengthOfDayInTicks/2;
                break;
        }
        ticks = tickFrom;
        PhaseTheDayLight();
        tickTill = tickFrom + LengthOfDayInTicks / 2;

        while (ticks < tickTill)
        {
            PhaseTheDayLight();
            ticks++;
            yield return null;
        }
        SetTimeOfDay(cycleTo);
        phaseDayCoroutine = null;
        yield break;
    }

    //private void PhaseTheDayLight()
    //{
    //    if (DirectionalLight == null)
    //        return;

    //    float t = ticks / FullCycleLength;
    //    t = Mathf.Repeat(t, 1f);

    //    float sunAngle = Mathf.Lerp(-90f, 270f, t);

    //    DirectionalLight.transform.rotation =
    //        Quaternion.Euler(sunAngle, dayLightOrientation.eulerAngles.y, 0f);
    //}

    private void PhaseTheDayLight()
    {
        if (DirectionalLight == null)
            return;

        float t = ((float)ticks) / ((float)FullCycleLength);
        t = Mathf.Repeat(t, 1f);

        float sunAngle = t * 360f;

        DirectionalLight.transform.rotation =
            Quaternion.Euler(sunAngle, dayLightOrientation.eulerAngles.y, 0f);
    }
}
