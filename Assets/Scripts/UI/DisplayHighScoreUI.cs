using System;
using UnityEngine;

public class DisplayHighScoreUI : MonoBehaviour
{
    [Space(10)]
    [Header("OBJECT REFERENCES")]

    [Tooltip("The transform of the child Content gameobject")]
    [SerializeField] private Transform contentAnchorTransform;

    private void Start()
    {
        DisplayScores();
    }

    private void DisplayScores()
    {
        HighScores highScores = HighScoreManager.Instance.GetHighScores();

        GameObject scoreGameObject;
        int rank = 0;

        foreach(Score score in highScores.scoreList)
        {
            rank++;

            scoreGameObject = Instantiate(GameResources.Instance.scorePrefab, contentAnchorTransform);

            ScorePrefab scorePrefab = scoreGameObject.GetComponent<ScorePrefab>();

            scorePrefab.rankText.text = rank.ToString();
            scorePrefab.nameText.text = score.playerName;
            scorePrefab.levelText.text = score.levelDescription;
            scorePrefab.scoreText.text = score.playerScore.ToString("###,###0");
        }

        scoreGameObject = Instantiate(GameResources.Instance.scorePrefab, contentAnchorTransform); //create a blank line      
    }
}
