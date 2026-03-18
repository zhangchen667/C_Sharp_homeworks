using System;
namespace ShapeProject
{
    public abstract class Shape
    {
        public abstract double GetArea();
        public abstract bool IsValid();
    }
    public class Circle : Shape
    {
        public double radius { get; set; }
        public Circle(double r) { radius = r; }
        public override double GetArea()
        {
            return Math.PI * radius * radius;
        }
        public override bool IsValid()
        {
            return radius > 0;
        }
    }
    public class Rectangle : Shape
    {
        public double width { get; set; }
        public double height { get; set; }
        public Rectangle(double w, double h)
        {
            width = w;
            height = h;
        }
        public override double GetArea()
        {
            return width * height;
        }
        public override bool IsValid()
        {
            return (width > 0) && (height > 0);
        }
    }
    public class Square : Rectangle
    {
        public Square(double side) : base(side, side)//base调用父类构造
        {

        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            double totalArea = 0;
            Random random = new Random();
            Console.WriteLine("生成10个形状");
            for (int i = 0; i<10; i++)
            {
                Shape shape = null;//初始化

                bool isGen = false;//判断是否生成成功
                while (!isGen)//生成失败了
                {
                    int type = random.Next(3);//生成一个确认形状的随机数,随机生成012
                    double val = random.NextDouble() * 10 + 0.1;//生成一个0.1-10的正数
                    double val2 = random.NextDouble() * 10 + 0.1;//生成一个0.1-10的正数
                    switch (type)
                    {
                        case 0:
                            shape = new Circle(val);
                            break;
                        case 1:
                            shape = new Square(val);
                            break;
                        case 2:
                            shape = new Rectangle(val,val2);
                            break;
                    }
                    if (shape.IsValid())
                    {
                        isGen = true;
                    }//生成成功

                }
                string name = shape.GetType().Name;//获得子类名称
                string area = shape.GetArea().ToString("F3");//保留三位小数
                Console.WriteLine($"第{i}个是：{name},面积是{area}");
                totalArea += shape.GetArea();

            }
            string total = totalArea.ToString("F3");
            Console.WriteLine($"总面积是{total}");
            
        }
    }
}