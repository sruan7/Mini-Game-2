using TMPro;
using UnityEngine;
using System.Collections;

public class MainGameController : MonoBehaviour
{
    public TextMeshProUGUI gameStartText; // Reference to GameStartText
    public GameObject orderPanel; // Reference to OrderPanel
    public TextMeshProUGUI orderDetailText; // The order recipe text
    public TextMeshProUGUI scoreText; // Reference to ScoreText for each round score
    public TextMeshProUGUI finalScoreText; // Reference to Final Score Text
    public GameObject[] stageIcons; // Array of 5 stage icons (Temp, Syrup, Milk, Espresso, AddOns)

    private int score = 100;
    private int drinkCount = 0;
    private int[] drinkScores = new int[4]; // To store the scores for 4 rounds

    // Ingredient values from the player's input
    private int playerTemperature = 0; // Default level 0 for temperature
    private int playerSyrupPercent = 0;
    private int playerMilkPercent = 0;
    private int playerEspressoPercent = 0;
    private int playerAddOnsPercent = 0;

    // Randomized order values for comparison
    private int targetTemperature;
    private int targetSyrup;
    private int targetMilk;
    private int targetEspresso;
    private int targetAddOns;

    // Store original icon colors to restore after highlighting/dimming
    private Color[] originalIconColors;

    void Start()
    {
        // Store the original colors of the icons so we can dim and highlight later
        originalIconColors = new Color[stageIcons.Length];
        for (int i = 0; i < stageIcons.Length; i++)
        {
            originalIconColors[i] = stageIcons[i].GetComponent<SpriteRenderer>().color;
        }

        StartCoroutine(GameFlow());
    }

    IEnumerator GameFlow()
    {
        // Initial waiting period
        yield return new WaitForSeconds(1f);

        // Show GameStartText
        gameStartText.gameObject.SetActive(true);

        // Wait for space bar to hide GameStartText
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
        // Wait 1 second
        yield return new WaitForSeconds(1f);

        // Randomize order recipe and display the order panel
        RandomizeOrder();
        orderPanel.SetActive(true);
        orderDetailText.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f); // Allow for half-second delay before starting the stages

        // Reset player input counts for new drink
        playerTemperature = 0; // Start at level 0
        playerSyrupPercent = 0;
        playerMilkPercent = 0;
        playerEspressoPercent = 0;
        playerAddOnsPercent = 0;

        // Reset score to 100 for the new round
        score = 100;

        // Start the stages
        for (int i = 0; i < stageIcons.Length; i++)
        {
            yield return StartCoroutine(HandleStage(i)); // Handle each stage individually
        }

        // Calculate score after all stages
        yield return new WaitForSeconds(1f);
        CalculateScore();

        // Store the score for this round
        drinkScores[drinkCount] = score;
        drinkCount++; // Increment drink count

        // Display the round score
        scoreText.text = "Round Score: " + score;
        scoreText.gameObject.SetActive(true);

        // Wait for 1 second before starting the next round
        yield return new WaitForSeconds(1f);

        // Hide the score after 1 second
        scoreText.gameObject.SetActive(false);

        // Hide the order panel and order text after the round
        orderPanel.SetActive(false);
        orderDetailText.gameObject.SetActive(false);
    }

    void RandomizeOrder()
    {
        // Generate random values for ingredients (target values)
        targetTemperature = Random.Range(1, 4); // Only level 1, 2, or 3 for temperature
        targetSyrup = Random.Range(0, 11) * 10; // Syrup percentage between 0 and 100%
        targetMilk = Random.Range(0, 11) * 10;  // Milk percentage between 0 and 100%
        targetEspresso = Random.Range(0, 11) * 10; // Espresso percentage between 0 and 100%
        targetAddOns = Random.Range(0, 11) * 10; // Add-ons percentage between 0 and 100%

        // Display the randomized recipe (order) in the order detail text
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
                HandleStageAction(stageIndex); // Handle the stage action on key press
            }
            timer += Time.deltaTime;
            yield return null;
        }

        // Dim the icon after the stage is done
        DimStage(stageIcons[stageIndex]);
    }

    void HandleStageAction(int stage)
    {
        switch (stage)
        {
            case 0: // Temperature stage
                playerTemperature++;
                if (playerTemperature > 3)
                    playerTemperature = 1; // Cycle between 1, 2, 3
                break;
            case 1: // Syrup stage
                playerSyrupPercent += 10;
                if (playerSyrupPercent > 100)
                    playerSyrupPercent = 100;
                break;
            case 2: // Milk stage
                playerMilkPercent += 10;
                if (playerMilkPercent > 100)
                    playerMilkPercent = 100;
                break;
            case 3: // Espresso stage
                playerEspressoPercent += 10;
                if (playerEspressoPercent > 100)
                    playerEspressoPercent = 100;
                break;
            case 4: // Add-ons stage
                playerAddOnsPercent += 10;
                if (playerAddOnsPercent > 100)
                    playerAddOnsPercent = 100;
                break;
        }
    }

    // Light up the current stage (you can modify this based on how your icon works)
    void LightUpStage(GameObject stageIcon)
    {
        var spriteRenderer = stageIcon.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.green; // Light up the stage
        }
    }

    // Dim the current stage (you can modify this based on how your icon works)
    void DimStage(GameObject stageIcon)
    {
        var spriteRenderer = stageIcon.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.gray; // Dim the stage
        }
    }

    void CalculateScore()
    {
        int score = 100;

        // Compare player values to the recipe and calculate the score
        if (playerTemperature != targetTemperature)
            score -= 10; // Minus 10 points for wrong temperature

        // Calculate score based on percentage differences
        score -= Mathf.Abs(targetSyrup - playerSyrupPercent) / 10 * 2;
        score -= Mathf.Abs(targetMilk - playerMilkPercent) / 10 * 2;
        score -= Mathf.Abs(targetEspresso - playerEspressoPercent) / 10 * 2;
        score -= Mathf.Abs(targetAddOns - playerAddOnsPercent) / 10 * 2;

        // Clamp the score between 0 and 100 to ensure it doesn't go below 0
        score = Mathf.Clamp(score, 0, 100);

        // Display the score
        scoreText.text = "Score: " + score;
    }

    void DisplayFinalScore()
    {
        int totalScore = 0;
        for (int i = 0; i < drinkScores.Length; i++)
        {
            totalScore += drinkScores[i]; // Sum the scores of all rounds
        }
        int finalScore = totalScore / 4; // Calculate the average

        // Display the final score after 4 rounds
        finalScoreText.text = "Final Score: " + finalScore;
        finalScoreText.gameObject.SetActive(true);
    }
}






















