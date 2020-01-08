using UnityEngine;
using UnityEngine.UI;
using DungeonDice.Dices;
using TMPro;
using DungeonDice.Characters;
using DungeonDice.Combat;
using System.Collections;
using DungeonDice.Stats;
using DungeonDice.Objects;

namespace DungeonDice.UI
{
    public class DiceUI : MonoBehaviour
    {
        [Header("Control UI")]
        [SerializeField] Image currentExploreDiceImage;
        [SerializeField] Image currentCombatDiceImage;
        [SerializeField] GameObject exploreRollButton;
        [SerializeField] GameObject combatRollButton;
        [SerializeField] GameObject playerDice;

        [Header("Explore Dice Detail Window")]
        [SerializeField] GameObject exploreDiceDetailWindow;
        [SerializeField] Image[] exploreDiceDetailWindowSides;
        [SerializeField] TextMeshProUGUI exploreNameText;
        [SerializeField] TextMeshProUGUI exploreDescriptionText;

        [Header("Combat Dice Detail Window")]
        [SerializeField] GameObject combatDiceDetailWindow;
        [SerializeField] Image[] combatDiceDetailWindowSides;
        [SerializeField] TextMeshProUGUI combatNameText;
        [SerializeField] TextMeshProUGUI combatDescriptionText;

        [Header("Enemy Dice Detail Window")]
        [SerializeField] GameObject enemyDiceDetailWindow;
        [SerializeField] Image[] enemyDiceDetailWindowSides;
        [SerializeField] TextMeshProUGUI enemyNameText;
        [SerializeField] TextMeshProUGUI enemyDescriptionText;

        int exploreDiceIndex = 0;
        int combatDiceIndex = 0;

        DiceContainer diceContainer;

        private void Awake()
        {
            diceContainer = FindObjectOfType<DiceContainer>();
        }

        private void Start()
        {
            UpdateDiceImages();
        }

        void UpdateDiceImages()
        {
            currentExploreDiceImage.sprite = diceContainer.currentExploreDice.repSprite;
            currentCombatDiceImage.sprite = diceContainer.currentCombatDice.repSprite;
        }

        public void ChangeExploreDiceToRight()
        {
            if (!CanClick()) return;

            if (exploreDiceIndex >= diceContainer.exploreDices.Count - 1)
            {
                exploreDiceIndex = 0;
            }
            else
            {
                exploreDiceIndex++;
            }

            diceContainer.currentExploreDice = diceContainer.exploreDices[exploreDiceIndex];

            UpdateDiceImages();
            UpdateSideImages(exploreDiceDetailWindowSides, diceContainer.currentExploreDice);
            UpdateSideInfo(0);
        }

        public void ChangeCombatDiceToLeft()
        {
            if (!CanClick()) return;

            if (combatDiceIndex <= 0)
            {
                combatDiceIndex = diceContainer.combatDices.Count - 1;
            }
            else
            {
                combatDiceIndex--;
            }

            diceContainer.currentCombatDice = diceContainer.combatDices[combatDiceIndex];

            UpdateDiceImages();
            UpdateSideImages(combatDiceDetailWindowSides, diceContainer.currentCombatDice);
            UpdateSideInfo(0);
        }

        public void ToggleExploreDiceDetailWindow()
        {
            if (!CanClick()) return;

            combatDiceDetailWindow.SetActive(false);
            enemyDiceDetailWindow.SetActive(false);

            if (exploreDiceDetailWindow.activeSelf)
            {
                exploreDiceDetailWindow.SetActive(false);
            }
            else
            {
                exploreDiceDetailWindow.SetActive(true);

                UpdateSideImages(exploreDiceDetailWindowSides, diceContainer.currentExploreDice);
                UpdateSideInfo(0);
            }
        }

        public void ToggleCombatDiceDetailWindow()
        {
            if (!CanClick()) return;

            exploreDiceDetailWindow.SetActive(false);
            enemyDiceDetailWindow.SetActive(false);

            if (combatDiceDetailWindow.activeSelf)
            {
                combatDiceDetailWindow.SetActive(false);
            }
            else
            {
                combatDiceDetailWindow.SetActive(true);

                UpdateSideImages(combatDiceDetailWindowSides, diceContainer.currentCombatDice);
                UpdateSideInfo(0);
            }
        }

        public void OpenEnemyDiceDetailWindow()
        {
            if (!CanClick()) return;

            exploreDiceDetailWindow.SetActive(false);
            combatDiceDetailWindow.SetActive(false);

            enemyDiceDetailWindow.SetActive(true);

            Dice enemyDice = FindObjectOfType<Enemy>().GetCurrentEnemyDice();

            UpdateSideImages(enemyDiceDetailWindowSides, enemyDice);
            UpdateSideInfo(0);
        }

        public void ShutEnemyDiceDetailWindow()
        {

            enemyDiceDetailWindow.SetActive(false);

        }

        void UpdateSideImages(Image[] windowSideImages, Dice diceToUpdate)
        {
            foreach (Image image in windowSideImages)
            {
                image.gameObject.SetActive(false);
            }

            Side[] currentDiceSides = diceToUpdate.sides;

            for (int i = 0; i < currentDiceSides.Length; i++)
            {
                windowSideImages[i].gameObject.SetActive(true);
                windowSideImages[i].sprite = currentDiceSides[i].sideSprite;
            }
        }

        public void UpdateSideInfo(int i)
        {
            Side[] currentDiceSides = null;
            TextMeshProUGUI nameText = null;
            TextMeshProUGUI descriptionText = null;

            currentDiceSides = diceContainer.currentExploreDice.sides;
            nameText = exploreNameText;
            descriptionText = exploreDescriptionText;

            if (combatDiceDetailWindow.activeSelf)
            {
                currentDiceSides = diceContainer.currentCombatDice.sides;
                nameText = combatNameText;
                descriptionText = combatDescriptionText;
            }
            else if (enemyDiceDetailWindow.activeSelf)
            {
                Dice enemyDice = FindObjectOfType<Enemy>().GetCurrentEnemyDice();
                currentDiceSides = enemyDice.sides;
                nameText = enemyNameText;
                descriptionText = enemyDescriptionText;
            }

            nameText.text = currentDiceSides[i].sideName + " " + currentDiceSides[i].value;
            descriptionText.text = currentDiceSides[i].description;
        }

        public void ShutWindows()
        {
            exploreDiceDetailWindow.SetActive(false);
            combatDiceDetailWindow.SetActive(false);
            enemyDiceDetailWindow.SetActive(false);
        }

        public void RollCurrentDice(string diceType) // 굴린다 버튼. explore : combat
        {
            Dice diceToRoll = diceContainer.currentExploreDice;
            Image currentDiceUIImage = currentExploreDiceImage;

            if (diceType == "combat")
            {
                diceToRoll = diceContainer.currentCombatDice;
                currentDiceUIImage = currentCombatDiceImage;

                FindObjectOfType<CombatManager>().SetupPlayerTurnState();
            }
            else
            {
                FindObjectOfType<Player>().GetComponent<FP>().DealFP(-1f);
            }

            ShutWindows();
            exploreRollButton.SetActive(false);
            combatRollButton.SetActive(false);

            currentDiceUIImage.color = new Color32(255, 255, 255, 127);
            currentDiceUIImage.GetComponent<Button>().enabled = false;

            StartCoroutine(Roll(diceToRoll));
        }

        IEnumerator Roll(Dice diceToRoll)
        {
            playerDice.SetActive(true);
            DiceRoller diceRoller = playerDice.GetComponent<DiceRoller>();
            diceRoller.TriggerDiceRoll(diceToRoll);

            while (diceRoller.isRolling)
            {
                yield return null;
            }

            Side resultSide = diceToRoll.sides[diceRoller.resultIndex];

            if (FindObjectOfType<StateHolder>().GetCurrentPhase() == Phase.COMBAT)
            {
                yield return StartCoroutine(FindObjectOfType<CombatManager>().DoAction(resultSide));
            }
            else
            {
                resultSide.diceEffect.Activate(resultSide.value, null);
            }
        }

        public void SetUIAtPhase(Phase phase)
        {
            playerDice.SetActive(false);

            exploreRollButton.SetActive(false);
            combatRollButton.SetActive(false);

            currentExploreDiceImage.color = new Color32(255, 255, 255, 255);
            currentExploreDiceImage.GetComponent<Button>().enabled = true;
            currentCombatDiceImage.color = new Color32(255, 255, 255, 255);
            currentCombatDiceImage.GetComponent<Button>().enabled = true;

            if (phase == Phase.EXPLORE)
            {
                exploreRollButton.SetActive(true);
            }
            else if (phase == Phase.COMBAT)
            {
                combatRollButton.SetActive(true);
            }
        }

        bool CanClick()
        {
            if (FindObjectOfType<StateHolder>().GetCurrentPhase() == Phase.COMBAT)
            {
                if (FindObjectOfType<CombatManager>().state != CombatState.STANDBY)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
