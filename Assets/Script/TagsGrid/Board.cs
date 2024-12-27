
using System.Collections.Generic;

using System.Linq;
using TMPro;
using UnityEngine;


public sealed class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }

    public Row[] rows;

    public int folderItemIndex;
    public bool defeat = false;

    public Tile[,] Tiles { get; private set; }

    public int width => Tiles.GetLength(0);
    public int height => Tiles.GetLength(1);

    public int scorePerMatch;
    public int currentScore = 0;
    int totalScore;
    public bool winning = false;
    public TextMeshProUGUI scoreText;
    public LineRendererUI lineRenderer;
    private void Awake() => Instance = this;

    private readonly List<Tile> _selection = new List<Tile>();
    private readonly int[] mRow = { -1, 0, 1, 0 }; 
    private readonly int[] mCol = {0, 1, 0, -1 };  

    public LineRendererUI matchLine;

    private void Start()
    {
        Tiles = new Tile[rows.Max(row => row.tiles.Length), rows.Length];

        scoreText.text = "Score: \n" + currentScore;
        ItemData.getItems(folderItemIndex);
        var items = ItemData.Items;
        int totalTiles = (width - 2) * (height - 2);
        totalScore = scorePerMatch * (totalTiles / 2);
        if (totalTiles % 2 != 0)
        {
            Debug.Log("Grid-Size not match ");
            return;
        }

        List<Item> itemPool = new List<Item>();
        foreach (var item in items)
        {
            itemPool.Add(item);
            itemPool.Add(item);
        }
        while (itemPool.Count < totalTiles)
        {
            var randomItem = items[Random.Range(0, items.Length)];
            itemPool.Add(randomItem);
            itemPool.Add(randomItem);
        }
        MixList(itemPool);
        int index = 0;
        for (int y = 0; y < height ; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var tile = rows[y].tiles[x];

                tile.x = x;
                tile.y = y;

                if(y > 0 && y < height - 1 && x > 0 && x < width - 1)
                {
                    tile.Item = itemPool[index];
                    index++;                
                }
                else
                {
                    tile.Item = null;
                    tile.ClearTile();
                }
                Tiles[x, y] = tile;

            }
        }
    }

    public void Select(Tile tile)
    {
        if (!_selection.Contains(tile)) _selection.Add(tile);

        if (_selection.Count < 2) return;

        //Debug.Log($"Seleted tiles at: ({_selection[0].x},{_selection[0].y}) and ({_selection[1].x},{_selection[1].y})");
        Match();   
        _selection.Clear();
       
    }
    List<(int, int)> shortestPath = new List<(int, int)> ();
    public bool CanMatch()
    {
        if (_selection[0].Item.value == _selection[1].Item.value)
        {
            int rows = width;
            int cols = height;
            int startX = _selection[0].x;
            int startY = _selection[0].y;
            int endX = _selection[1].x;
            int endY = _selection[1].y;
            bool[,] visited = new bool[rows, cols];
            Queue<Node> queue = new Queue<Node>();

            shortestPath = null;
            queue.Enqueue(new Node(startX, startY, -1, 0, new List<(int, int)>()));
            visited[startX, startY] = true;

            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();

                if (current.Row == endX && current.Col == endY)
                {
                    shortestPath = current.Path; 
                    return true;
                }

                for (int i = 0; i < 4; i++)
                {
                    int newRow = current.Row + mRow[i];
                    int newCol = current.Col + mCol[i];

                    if (IsValid(newRow, newCol, rows, cols) && !visited[newRow, newCol] && (Tiles[newRow, newCol].Item == null || (newRow == endX && newCol == endY)))
                    {
                        int newCornerCount = current.CornerCount + ((i != current.Direction && current.Direction != -1) ? 1 : 0);

                        if (newCornerCount <= 2)
                        {
                            queue.Enqueue(new Node(newRow, newCol, i, newCornerCount, current.Path)); 
                            visited[newRow, newCol] = true;
                        }
                    }
                }
            }

            return false;
        }
        return false;
    }
    private static bool IsValid(int row, int col, int rows, int cols) 
    { 
        return row >= 0 && row < rows && col >= 0 && col < cols;
    }
    void Match()
    {
        if (CanMatch())
        {
            if (_selection.Count > 1)
            {   

                if(AudioController.instance != null)
                {
                    AudioController.instance.PlaySFX(AudioController.instance.sfxSounds[0].name);
                }
                if(lineRenderer != null)
                {
                    Vector3[] linePoint = new Vector3[shortestPath.Count];
                    for(int i = 0; i < shortestPath.Count; i++)
                    {
                        int x = shortestPath[i].Item1;
                        int y = shortestPath[i].Item2;
                        linePoint[i] = Tiles[x, y].btn.transform.position;
                    }
                    lineRenderer.DrawLine(linePoint);
                }
                RemoveConnectedItems(Tiles, _selection[0], _selection[1]);
            }

        }
        else if (!CanMatch())
        {
            Debug.Log($"Seleted tiles at: ({_selection[0].x},{_selection[0].y}) and ({_selection[1].x},{_selection[1].y})");
            _selection.Clear();
            Debug.Log("can't match");
        }
    }
    void MixList(List<Item> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
    void RemoveConnectedItems(Tile[,] matrix, Tile tile1, Tile tile2)
    {
        int x1 = tile1.x;
        int y1 = tile1.y;
        int x2 = tile2.x;
        int y2 = tile2.y;
        matrix[x1, y1].ClearTile();
        matrix[x2, y2].ClearTile();
        currentScore += scorePerMatch;
        scoreText.text = "Score: \n" + currentScore;
        if (currentScore >= totalScore) winning = true;
    }

    private void Update()
    {
        if (winning || defeat)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        
    }

}
public class Node 
{
    public int Row { get; }
    public int Col { get; }
    public int Direction { get; }
    public int CornerCount { get; }
    public List<(int, int)> Path { get; }

    public Node(int row, int col, int direction, int cornerCount, List<(int, int)> path)
    {
        Row = row;
        Col = col;
        Direction = direction;
        CornerCount = cornerCount;
        Path = new List<(int, int)>(path) { (row, col) };
    }
}