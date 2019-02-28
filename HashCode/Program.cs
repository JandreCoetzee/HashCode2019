using System;
using System.IO;
using MathNet.Numerics.LinearAlgebra;

namespace HashCode
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("HashCode");
            Console.WriteLine("========");
            Console.WriteLine();

            var data = ReadFromFile("example.in");
            WriteToFile(data);

            Console.ReadLine();
        }

        public static string ReadFromFile(string fileName)
        {
            var data = string.Empty;
            using (var fileStream = new FileStream(fileName, FileMode.Open))
            {
                using (var reader = new StreamReader(fileStream))
                {
                    data = reader.ReadToEnd();
                }
            }
            Console.WriteLine(data);
            return data;
        }

        public static void WriteToFile(string result)
        {
            using (var file = new StreamWriter(File.Create("result.txt")))
            {
                file.WriteLine(result);
            }
        }

        public static void MathDotNetExample()
        {
            var m = Matrix<double>.Build.Random(3, 4, 1);
            Console.WriteLine(m.ToString());

            var v = Vector<double>.Build.Random(4, 1);
            Console.WriteLine(v.ToString());

            var v2 = m * v;
            Console.WriteLine(v2.ToString());

            var m2 = m + 2.0 * m;
            Console.WriteLine(m2.ToString());

            var v3 = m.Multiply(v);
            Console.WriteLine(v3.ToString());

            var m3 = m.Add(m.Multiply(2));
            Console.WriteLine(m3.ToString());
        }
    }
}
