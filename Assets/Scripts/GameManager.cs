using System;
using UnityEngine;

/// <summary>
/// Main script of the game, that manages its state and is responsible for creating and destroying gems.
/// </summary>
public class GameManager : MonoBehaviour
{
    public GameObject[] GemPrefabs;
    public Board Gems;

    private Vector3[] SpawnPoints;
    private GameState State = GameState.Default;

    // Use this for initialization
    private void Start()
    {
        SetGemPrefabColors();
        InitializeBoardAndSpawnPoints();
    }

    // Update is called once per frame
    private void Update()
    {
        if(State == GameState.Default)
        {
            if (Input.GetMouseButtonDown(0)) // Player clicked
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// This method is used to set the prefab color string to its name.
    /// </summary>
    private void SetGemPrefabColors()
    {
        // We assign the name of the prefab as the color for easy matching
        foreach (var gem in GemPrefabs)
        {
            gem.GetComponent<Gem>().Color = gem.name;
        }
    }

    /// <summary>
    /// This method is used to get a random gem from the prefabs.
    /// </summary>
    /// <returns>Random gem.</returns>
    private GameObject GetRandomGem()
    {
        return GemPrefabs[UnityEngine.Random.Range(0, GemPrefabs.Length)];
    }

    /// <summary>
    /// This method is used to create a new gem on board.
    /// </summary>
    /// <param name="row">Row of the gem.</param>
    /// <param name="column">Column of the gem.</param>
    /// <param name="gemPrefab">The gem prefab that is to be used to instantiate a new gem.</param>
    private void InstantiateGem(int row, int column, GameObject gemPrefab)
    {
        float posX = Constants.FirstGemPosX + column * Constants.GemOffset;
        float posY = Constants.FirstGemPosY + row * Constants.GemOffset;
        float posZ = Constants.FirstGemPosZ;
        // Instantiate the object on scene
        GameObject obj = Instantiate(gemPrefab, new Vector3(posX, posY, posZ), gemPrefab.transform.rotation)
            as GameObject;

        // Set the properties of the gem
        var gem = obj.GetComponent<Gem>();
        gem.Row = row;
        gem.Column = column;
        // Add object to board
        Gems[row, column] = obj;
    }

    /// <summary>
    /// This method is used to set the positions where new gems will spawn.
    /// </summary>
    private void SetSpawnPositions()
    {
        SpawnPoints = new Vector3[Constants.Columns];
        Vector3 bottomLeft = new Vector3(Constants.FirstGemPosX, Constants.FirstGemPosY, Constants.FirstGemPosZ);
        float offset = Constants.GemOffset;
        for (int column = 0; column < Constants.Columns; column++)
        {
            SpawnPoints[column] = bottomLeft
                + new Vector3(column * offset, Constants.Rows * offset);
        }

    }

    /// <summary>
    /// This method is used to remove all gems from scene.
    /// </summary>
    private void DestroyBoard()
    {
        for (int row = 0; row < Constants.Rows; row++)
        {
            for (int column = 0; column < Constants.Columns; column++)
            {
                Destroy(Gems[row, column]);
            }
        }
    }

    private void InitializeBoard()
    {
        if (Gems != null)
            DestroyBoard();

        Gems = new Board();

        for (int row = 0; row < Constants.Rows; row++)
        {
            for (int column = 0; column < Constants.Columns; column++)
            {
                GameObject randomGem = GetRandomGem();

                // make sure we don't create a match
                while (column >= 2
                    && Gems[row, column - 1].GetComponent<Gem>().IsSameColor(randomGem.GetComponent<Gem>())
                    && Gems[row, column - 2].GetComponent<Gem>().IsSameColor(randomGem.GetComponent<Gem>()))
                    randomGem = GetRandomGem();

                while (row >= 2
                    && Gems[row - 1, column].GetComponent<Gem>().IsSameColor(randomGem.GetComponent<Gem>())
                    && Gems[row - 2, column].GetComponent<Gem>().IsSameColor(randomGem.GetComponent<Gem>()))
                    randomGem = GetRandomGem();

                InstantiateGem(row, column, randomGem);
            }
        }
    }

    public void InitializeBoardAndSpawnPoints()
    {
        InitializeBoard();
        SetSpawnPositions();
    }

    /// <summary>
    /// This method is used to fill columns that not full with new gems.
    /// </summary>
    private void FillColumns()
    {
        var columnsNotFull = Gems.GetColumnsMissingGems();
        //find how many null values the column has
        foreach (int column in columnsNotFull)
        {
            var emptySlots = Gems.GetEmptySlotsInColumn(column);
            foreach (var slot in emptySlots)
            {
                var randomGem = GetRandomGem();
                InstantiateGem(slot.Row, slot.Column, randomGem); // TODO Spawn in spawn points and move them.
            }
        }
    }
}