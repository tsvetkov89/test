using System.Collections.Generic;
using System.Linq;
using Test.Interfaces;
using UnityEngine;

namespace Test.Tools
{
    public class AssetBundlesStorage : IAssetBundlesStorage
    {
        #region Private Data

        private Dictionary<string, GameObject> _dictionaryPrefabs;
        private IObjectLoader _objectLoader;

        #endregion

        public AssetBundlesStorage(IObjectLoader objectLoader)
        {
            _objectLoader = objectLoader;
            LoadAssets();
        }

        #region Public Methods

        public GameObject GetPrefabByObjectType(string objectType)
        {
            return _dictionaryPrefabs.ContainsKey(objectType) ? _dictionaryPrefabs[objectType] : null;
        }

        #endregion

        #region Private Methods

        private void LoadAssets()
        {
            var assetBundleDirectory = "Assets/AssetBundles";
            var contentDirectory = "content/prefabs";
            var prefabs = _objectLoader.LoadAllAssets<GameObject>(assetBundleDirectory, contentDirectory).ToList();
            _dictionaryPrefabs = new Dictionary<string, GameObject>();
            for (var i = 0; i < prefabs.Count; i++)
            {
                _dictionaryPrefabs[prefabs[i].name] = prefabs[i];
            }
        }

        #endregion
    }
}

