using UnityEngine;
using TMPro; // For text display

public class RecipeManager : MonoBehaviour
{
    public TextMeshProUGUI orderText; // The text component to display the recipe

    public int temperature; // Temperature level (1, 2, or 3)
    public int syrupPercent; // Syrup percentage
    public int milkPercent; // Milk percentage
    public int espressoPercent; // Espresso percentage
    public int addOnsPercent; // Add-ons percentage

    // Method to generate a random recipe
    public void GenerateRandomRecipe()
    {
        // Generate random values for each stage
        temperature = Random.Range(1, 4); // 1 to 3 for temperature
        syrupPercent = Random.Range(1, 11) * 10; // Multiples of 10 between 10% and 100%
        milkPercent = Random.Range(1, 11) * 10;
        espressoPercent = Random.Range(1, 11) * 10;
        addOnsPercent = Random.Range(1, 11) * 10;

        // Display the recipe in the order panel (as text)
        orderText.text = $"Order Recipe:\n" +
                         $"Temperature: Level {temperature}\n" +
                         $"Syrup: {syrupPercent}%\n" +
                         $"Milk: {milkPercent}%\n" +
                         $"Espresso: {espressoPercent}%\n" +
                         $"Add-ons: {addOnsPercent}%";
    }
}

