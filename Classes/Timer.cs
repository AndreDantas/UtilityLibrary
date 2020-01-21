
using UnityEngine;
namespace UtilityLibrary.Classes
{
    [System.Serializable]
    public struct Timer
    {
        [SerializeField, HideInInspector]
        private float time;

        public Timer(float time)
        {
            this.time = UtilityFunctions.ClampMin(time, 0f);
            counter = 0f;
            isFinished = false;
        }

        public Timer(float time, bool isFinished) : this(time)
        {
            this.isFinished = isFinished;
        }

        //[ShowInInspector, PropertyOrder(-1)]
        public float Time
        {
            get
            {
                return time;
            }

            set
            {
                time = UtilityFunctions.ClampMin(value, 0f);
            }
        }

        [SerializeField, HideInInspector] private float counter;
        //[ShowInInspector, ReadOnly, ProgressBar(0f, "Time")]
        public float Counter { get => counter; set => counter = Mathf.Clamp(value, 0f, Time); }
        //[ReadOnly, ShowInInspector]
        public bool isFinished { get; private set; }


        //[Button(ButtonSizes.Medium), ButtonGroup("G1")]
        public void Update()
        {
            if (isFinished)
            {
                Counter = Time;
                return;
            }
            Counter += UnityEngine.Time.deltaTime;
            if (Counter >= Time)
            {
                isFinished = true;
                Counter = Time;
            }
            else
                isFinished = false;


        }

        public void Reverse()
        {
            if (isFinished)
            {
                isFinished = false;
            }

            Counter -= UnityEngine.Time.deltaTime;
            Counter = UtilityFunctions.ClampMin(Counter, 0f);

        }
        //[Button(ButtonSizes.Medium), ButtonGroup("G1")]
        public void Reset()
        {
            Counter = 0f;
            isFinished = false;
        }

        //[Button(ButtonSizes.Medium), ButtonGroup("G1")]
        public void Finish()
        {
            Counter = Time;
            isFinished = true;
        }


    }
}
