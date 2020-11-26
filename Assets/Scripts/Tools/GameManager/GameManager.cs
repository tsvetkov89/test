using System.Collections.Generic;
using Test.Data;
using Test.Interfaces;
using Test.Struct;
using UniRx;
using UnityEngine;

namespace Test.Tools
{
    public class GameManager : ISubscriber
    {
        #region Private Data

        private IPublisher _publisher;
        private IAssetBundlesStorage _assetBundlesStorage;
        private IObjectLoader _objectLoader;
        private IConfigurationTool _configurationTool;

        private Dictionary<GameObject, ObjectData> _objectsOnScene;
        private List<string> _objectTypes;
        private Dictionary<string, List<ClickColorData>> _clickColorDatasByObjectType;

        private int _observableTime;

        #endregion

        public GameManager(IPublisher publisher, IAssetBundlesStorage assetBundlesStorage, IObjectLoader objectLoader,
            IConfigurationTool configurationTool)
        {
            _publisher = publisher;
            _assetBundlesStorage = assetBundlesStorage;
            _objectLoader = objectLoader;
            _configurationTool = configurationTool;

            Initialization();
        }

        #region Public Methods

        #endregion

        public void OnEvent(CustomEventData messageData)
        {

            var eventName = messageData.Message;
            switch (eventName)
            {
                case GameEventName.CreateNewObject:
                    var position = (Vector3) messageData.Value;
                    CreateNewObject(position);
                    break;
                case GameEventName.ClickedObject:
                    var target = (GameObject) messageData.Value;
                    ClickedObject(target);
                    break;
            }
        }

        #region Private Methods

        private void Initialization()
        {
            _objectsOnScene = new Dictionary<GameObject, ObjectData>();
            _publisher.AddSubscriber(GameEventName.CreateNewObject, this);
            _publisher.AddSubscriber(GameEventName.ClickedObject, this);

            var file = _objectLoader.Load<TextAsset>("JsonFiles/ObjectTypes");
            var content = file.text;
            _objectTypes = new List<string>(4);
            var objects = JsonUtility.FromJson<Objects>(content);
            for (var i = 0; i < objects.BaseObjects.Length; i++)
            {
                var item = objects.BaseObjects[i];
                _objectTypes.Add(item.ObjectType);
            }

            _clickColorDatasByObjectType = new Dictionary<string, List<ClickColorData>>();
            var geometryObjectData =
                _objectLoader.Load<GeometryObjectData>("ScriptableObject/GeometryObjectData/Default");

            for (var i = 0; i < geometryObjectData.ClicksData.Count; i++)
            {
                var data = geometryObjectData.ClicksData[i];
                if (_clickColorDatasByObjectType.ContainsKey(data.ObjectType))
                    _clickColorDatasByObjectType[data.ObjectType].Add(data);
                else
                {
                    _clickColorDatasByObjectType[data.ObjectType] = new List<ClickColorData>
                    {
                        data
                    };
                }
            }

            var gameData = _objectLoader.Load<GameData>("ScriptableObject/GameData/Default");
            _observableTime = gameData.ObservableTime;
        }

        private void CreateNewObject(Vector3 position)
        {
            var index = Random.Range(0, _objectTypes.Count);
            var objectType = _objectTypes[index];
            var prefab = _assetBundlesStorage.GetPrefabByObjectType(objectType);

            if (prefab == null)
            {
                // Вывод сообщения, что префаба с таким ObjectType нет.
                return;
            }

            var clone = _objectLoader.Instantiate(prefab);
            clone = _configurationTool.ConfigurateBaseObject(clone);
            clone.transform.localPosition = position;
            var renderer = clone.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", Random.ColorHSV());

            var geometryObjectModel = new GeometryObjectModel
            {
                ClickCount = 0,
                Color = renderer.material.color,
                Renderer = renderer
            };

            var objectData = new ObjectData
            {
                ObjectType = objectType, GeometryObjectModel = geometryObjectModel
            };

            _objectsOnScene[clone] = objectData;

            Observable.Interval(System.TimeSpan.FromSeconds(_observableTime))
                .Subscribe(_ => ChangeColor(clone));
        }

        private void ClickedObject(GameObject target)
        {
            var objectModel = _objectsOnScene[target];
            objectModel.GeometryObjectModel.ClickCount += 1;
            CheckClickCount(objectModel);
        }

        private void CheckClickCount(ObjectData objectData)
        {
            if (!_clickColorDatasByObjectType.ContainsKey(objectData.ObjectType))
                return;

            var clickColorDatas = _clickColorDatasByObjectType[objectData.ObjectType];

            var clickCount = objectData.GeometryObjectModel.ClickCount;
            for (var i = 0; i < clickColorDatas.Count; i++)
            {
                if (clickCount >= clickColorDatas[i].MinClicksCount && clickCount <= clickColorDatas[i].MaxClicksCount)
                {
                    objectData.GeometryObjectModel.Renderer.material.SetColor("_Color", clickColorDatas[i].Color);
                    break;
                }
            }
        }

        private void ChangeColor(GameObject target)
        {
            var objectData = _objectsOnScene[target];
            objectData.GeometryObjectModel.Renderer.material.SetColor("_Color", Random.ColorHSV());
        }

        #endregion
    }
}