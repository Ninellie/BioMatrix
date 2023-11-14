using UnityEngine;
using UnityEngine.Events;

public class StatListener : MonoBehaviour
{
    [Tooltip("Event to register with")]
    public StatVariable statChangedEvent;

    [Tooltip("Response to invoke when stat value changes")]
    public UnityEvent<StatVariable> response;
    /*
     * Size
     * MagnetismRadius
     *
     * Player reserves
     * Life onEdge
     * Life decrease
     * Life on zero
     *
     * Last ammo
     */

    private void OnEnable()
    {
        statChangedEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        statChangedEvent.UnregisterListener(this);
    }

    public void OnEventRaised(StatVariable stat)
    {
        response.Invoke(stat);
    }
}