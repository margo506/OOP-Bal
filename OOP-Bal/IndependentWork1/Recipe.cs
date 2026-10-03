class Recipe
{
    private string name = "";
    private int cookingTime;
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public int CookingTime
    {
        get { return cookingTime; }
        set
        {
            if (value > 0)
                cookingTime = value;
            else
                cookingTime = 1;
        }
    }
    public Recipe(string name, int cookingTime)
    {
        Name = name;
        CookingTime = cookingTime;
    }
    public bool IsQuickRecipe()
    {
        return CookingTime <= 30;
    }
}