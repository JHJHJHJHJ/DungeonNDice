using UnityEngine;
using System.Collections;
using DungeonDice.Tiles;
using DungeonDice.Characters;

namespace DungeonDice.Dices
{
    [CreateAssetMenu(fileName = "Teleport", menuName = "DungeonDice/Dice Effect/Teleport")]
    public class Teleport : DiceEffect
    {
        string instructionText = "이동할 타일을 선택하세요.";

        public override void Activate(int value, GameObject target)
        {
            FindObjectOfType<TileSelector>().ActivateTileSelector(TeleportCoroutine.TeleportToTile, 1, instructionText);
        }

        public override bool isSelf()
        {
            return false;
        }

        public override string GetCombatMessage(string target, int value)
        {
            return null;
        }
    }

    public class TeleportCoroutine : MonoBehaviour
    {
        public static IEnumerator TeleportToTile(Tile tile)
        {
            Player player = FindObjectOfType<Player>();
            player.playerDice.SetActive(false);

            float alpha = 1f;

            while (alpha >= 0f)
            {
                foreach (Transform child in player.transform)
                {
                    if (!child.GetComponent<SpriteRenderer>()) continue;

                    child.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha);
                }
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);

                alpha -= Time.deltaTime / 0.3f;
                yield return null;
            }

            foreach (Transform child in player.transform)
            {
                if (!child.GetComponent<SpriteRenderer>()) continue;

                child.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha);
            }

            yield return new WaitForSeconds(0.3f);

            alpha = 0f;

            while (alpha <= 1f)
            {
                foreach (Transform child in player.transform)
                {
                    if (!child.GetComponent<SpriteRenderer>()) continue;

                    child.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha);
                }
                player.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);

                alpha += Time.deltaTime / 0.3f;
                yield return null;
            }

            foreach (Transform child in player.transform)
            {
                if (!child.GetComponent<SpriteRenderer>()) continue;

                child.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha);
            }

            player.currentTileIndex = tile.index;
            player.UpdateCurrentTile();

            yield return new WaitForSeconds(0.3f);

            FindObjectOfType<StateHolder>().SetPhaseToEvent();
        }
    }
}



