using UnityEngine;

public class HexMapEditor : MonoBehaviour
{
    float[] xMin, xMax, y, zMin, zMax;
    int zoomLevel = 2;

    // 50 / 200 / 5 / 150

    public Color[] colors;

    public HexGrid hexGrid;

    public bool ignoreNextInput,
                selecting;

    public Transform cameraTarget;

    public float camSensitivity;

    Color activeColor;

    Vector3 startPos;

    Ray ray;
    RaycastHit hit;

    void Awake()
    {
        SelectColor(0);
        ignoreNextInput = false;
        selecting = false;
        CreateBounds();
    }



    void AdjustToEdge()
    {
        if (cameraTarget.position.x < xMin[zoomLevel])
            cameraTarget.position = new Vector3(xMin[zoomLevel] + 1, cameraTarget.position.y, cameraTarget.position.z);
        if (cameraTarget.position.x > xMax[zoomLevel])
            cameraTarget.position = new Vector3(xMax[zoomLevel] - 1, cameraTarget.position.y, cameraTarget.position.z);
        if (cameraTarget.position.z < zMin[zoomLevel])
            cameraTarget.position = new Vector3(cameraTarget.position.x, cameraTarget.position.y, zMin[zoomLevel] + 1);
        if (cameraTarget.position.z > zMax[zoomLevel])
            cameraTarget.position = new Vector3(cameraTarget.position.x, cameraTarget.position.y, zMax[zoomLevel] - 1);
    }


    void CreateBounds()
    {
        xMin = new float[5];
        xMax = new float[5];
        zMin = new float[5];
        zMax = new float[5];
        y = new float[5];

        xMin[0] = 10;
        xMax[0] = 235;
        y[0] = 56;
        zMin[0] = 3;
        zMax[0] = 200;

        xMin[1] = 25;
        xMax[1] = 225;
        y[1] = 76;
        zMin[1] = 11;
        zMax[1] = 185;

        xMin[2] = 40;
        xMax[2] = 210;
        y[2] = 96;
        zMin[2] = 15;
        zMax[2] = 175;

        xMin[3] = 45;
        xMax[3] = 205;
        y[3] = 106;
        zMin[3] = 20;
        zMax[3] = 165;

        xMin[4] = 60;
        xMax[4] = 190;
        y[4] = 126;
        zMin[4] = 25;
        zMax[4] = 150;
    }

    void Update()
    {
        //tile interaction
        if (Input.GetMouseButtonUp(0))
        {
            if (!ignoreNextInput)
            {
                HandleInput("menu");
            }
            else
            {
                ignoreNextInput = false;
            }
        }

        //quick actions
        if (Input.GetMouseButton(1))
        {
            if (!ignoreNextInput)
            {
                if (!selecting)
                {
                    hexGrid.RemoveSelection();
                }
                HandleInput("select");
                selecting = true;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            selecting = false;
            HandleInput("menuselect");
        }

        //camera movement
        if (Input.GetMouseButtonDown(2))
        {
            startPos = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            Vector3 delta = (Input.mousePosition - startPos) / (camSensitivity * 5);
            //Debug.Log(Input.mousePosition.ToString() + " " + startPos.ToString());

            if (!delta.Equals(Vector3.zero))
            {
                //Debug.Log(delta.x + "," + delta.y + "," + delta.z);
                cameraTarget.Rotate(Vector3.right, -80.0f);
                cameraTarget.Translate(-delta.x, 0.0f, -delta.y);
                AdjustToEdge();
                cameraTarget.Rotate(Vector3.right, 80.0f);
                //cameraTarget.transform.position = new Vector3(-delta.x, 0.0f, -delta.y, Space.Self);
            }
        }

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (selecting == false)
            {
                hexGrid.SelectOutline(hit.point);
            }
        }


        var zoom = Input.GetAxis("Mouse ScrollWheel");
        if (zoom != 0)
        {
            int currentZoom = zoomLevel;
            if (zoom > 0)
                zoomLevel = Mathf.Max(zoomLevel - 1, 0);
            if (zoom < 0)
                zoomLevel = Mathf.Min(zoomLevel + 1, 4);

            if (currentZoom != zoomLevel)
            {
                cameraTarget.Rotate(Vector3.right, -80.0f);
                //cameraTarget.Translate(0.0f, -zoom * 100.0f, 0.0f);
                cameraTarget.position = new Vector3(cameraTarget.position.x, y[zoomLevel], cameraTarget.position.z);
                AdjustToEdge();
                cameraTarget.Rotate(Vector3.right, 80.0f);
            }
        }
    }

    void HandleInput(string type)
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(inputRay, out hit))
        {
            //hexGrid.ColorCell(hit.point, activeColor);
            switch (type)
            {
                case "menu":
                    hexGrid.SelectCell(hit.point, false);
                    break;
                case "select":
                    hexGrid.SelectOutline(hit.point, selecting);
                    break;
                case "menuselect":
                    hexGrid.SelectCell(hit.point, true);
                    break;
            }
        }
    }

    public void SelectColor(int index)
    {
        activeColor = colors[index];
    }
}