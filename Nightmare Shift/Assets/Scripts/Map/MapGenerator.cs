using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Transform boardContainer;
    [SerializeField] private List<PointOfInterest> pointsOfInterestPrefabs;
    [SerializeField] private GameObject pathPrefab;
    [SerializeField] private PointOfInterest bossPOIPrefab;  // Prefab for the final boss node
    [SerializeField] private int numberOfStartingPoints = 4;
    [SerializeField] private int mapLength = 10;
    [SerializeField] private int maxWidth = 5;
    [SerializeField] private float xMaxSize;
    [SerializeField] private float yPadding;
    [SerializeField] private bool allowCrisscrossing;
    [Range(0.1f, 1f), SerializeField] private float chancePathMiddle;
    [Range(0f, 1f), SerializeField] private float chancePathSide;
    [SerializeField, Range(0.9f, 5f)] private float multiplicativeSpaceBetweenLines = 2.5f;
    [SerializeField, Range(1f, 5.5f)] private float multiplicativeNumberOfMinimumConnections = 3f;

    private PointOfInterest[][] _pointOfInterestsPerFloor;
    private readonly List<PointOfInterest> pointsOfInterest = new();
    private int _numberOfConnections = 0;
    private int recreateAttempts = 0;
    private PointOfInterest bossPOI;

    private void Start()
    {
        RecreateBoard();
    }

    public void RecreateBoard()
    {
        recreateAttempts++;
        if (recreateAttempts > 10)
        {
            Debug.LogError("Failed to create a map with enough connections after multiple attempts.");
            return;
        }

        DestroyImmediateAllChildren(boardContainer);
        _numberOfConnections = 0;
        GenerateRandomSeed();
        pointsOfInterest.Clear();
        _pointOfInterestsPerFloor = new PointOfInterest[mapLength][];
        for (int i = 0; i < _pointOfInterestsPerFloor.Length; i++)
        {
            _pointOfInterestsPerFloor[i] = new PointOfInterest[maxWidth];
        }
        CreateMap();
    }

    private void GenerateRandomSeed()
    {
        int tempSeed = (int)System.DateTime.Now.Ticks;
        Random.InitState(tempSeed);
    }

    private PointOfInterest InstantiatePointOfInterest(int floorN, int xNum)
    {
        if (_pointOfInterestsPerFloor[floorN][xNum] != null)
        {
            return _pointOfInterestsPerFloor[floorN][xNum];
        }

        float xSize = xMaxSize / maxWidth;
        float xPos = (xSize * xNum) + (xSize / 2f);
        float yPos = yPadding * floorN;

        //Add a random padding
        xPos += Random.Range(-xSize / 4f, xSize / 4f);
        yPos += Random.Range(-yPadding / 4f, yPadding / 4f);

        Vector3 pos = new Vector3(xPos, 0, yPos);
        PointOfInterest randomPOI = pointsOfInterestPrefabs[Random.Range(0, pointsOfInterestPrefabs.Count)];
        PointOfInterest instance = Instantiate(randomPOI, boardContainer);
        pointsOfInterest.Add(instance);

        instance.transform.localPosition = pos;
        _pointOfInterestsPerFloor[floorN][xNum] = instance;
        int created = 0;

        void InstantiateNextPoint(int index_i, int index_j)
        {
            PointOfInterest nextPOI = InstantiatePointOfInterest(index_i, index_j);
            AddLineBetweenPoints(instance, nextPOI);
            //instance.NextPointsOfInterestWithPath.Add(nextPOI);
            instance.NextPointsOfInterest.Add(nextPOI);
            created++;
            _numberOfConnections++;
        }

        while (created == 0 && floorN < mapLength - 1)
        {
            if (xNum > 0 && Random.Range(0f, 1f) < chancePathSide)
            {
                if (allowCrisscrossing || _pointOfInterestsPerFloor[floorN + 1][xNum] == null)
                {
                    InstantiateNextPoint(floorN + 1, xNum - 1);
                }
            }

            if (xNum < maxWidth - 1 && Random.Range(0f, 1f) < chancePathSide)
            {
                if (allowCrisscrossing || _pointOfInterestsPerFloor[floorN + 1][xNum] == null)
                {
                    InstantiateNextPoint(floorN + 1, xNum + 1);
                }
            }

            if (Random.Range(0f, 1f) < chancePathMiddle)
            {
                InstantiateNextPoint(floorN + 1, xNum);
            }
        }

        return instance;
    }

    private void CreateMap()
    {
        List<int> positions = GetRandomIndexes(numberOfStartingPoints);
        foreach (int j in positions)
        {
            _ = InstantiatePointOfInterest(0, j);
        }

        if (_numberOfConnections <= mapLength * multiplicativeNumberOfMinimumConnections)
        {
            Debug.Log($"Recreating board with {_numberOfConnections} connections");
            RecreateBoard();
            return;
        }

        // Instantiate and connect to the boss node
        CreateBossNode();

        Debug.Log($"Created board with {_numberOfConnections} connections");
        Debug.Log($"Created board with {pointsOfInterest.Count} points");
        recreateAttempts = 0;
    }

    private void CreateBossNode()
    {
        // Instantiate boss POI at the final floor level
        Vector3 bossPos = new Vector3(xMaxSize / 2, 0, yPadding * (mapLength + 1));
        bossPOI = Instantiate(bossPOIPrefab, bossPos, Quaternion.identity, boardContainer);
        pointsOfInterest.Add(bossPOI);

        // Connect all last nodes in the map to the boss node
        foreach (var poi in _pointOfInterestsPerFloor[mapLength - 1])
        {
            if (poi != null)
            {
                AddLineBetweenPoints(poi, bossPOI);
                //poi.NextPointsOfInterestWithPath.Add(bossPOI);
                poi.NextPointsOfInterest.Add(bossPOI);
                _numberOfConnections++;
            }
        }

        Debug.Log($"Boss node created with {pointsOfInterest.Count} total points.");
    }

    private void AddLineBetweenPoints(PointOfInterest thisPoint, PointOfInterest nextPoint)
    {
        GameObject lineObj = Instantiate(pathPrefab, boardContainer);
        LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();

        if (lineRenderer == null)
        {
            Debug.LogError("The path prefab does not contain a LineRenderer component!");
            return;
        }

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, thisPoint.transform.position);
        lineRenderer.SetPosition(1, nextPoint.transform.position);
    }

    private List<int> GetRandomIndexes(int n)
    {
        List<int> indexes = new List<int>();
        if (n > maxWidth)
        {
            throw new System.Exception("Number of starting points greater than maxWidth!");
        }

        while (indexes.Count < n)
        {
            int randomNum = Random.Range(0, maxWidth);
            if (!indexes.Contains(randomNum))
            {
                indexes.Add(randomNum);
            }
        }
        return indexes;
    }

    private void DestroyImmediateAllChildren(Transform transform)
    {
        List<Transform> toKill = new();

        foreach (Transform child in transform)
        {
            toKill.Add(child);
        }

        for (int i = toKill.Count - 1; i >= 0; i--)
        {
            DestroyImmediate(toKill[i].gameObject);
        }
    }
}
