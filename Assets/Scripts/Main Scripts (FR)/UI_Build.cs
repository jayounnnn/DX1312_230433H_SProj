namespace jayounnnn_HeroBrew
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_Build : MonoBehaviour
    {
        [SerializeField] public GameObject _elements = null;
        [SerializeField] private Canvas _canvas; 


        public RectTransform buttonConfirm = null;
        public RectTransform buttonCancel = null;
        public RectTransform buttonRotate = null;

        private static UI_Build _instance = null; public static UI_Build instance { get { return _instance; } }

        private void Awake()
        {
            _instance = this;
            _elements.SetActive(false);
        }

        private void Start()
        {
            buttonConfirm.gameObject.GetComponent<Button>().onClick.AddListener(ConfirmBuild);
            buttonCancel.gameObject.GetComponent<Button>().onClick.AddListener(CancelBuild);
            buttonRotate.gameObject.GetComponent<Button>().onClick.AddListener(RotateBuilding);
            var c = Vector2.one * 0.5f;
            buttonConfirm.anchorMin = c; buttonConfirm.anchorMax = c;
            buttonCancel.anchorMin = c; buttonCancel.anchorMax = c;
            buttonRotate.anchorMin = c; buttonRotate.anchorMax = c;

        }

        private void Update()
        {
            if (Building.instance != null && CameraController.instance.isPlacingBuilding)
            {
                Vector3 world = Building.instance.transform.position;

                Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(
                    CameraController.instance.UICamera, world);

                RectTransform canvasRect = (RectTransform)_canvas.transform;

                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect, screenPoint, _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera,
                    out localPoint);

                buttonConfirm.anchoredPosition = localPoint + new Vector2(+(buttonConfirm.rect.width + 10f), 100f);
                buttonCancel.anchoredPosition = localPoint + new Vector2(-(buttonCancel.rect.width + 10f), 100f);
                buttonRotate.anchoredPosition = localPoint + new Vector2(0f, -(buttonRotate.rect.height + 5.0f));
            }
        }

        public void SetStatus(bool status)
        {
            _elements.SetActive(status);
        }

        private void ConfirmBuild()
        {
            if (Building.instance == null) return;
            var player = FindObjectOfType<Player>();
            if (player == null) return;

            var building = Building.instance;

            if (!UI_Main.instance._grid.CanPlaceBuilding(building, building.currentX, building.currentY))
                return;

            bool enoughGold = player.SpendGold(building.CostGold);
            bool enoughCrystal = player.SpendCrystal(building.CostCrystal);
            bool enoughStamina = player.SpendStamina(building.CostStamina);
            if (!enoughGold || !enoughCrystal || !enoughStamina) return;

            building.SetPlaced(true);
            building.RemoveBaseColour();

            UI_Main.instance._grid.buildings.Add(building);

            CameraController.instance.isPlacingBuilding = false;
            BuildingManager.FinalizePlacement(building);
            Building.instance = null;

            UI_Main.instance.SetStatus(true);
            UI_Build.instance.SetStatus(false);
            UI_Main.instance.UpdateCurrencyUI();
        }

        public void CancelBuild()
        {
            if (Building.instance != null)
            {
                CameraController.instance.isPlacingBuilding = false;
                Building.instance.RemoveFromGrid();
            }
        }

        private void RotateBuilding()
        {
            if (Building.instance != null && CameraController.instance.isPlacingBuilding)
            {
                Building.instance.transform.Rotate(0f, 90f, 0f);
            }
        }
    }
}
