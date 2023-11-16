using UnityEngine;

[CreateAssetMenu(fileName = "New Int Variable", menuName = "Variables/Int", order = 51)]
public class IntVariable : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string developerDescription = "";
#endif
    public int value;

    public void SetValue(int value)
    {
        this.value = value;
    }

    public void SetValue(IntVariable value)
    {
        this.value = value.value;
    }

    public void ApplyChange(int amount)
    {
        value += amount;
    }

    public void ApplyChange(IntVariable amount)
    {
        value += amount.value;
    }
}