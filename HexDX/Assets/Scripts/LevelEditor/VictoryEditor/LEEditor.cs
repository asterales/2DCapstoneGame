using UnityEngine;

// TODO :: REFACTOR TO USE THIS FOR ALL EDITOR OBJECTS

public abstract class LEEditor : MonoBehaviour
{
    public abstract void Increment(int modifier);
    public abstract void Decrement(int modifier);
    public abstract void TurnOn();
    public abstract void TurnOff();
}
