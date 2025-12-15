using TMPro;
using UnityEngine;

public class TickSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI FPSDebugText;
    private event System.Action OnTick;
    private bool canTick = false;
    public void StartTicking() => canTick = true;
    public void StopTicking() => canTick = false;

    public void Init()
    {
        StartTicking();
        InvokeRepeating("calcFPS",1f,1f);
    }

    void calcFPS()
    {
        FPSDebugText.text = "FPS : "+ (int)(1f/Time.unscaledDeltaTime);
    }

    private void Update()
    {
        if(canTick) OnTick?.Invoke();

    }

    public void Subscribe(System.Action tickAction)
    {
        OnTick += tickAction;
    }

    public void Unsubscribe(System.Action tickAction)
    {
        OnTick -= tickAction;
    }

}
