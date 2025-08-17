using UnityEngine;

namespace jayounnnn_HeroBrew
{
    public class TownHall : MonoBehaviour
    {
        [SerializeField] private Building townHall;
        [SerializeField] private int startX = 10;
        [SerializeField] private int startY = 10;

        private void Start()
        {
            if (townHall == null) townHall = GetComponent<Building>();

            townHall.PlaceOnGrid(startX, startY);
            townHall.SetPlaced(true);
            townHall.RemoveBaseColour();
            UI_Main.instance._grid.buildings.Add(townHall);
        }
    }
}