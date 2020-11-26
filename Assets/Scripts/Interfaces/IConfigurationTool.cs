using UnityEngine;

namespace Test.Interfaces
{
    public interface IConfigurationTool
    {
        GameObject ConfigurateBaseObject(GameObject originalm, GameObject clone);
    }
}