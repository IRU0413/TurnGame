using System.Collections.Generic;
using UnityEngine;

public class TileClicker : MonoBehaviour
{
    [SerializeField]
    private Vector3 _offset;

    private Color _ableColor = Color.green;
    private Color _notAbleColor = Color.red;

    private Transform _tr;

    private HashSet<Vector2Int> _checkTileList;
    private GameObject _hitTile;

    private void Start()
    {
        _tr = GetComponent<Transform>();
        _hitTile = null;
    }

    private void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector2Int posInt = new Vector2Int((int)pos.x, (int)pos.y);

        _tr.position = new Vector3(
            posInt.x + ((pos.x >= 0) ? _offset.x : (-1 * _offset.x)),
            posInt.y + ((pos.y >= 0) ? _offset.y : (-1 * _offset.y))
            );

        RaycastHit2D hit = Physics2D.Raycast(pos, Vector3.forward, Mathf.Infinity);

        if (!hit)
        {
            if (_hitTile != null)
            {
                _hitTile.GetComponent<SpriteRenderer>().color = _notAbleColor;
                _hitTile = null;
            }
            return;
        }

        if (_checkTileList == null)
            return;

        foreach (Vector2Int tilePos in _checkTileList)
        {
            if (tilePos == posInt)
            {
                _hitTile = hit.collider.gameObject;
                _hitTile.GetComponent<SpriteRenderer>().color = _ableColor;
                return;
            }
        }


    }


}
