using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SpriteColorer
{
    internal class Program
    {
        public static float lerp(float a, float b, float t)
        {
            return (b - a) * t + a;
        }
        public static Color lerpColor(Color a, Color b, float t)
        {
            return Color.FromArgb((int) Math.Floor(lerp(a.R, b.R, t)), (int) Math.Floor(lerp(a.G, b.G, t)),
                (int) Math.Floor(lerp(a.B, b.B, t)));
        }
        public static Color multiplyColor(Color a, Color b)
        {
            return Color.FromArgb((int)Math.Floor((a.A / 255f * b.A / 255f)*255f) ,(int)Math.Floor((a.R / 255f * b.R / 255f)*255f),(int)Math.Floor((a.G / 255f * b.G / 255f)*255f),(int)Math.Floor((a.B / 255f * b.B / 255f)*255f));
        }

        [STAThreadAttribute]
        public static void Main(string[] args)
        {
            var path = Directory.GetCurrentDirectory() + "/Sprite/";
            Directory.CreateDirectory(path + "Output");
            string spN = path + "sprite.png";
            string maskN = path + "mask.png";
            int range = 15;
            Console.WriteLine(args.Length);
            if (args.Length == 2)
            {
                spN = args[0];
                maskN = args[1];
            }
            else if (args.Length == 3)
            {
                spN = args[0];
                maskN = args[1];
                range = int.Parse(args[2]);
            }
            else
            {
                Console.Write("Sprite Path: ");
                spN = Console.ReadLine();
                Console.Write("Mask Path: ");
                maskN = Console.ReadLine();
                Console.Write("Color Range(1-255)(Default 15): ");
                range = int.Parse(Console.ReadLine());
            }
            var mask = new Bitmap(maskN);
            var sprite = new Bitmap(spN);
            int c = range;
            int i = 0;
            float rangeSize = 255f/c;
            for (int r = 0; r < c; r++)
            {
                for (int g = 0; g < c; g++)
                {
                    for (int b = 0; b < c; b++)
                    {
                        Color col = Color.FromArgb((int) Math.Floor(r * rangeSize), (int) Math.Floor(g * rangeSize),
                            (int) Math.Floor(b * rangeSize));
                        Bitmap newMap = new Bitmap(sprite.Width,sprite.Height);
                        for (int x = 0; x < newMap.Width; x++)
                        {
                            for (int y = 0; y < newMap.Height; y++)
                            {
                                var newCol = lerpColor(sprite.GetPixel(x, y), multiplyColor(sprite.GetPixel(x, y), col),
                                    mask.GetPixel(x, y).R / 255f);
                                var a = sprite.GetPixel(x, y).A;
                                newMap.SetPixel(x,y,multiplyColor(newCol, Color.FromArgb(a,a,a,a)));
                            }
                        }
                        string outp = $"outp_{i}";
                        newMap.Save($"{path}/Output/{outp}.png");
                        i++;
                    }
                }
            }
        }
    }
}