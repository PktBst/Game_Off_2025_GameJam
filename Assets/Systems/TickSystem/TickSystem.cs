using UnityEngine;

public class TickSystem : MonoBehaviour
{
    private event System.Action OnTick;
    private bool canTick = false;
    public void StartTicking() => canTick = true;
    public void StopTicking() => canTick = false;

    public void Init()
    {
        StartTicking();
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
