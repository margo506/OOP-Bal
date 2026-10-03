class Rectangle
{
    private double width;
    private double height;
    public double Width
    {
        get { return width; }
        set
        {
            if (value > 0)
                width = value;
            else
                width = 1;
        }
    }
    public double Height
    {
        get { return height; }
        set
        {
            if (value > 0)
                height = value;
            else
                height = 1;
        }
    }
    public double Area
    {
        get { return width * height; }
    }
    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }
    public double CalculatePerimeter()
    {
        return 2 * (Width + Height);
    }
}