using UnityEngine;

namespace Test.Interfaces
{
    public interface IObjectLoader
    {
        T Load<T>(string path) where T : Object;
        T[] LoadAll<T>(string path) where T : Object;
        T[] LoadAllAssets<T>(string contentDirectory) where T : Object;
        T Instantiate<T>(T original) where T : Object;
        T Instantiate<T>(T original, Transform parent, bool worldPositionStays) where T : Object;
        T Instantiate<T>(T original, Transform parent) where T : Object;
    }
}