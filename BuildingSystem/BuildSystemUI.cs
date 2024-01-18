using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class BuildSystemUI : MonoBehaviour
{
    [SerializeField] private Image tileInformation;
    [SerializeField] private TMP_Text placeForTreeTMP;
    [SerializeField] private TMP_Text placeForFactoryTMP;
    [Inject] private BuildSystem _buildSystem;
    private bool _buttonsActivate = false;

    private readonly List<Button> _buildingsButtons = new List<Button>();

    private void Awake() 
    {
        tileInformation.gameObject.SetActive(false);

        _buildingsButtons.AddRange(GetComponentsInChildren<Button>());
        _buildingsButtons.Remove(GetComponent<Button>());

        foreach(var btn in _buildingsButtons)
        {
            Debug.Log(btn.name);
            btn.gameObject.SetActive(false);
        }
    }

    public void OnBuildButtonClick()
    {
        SetButtonsActivate(!_buttonsActivate);
    }

    public void Build(Building building)
    {
        _buildSystem.ReadyBuild(building);

        SetButtonsActivate(false);
    }

    public void ConfirmBuilding()
    {
        _buildSystem.Build();
    }

    private void SetButtonsActivate(bool value)
    {
        foreach(var btn in _buildingsButtons)
            btn.gameObject.SetActive(value);

        _buttonsActivate = !_buttonsActivate;
    }

    public void SetTileInformation(Tile tile)
    {
        tileInformation.gameObject.SetActive(true);
        
        placeForFactoryTMP.text = tile.PlaceForFactory.ToString();
        placeForTreeTMP.text = tile.PlaceForTree.ToString();
    }

    public void HideTileInformation()
    {
        tileInformation.gameObject.SetActive(false);
    }
}