using UnityEngine;
namespace UtilityLibrary.Classes
{
    [System.Serializable]
    public class SineWave
    {
        //[ShowInInspector, ReadOnly]
        protected Range range = new Range(-1, 1);
        protected Angle angle;

        public float Speed = 1f;


        public float Value
        {
            get => range.Value;
        }

        public SineWave Update()
        {
            range.Value = Mathf.Sin(angle.Rad);
            angle = (Time.deltaTime * Speed + angle.Rad) * Mathf.Rad2Deg;
            return this;
        }

        public void Reset()
        {
            angle = 0;
            range.Value = 0;
        }
    }
}
