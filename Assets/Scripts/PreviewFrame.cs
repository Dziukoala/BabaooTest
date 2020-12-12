using UnityEngine;
using System.Collections.Generic;

public class PreviewFrame : MonoBehaviour
{
    public List<Transform> TilesTransform = new List<Transform>();

    [HideInInspector]
    public GameObject LastTile;

    public void UpdatePreview(List<Transform> gameTiles)
    {
        for(int i = 0; i < TilesTransform.Count; i++)
        {
            foreach(Transform T in gameTiles)
            {
                if(T.name == TilesTransform[i].name)
                {
                    TilesTransform[i].localPosition = T.localPosition;
                }
            }
        }
    }
}