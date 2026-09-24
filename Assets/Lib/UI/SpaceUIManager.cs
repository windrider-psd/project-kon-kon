using UnityEngine;

public class SpaceUIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private CameraSpaceEntityRaycast raycast;

    [Header("Selection Panel")]
    public SpaceEntitySelectionPanel selectionPanel;
    void Start()
    {
        raycast = FindAnyObjectByType<CameraSpaceEntityRaycast>();
        raycast.onSpaceEntitySelected += OnSpaceEntitySelected;
    }


    private void OnSpaceEntitySelected(SpaceEntity e)
    {
        if(e != null) {
            selectionPanel.text.text = e.name;
            selectionPanel.image.sprite = e.GetComponentInChildren<SpriteRenderer>().sprite;
            selectionPanel.gameObject.SetActive(true);
        }
        else
        {
            selectionPanel.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
