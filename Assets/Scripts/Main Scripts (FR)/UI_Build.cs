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

        private static UI_Build _instance = null; public static UI_Build instance { get { return _instance; } }

        private void Awake()
        {
            _instance = this;
            _elements.SetActive(true);
        }

        private void Start()
        {
            buttonConfirm.gameObject.GetComponent<Button>().onClick.AddListener(ConfirmBuild);
            buttonCancel.gameObject.GetComponent<Button>().onClick.AddListener(CancelBuild);
            buttonConfirm.anchorMin = Vector3.zero;
            buttonConfirm.anchorMax = Vector3.zero;
            buttonCancel.anchorMin = Vector3.zero;
            buttonCancel.anchorMax = Vector3.zero;

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
            }
        }

        public void SetStatus(bool status)
        {
            _elements.SetActive(status);
        }

        private void ConfirmBuild()
        {
            if (Building.instance != null)
            {

            }
        }

        public void CancelBuild()
        {
            if (Building.instance != null)
            {
                CameraController.instance.isPlacingBuilding = false;
                Building.instance.RemoveFromGrid();
            }
        }
    }
}
