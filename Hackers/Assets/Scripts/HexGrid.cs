using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {

    public HexMapEditor mapEditor;


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

    public Vector3 menuOffset;

    public Transform menuPos;

    public Animator menuAnimator;

    Canvas gridCanvas;
    HexMesh hexMesh;

    HexCell[] cells;

    int selectedIndex;


    void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();
        cells = new HexCell[height * width];

        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }

		contentGenerator.generateRandomUnits(0, cells);

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
        selectedIndex = -1;
    }

    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        // generate cells matrix previously
        // determine cells[i] type

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        
        // switch (type)
        // case terrain: cell.terrain = ...
        // etc.
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = defaultColor;
        
        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();

		//initialize cell population
		cell.population = 0;
    }

    public void SelectCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;

        if (index != selectedIndex && index < cells.Length)
        {
            selectedIndex = index;
            HexCell cell = cells[index];
            //cell.color = color;
            //hexMesh.Triangulate(cells);

            RectTransform rTrans = menuPos as RectTransform;
            rTrans.anchoredPosition3D = new Vector3(cell.transform.position.x + menuOffset.x, cell.transform.position.z + menuOffset.y, menuOffset.z);
            rTrans.SetAsLastSibling();
            tileMenu.SetActive(true);
            menuAnimator.Play("Menu", -1, 0f);
        }
        else
        {
            HideMenu(false);
        }
    }

    public void Build()
    {
        HexCell cell = cells[selectedIndex];
        cell.color = buildColor;
        hexMesh.Triangulate(cells);
        HideMenu(true);
    }

    public void ClearTile()
    {
        HexCell cell = cells[selectedIndex];
        cell.color = defaultColor;
        hexMesh.Triangulate(cells);
        HideMenu(true);
    }

    public void Upgrade()
    {
        HexCell cell = cells[selectedIndex];
        cell.color = touchedColor;
        hexMesh.Triangulate(cells);
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
        HexCell cell = cells[index];
        //cell.color = color;
        //hexMesh.Triangulate(cells);

        RectTransform rTrans = menuPos as RectTransform;
        rTrans.anchoredPosition3D = new Vector3(cell.transform.position.x + menuOffset.x, cell.transform.position.z + menuOffset.y, menuOffset.z);
        rTrans.SetAsLastSibling();
    }

}
