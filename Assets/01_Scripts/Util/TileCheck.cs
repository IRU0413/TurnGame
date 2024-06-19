using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class TileCheck : MonoBehaviour
{
    private Camera _mainCamera;

    [SerializeField]
    private Tilemap _tileMap;
    [SerializeField]
    private TileBase _setTile;

    [SerializeField]
    private Vector2Int _tilemapCell;
    [SerializeField]
    private Vector3 _clickPos;

    private bool _isClicked = false;
    private bool _isClickable = false;

    public bool IsClickable
    {
        get { return _isClickable; }
        set { _isClickable = value; }
    }

    public bool IsClicked => _isClicked;

    public Tilemap Tilemap
    {
        get { return _tileMap; }
        set { _tileMap = value; }
    }

    public Vector2Int TileNodePos
    {
        get
        {
            return _tilemapCell;
        }
    }

    // Ready is called before the first frame update
    void Start()
    {
        _mainCamera = GetComponent<Camera>();
        _isClicked = false;
        if (_mainCamera == null)
        {
            _mainCamera = FindObjectOfType<Camera>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isClickable)
        {
            _isClicked = false;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);


            if (!hit)
                return;
            _tilemapCell = new Vector2Int((int)hit.transform.localPosition.x, (int)hit.transform.localPosition.y);
            hit.collider.GetComponent<SpriteRenderer>().color = Color.red;
            hit.collider.transform.localScale = Vector3.one * 1.2f;

            _isClicked = true;
        }
        // DebugPrint.LogPrint("Update");
    }

    public Vector2Int GetTileCellPos()
    {
        return _tilemapCell;
    }

    IEnumerator OnMouseUp()
    {
        Debug.Log($"EventSystem.current{EventSystem.current}");

        /*if (EventSystem.current.IsPointerOverGameObject())
        {
            yield break;
        }*/

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

        hit.collider.GetComponent<SpriteRenderer>().color = Color.red;
        yield return null;

        Vector3 scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 tilePos = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, scrSpace.z)) - transform.position;

        /*_tilemapCell = _tileMap.LocalToCell(tilePos);
        _clickPos = _tileMap.GetCellCenterLocal(_tilemapCell);

        _tileMap.SetTile(_tilemapCell, _setTile);*/

        Debug.Log($"타일 체크 {_clickPos}");
        yield return null;
    }
}
