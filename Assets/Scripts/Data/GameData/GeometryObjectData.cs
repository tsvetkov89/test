using System.Collections.Generic;
using UnityEngine;

namespace Test.Data
{
    [CreateAssetMenu(menuName = "Data/New Geometry Object Data")]
    public class GeometryObjectData : ScriptableObject
    {
        public List<ClickColorData> ClicksData;
    }
}