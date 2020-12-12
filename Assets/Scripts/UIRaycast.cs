using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIRaycast : MonoBehaviour
{
    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;
    private PointerEventData eventData;
    private TaquinController taquinController;

    private void Awake()
    {
        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = FindObjectOfType<EventSystem>();
        eventData = new PointerEventData(eventSystem);
        taquinController = FindObjectOfType<TaquinController>();
    }

    // Update is called once per frame
    void Update()
    {
        Touch[] touches = Input.touches;

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                eventData.position = touch.position;

                List<RaycastResult> results = new List<RaycastResult>();

                raycaster.Raycast(eventData, results);

                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.GetComponent<Button>() != null)
                    {
                        taquinController.NewTileSelected(result.gameObject.name);
                    }
                }
            }
        }
    }
}
