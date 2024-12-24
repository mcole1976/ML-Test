// See https://aka.ms/new-console-template for more information
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Newtonsoft.Json;

Console.WriteLine("Hello, World!");
var json = File.ReadAllText("C:\\Users\\Marcus\\Documents\\Code\\ExerciseDB.Food_DiaryA.json"); 
var mealData = JsonConvert.DeserializeObject<List<MealData>>(json); 
TrainModel(mealData);

void TrainModel(List<MealData>? mealData)
{
    // ML.NET setup var
    MLContext mlContext = new MLContext(); 
    // Load data
    var data = mlContext.Data.LoadFromEnumerable(mealData); 
    // Data preparation
    var dataProcessPipeline = mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "MealEncoded", inputColumnName: "Meal") .Append(mlContext.Transforms.Concatenate("Features", "MealEncoded")); 
    // Define trainer
    var trainer = mlContext.Regression.Trainers.Sdca(labelColumnName: "Calorie_Count", featureColumnName: "Features"); 
    // Train model
    var trainingPipeline = dataProcessPipeline.Append(trainer); 
    var model = trainingPipeline.Fit(data); 
    // Save the model
    mlContext.Model.Save(model, data.Schema, "model.zip");
    // Make predictions
    Single user_calories = 450;
    MakePredictions(mlContext, model, mealData , user_calories);
}

void MakePredictions(MLContext mlContext, TransformerChain<RegressionPredictionTransformer<LinearRegressionModelParameters>> model, List<MealData>? mealData,  Single cals)
{
    var suggestions = mealData
        .Where(m => m.Balance && m.Calorie_Count <= cals).OrderBy(m => m.Calorie_Count).Take(25).ToList();
    Console.WriteLine("Suggested balanced meals to help hit your calorie goal:"); 
    foreach (var suggestion in suggestions)
    {
        Console.WriteLine($"Meal: {suggestion.Meal}, Description: {suggestion.Meal_Description}, Calories: {suggestion.Calorie_Count}");
    }
}

public class MealPrediction { public float Calorie_Count { get; set; } }

//foreach(MealData m in mealData)
//{
//    Console.WriteLine(m.Meal);
//    Console.WriteLine(m.Meal_Description);
//    Console.WriteLine(m.Calorie_Count);
//}

