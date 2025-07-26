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

    private GridData shipData;

    private Renderer previewRenderer;
    private List<GameObject> placedGameObjects = new();

    private void Start()
    {
        StopPlacement();
        shipData = new();
        previewRenderer = cellIndicator.GetComponentInChildren<Renderer>();
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

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjIndex);
        if (placementValidity == false)
            return;

        GameObject newObject = Instantiate(database.objectData[selectedObjIndex].Prefab);
        //maybe change mouseIndicator to Selected Object! <- nice to have! 
        newObject.transform.position = grid.CellToWorld(gridPosition);

        //add obj to list
        placedGameObjects.Add(newObject);
        //add obj to Data dict
        GridData selectedData = shipData;
        selectedData.AddObjectAt(gridPosition,
            database.objectData[selectedObjIndex].Size,
            database.objectData[selectedObjIndex].ID,
            placedGameObjects.Count - 1);

    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjIndex)
    {
        // GridData selectedData = database.objectData[selectedObjIndex].ID == 0 ? minesData : shipData; // use this later for mines etc..
        GridData selectedData = shipData;

        return selectedData.CanPlaceObjectAt(gridPosition, database.objectData[selectedObjIndex].Size); //get obj size from Database!  
    }

    // stops the placement. unchecks the listiner on methods placeStructure and stopPlacement
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

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjIndex);
        previewRenderer.material.color = placementValidity ? Color.white : Color.red; //change Obj prefab!! (one child)

        mouseIndicator.transform.position = mousePosition;
        Vector3 offset = new Vector3(.5f, 0, .5f);
        cellIndicator.transform.position = grid.CellToWorld(gridPosition) + offset;
    }


}
