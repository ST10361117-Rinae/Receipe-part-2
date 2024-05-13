using System;
using System.Collections.Generic;

class Ingredient
{
    public string Name { get; set; }
    public double Quantity { get; set; }
    public string Unit { get; set; }
    public double Calories { get; set; }
    public string FoodGroup { get; set; }
}

class Recipe
{
    public string RecipeID { get; set; }
    public string Name { get; set; }
    public List<Ingredient> Ingredients { get; }
    public List<string> Steps { get; }

    public Recipe(string recipeID)
    {
        RecipeID = recipeID;
        Ingredients = new List<Ingredient>();
        Steps = new List<string>();
    }

    public void AddIngredient(string name, double quantity, string unit, double calories, string foodGroup)
    {
        Ingredients.Add(new Ingredient { Name = name, Quantity = quantity, Unit = unit, Calories = calories, FoodGroup = foodGroup });
    }

    public void AddStep(string description)
    {
        Steps.Add(description);
    }

    public void DisplayRecipe()
    {
        Console.WriteLine($"Recipe: {Name}");
        Console.WriteLine("Ingredients:");
        double totalCalories = 0;
        foreach (var ingredient in Ingredients)
        {
            Console.WriteLine($"- {ingredient.Quantity} {ingredient.Unit} of {ingredient.Name} ({ingredient.Calories} calories)");
            totalCalories += ingredient.Calories;
        }
        Console.WriteLine($"Total Calories: {totalCalories}");
        Console.WriteLine("Steps:");
        for (int i = 0; i < Steps.Count; i++)
        {
            Console.WriteLine($"Step {i + 1}: {Steps[i]}");
        }

        if (totalCalories > 300)
        {
            NotifyExceedingCalories?.Invoke(this, EventArgs.Empty);
        }
    }

    public delegate void ExceedingCaloriesHandler(object sender, EventArgs e);
    public event ExceedingCaloriesHandler NotifyExceedingCalories;
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to the Recipe Application!");

        List<Recipe> recipes = new List<Recipe>();

        while (true)
        {
            Console.WriteLine("\nMain Menu:");
            Console.WriteLine("1. Add Recipe");
            Console.WriteLine("2. Display Recipes");
            Console.WriteLine("3. Exit");
            Console.Write("Select an option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    AddRecipe(recipes);
                    break;
                case "2":
                    DisplayRecipes(recipes);
                    break;
                case "3":
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void AddRecipe(List<Recipe> recipes)
    {
        Recipe myRecipe = new Recipe("");

        do
        {
            Console.Write("Enter recipe name: ");
            myRecipe.Name = Console.ReadLine();

            if (ContainsDigit(myRecipe.Name))
            {
                Console.WriteLine("Recipe name cannot contain numbers. Please try again.");
            }

        } while (ContainsDigit(myRecipe.Name));

        int numIngredients;
        do
        {
            Console.Write("Enter the number of ingredients: ");
        } while (!int.TryParse(Console.ReadLine(), out numIngredients) || numIngredients <= 0);

        for (int i = 0; i < numIngredients; i++)
        {
            Console.Write($"Ingredient {i + 1} name: ");
            string ingredientName = Console.ReadLine();

            double ingredientQuantity;
            do
            {
                Console.Write("Quantity: ");
            } while (!double.TryParse(Console.ReadLine(), out ingredientQuantity) || ingredientQuantity <= 0);

            string ingredientUnit;
            do
            {
                Console.WriteLine("Available units: g, kg, ml, l, tsp, cups, whole");
                Console.Write("Select unit: ");
            } while (!IsValidUnit(ingredientUnit = Console.ReadLine()));

            double ingredientCalories;
            do
            {
                Console.Write("Calories: ");
            } while (!double.TryParse(Console.ReadLine(), out ingredientCalories) || ingredientCalories < 0);

            Console.Write("Food Group: ");
            string foodGroup = Console.ReadLine();

            myRecipe.AddIngredient(ingredientName, ingredientQuantity, ingredientUnit, ingredientCalories, foodGroup);
        }

        int numSteps;
        do
        {
            Console.Write("Enter the number of steps: ");
        } while (!int.TryParse(Console.ReadLine(), out numSteps) || numSteps <= 0);

        for (int i = 0; i < numSteps; i++)
        {
            Console.Write($"Step {i + 1}: ");
            myRecipe.AddStep(Console.ReadLine());
        }

        recipes.Add(myRecipe);
        Console.WriteLine("Recipe added successfully.");
    }

    static void DisplayRecipes(List<Recipe> recipes)
    {
        if (recipes.Count == 0)
        {
            Console.WriteLine("No recipes found.");
            return;
        }

        recipes.Sort((x, y) => string.Compare(x.Name, y.Name));

        Console.WriteLine("\nList of Recipes:");
        foreach (var recipe in recipes)
        {
            Console.WriteLine(recipe.Name);
        }

        Console.Write("\nEnter the name of the recipe you want to display: ");
        string selectedRecipeName = Console.ReadLine();

        Recipe selectedRecipe = recipes.Find(r => r.Name.Equals(selectedRecipeName, StringComparison.OrdinalIgnoreCase));
        if (selectedRecipe != null)
        {
            selectedRecipe.DisplayRecipe();
        }
        else
        {
            Console.WriteLine("Recipe not found.");
        }
    }

    static bool ContainsDigit(string input)
    {
        foreach (char c in input)
        {
            if (char.IsDigit(c))
            {
                return true;
            }
        }
        return false;
    }

    static bool IsValidUnit(string unit)
    {
        List<string> validUnits = new List<string> { "g", "kg", "ml", "l", "tsp", "cups", "whole" };
        return validUnits.Contains(unit.ToLower());
    }
}
