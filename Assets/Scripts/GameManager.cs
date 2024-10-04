using TMPro;
using UnityEngine;
using System.Collections;

public class MainGameController : MonoBehaviour
{
    public TextMeshProUGUI gameStartText; 
    public GameObject orderPanel; 
    public TextMeshProUGUI orderDetailText;
    public TextMeshProUGUI scoreText; 
    public TextMeshProUGUI finalScoreText; 
    public GameObject[] stageIcons; 

    private int score = 100;
    private int drinkCount = 0;
    private int[] drinkScores = new int[4]; // To store the scores for 4 rounds

    // Ingredient values from the player's input
    private int playerTemperature = 0; 
    private int playerSyrupPercent = 0;
    private int playerMilkPercent = 0;
    private int playerEspressoPercent = 0;
    private int playerAddOnsPercent = 0;


    private int targetTemperature;
    private int targetSyrup;
    private int targetMilk;
    private int targetEspresso;
    private int targetAddOns;

   
    private Color[] originalIconColors;

    void Start()
    {
        originalIconColors = new Color[stageIcons.Length];
        for (int i = 0; i < stageIcons.Length; i++)
        {
            originalIconColors[i] = stageIcons[i].GetComponent<SpriteRenderer>().color;
        }

        StartCoroutine(GameFlow());
    }

    IEnumerator GameFlow()
    {
        yield return new WaitForSeconds(1f);

        gameStartText.gameObject.SetActive(true);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        gameStartText.gameObject.SetActive(false);

        // Start the rounds
        for (int round = 0; round < 4; round++)
        {
            yield return StartCoroutine(StartDrinkRound());
        }

        // After 4 rounds, display the final score
        DisplayFinalScore();
    }

    IEnumerator StartDrinkRound()
    {
        yield return new WaitForSeconds(1f);

        // Randomize order recipe and display the order panel
        RandomizeOrder();
        orderPanel.SetActive(true);
        orderDetailText.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        // Reset player input counts for new drink
        playerTemperature = 0;
        playerSyrupPercent = 0;
        playerMilkPercent = 0;
        playerEspressoPercent = 0;
        playerAddOnsPercent = 0;

        score = 100;

        // Start the stages
        for (int i = 0; i < stageIcons.Length; i++)
        {
            yield return StartCoroutine(HandleStage(i)); 
        }

        yield return new WaitForSeconds(1f);
        CalculateScore();

        // Store the score for this round
        drinkScores[drinkCount] = score;
        drinkCount++; 

        scoreText.text = "Round Score: " + score;
        scoreText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        scoreText.gameObject.SetActive(false);

        orderPanel.SetActive(false);
        orderDetailText.gameObject.SetActive(false);
    }

    void RandomizeOrder()
    {
        targetTemperature = Random.Range(1, 4); 
        targetSyrup = Random.Range(0, 11) * 10; 
        targetMilk = Random.Range(0, 11) * 10;  
        targetEspresso = Random.Range(0, 11) * 10; 
        targetAddOns = Random.Range(0, 11) * 10; 

        orderDetailText.text = $"Temp: {targetTemperature}, Syrup: {targetSyrup}%, Milk: {targetMilk}%, Espresso: {targetEspresso}%, AddOns: {targetAddOns}%";
    }

    IEnumerator HandleStage(int stageIndex)
    {
        // Highlight the current stage icon
        LightUpStage(stageIcons[stageIndex]);

        // Track player input during this stage
        float timer = 0;
        while (timer < 3f)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HandleStageAction(stageIndex);
            }
            timer += Time.deltaTime;
            yield return null;
        }

        DimStage(stageIcons[stageIndex]);
    }

    void HandleStageAction(int stage)
    {
        switch (stage)
        {
            case 0: 
                playerTemperature++;
                if (playerTemperature > 3)
                    playerTemperature = 1; 
                break;
            case 1: 
                playerSyrupPercent += 10;
                if (playerSyrupPercent > 100)
                    playerSyrupPercent = 100;
                break;
            case 2: 
                playerMilkPercent += 10;
                if (playerMilkPercent > 100)
                    playerMilkPercent = 100;
                break;
            case 3: 
                playerEspressoPercent += 10;
                if (playerEspressoPercent > 100)
                    playerEspressoPercent = 100;
                break;
            case 4: 
                playerAddOnsPercent += 10;
                if (playerAddOnsPercent > 100)
                    playerAddOnsPercent = 100;
                break;
        }
    }

    void LightUpStage(GameObject stageIcon)
    {
        var spriteRenderer = stageIcon.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.green;
        }
    }

    void DimStage(GameObject stageIcon)
    {
        var spriteRenderer = stageIcon.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.gray; 
        }
    }

    void CalculateScore()
    {
        int score = 100;

        if (playerTemperature != targetTemperature)
            score -= 10;

        score -= Mathf.Abs(targetSyrup - playerSyrupPercent) / 10 * 2;
        score -= Mathf.Abs(targetMilk - playerMilkPercent) / 10 * 2;
        score -= Mathf.Abs(targetEspresso - playerEspressoPercent) / 10 * 2;
        score -= Mathf.Abs(targetAddOns - playerAddOnsPercent) / 10 * 2;

        score = Mathf.Clamp(score, 0, 100);

        // Display the score
        scoreText.text = "Score: " + score;
    }

    void DisplayFinalScore()
    {
        int totalScore = 0;
        for (int i = 0; i < drinkScores.Length; i++)
        {
            totalScore += drinkScores[i]; 
        }
        int finalScore = totalScore / 4; 

        finalScoreText.text = "Final Score: " + finalScore;
        finalScoreText.gameObject.SetActive(true);
    }
}






















