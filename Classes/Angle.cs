using UnityEngine;
namespace UtilityLibrary.Classes
{
    [System.Serializable]
    public struct Angle
    {
        [SerializeField, HideInInspector]
        private float value;

        public Angle(Angle other) : this()
        {
            Value = other.value;
        }

        public Angle(float value) : this()
        {
            Value = value;
        }


        public float Value
        {
            get
            {
                return UtilityFunctions.ClampAngle(value);
            }

            set
            {
                this.value = UtilityFunctions.ClampAngle(value);
            }
        }

        public float Rad
        {
            get
            {
                return Mathf.Deg2Rad * Value;
            }
        }


        public bool InsideRange(Angle start, Angle end)
        {
            if (start.Value < end.Value)
                return start.Value <= Value && Value <= end.Value;
            return start.Value <= Value || Value <= end.Value;

        }

        public static implicit operator Angle(float d)
        {
            return new Angle(d);
        }

        public static implicit operator float(Angle d)
        {
            return d.value;
        }
        public override string ToString()
        {
            return Value + "°";
        }
    }
}