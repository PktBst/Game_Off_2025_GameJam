using TMPro;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycleCounter : MonoBehaviour
{
    [SerializeField] bool automaticCycle;
    int ticks = 0;

    [SerializeField] int lengthOfDayInTicks = 12000;
    public int LengthOfDayInTicks => automaticCycle? lengthOfDayInTicks : 100;
    private TimeOfDay CurrentTime;
    public event System.Action<TimeOfDay> OnTimeOfDayChange;
    [SerializeField] TextMeshProUGUI CycleOfTheDayTmp;
    [SerializeField] Light DirectionalLight;

    private Quaternion dayLightOrientation;
    int FullCycleLength => LengthOfDayInTicks == 0 ? 12000 : 2 * LengthOfDayInTicks;

    public static DayNightCycleCounter Instance;


    private Coroutine phaseDayCoroutine;
    public bool PayTaxesOnDay;
    public bool HideCardsAtNight;

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
    }

    private void Start()
    {
        GameManager.Instance.TickSystem.Subscribe(Tick);
    }

    private void OnDestroy()
    {
        GameManager.Instance.TickSystem.Unsubscribe(Tick);
    }

    private void Tick()
    {
        if (!automaticCycle)
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
        if (CurrentTime == newTime)
        {
            return;
        }
        CurrentTime = newTime;
        if(HideCardsAtNight && GameManager.Instance!=null)
        {
            if((GameManager.Instance.CardSystem.IsDeckUp && CurrentTime == TimeOfDay.Night) || (CurrentTime == TimeOfDay.Day && !GameManager.Instance.CardSystem.IsDeckUp))
            {
                GameManager.Instance.CardSystem.ToggleAndMoveDeck();
            }
        }
        OnTimeOfDayChange?.Invoke(CurrentTime);
        UpdateTimeOfDayTMP();
    }

    [Button]
    public void SetDay()
    {
        if(automaticCycle)
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
            CycleOfTheDayTmp.SetText(CurrentTime.ToString());
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
                SetTimeOfDay(TimeOfDay.Night);
                tickFrom = 0;
                break;
            case TimeOfDay.Night:
                SetTimeOfDay(TimeOfDay.Day);
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
