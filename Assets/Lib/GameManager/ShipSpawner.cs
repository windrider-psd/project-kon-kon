using UnityEngine;
using UnityEditor;


[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class ShipSpawner : MonoBehaviour
{
    public SpaceEntityClassId id;

    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;

    private Vector3 editorScale;

   

    public ShipSpawnSettings spawnSettings;


    [Header("AI Settings")]
    public bool isAi = true;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindAnyObjectByType<GameManager>();

        if (Application.isPlaying)
        {
            gameManager.SpawnShip(
                this.id,
                this.isAi,
                this.transform.position,
                spawnSettings
            );
            Destroy(this.gameObject);
        }
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        EditorApplication.delayCall -= UpdateEditor;
        EditorApplication.delayCall += UpdateEditor;
#endif
    }

#if UNITY_EDITOR
    private void UpdateEditor()
    {
        // Object may have been destroyed before the delayed call
        if (this == null)
            return;

        if (Application.isPlaying)
            return;

        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindAnyObjectByType<GameManager>();

        if (gameManager == null)
            return;

        var go = gameManager.GetShipGO(id);

        if (go == null)
            return;

        var sprite = go.GetComponentInChildren<SpriteRenderer>();

        if (sprite == null)
            return;

        editorScale = sprite.transform.localScale;

        spriteRenderer.sprite = sprite.sprite;
        transform.localScale = editorScale;
    }
#endif
}

public static class ShipSpawnerEditor
{
    [MenuItem("GameObject/Ship Spawner", false, 10)]
    private static void CreateShipSpawner(MenuCommand menuCommand)
    {
        // Create the GameObject
        GameObject go = new GameObject("Ship Spawner");

        // Add your component
        go.AddComponent<ShipSpawner>();

        // If something was selected, make the new object a child of it
        GameObject parent = menuCommand.context as GameObject;

        if (parent != null)
        {
            go.transform.SetParent(parent.transform);
        }

        // Register with Unity's undo system
        Undo.RegisterCreatedObjectUndo(go, "Create Ship Spawner");

        // Select the new object
        Selection.activeGameObject = go;
    }

    [MenuItem("GameObject/Ship Spawner", true)]
    private static bool ValidateCreateShipSpawner()
    {
        return true;
    }
}