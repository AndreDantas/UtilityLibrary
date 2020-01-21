using UnityEngine;
namespace UtilityLibrary.Classes
{
    [System.Serializable]
    public struct Range
    {

        [SerializeField, HideInInspector] private float min;
        [SerializeField, HideInInspector] private float max;
        [SerializeField, HideInInspector] private float value;

        public float Min { get => min; set => min = Mathf.Min(value, Max); }
        /*[ShowInInspector, PropertyRange("Min", "Max")]*/
        public float Value { get => Mathf.Clamp(value, Min, Max); set => this.value = Mathf.Clamp(value, Min, Max); }
        public float Max { get => max; set => max = Mathf.Max(value, Min); }

        public Range(float min, float value, float max) : this()
        {
            Min = min;
            Value = value;
            Max = max;
        }

        public Range(float min, float max) : this()
        {
            Min = min;
            Max = max;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Min + " --{" + Value + "}-- " + Max;
        }
    }
}