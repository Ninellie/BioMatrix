namespace Assets.Scripts.Core.Sets
{
    public class DisabledThing : Thing
    {
        private void OnEnable()
        {
            runtimeSet.Remove(this);
        }

        private void OnDisable()
        {
            runtimeSet.Add(this);
        }
    }
}