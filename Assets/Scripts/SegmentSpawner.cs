using System.Collections.Generic;
using UnityEngine;

public class SegmentSpawner : MonoBehaviour
{
    [SerializeField] private Transform player;            
    [SerializeField] private Transform followCamera;      
    [SerializeField] private GameObject segmentPrefab;    

    [SerializeField] private int initialSegments = 6;
    [SerializeField] private float spawnAheadDistance = 25f;     
    [SerializeField] private float despawnBehindDistance = 30f;  

    [SerializeField] private Vector2 gapRange = new Vector2(5f, 10f);
    [SerializeField] private Vector2 heightOffsetRange = new Vector2(-1.0f, 2.0f);

    [SerializeField] private float baseY = -6f; 

    private readonly List<GameObject> activeSegments = new List<GameObject>();

    private float lastEndX;     
    private float segmentWidth;

    // On Awake, we ensure we have a reference to the camera and calculate the width of the segment prefab for proper spacing
    private void Awake()
    {
        if (followCamera == null && Camera.main != null)
            followCamera = Camera.main.transform;

        segmentWidth = GetSegmentWidth(segmentPrefab);
    }

    // Initial spawn of segments to create a runway for the player at the start of the game
    private void Start()
    {
        if (player != null)
            ResetAndBuild();
    }

    // Continuously check if we need to spawn new segments ahead of the camera and clean up old ones behind it
    private void Update()
    {
        if (player == null || segmentPrefab == null || followCamera == null) return;

        float camX = followCamera.position.x;

        while (lastEndX < camX + spawnAheadDistance)
            SpawnNext();

        Cleanup(camX);
    }

    // Called by GameManager once it has the player reference
    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
        ResetAndBuild();
    }

    // Called by GameManager on restart if you want the world rebuilt
    public void ResetAndBuild()
    {
        // Clear existing platforms
        for (int i = 0; i < activeSegments.Count; i++)
        {
            if (activeSegments[i] != null)
                Destroy(activeSegments[i]);
        }
        activeSegments.Clear();

        // Reset spawn position near player
        float startX = player != null ? player.position.x : 0f;

        lastEndX = startX - (segmentWidth * 0.5f);

        // Build initial runway
        for (int i = 0; i < initialSegments; i++)
            SpawnNext();
    }

    
    private void SpawnNext()
    {
        float gap = Random.Range(gapRange.x, gapRange.y);
        float heightOffset = Random.Range(heightOffsetRange.x, heightOffsetRange.y);

        float centerX = lastEndX + gap + (segmentWidth * 0.5f);
        float y = SpawnY(heightOffset);

        GameObject seg = Instantiate(segmentPrefab, new Vector3(centerX, y, 0f), Quaternion.identity);
        activeSegments.Add(seg);

        lastEndX = centerX + (segmentWidth * 0.5f);
    }

    // Remove segments that are far behind the camera
    private void Cleanup(float camX)
    {
        while (activeSegments.Count > 0)
        {
            GameObject first = activeSegments[0];
            if (first == null)
            {
                activeSegments.RemoveAt(0);
                continue;
            }

            float firstCenterX = first.transform.position.x;
            float firstRightEdgeX = firstCenterX + (segmentWidth * 0.5f);

            if (firstRightEdgeX < camX - despawnBehindDistance)
            {
                Destroy(first);
                activeSegments.RemoveAt(0);
                Debug.Log($"[Spawner] Removing segment: {first.name} at x={first.transform.position.x:0.00}");
            }
            else
            {
                break;
            }
        }
    }

    // Utility to determine width of the segment prefab for proper spacing
    private float GetSegmentWidth(GameObject prefab)
    {
        if (prefab == null) return 1f;

        var box = prefab.GetComponent<BoxCollider2D>();
        if (box != null)
            return box.size.x * prefab.transform.lossyScale.x;

        var sr = prefab.GetComponent<SpriteRenderer>();
        if (sr != null && sr.sprite != null)
            return sr.bounds.size.x;

        return 1f;
    }
    
    private float SpawnY(float heightOffset)
    {
        var box = segmentPrefab.GetComponent<BoxCollider2D>();

        if (box != null)
        {
            float halfHeight = box.size.y * 0.5f * segmentPrefab.transform.lossyScale.y;

            return baseY + heightOffset + halfHeight;
        }

        return baseY + heightOffset;
    }
}