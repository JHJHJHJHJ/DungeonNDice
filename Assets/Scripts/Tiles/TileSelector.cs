using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace DungeonDice.Tiles
{
    public class TileSelector : MonoBehaviour
    {
        public bool isSelecting = false;
        int selectCount = 0;

        public delegate IEnumerator DoSomethingToSelectedTile(Tile tile);
        DoSomethingToSelectedTile DoSomething;

        List<Tile> selectedTiles;

        [SerializeField] GameObject button;
        InstructionMessage instructionMessage;
        public string instructionText;

        private void Awake() 
        {
            instructionMessage = FindObjectOfType<InstructionMessage>();
        }

        public void ActivateTileSelector(DoSomethingToSelectedTile doSomething, int count, string instructionText)
        {
            selectedTiles = new List<Tile>();
            DoSomething = new DoSomethingToSelectedTile(doSomething);
            selectCount = count;
            this.instructionText = instructionText;

            isSelecting = true;
            UpdateMessage();
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

            if(selectedTiles.Count >= selectCount)
            {
                button.SetActive(true);
            }
        }

        public void UnselectTile(Tile tile)
        {
            selectedTiles.Remove(tile);
            tile.SelectThisTile(false);

            UpdateMessage();

            button.SetActive(false);
        }

        void UpdateMessage()
        {
            string countText = "(" + selectedTiles.Count.ToString() + "/" + selectCount.ToString() + ")";
            string messageToUpdate = instructionText + " " + countText;

            instructionMessage.UpdateMessage(messageToUpdate);
        }

        public void Effect()
        {
            isSelecting = false;         
            button.SetActive(false);

            for(int i = 0; i < selectedTiles.Count; i++)
            {
                StartCoroutine(DoSomething(selectedTiles[i]));        
            }

            for(int i = 0; i < selectedTiles.Count; i++)
            {
                UnselectTile(selectedTiles[i]);    
            }       

            instructionMessage.UpdateMessage("");
        }
    }
}
