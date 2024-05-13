using NUnit.Framework;
using System;

[TestFixture]
public class RecipeTests
{
    [Test]
    public void AddIngredient_ValidIngredient_IngredientAdded()
    {
        // Arrange
        var recipe = new Recipe("test");

        // Act
        recipe.AddIngredient("Ingredient1", 100, "g", 50, "FoodGroup");

        // Assert
        Assert.AreEqual(1, recipe.Ingredients.Count);
        Assert.AreEqual("Ingredient1", recipe.Ingredients[0].Name);
        Assert.AreEqual(100, recipe.Ingredients[0].Quantity);
        Assert.AreEqual("g", recipe.Ingredients[0].Unit);
        Assert.AreEqual(50, recipe.Ingredients[0].Calories);
        Assert.AreEqual("FoodGroup", recipe.Ingredients[0].FoodGroup);
    }

    [Test]
    public void DisplayRecipe_NoIngredients_NoCalories()
    {
        // Arrange
        var recipe = new Recipe("test");
        recipe.Name = "Test Recipe";

        // Act
        var output = TestHelper.GetConsoleOutput(() => recipe.DisplayRecipe());

        // Assert
        StringAssert.Contains("Recipe: Test Recipe", output);
        StringAssert.Contains("Total Calories: 0", output);
    }

    [Test]
    public void DisplayRecipe_WithIngredients_CalculatesTotalCalories()
    {
        // Arrange
        var recipe = new Recipe("test");
        recipe.Name = "Test Recipe";
        recipe.AddIngredient("Ingredient1", 100, "g", 50, "FoodGroup");
        recipe.AddIngredient("Ingredient2", 200, "g", 75, "FoodGroup");

        // Act
        var output = TestHelper.GetConsoleOutput(() => recipe.DisplayRecipe());

        // Assert
        StringAssert.Contains("Total Calories: 125", output);
    }

    // Add more test methods as needed
}

[TestFixture]
public class IngredientTests
{
    [Test]
    public void Constructor_ValidParameters_IngredientCreated()
    {
        // Arrange & Act
        var ingredient = new Ingredient
        {
            Name = "Ingredient1",
            Quantity = 100,
            Unit = "g",
            Calories = 50,
            FoodGroup = "FoodGroup"
        };

        // Assert
        Assert.IsNotNull(ingredient);
        Assert.AreEqual("Ingredient1", ingredient.Name);
        Assert.AreEqual(100, ingredient.Quantity);
        Assert.AreEqual("g", ingredient.Unit);
        Assert.AreEqual(50, ingredient.Calories);
        Assert.AreEqual("FoodGroup", ingredient.FoodGroup);
    }

    // Add more test methods as needed
}

// Helper class to capture console output for testing
public static class TestHelper
{
    public static string GetConsoleOutput(Action action)
    {
        var consoleOut = new System.IO.StringWriter();
        var originalOut = Console.Out;
        Console.SetOut(consoleOut);

        action.Invoke();

        Console.SetOut(originalOut);
        return consoleOut.ToString();
    }
}
