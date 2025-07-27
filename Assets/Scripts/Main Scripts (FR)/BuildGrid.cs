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
            if (building.currentX < 0 || building.currentY < 0 || building.currentX + building.columns > _columns || building.currentY + building.rows > _rows )
            {
                return false;
            }

            for (int i = 0; i < buildings.Count; i++)
            {
                Rect rect1 = new Rect(buildings[i].currentX, buildings[i].currentY, buildings[i].columns, buildings[i].rows);
                Rect rect2 = new Rect(building.currentX, building.currentY, building.columns, building.rows);
                if (rect2.Overlaps(rect1))
                {
                    return false;
                }
            }

                return true;
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
