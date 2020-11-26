using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Test.Interfaces;
using UnityEngine;
using UnityEngine.Networking;

namespace Test.Tools
{
    public class AssetBundlesStorage : IAssetBundlesStorage
    {
        #region Private Data

        private Dictionary<string, GameObject> _dictionaryPrefabs;
        private IObjectLoader _objectLoader;
        private ICoroutiner _coroutiner;

        #endregion

        public AssetBundlesStorage(IObjectLoader objectLoader, ICoroutiner coroutiner)
        {
            _coroutiner = coroutiner;
            _objectLoader = objectLoader;
            LoadAssets();
            // _coroutiner.StartCoroutine(DownloadAssetsAndCache());
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

        private IEnumerator DownloadAssetsAndCache()
        {

            var assetBundleUrl = "";
            var contentDirectory = "content/prefabs";
            var version = 0;
            while (!Caching.ready)
                yield return null;
            var www = WWW.LoadFromCacheOrDownload(assetBundleUrl, version);
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log(www.error);
            }

            var assetBundle = www.assetBundle;
            var prefabs = assetBundle.LoadAllAssets<GameObject>().ToList();

            _dictionaryPrefabs = new Dictionary<string, GameObject>();
            for (var i = 0; i < prefabs.Count; i++)
            {
                _dictionaryPrefabs[prefabs[i].name] = prefabs[i];
            }
        }

        #endregion
    }
}

