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
    private int selectedObjIndex = -1; // only -1 if no object is selected

    [SerializeField]
    private GameObject gridVisualization;

    private GridData shipData;
    private bool isRotated;

    private Renderer[] previewRenderer;
    private List<GameObject> placedGameObjects = new();

    private void Start()
    {
        StopPlacement();
        shipData = new();
        previewRenderer = cellIndicator.GetComponentsInChildren<Renderer>();
        isRotated = false;
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
        //rotate and offset prefab <- TODO set the Grid logic to work with the placment offest... 
        if (isRotated)
        {
            newObject.transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
            newObject.transform.position = grid.CellToWorld(gridPosition) + new Vector3(0.0f, 0.0f, 1.0f);
        }
        else
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

        //switch back to original
        rotateObject();
        isRotated = false;
    }

    private void Update()
    {
        if (selectedObjIndex < 0)
        {
            return;
        }

        //switch rotation of ship
        if (Input.GetKeyDown(KeyCode.E))
        {
            rotateObject();
        }


        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjIndex);
        foreach (var renderer in previewRenderer)
        {
            renderer.material.color = placementValidity ? Color.white : Color.red;
        }
        // previewRenderer.material.color = placementValidity ? Color.white : Color.red; //change Obj prefab!! (one child)

        mouseIndicator.transform.position = mousePosition;
        Vector3 offset = new Vector3(.5f, 0, .5f);
        cellIndicator.transform.position = grid.CellToWorld(gridPosition) + offset;
    }

    private void rotateObject()
    {
        //no obj is selected
        if (selectedObjIndex < 0)
            return;
        
        Debug.Log("rotate!");
            Vector2Int temp = database.objectData[selectedObjIndex].Size;
            Vector2Int invertSize = new();
            invertSize.x = temp.y;
            invertSize.y = temp.x;
            //update in DB
            database.objectData[selectedObjIndex].Size = invertSize;
            isRotated = !isRotated; //flip isRotated bool
    }
}
