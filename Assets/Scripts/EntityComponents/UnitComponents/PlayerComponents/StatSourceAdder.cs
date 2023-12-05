using Assets.Scripts.SourceStatSystem;
using UnityEngine;

namespace Assets.Scripts.EntityComponents.UnitComponents.PlayerComponents
{
    public class StatSourceAdder : MonoBehaviour
    {
        [SerializeField] private StatSources _list;
        [SerializeField] private StatSourceData _value;

        public void Activate()
        {
            _list.AddStatSource(_value);
        }
    }
}