using Test.Data;
using Test.Interfaces;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{

    #region Private Data

    [SerializeField] private float _rayLength;

    private IPublisher _publisher;
    private Camera _camera;
    private LayerMask _layerObjects;
    private LayerMask _layerPlane;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        Initialization();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var rayMouseClick = _camera.ScreenPointToRay(Input.mousePosition);

            var raycast = Physics.Raycast(rayMouseClick, out var hit,
                _rayLength, _layerObjects | _layerPlane);
            if (!raycast) return;

            OnClick(hit);
        }
    }

    #endregion

    #region Public Methods

    public void SetDependence(IPublisher publisher)
    {
        _publisher = publisher;
    }

    #endregion

    #region Private Methods

    private void Initialization()
    {
        _camera = Camera.main;
        _layerObjects = LayerMask.GetMask("Object");
        _layerPlane = LayerMask.GetMask("SpawnArea");
    }

    private void OnClick(RaycastHit hit)
    {
        var layer = hit.transform.gameObject.layer;
        
        if (layer == (int) Mathf.Log((uint) _layerPlane.value, 2))
        {
            _publisher.Publish(new CustomEventData(GameEventName.CreateNewObject, hit.point));
        }

        if (layer == (int) Mathf.Log((uint) _layerObjects.value, 2))
        {
            var targetObject = hit.collider.gameObject;
            _publisher.Publish(new CustomEventData(GameEventName.ClickedObject, targetObject));
        }
    }

    #endregion
}
