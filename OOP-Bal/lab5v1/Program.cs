using System;
using System.Collections.Generic;

namespace lab5v1
{
    class Shape
    {
        public string Color { get; set; }
        public Shape(string color)
        {
            Color = color;
        }
        public virtual double GetArea()
        {
            return 0;
        }
        public virtual double GetPerimeter()
        {
            return 0;
        }
    }
    class Circle : Shape
    {
        public double Radius { get; set; }
        public Circle(string color, double radius)
            : base(color)
        {
            Radius = radius;
        }
        public override double GetArea()
        {
            return Math.PI * Radius * Radius;
        }
        public override double GetPerimeter()
        {
            return 2 * Math.PI * Radius;
        }
    }
    class Rectangle : Shape
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public Rectangle(string color, double width, double height)
            : base(color)
        {
            Width = width;
            Height = height;
        }
        public override double GetArea()
        {
            return Width * Height;
        }
        public override double GetPerimeter()
        {
            return 2 * (Width + Height);
        }
    }
    class Triangle : Shape
    {
        public double SideA { get; set; }
        public double SideB { get; set; }
        public double SideC { get; set; }
        public Triangle(string color, double sideA,
                        double sideB, double sideC)
            : base(color)
        {
            if (sideA <= 0 || sideB <= 0 || sideC <= 0 ||
                sideA + sideB <= sideC ||
                sideA + sideC <= sideB ||
                sideB + sideC <= sideA)
            {
                throw new ArgumentException(
                    "Сторони не утворюють правильний трикутник.");
            }
            SideA = sideA;
            SideB = sideB;
            SideC = sideC;
        }
        public override double GetArea()
        {
            double p = GetPerimeter() / 2;
            return Math.Sqrt(
                p * (p - SideA) *
                (p - SideB) * (p - SideC));
        }
        public override double GetPerimeter()
        {
            return SideA + SideB + SideC;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<Shape> shapes = new List<Shape>
            {
                new Circle("Червоний", 5),
                new Rectangle("Синій", 4, 6),
                new Triangle("Зелений", 3, 4, 5)
            };
            double totalArea = 0;
            double totalPerimeter = 0;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            foreach (Shape shape in shapes)
            {
                double area = shape.GetArea();
                double perimeter = shape.GetPerimeter();
                Console.WriteLine($"Тип фігури: {shape.GetType().Name}");
                Console.WriteLine($"Колір: {shape.Color}");
                Console.WriteLine($"Площа: {area:F2}");
                Console.WriteLine($"Периметр: {perimeter:F2}");
                Console.WriteLine();
                totalArea += area;
                totalPerimeter += perimeter;
            }
            Console.WriteLine($"Сумарна площа: {totalArea:F2}");
            Console.WriteLine($"Сумарний периметр: {totalPerimeter:F2}");
        }
    }
}