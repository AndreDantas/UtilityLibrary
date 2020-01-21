
using System.Collections;
using UnityEngine;
namespace UtilityLibrary.Classes
{
    [System.Serializable]
    public struct Rectangle
    {

        [SerializeField, HideInInspector] private float width;
        [SerializeField, HideInInspector] private float height;
        [SerializeField, HideInInspector] private Angle rotation;
        [SerializeField, HideInInspector] private Vector2 center;

        public float Height { get => height; set => height = value; }
        public float Width { get => width; set => width = value; }
        public Angle Rotation { get => rotation; set => rotation = value; }
        public Vector2 Center { get => center; set => center = value; }

        private Vector2 topLeftRaw { get => new Vector2(Center.x - Width / 2f, Center.y + Height / 2f); }
        public Vector2 TopLeft { get => UtilityFunctions.RotatePoint(topLeftRaw, Center, Rotation); }

        private Vector2 topRightRaw { get => new Vector2(Center.x + Width / 2f, Center.y + Height / 2f); }
        public Vector2 TopRight { get => UtilityFunctions.RotatePoint(topRightRaw, Center, Rotation); }

        private Vector2 bottomLeftRaw { get => new Vector2(Center.x - Width / 2f, Center.y - Height / 2f); }
        public Vector2 BottomLeft { get => UtilityFunctions.RotatePoint(bottomLeftRaw, Center, Rotation); }

        private Vector2 bottomRightRaw { get => new Vector2(Center.x + Width / 2f, Center.y - Height / 2f); }
        public Vector2 BottomRight { get => UtilityFunctions.RotatePoint(bottomRightRaw, Center, Rotation); }

        public Rectangle(float height, float width, Angle rotation, Vector2 center) : this()
        {
            Height = height;
            Width = width;
            Rotation = rotation;
            Center = center;
        }

        public Rectangle(float height, float width, Angle rotation) : this(height, width)
        {
            Rotation = rotation;
        }

        public Rectangle(float height, float width) : this()
        {
            Height = height;
            Width = width;
        }


    }
}
