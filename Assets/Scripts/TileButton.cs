using UnityEngine;
using UnityEngine.UI;

public class TileButton : MonoBehaviour
{
    private TaquinController GlobTaquinController;
    private Button SelfButton;

    private void Awake()
    {
        GlobTaquinController = FindObjectOfType<TaquinController>();
        SelfButton = GetComponent<Button>();
    }

    private void OnMouseOver()
    {
        GlobTaquinController.NewTileSelected(name);
    }
}