
using UnityEngine;
namespace UtilityLibrary.Classes
{
    [System.Serializable]
    public struct Triangle
    {

        [SerializeField, HideInInspector] private Vector2 v1;
        [SerializeField, HideInInspector] private Vector2 v2;
        [SerializeField, HideInInspector] private Vector2 v3;

        public Vector2 V1 { get => v1; set => v1 = value; }
        public Vector2 V2 { get => v2; set => v2 = value; }
        public Vector2 V3 { get => v3; set => v3 = value; }

        public Triangle(Vector2 v1) : this()
        {
            V1 = v1;
        }

        public Triangle(Vector2 v1, Vector2 v2) : this()
        {
            V1 = v1;
            V2 = v2;
        }

        public Triangle(Vector2 v1, Vector2 v2, Vector2 v3) : this()
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;
        }
    }
}