// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;

internal class MealData
{
    private string _meal;
  private Single _calorie_Count;
  private string _meal_Description;
    private bool _balance ;

    public string Meal { get => _meal; set => _meal = value; }
    public Single Calorie_Count { get => _calorie_Count; set => _calorie_Count = value; }
    public string Meal_Description { get => _meal_Description; set => _meal_Description = value; }
    [JsonProperty("Balanced_Meal")]
    public bool Balance { get => _balance; set => _balance = value; }
}