using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[DisallowMultipleComponent]
public class CharacterSelectorUI : MonoBehaviour
{
    [Tooltip("The transform of the SelectorAnchor gameobject")]
    [SerializeField] private Transform characterSelector;

    [Tooltip("The TMP on PlayerNameInput gameobject")]
    [SerializeField] private TMP_InputField playerNameInput;

    private List<PlayerDetailsSO> playerDetailsList;
    private GameObject playerSelectionPrefab;
    private CurrentPlayerSO currentPlayer;
    private List<GameObject> playerCharacterGameObjectList = new List<GameObject>();
    private Coroutine coroutine;
    private int selectedPlayerIndex = 0;
    private float offset = 4f;

    private void Awake()
    {
        playerSelectionPrefab = GameResources.Instance.playerSelectionPrefab;
        playerDetailsList = GameResources.Instance.playerDetailsList;
        currentPlayer = GameResources.Instance.currentPlayer;
    }

    private void Start()
    {
        for(int i = 0; i < playerDetailsList.Count; i++)
        {
            GameObject playerSelectionObject = Instantiate(playerSelectionPrefab, characterSelector);

            playerCharacterGameObjectList.Add(playerSelectionObject);

            playerSelectionObject.transform.localPosition = new Vector3(offset * i, 0f, 0f);

            PopulatePlayerDetails(playerSelectionObject.GetComponent<PlayerSelectionUI>(), playerDetailsList[i]);
        }

        playerNameInput.text = currentPlayer.playerName;

        currentPlayer.playerDetails = playerDetailsList[selectedPlayerIndex];
    }

    private void PopulatePlayerDetails(PlayerSelectionUI playerSelection, PlayerDetailsSO playerDetails)
    {
        playerSelection.playerHandSpriteRenderer.sprite = playerDetails.playerHandSprite;
        playerSelection.playerHandNoWeaponSpriteRenderer.sprite = playerDetails.playerHandSprite;
        playerSelection.playerWeaponSpriteRenderer.sprite = playerDetails.startingWeapon.weaponSprite;
        playerSelection.animator.runtimeAnimatorController = playerDetails.runtimeAnimatorController;
    }

    public void NextCharacter()
    {
        if (selectedPlayerIndex >= playerDetailsList.Count - 1) return;

        selectedPlayerIndex++;

        currentPlayer.playerDetails = playerDetailsList[selectedPlayerIndex];

        MoveToSelectedCharacter(selectedPlayerIndex);
    }

    public void PreviousCharacter()
    {
        if (selectedPlayerIndex == 0) return;

        selectedPlayerIndex--;

        currentPlayer.playerDetails = playerDetailsList[selectedPlayerIndex];

        MoveToSelectedCharacter(selectedPlayerIndex);
    }

    private void MoveToSelectedCharacter(int index)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(MoveToSelectedCharacterRoutine(index));
    }

    private IEnumerator MoveToSelectedCharacterRoutine(int index)
    {
        float currentLocalXPos = characterSelector.localPosition.x;
        float targetLocalXPos = index * offset * characterSelector.localScale.x * -1f;

        while(Mathf.Abs(currentLocalXPos - targetLocalXPos) > 0.01f)
        {
            currentLocalXPos = Mathf.Lerp(currentLocalXPos, targetLocalXPos, Time.deltaTime * 10f);

            characterSelector.localPosition = new Vector3(currentLocalXPos, characterSelector.localPosition.y, 0f);

            yield return null;  
        }

        characterSelector.localPosition = new Vector3(targetLocalXPos, characterSelector.localPosition.y, 0f);
    }

    public void UpdatePlayerName()
    {
        playerNameInput.text = playerNameInput.text.ToUpper();

        currentPlayer.playerName = playerNameInput.text;
    }

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(characterSelector), characterSelector);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerNameInput), playerNameInput);
    }
#endif
    #endregion
}
