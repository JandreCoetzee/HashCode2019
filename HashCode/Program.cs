using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


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

            var verticalSlides = VerticalPhotos(data.PhotosV).ToList();
            var horizontalSlides = HorizontalPhotos(data.PhotosH);

           // var horizontalSlides = data.PhotosH.Select(x => new Slide(){ Ids = new List<int>() { x.Id }, Tags = x.Tags }).ToList();


            var slides = verticalSlides;
            slides.AddRange(horizontalSlides);

            WriteToFile(slides);

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

        public static void WriteToFile(List<Slide> slides)
        {
            using (var file = new StreamWriter(File.Create("result.txt")))
            {
                var totalSlides = slides.Count;

                file.WriteLine(totalSlides);

                foreach (var slide in slides)
                {
                    foreach (var id in slide.Ids)
                    {
                        file.Write(id + " ");
                    }
                    file.Write( "\n");
                }
            }
        }

        public static List<Slide> VerticalPhotos(List<Photo> vs)
        {
            var slides = new List<Slide>();
            var noMatchPhoto = new List<Photo>();

            var counter = vs.Count;

            while (counter != 0)
            {
               var targetPhoto = vs.First();
                vs.RemoveAt(0);
                var hasFoundMatch = false;
                for (int i = 0; i < vs.Count; i++)
                {
                    if (targetPhoto.Tags.Intersect(vs[i].Tags).Any())
                    {             
                        var slide = new Slide()
                        {
                            Ids = new List<int> {targetPhoto.Id, vs[i].Id },
                            Tags = targetPhoto.Tags.Union(vs[i].Tags).ToList()
                        };
                        slides.Add(slide);
                        vs.RemoveAt(i);
                        hasFoundMatch = true;
                        break;
                    }
                }

                if (!hasFoundMatch)
                {
                    noMatchPhoto.Add(targetPhoto);
                }

                counter = vs.Count;
            }

            return slides;
        }

        public static List<Slide> HorizontalPhotos(List<Photo> hs)
        {
            var slides = new List<Slide>();
            var noMatchPhoto = new List<Photo>();

            var counter = hs.Count;

            while (counter != 0)
            {
                var targetPhoto = hs.First();
                hs.RemoveAt(0);
                var hasFoundMatch = false;
                for (int i = 0; i < hs.Count; i++)
                {
                    if (targetPhoto.Tags.Intersect(hs[i].Tags).Any())
                    {
                        var slideTarget = new Slide()
                        {
                            Ids = new List<int> { targetPhoto.Id},
                            Tags = targetPhoto.Tags
                        };
                        slides.Add(slideTarget);

                        var slideMatch = new Slide()
                        {
                            Ids = new List<int> { hs[i].Id },
                            Tags = hs[i].Tags
                        };
                        slides.Add(slideMatch);
                        hs.RemoveAt(i);
                        hasFoundMatch = true;
                        break;
                    }
                }

                if (!hasFoundMatch)
                {
                    noMatchPhoto.Add(targetPhoto);
                }

                counter = hs.Count;
            }

            return slides;
        }

        //public List<Slide> MatchVerticalAndHorizontal(List<Slide> verticalSlides, List<Slide> horizontalSlides) 
        //{
        //}
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
