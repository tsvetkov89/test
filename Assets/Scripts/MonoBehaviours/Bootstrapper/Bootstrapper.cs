using Test.Interfaces;
using Test.Tools;
using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    #region Private Data

    [SerializeField] private Transform _folder;
    [SerializeField] private ClickHandler _clickHandler;

    private IObjectLoader _objectLoader;
    private IPublisher _publisher;
    private IAssetBundlesStorage _assetBundlesStorage;
    private IConfigurationTool _configurationTool;
    private GameManager _gameManager;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        Initialization();
    }

    #endregion

    #region Private Methods

    private void Initialization()
    {
        _objectLoader = new ObjectLoader();
        _publisher = new Publisher();
        _configurationTool = new ConfigurationTool(_objectLoader, _folder);
        _assetBundlesStorage = new AssetBundlesStorage(_objectLoader);
        _gameManager = new GameManager(_publisher, _assetBundlesStorage, _objectLoader, _configurationTool);

        _clickHandler.SetDependence(_publisher);
    }

    #endregion
}
