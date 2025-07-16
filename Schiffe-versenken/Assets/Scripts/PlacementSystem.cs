using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{

    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectDatabaseSO database;
    private int selectedObjIndex = -1;

    [SerializeField]
    private GameObject gridVisualization;

    private void Start()
    {
        StopPlacement();
    }

    public void StartPlacement(int ID)
    {
        //make sure no placement is active!
        StopPlacement();

        selectedObjIndex = database.objectData.FindIndex(data => data.ID == ID);

        if (selectedObjIndex < 0)
        {
            Debug.Log($"No ID found {ID}");
            return;
        }
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        GameObject newObject = Instantiate(database.objectData[selectedObjIndex].Prefab);
        //maybe change mouseIndicator to Selected Object! <- nice to have! 
        newObject.transform.position = grid.CellToWorld(gridPosition);
    }

    private void StopPlacement()
    {
        selectedObjIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
    }

    private void Update()
    {
        if (selectedObjIndex < 0)
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        Vector3 offset = new Vector3(.5f, 0, .5f);
        cellIndicator.transform.position = grid.CellToWorld(gridPosition) + offset;
    }


}
