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
            buttonConfirm.anchorMin = Vector3.zero;
            buttonConfirm.anchorMax = Vector3.zero;
            buttonCancel.anchorMin = Vector3.zero;
            buttonCancel.anchorMax = Vector3.zero;
            buttonRotate.anchorMin = Vector3.zero;
            buttonRotate.anchorMax = Vector3.zero;

        }

        private void Update()
        {
            if (Building.instance != null && CameraController.instance.isPlacingBuilding)
            {
                Vector3 end = UI_Main.instance._grid.GetEndPosition(Building.instance);

                Vector3 planeDownLeft = CameraController.instance.CameraScreenPositionToPlanePosition(Vector2.zero);
                Vector3 planeTopRight = CameraController.instance.CameraScreenPositionToPlanePosition(new Vector2(Screen.width, Screen.height));

                float width = planeTopRight.x - planeDownLeft.x;
                float height = planeTopRight.z - planeDownLeft.z;

                float endWidth = end.x - planeDownLeft.x;
                float endHeight = end.z - planeDownLeft.z;

                Vector2 screenPoint = new Vector2(endWidth / width * Screen.width, endHeight / height * Screen.height);

                Vector2 confirmPoint = screenPoint;
                confirmPoint.x += (buttonConfirm.rect.width + 10.0f);
                buttonConfirm.anchoredPosition = confirmPoint;

                Vector2 cancelPoint = screenPoint;
                cancelPoint.x -= (buttonCancel.rect.width + 10.0f);
                buttonCancel.anchoredPosition = cancelPoint;

                Vector2 rotatePoint = screenPoint;
                rotatePoint.y -= (buttonRotate.rect.height + 35.0f);
                buttonRotate.anchoredPosition = rotatePoint;
            }
        }

        public void SetStatus(bool status)
        {
            _elements.SetActive(status);
        }

        private void ConfirmBuild()
        {
            if (Building.instance == null)
            {
                return;
            }

            var player = FindObjectOfType<Player>();
            if (player == null)
            {
                return;
            }

            var building = Building.instance;

            bool enoughGold = player.SpendGold(building.CostGold);
            bool enoughCrystal = player.SpendCrystal(building.CostCrystal);
            bool enoughStamina = player.SpendStamina(building.CostStamina);

            if (!enoughGold || !enoughCrystal || !enoughStamina)
            {
                return;
            }

            // Finalise placement
            building.SetPlaced(true);
            building.RemoveBaseColour();
            CameraController.instance.isPlacingBuilding = false;
            Building.instance = null;

            // Hide UI elements
            UI_Main.instance.SetStatus(true);
            UI_Build.instance.SetStatus(false);
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
