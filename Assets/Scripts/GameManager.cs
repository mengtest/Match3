using UnityEngine;

/// <summary>
/// Main script of the game, that manages its state and is responsible for creating and destroying gems.
/// </summary>
public class GameManager : MonoBehaviour
{
    public GameObject[] GemPrefabs;
    public Board gems;

    private Vector3[] SpawnPoints;

    // Use this for initialization
    private void Start()
    {
        SetGemPrefabColors();
        InitializeBoardAndSpawnPoints();
    }

    // Update is called once per frame
    private void Update()
    {
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
        return GemPrefabs[Random.Range(0, GemPrefabs.Length)];
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
        gems[row, column] = obj;
    }

    /// <summary>
    /// This method is used to set the positions where new gems will spawn.
    /// </summary>
    private void SetSpawnPositions()
    {
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
                Destroy(gems[row, column]);
            }
        }
    }

    private void InitializeBoard()
    {
        if (gems != null)
            DestroyBoard();

        gems = new Board();

        for (int row = 0; row < Constants.Rows; row++)
        {
            for (int column = 0; column < Constants.Columns; column++)
            {
                GameObject newGem = GetRandomGem();

                // make sure we don't create a match
                while (column >= 2
                    && gems[row, column - 1].GetComponent<Gem>().IsSameColor(newGem.GetComponent<Gem>())
                    && gems[row, column - 2].GetComponent<Gem>().IsSameColor(newGem.GetComponent<Gem>()))
                    newGem = GetRandomGem();

                while (row >= 2
                    && gems[row - 1, column].GetComponent<Gem>().IsSameColor(newGem.GetComponent<Gem>())
                    && gems[row - 2, column].GetComponent<Gem>().IsSameColor(newGem.GetComponent<Gem>()))
                    newGem = GetRandomGem();

                InstantiateGem(row, column, newGem);
            }
        }
    }

    public void InitializeBoardAndSpawnPoints()
    {
        InitializeBoard();
        SetSpawnPositions();
    }
}