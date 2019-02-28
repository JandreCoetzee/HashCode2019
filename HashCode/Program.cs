using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

            var data = ReadFromFile("c_memorable_moments.txt");



           // WriteToFile(data);

           // Console.ReadLine();
        }

        public static GooglePhotos ReadFromFile(string fileName)
        {
            var googlePhotos = new GooglePhotos();

            using (var fileStream = new FileStream(fileName, FileMode.Open))
            {
                using (var reader = new StreamReader(fileStream))
                {
                    var totalPhotos = reader.ReadLine();
                    var photosString = reader.ReadToEnd();

                    googlePhotos.TotalPhotos = Int32.Parse(totalPhotos);
                    googlePhotos.PhotosString = photosString;
                }
            }

            var counter = 0;

            foreach (var googlePhoto in googlePhotos.PhotosString.Split('\n'))
            {
                if (googlePhoto.StartsWith("V"))
                {
                   var items = googlePhoto.Split(" ");

                    var orientation = items[0];
                    var totalTags = Int32.Parse(items[1]);
                    var tags = new List<string>();

                    for (int i = 2; i< items.Length; i++)
                    {
                        tags.Add(items[i]);
                    }

                    var photo = new Photo()
                    {
                        Orientation =  orientation,
                        TotalTags = totalTags,
                        Tags = tags,
                        Id = counter                      
                    };

                    googlePhotos.PhotosV.Add(photo);
                    counter++;
                }
                else if(googlePhoto.StartsWith("H"))
                {
                    var items = googlePhoto.Split(" ");

                    var orientation = items[0];
                    var totalTags = Int32.Parse(items[1]);
                    var tags = new List<string>();

                    for (int i = 2; i < items.Length; i++)
                    {
                        tags.Add(items[i]);
                    }

                    var photo = new Photo()
                    {
                        Orientation = orientation,
                        TotalTags = totalTags,
                        Tags = tags,
                        Id = counter
                    };

                    googlePhotos.PhotosH.Add(photo);
                    counter++;
                }         
            }

            return googlePhotos;
        }

        public static void WriteToFile(string result)
        {
            using (var file = new StreamWriter(File.Create("result.txt")))
            {
                file.WriteLine(result);
            }
        }

        public static void VerticalPhotos(List<Photo> vs)
        {
            var slides = new List<Slide>();

            //Photo targetPhoto = null;
            //for (var i = 0; i < vs.Count; i++)
            //{
            //    targetPhoto = vs[i];
            //    targetPhoto.Tags.in
            //}
        }
    }





    public class GooglePhotos
    {
        public int TotalPhotos { get; set; }
        public string PhotosString { get; set; }
        public List<Photo> PhotosH { get; set; } = new List<Photo>();
        public List<Photo> PhotosV{ get; set; } = new List<Photo>();
    }

    public class Photo
    {
        public int Id { get; set; }
        public string Orientation { get; set; }
        public int TotalTags { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }

    public class Slide
    {
        public List<int> Ids { get; set; }
        public List<string> Tags { get; set; }
    }
}
