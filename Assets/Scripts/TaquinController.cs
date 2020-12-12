using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TaquinController : MonoBehaviour
{
    private GameController GlobGameController;
    private Transform SelfTransform;
    private PreviewFrame GlobPreviewFrame;

    public GameObject TilePrefab;
    public List<Sprite> AndroidTiles;
    public Sprite AndroidLastTile;
    public List<Sprite> IOSTiles;
    public Sprite IOSLastTile;
    public GameObject PreviewTilePrefab;
    public GameObject LastTilePrefab;

    private string[,] TaquinFrame = new string[3, 3]; 
    private List<Sprite> Tiles;
    private Sprite LastTileSprite;
    private string EmptyTile;
    private List<Tile> TilesScript = new List<Tile>();
    private List<Transform> TilesTransform = new List<Transform>();
    private Tile SelectedTile;
    private GameObject LastTile;

    private void Awake()
    {
        GlobGameController = FindObjectOfType<GameController>();
        SelfTransform = GetComponent<Transform>();
        GlobPreviewFrame = FindObjectOfType<PreviewFrame>();
    }

    public void Initialize()
    {
        if (SystemInfo.operatingSystem.Contains("Android"))
        {
            Tiles = AndroidTiles;
            LastTileSprite = AndroidLastTile;
        }
        else
        {
            Tiles = IOSTiles;
            LastTileSprite = IOSLastTile;
        }

        EmptyTile = Random.Range(0, 2).ToString() + Random.Range(0, 2).ToString();
        List<int> availableTile = new List<int>();
        for(int i = 0; i < Tiles.Count; i++)
        {
            availableTile.Add(i);
        }

        Transform previewFrameTransform = GlobPreviewFrame.GetComponent<Transform>();

        for (int X = 0; X < 3; X++)
        {
            for(int Y = 2; Y >= 0; Y--)
            {
                if(X.ToString() + Y.ToString() != EmptyTile)
                {
                    GameObject newTile = Instantiate(TilePrefab, SelfTransform);
                    int availableTileChosen = Random.Range(0, availableTile.Count);
                    int tileNumber = availableTile[availableTileChosen];
                    availableTile.RemoveAt(availableTileChosen);
                    newTile.GetComponentInChildren<SpriteRenderer>().sprite = Tiles[tileNumber];

                    GameObject newPreviewTile = Instantiate(PreviewTilePrefab, previewFrameTransform);
                    newPreviewTile.GetComponent<SpriteRenderer>().sprite = Tiles[tileNumber];

                    if (tileNumber > (Tiles.Count - 1) * 0.5f)  //Here to avoid the center tile
                    {
                        tileNumber++;
                    }
                    newTile.name = (tileNumber % 3).ToString() + (tileNumber / 3).ToString();
                    newTile.GetComponent<Transform>().localPosition = new Vector3(X, 2 - Y, 0);

                    TaquinFrame[X,Y] = newTile.name;
                    TilesScript.Add(newTile.GetComponent<Tile>());
                    TilesTransform.Add(newTile.GetComponent<Transform>());

                    //Create the preview

                    newPreviewTile.name = newTile.name;
                    newPreviewTile.GetComponent<Transform>().localPosition = new Vector3(X, 2 - Y, 0);
                    GlobPreviewFrame.TilesTransform.Add(newPreviewTile.GetComponent<Transform>()); 
                }
                else
                {
                    LastTile = Instantiate(LastTilePrefab, SelfTransform);
                    LastTile.GetComponent<Transform>().localPosition = new Vector3(1, 1, 0);
                    LastTile.GetComponentInChildren<SpriteRenderer>().sprite = LastTileSprite;
                    LastTile.name = "11";
                    LastTile.SetActive(false);

                    TaquinFrame[X,Y] = "Empty";

                    GameObject newPreviewTile = Instantiate(PreviewTilePrefab, previewFrameTransform);
                    newPreviewTile.GetComponent<SpriteRenderer>().sprite = LastTileSprite;
                    newPreviewTile.name = "11";
                    newPreviewTile.GetComponent<Transform>().localPosition = new Vector3(1, 1, 0);
                    newPreviewTile.SetActive(false);
                    GlobPreviewFrame.LastTile = newPreviewTile;
                }
            }
        }

        MakeTaquinWinable();
    }

    private void MakeTaquinWinable()
    {
        string[,] solutionFrame = new string[3, 3];
        for(int X = 0; X < 3; X++)
        {
            for(int Y = 0; Y < 3; Y++)
            {
                solutionFrame[X,Y] = TaquinFrame[X,Y];
            }
        }

        bool isEmptyPair;

        if((Mathf.Abs(int.Parse(EmptyTile.Substring(0, 1))) + Mathf.Abs(int.Parse(EmptyTile.Substring(EmptyTile.Length - 1, 1)))) % 2 == 0)
        {
            isEmptyPair = true;
        }
        else
        {
            isEmptyPair = false;
        }

        int switchCount = 0;

        for(int X = 0; X < 3; X++)
        {
            for(int Y = 0; Y < 3; Y++)
            {
                while(solutionFrame[X,Y] != X.ToString() + Y.ToString())
                {
                    if (solutionFrame[X, Y] == "Empty")
                    {
                        if (X != 1 || Y != 1)
                        {
                            solutionFrame[X, Y] = solutionFrame[1, 1];
                            solutionFrame[1, 1] = "Empty";
                            switchCount++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        int newX = int.Parse(solutionFrame[X, Y].Substring(0, 1));
                        int newY = int.Parse(solutionFrame[X, Y].Substring(solutionFrame[X, Y].Length - 1, 1));

                        solutionFrame[X, Y] = solutionFrame[newX, newY];
                        solutionFrame[newX, newY] = newX.ToString() + newY.ToString();
                        switchCount++;
                    }
                }
            }
        }

        if (isEmptyPair != (switchCount % 2 == 0))
        {
            int firstX = Random.Range(0, 2);
            int firstY = Random.Range(0, 2);
            int secondX = Random.Range(0, 2);
            int secondY = Random.Range(0, 2);

            string bufferSave = TaquinFrame[firstX, firstY];
            TaquinFrame[firstX, firstY] = TaquinFrame[secondX, secondY];
            TaquinFrame[secondX, secondY] = bufferSave;

            MakeTaquinWinable();
        }
    }

    public void NewTileSelected(string tileName)
    {
        int newX = int.Parse(tileName.Substring(0, 1));
        int newY = int.Parse(tileName.Substring(tileName.Length - 1, 1));

        SelectedTile = TilesScript.Where(tile => tile.name == TaquinFrame[newX, newY]).First();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NewTilesPosition();
        }

        if(Input.touchCount > 0)
        {
            SelectedTile.Move();

            GlobGameController.StartGame();
        }

        GlobPreviewFrame.UpdatePreview(TilesTransform);
    }

    public void NewTilesPosition()
    {
        foreach(Tile tile in TilesScript)
        {
            Vector2Int newPos = tile.GetPosition();
            TaquinFrame[newPos.x, newPos.y] = tile.name;
        }

        if (!GlobGameController.IsGameFinished)
        {
            CheckIfWin();
        }
    }

    private void CheckIfWin()
    {
        for(int X = 0; X < 3; X++)
        {
            for(int Y = 2; Y >= 0; Y--)
            {
                if (X != 1 || Y != 1)
                {
                    if (TaquinFrame[X, Y] != X.ToString() + Y.ToString())
                    {
                        return;
                    }
                }
            }
        }

        LastTile.SetActive(true);
        GlobGameController.Win();
    }
}