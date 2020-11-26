using UnityEngine;

namespace Test.Interfaces
{
    public interface IAssetBundlesStorage
    {
        GameObject GetPrefabByObjectType(string objectType);
    }
}