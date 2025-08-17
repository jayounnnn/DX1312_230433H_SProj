namespace jayounnnn_HeroBrew
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public class BuildGrid : MonoBehaviour
    {
        private int _rows = 45;
        private int _columns = 45;
        private float _cellSize = 1.0f; public float cellSize { get { return _cellSize;  } }

        public List<Building> buildings = new List<Building>();

        private readonly HashSet<Vector2Int> _reserved = new HashSet<Vector2Int>();

        private void Start()
        {
            ReserveCenterArea(5, 5);
        }


        public Vector3 GetStartPosition(int x, int y)
        {
            Vector3 position = transform.position;
            position += (transform.right.normalized * x * _cellSize) + (transform.forward.normalized * y * _cellSize);
            return position;
        }

        public Vector3 GetCenterPosition(int x, int y, int rows, int columns)
        {
            Vector3 position = GetStartPosition(x,y);
            position += (transform.right.normalized * columns * _cellSize / 2.0f) + (transform.forward.normalized * rows * _cellSize / 2.0f);
            return position;
        }

        public Vector3 GetEndPosition(int x, int y, int rows, int columns)
        {
            Vector3 position = GetStartPosition(x, y);
            position += (transform.right.normalized * columns * _cellSize) + (transform.forward.normalized * rows * _cellSize);
            return position;
        }

        public Vector3 GetEndPosition(Building building)
        {
            return GetEndPosition(building.currentX, building.currentY, building.columns, building.rows);
        }

        public bool IsWorldPositionIsOnPlane(Vector3 position, int x, int y , int rows, int columns)
        {
            position = transform.InverseTransformPoint(position);
            Rect rect = new Rect(x, y, columns, rows);
            if (rect.Contains(new Vector2(position.x, position.z)))
            {
                return true;
            }
            return false;
        }

        public bool CanPlaceBuilding(Building building, int x, int y)
        {
            if (x < 0 || y < 0 || x + building.columns > _columns || y + building.rows > _rows)
                return false;

            Rect candidate = new Rect(x, y, building.columns, building.rows);

            for (int i = 0; i < buildings.Count; i++)
            {
                var b = buildings[i];
                if (b == null || b == building || !b.Placed) continue;

                Rect occupied = new Rect(b.currentX, b.currentY, b.columns, b.rows);
                if (candidate.Overlaps(occupied))
                    return false;
            }

            for (int ix = x; ix < x + building.columns; ix++)
            {
                for (int iy = y; iy < y + building.rows; iy++)
                {
                    if (_reserved.Contains(new Vector2Int(ix, iy)))
                        return false;
                }
            }

            return true;
        }

        public void ReserveArea(int x, int y, int width, int height)
        {
            for (int ix = x; ix < x + width; ix++)
                for (int iy = y; iy < y + height; iy++)
                    _reserved.Add(new Vector2Int(ix, iy));
        }

        public void ReserveCenterArea(int width, int height)
        {
            int startX = Mathf.Max(0, (_columns - width) / 2);
            int startY = Mathf.Max(0, (_rows - height) / 2);
            ReserveArea(startX, startY, width, height);
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            for (int i = 0; i <= _rows; i++)
            {
                Vector3 point = transform.position + transform.forward.normalized * _cellSize * (float)i;
                Gizmos.DrawLine(point, point + transform.right.normalized * _cellSize * (float)_columns);
            }
            for (int i = 0; i <= _columns; i++)
            {
                Vector3 point = transform.position + transform.right.normalized * _cellSize * (float)i;
                Gizmos.DrawLine(point, point + transform.forward.normalized * _cellSize * (float)_rows);
            }

        }
#endif
    }
}
