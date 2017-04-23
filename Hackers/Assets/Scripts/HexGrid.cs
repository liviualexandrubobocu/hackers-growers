using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class HexGrid : MonoBehaviour
{

    public HexMapEditor mapEditor;
    public Image HexCellOutline;
    public Image RadiusOutline;

    public GameObject tileMenu;
    public GameObject infoPanel;

    public Color defaultColor = Color.white;
    public Color touchedColor = Color.magenta;
    public Color buildColor = Color.cyan;
    public Color upgradeColor = Color.red;

    public int width = 6;
    public int height = 6;

    public ContentGenerator contentGenerator;

    public HexCell cellPrefab;
    public Text cellLabelPrefab;
    public Image workerUnitPrefab;

    public Vector3 menuOffset;

    public Transform menuPos, highlightPos;

    public Animator menuAnimator;

    Canvas gridCanvas;
    HexMesh hexMesh;

    HexCell[][] cells;
    
    List<Image> selectionPath, selectionRadius;
    Image highlightOutline;
    Position selectedCellPos;

    int selectionMaxLength = 2;

    bool radiusEnabled;

    HexCell CurrentCell
    {
        get
        {
            if (selectedCellPos != null)
                return cells[selectedCellPos.X][selectedCellPos.Y];
            return null;
        }
    }

    void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();
        cells = new HexCell[height][];
        for (int i = 0; i < height; i++)
        {
            cells[i] = new HexCell[width];
        }
        contentGenerator = new ContentGenerator();
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z);
            }
        }

        selectionPath = new List<Image>();
        selectionRadius = new List<Image>();
        radiusEnabled = true;

        //contentGenerator.generateRandomUnits(0, cells);
        highlightOutline = Instantiate<Image>(HexCellOutline);
        highlightOutline.gameObject.SetActive(false);
        highlightOutline.rectTransform.SetParent(gridCanvas.transform, false);
        highlightOutline.rectTransform.SetAsLastSibling();
        HideMenu(false);
        infoPanel.SetActive(false);
    }

    void Start()
    {
        hexMesh.Triangulate(cells);
    }

    void HideMenu(bool ignoreInput)
    {
        mapEditor.ignoreNextInput = ignoreInput;
        tileMenu.SetActive(false);
        selectedCellPos = null;
    }

    void PopulateCell(ref HexCell targetCell)
    {
        targetCell.population = 0;
        targetCell.unit = new LifeForm(0, 0, 0, 0, 0);
        targetCell.SetActionRadius(1);

        Vector2 allPosition = new Vector2(targetCell.transform.localPosition.x + menuOffset.x, targetCell.transform.localPosition.z + menuOffset.y * 2);
        //Text label = Instantiate<Text>(cellLabelPrefab);
        if (targetCell.unit != null) {
            //TO DO - logic to figure out each unit type here
            Image workerUnit = Instantiate(workerUnitPrefab);
            workerUnit.rectTransform.SetParent(gridCanvas.transform, false);
            workerUnit.rectTransform.anchoredPosition = new Vector2(targetCell.transform.localPosition.x + menuOffset.x, targetCell.transform.localPosition.z + menuOffset.y * 1.4f);

            Text unitText = Instantiate(cellLabelPrefab);
            unitText.text = "4";
            unitText.rectTransform.SetParent(gridCanvas.transform, false);
            unitText.rectTransform.anchoredPosition = new Vector2(targetCell.transform.localPosition.x + menuOffset.x, targetCell.transform.localPosition.z + menuOffset.y * 0.5f);
        }
        //label.rectTransform.SetParent(gridCanvas.transform, false);
        //label.rectTransform.anchoredPosition = new Vector2(targetCell.transform.localPosition.x + menuOffset.x, targetCell.transform.localPosition.z + menuOffset.y);
        //label.text = targetCell.coordinates.ToStringOnSeparateLines();

        //initialize cell population
    }

    void CreateCell(int x, int z)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        // generate cells matrix previously
        // determine cells[i] type

        HexCell cell = cells[z][x] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;

        // switch (type)
        // case terrain: cell.terrain = ...
        // etc.
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        PopulateCell(ref cell);
        
        
    }

    Position GetCellIndex(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        return new Position(index / width, index % height);
    }

    void ShowRadius(Position cellPos)
    {
        RemoveRadius();
        List<Position> neighbours = GetNeighbours(cellPos, cells[cellPos.X][cellPos.Y].GetActionRadius());
        foreach (Position pos in neighbours)
        {
            Image outline = Instantiate<Image>(RadiusOutline);
            outline.transform.SetParent(gridCanvas.transform, false);
            RectTransform rTranss = outline.rectTransform;
            rTranss.anchoredPosition3D = new Vector3(cells[pos.X][pos.Y].transform.position.x + menuOffset.x, cells[pos.X][pos.Y].transform.position.z + menuOffset.y, menuOffset.z);
            rTranss.SetAsLastSibling();
            selectionRadius.Add(outline);
        }
    }

    void RemoveRadius()
    {
        foreach (Image tile in selectionRadius)
        {
            Destroy(tile.gameObject);
        }
        selectionRadius.Clear();
    }

    public void SelectOutline(Vector3 position)
    {
        Position cellPos = GetCellIndex(position);
        if (cellPos.IsValid(height, width)) {
            highlightOutline.rectTransform.anchoredPosition3D = new Vector3(cells[cellPos.X][cellPos.Y].transform.position.x + menuOffset.x, cells[cellPos.X][cellPos.Y].transform.position.z + menuOffset.y, 0);
            highlightOutline.gameObject.SetActive(true);
        }
    }

    public void SelectCell(Vector3 position, bool fromPath)
    {
        Position cellPos = GetCellIndex(position);
        if (!cellPos.Equals(selectedCellPos) && cellPos.IsValid(height, width))
        {
            selectedCellPos = cellPos;
            Debug.Log(cellPos);
            HexCell cell = cells[cellPos.X][cellPos.Y];
            ShowRadius(cellPos);
          
            //cell.color = color;
            //hexMesh.Triangulate(cells);

            RectTransform rTrans = menuPos as RectTransform;
            rTrans.anchoredPosition3D = new Vector3(cell.transform.position.x + menuOffset.x, cell.transform.position.z + menuOffset.y, menuOffset.z);
            rTrans.SetAsLastSibling();
            tileMenu.SetActive(true);
            menuAnimator.Play("Menu", -1, 0f);

            if (!fromPath)
            {
                RemoveSelection();
            }
        }
        else
        {
            HideMenu(false);
            RemoveRadius();
            RemoveSelection();
        }
    }

    void isInRadius(int index, int center, int radius)
    {
        //even row
        if ((selectedCellPos.X % height) % 2 == 0)
        {

        }
    }

    List<Position> GetNeighbours(Position pos, int radius)
    {
        if (radius != 0)
        {
            List<Position> neighbours = new List<Position>();
            int offset = pos.X % 2;
            Position topLeft = new Position(pos.X + 1, pos.Y - 1 + offset);
            if (topLeft.IsValid(height, width))
            {
                neighbours.Add(topLeft);
                neighbours.AddRange(GetNeighbours(topLeft, radius - 1));
            }
            Position topRight = new Position(pos.X + 1, pos.Y + offset);
            if (topRight.IsValid(height, width))
            {
                neighbours.Add(topRight);
                neighbours.AddRange(GetNeighbours(topRight, radius - 1));
            }
            Position left = new Position(pos.X, pos.Y - 1);
            if (left.IsValid(height, width))
            {
                neighbours.Add(left);
                neighbours.AddRange(GetNeighbours(left, radius - 1));
            }
            Position right = new Position(pos.X, pos.Y + 1);
            if (right.IsValid(height, width))
            {
                neighbours.Add(right);
                neighbours.AddRange(GetNeighbours(right, radius - 1));
            }
            Position bottomLeft = new Position(pos.X - 1, pos.Y - 1 + offset);
            if (bottomLeft.IsValid(height, width))
            {
                neighbours.Add(bottomLeft);
                neighbours.AddRange(GetNeighbours(bottomLeft, radius - 1));
            }
            Position bottomRight = new Position(pos.X - 1, pos.Y + offset);
            if (bottomRight.IsValid(height, width))
            {
                neighbours.Add(bottomRight);
                neighbours.AddRange(GetNeighbours(bottomRight, radius - 1));
            }
            return neighbours.Distinct().ToList();
        }
        return new List<Position>();
    }

    public void SelectOutline(Vector3 position, bool isCurrent)
    {
        int radius = 1;
        if (!isCurrent)
        {
            HideMenu(false);
            RemoveSelection();
        }

        Position cellPos = GetCellIndex(position);
        if (!cellPos.Equals(selectedCellPos) && cellPos.IsValid(height, width))
        {
            HexCell cell = cells[cellPos.X][cellPos.Y];
            Image selOutline = Instantiate(HexCellOutline);
            selOutline.transform.SetParent(gridCanvas.transform, false);
            if (selectionPath.Count == selectionMaxLength)
            {
                Destroy(selectionPath[selectionMaxLength - 1].gameObject);
                selectionPath.RemoveAt(selectionMaxLength - 1);
            }
            selectionPath.Add(selOutline);
            RectTransform rTrans = selOutline.rectTransform;
            rTrans.anchoredPosition3D = new Vector3(cell.transform.position.x + menuOffset.x, cell.transform.position.z + menuOffset.y, menuOffset.z);
            rTrans.SetAsLastSibling();
        }
    }

    public void RemoveSelection()
    {
        foreach (Image tile in selectionPath)
        {
            Destroy(tile.gameObject);
        }
        selectionPath.Clear();
    }


    public void Build()
    {
        CurrentCell.color = buildColor;
        hexMesh.Triangulate(cells);
        RemoveSelection();
        HideMenu(true);
    }

    public void ClearTile()
    {
        CurrentCell.color = defaultColor;
        hexMesh.Triangulate(cells);
        RemoveSelection();
        HideMenu(true);
    }

    public void Upgrade()
    {
        CurrentCell.color = touchedColor;
        hexMesh.Triangulate(cells);
        RemoveSelection();
        HideMenu(true);
    }

    public void ShowInfo()
    {
        infoPanel.SetActive(true);
        HideMenu(true);
    }

    public void HideInfo()
    {
        infoPanel.SetActive(false);
        HideMenu(true);
    }

    public void ColorCell(Vector3 position, Color color)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        HexCell cell = cells[index / width][index % height];
        //cell.color = color;
        //hexMesh.Triangulate(cells);

        RectTransform rTrans = menuPos as RectTransform;
        rTrans.anchoredPosition3D = new Vector3(cell.transform.position.x + menuOffset.x, cell.transform.position.z + menuOffset.y, menuOffset.z);
        rTrans.SetAsLastSibling();
    }

}
