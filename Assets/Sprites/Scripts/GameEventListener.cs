using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    // The game event instance to register to.
    public GameEvent GameEvent;
    // The unity event response created for the event.
    public UnityEvent Response;

    private void OnEnable()
    {
        GameEvent.RegisterListerner(this);
    }

    private void OnDisable()
    {
        GameEvent.UnregisterListener(this);
    }

    public void RaiseEvent()
    {
        Response.Invoke();
    }
}