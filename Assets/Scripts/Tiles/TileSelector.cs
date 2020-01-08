using UnityEngine;
using System.Collections.Generic;

namespace DungeonDice.Tiles
{
    public class TileSelector : MonoBehaviour
    {
        public bool isSelecting = false;
        int selectCount = 3;

        delegate void DoSomethingToSelectedTile(Tile tile);
        DoSomethingToSelectedTile DoSomething;

        List<Tile> selectedTiles = new List<Tile>();

        InstructionMessage instructionMessage;
        public string instructionText;

        private void Awake() 
        {
            instructionMessage = FindObjectOfType<InstructionMessage>();
        }

        public void SelectTile(Tile tile)
        {
            if(selectedTiles.Count >= selectCount)
            {
                UnselectTile(selectedTiles[selectedTiles.Count - 1]);
            }
            selectedTiles.Add(tile);
            tile.SelectThisTile(true);

            UpdateMessage();
        }

        public void UnselectTile(Tile tile)
        {
            selectedTiles.Remove(tile);
            tile.SelectThisTile(false);

            UpdateMessage();
        }

        void UpdateMessage()
        {
            string countText = "(" + selectedTiles.Count.ToString() + "/" + selectCount.ToString() + ")";
            string messageToUpdate = instructionText + " " + countText;

            instructionMessage.UpdateMessage(messageToUpdate);
        }
    }
}
