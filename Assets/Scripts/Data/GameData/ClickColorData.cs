using UnityEngine;

namespace Test.Data
{
    [CreateAssetMenu(menuName = "Data/New Click Color Data")]
    public class ClickColorData : ScriptableObject
    {
        public string ObjectType;
        public int MinClicksCount;
        public int MaxClicksCount;
        public Color Color;
    }
}