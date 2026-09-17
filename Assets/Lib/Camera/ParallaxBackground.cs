using UnityEngine;

public class InfiniteParallaxBackground : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Sprite backgroundSprite;

    [Header("Parallax")]
    [SerializeField, Range(0f, 1f)]
    private float parallaxAmount = 0.1f;

    [Header("Background Size")]
    [SerializeField] private float tileScale = 1f;

    [Header("Tiles")]
    [SerializeField] private int tilesX = 3;
    [SerializeField] private int tilesY = 3;

    private Transform[,] tiles;

    private float tileWidth;
    private float tileHeight;

    private Vector3 startingPosition;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        if (backgroundSprite == null)
        {
            Debug.LogError("Background Sprite is not assigned!");
            enabled = false;
            return;
        }

        tilesX = Mathf.Max(3, tilesX);
        tilesY = Mathf.Max(3, tilesY);

        startingPosition = transform.position;

        CreateTiles();
    }

    void CreateTiles()
    {
        // Get the sprite's original size.
        float originalWidth = backgroundSprite.bounds.size.x;
        float originalHeight = backgroundSprite.bounds.size.y;

        // Apply our scale.
        tileWidth = originalWidth * tileScale;
        tileHeight = originalHeight * tileScale;

        tiles = new Transform[tilesX, tilesY];

        int centerX = tilesX / 2;
        int centerY = tilesY / 2;

        for (int x = 0; x < tilesX; x++)
        {
            for (int y = 0; y < tilesY; y++)
            {
                GameObject tile = new GameObject(
                    $"SkyTile_{x}_{y}"
                );

                tile.transform.SetParent(transform);

                tile.transform.localPosition = new Vector3(
                    (x - centerX) * tileWidth,
                    (y - centerY) * tileHeight,
                    0f
                );

                SpriteRenderer renderer =
                    tile.AddComponent<SpriteRenderer>();

                renderer.sprite = backgroundSprite;

                // Scale the sprite.
                tile.transform.localScale =
                    Vector3.one * tileScale;

                // Put background behind gameplay.
                renderer.sortingOrder = -100;

                tiles[x, y] = tile.transform;
            }
        }
    }

    void LateUpdate()
    {
        Vector3 cameraOffset =
            cameraTransform.position - startingPosition;

        transform.position =
            startingPosition +
            cameraOffset * parallaxAmount;

        RepositionTiles();
    }

    void RepositionTiles()
    {
        Vector3 cameraPosition = cameraTransform.position;

        float totalWidth = tileWidth * tilesX;
        float totalHeight = tileHeight * tilesY;

        foreach (Transform tile in tiles)
        {
            Vector3 position = tile.position;

            float differenceX =
                cameraPosition.x - position.x;

            float differenceY =
                cameraPosition.y - position.y;

            if (Mathf.Abs(differenceX) > tileWidth)
            {
                position.x +=
                    Mathf.Sign(differenceX) * totalWidth;
            }

            if (Mathf.Abs(differenceY) > tileHeight)
            {
                position.y +=
                    Mathf.Sign(differenceY) * totalHeight;
            }

            tile.position = position;
        }
    }
}