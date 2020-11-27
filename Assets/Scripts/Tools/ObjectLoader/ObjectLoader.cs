using System.Collections.Generic;
using System.IO;
using Test.Interfaces;
using UnityEngine;

namespace Test.Tools
{
    public class ObjectLoader : IObjectLoader
    {
        #region Public Methods

        public T Load<T>(string path) where T : Object
        {
            return (T) Resources.Load(path, typeof(T));
        }

        public T[] LoadAll<T>(string path) where T : Object
        {
            return ConvertObjects<T>(Resources.LoadAll(path, typeof(T)));
        }

        public T Instantiate<T>(T original) where T : Object
        {
            return (T) Object.Instantiate((Object) original);
        }
        
        public T Instantiate<T>(T original, Transform parent, bool worldPositionStays) where T : Object
        {
            return (T) Object.Instantiate((Object) original, parent, worldPositionStays);
        }

        public T Instantiate<T>(T original, Transform parent) where T : Object
        {
            return (T) Object.Instantiate((Object) original, parent, false);
        }

        public T[] LoadAllAssets<T>(string contentDirectory) where T : Object
        {
            var localAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, contentDirectory));
            if (localAssetBundle == null)
            {
                Debug.LogError("Failed to load AssetBundle!");
                return null;
            }

            var assets = localAssetBundle.LoadAllAssets<T>();
            localAssetBundle.Unload(false);
            return assets;
        }

        #endregion

        #region Private Methods

        private T[] ConvertObjects<T>(IList<Object> rawObjects) where T : Object
        {
            if (rawObjects == null)
                return null;
            var objArray = new T[rawObjects.Count];
            for (var index = 0; index < objArray.Length; ++index)
                objArray[index] = (T) rawObjects[index];
            return objArray;
        }

        #endregion
    }
}