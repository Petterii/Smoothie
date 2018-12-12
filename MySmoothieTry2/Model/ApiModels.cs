using System;
using System.Collections.Generic;

namespace MySmoothieTry2.Model
{
    public class Nutrients
    {
        public double ENERC_KCAL { get; set; }
        public double PROCNT { get; set; }
        public double FAT { get; set; }
        public double CHOCDF { get; set; }
    }

    public class Food
    {
        public string FoodId { get; set; }
        public string Uri { get; set; }
        public string Label { get; set; }
        public Nutrients Nutrients { get; set; }
        public string Category { get; set; }
        public string CategoryLabel { get; set; }
    }

    public class Parsed
    {
        public Food Food { get; set; }
    }

    public class Nutrients2
    {
        public double ENERC_KCAL { get; set; }
        public double PROCNT { get; set; }
        public double FAT { get; set; }
        public double CHOCDF { get; set; }
    }

    public class Food2
    {
        public string FoodId { get; set; }
        public string Uri { get; set; }
        public string Label { get; set; }
        public Nutrients2 Nutrients { get; set; }
        public string Category { get; set; }
        public string CategoryLabel { get; set; }
        public string Brand { get; set; }
        public string FoodContentsLabel { get; set; }
    }

    public class Measure
    {
        public string Uri { get; set; }
        public string Label { get; set; }
    }

    public class Hint
    {
        public Food2 Food { get; set; }
        public List<Measure> Measures { get; set; }
    }

    public class Next
    {
        public string Title { get; set; }
        public string Href { get; set; }
    }

    public class Links
    {
        public Next Next { get; set; }
    }

    public class IngredientMainModel
    {
        public string Text { get; set; }
        public List<Parsed> Parsed { get; set; }
        public List<Hint> Hints { get; set; }
        public Links _links { get; set; }
    }


    // Model used for making a POST request (for receiving nutrition info)
    public class NutritionModelPOST
    {
        //public int yield = 1;
        public List<NutritionPOST> ingredients = new List<NutritionPOST>();
    }

    public class NutritionPOST
    {
        public int quantity = 100; // = 100 ml
        public string measureURI = "http://www.edamam.com/ontologies/edamam.owl#Measure_milliliter";
        public string foodURI { get; set; }
    }

    // For parsing the received nutrition data
    public class NutritionPOSTReply
    {
        public string Uri { get; set; }
        public long Yield { get; set; }
        public long Calories { get; set; }
        public long GlycemicIndex { get; set; }
        public long TotalWeight { get; set; }
        public List<string> DietLabels { get; set; }
        public List<string> HealthLabels { get; set; }
        public List<object> Cautions { get; set; }
        public Total TotalNutrients { get; set; }
        public Total TotalDaily { get; set; }
        public List<SingleIngredient> Ingredients { get; set; }
    }

    public class SingleIngredient
    {
        public List<Parsed> ParsedNutr { get; set; }
    }

    public class ParsedNutr
    {
        public long Quantity { get; set; }
        public string Measure { get; set; }
        public string Food { get; set; }
        public string FoodId { get; set; }
        public string FoodUri { get; set; }
        public long Weight { get; set; }
        public long RetainedWeight { get; set; }
        public string MeasureUri { get; set; }
        public string Status { get; set; }
    }

    public class Total
    {
        public EnercKcal EnercKcal { get; set; }
        public EnercKcal Fat { get; set; }
    }

    public class EnercKcal
    {
        public string Label { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
    }
}
