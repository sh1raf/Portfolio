using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class BuildSystem : MonoBehaviour
{
    private Building _currentBuilding;
    private Tile _lastTile;
    private bool _readyBuild = false;

    [Inject] private BuildSystemUI _buildSystem;
    [Inject] private PlayerInput _playerInput;

    private void Awake() 
    {
        _playerInput.Computer.Click.performed += context => OnClick();
    }
    

    private void OnClick()
    {
        if(_readyBuild)
        {
            Ray ray = Camera.main.ScreenPointToRay(_playerInput.Computer.MousePosition.ReadValue<Vector2>());

            RaycastHit hit;

            if(EventSystem.current.IsPointerOverGameObject())
                return;

            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.GetComponentInParent<Tile>().TryGetComponent(out Tile tile))
                {
                    if(_lastTile != null)
                        _lastTile.GetComponent<Outline>().enabled = false;

                    tile.GetComponent<Outline>().enabled = true;
                    _buildSystem.SetTileInformation(tile);
                    _lastTile = tile;
                }
            }
        }
    }

    public void ReadyBuild(Building building)
    {
        _currentBuilding = building;
        _readyBuild = true;
    }

    public void Build()
    {
        if(_currentBuilding != null && _readyBuild == true)
        {
            _lastTile.Build(_currentBuilding.gameObject);
            _lastTile.GetComponent<Outline>().enabled = false;

            _currentBuilding = null;
            _readyBuild = false;
            _buildSystem.HideTileInformation();
        }
    }
}

public enum Actions
{
    Collect,
    Factory,
    Fermentation,
    Shop,
    BlackShop
}
