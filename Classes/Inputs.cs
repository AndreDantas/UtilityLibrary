namespace UtilityLibrary.Classes
{
    [System.Serializable]
    public class Inputs
    {

        #region Fields/Constructors

        private bool left, right, up, down;

        public bool oneDirectionEnabledPerAxis = true;

        public Inputs(bool oneDirectionEnabledPerAxis)
        {
            this.oneDirectionEnabledPerAxis = oneDirectionEnabledPerAxis;
        }

        public Inputs()
        {
        }


        public bool Left
        {
            get
            {
                return left;
            }

            set
            {
                left = value;
                if (oneDirectionEnabledPerAxis && value)
                    Right = false;

            }
        }


        public bool Right
        {
            get
            {
                return right;
            }

            set
            {
                right = value;
                if (oneDirectionEnabledPerAxis && value)
                    Left = false;
            }
        }


        public bool Up
        {
            get
            {
                return up;
            }

            set
            {
                up = value;
                if (oneDirectionEnabledPerAxis && value)
                    Down = false;
            }
        }


        public bool Down
        {
            get
            {
                return down;
            }

            set
            {
                down = value;
                if (oneDirectionEnabledPerAxis && value)
                    Up = false;
            }
        }

        #endregion

        public override string ToString()
        {
            return "Right: " + right + " | Left: " + left + " | Up: " + up + " | Down: " + down;
        }
    }
}