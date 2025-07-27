namespace jayounnnn_HeroBrew
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Building : MonoBehaviour
    {
        public string BuildingID = "none";
        public int CostGold = 0;
        public int CostCrystal = 0;
        public int CostStamina = 0;

        //public string id = "";
        private static Building _instance = null; public static Building instance { get { return _instance; } set { _instance = value; } }

        [System.Serializable] public class Level
        {
            public int level = 1;
            public Sprite icon = null;
            public GameObject mesh = null;
        }

        private BuildGrid _grid = null;

        [SerializeField] private int _rows = 1; public int rows { get { return _rows; } }
        [SerializeField] private int _columns = 1; public int columns { get { return _columns; } }

        [SerializeField] private MeshRenderer _baseArea = null;

        [SerializeField] private Level[] _levels = null;

        private int _currentX = 0; public int currentX { get { return _currentX; } }
        private int _currentY = 0; public int currentY { get { return _currentY; } }
        private int _X = 0;
        private int _Y = 0;

        public void PlaceOnGrid(int x, int y)
        {
            _currentX = x;
            _currentY = y;
            _X = x;
            _Y = y;
            Vector3 position = UI_Main.instance._grid.GetCenterPosition(x,y,_rows, _columns);
            transform.position = position;
            SetBaseColour();
        }

        public void StartMovingOnGrid()
        {
            _X = _currentX;
            _Y = _currentY;

        }

        public void RemoveFromGrid()
        {
            _instance = null;
            UI_Build.instance.SetStatus(false);
            CameraController.instance.isPlacingBuilding = false;
            Destroy(gameObject);
        }

        public void UpdateGridPosition(Vector3 basePosition, Vector3 currentPosition)
        {
            Vector3 dir = UI_Main.instance._grid.transform.TransformPoint(currentPosition) - UI_Main.instance._grid.transform.TransformPoint(basePosition);

            int xDistance = Mathf.RoundToInt(dir.z / UI_Main.instance._grid.cellSize);
            int yDistance = Mathf.RoundToInt(-dir.x / UI_Main.instance._grid.cellSize);

            _currentX = _X + xDistance;
            _currentY = _Y + yDistance;

            Vector3 position = UI_Main.instance._grid.GetCenterPosition(_currentX, _currentY, _rows, _columns);
            transform.position = position;

            SetBaseColour();
        }

        private void SetBaseColour()
        {
            if (UI_Main.instance._grid.CanPlaceBuilding(this, currentX, currentY))
            {
                _baseArea.sharedMaterial.color = Color.green;
            }
            else
            {
                _baseArea.sharedMaterial.color = Color.red;
            }
        }
    }
}
