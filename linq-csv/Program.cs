using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;

namespace linq_csv
{
    class Program
    {
        static void Main(string[] args)
        {
            string csvPath = @"D:\Projects\linq-csv\linq-csv\googleplaystore1.csv";
            var googleApps = LoadGoogleAps(csvPath);

            //Display(googleApps);
            //GetData(googleApps);
            ProjectData(googleApps);
        }

        static void ProjectData(IEnumerable<GoogleApp> googleApps)
        {
            var highRatedBeautyApps = googleApps.Where(app => app.Rating > 4.6 && app.Category == Category.BEAUTY);
            var highRaterBeautyAppsNames = highRatedBeautyApps.Select(app => app.Name);
            //Console.WriteLine(string.Join(", ", highRaterBeautyAppsNames));

            var dtos = highRatedBeautyApps.Select(app => new GoogleAppDto()
            {
                Reviews = app.Reviews,
                Name = app.Name
            });

            // typ anonimowy
            var anonymousDtos = highRatedBeautyApps.Select(app => new
            {
                Reviews = app.Reviews,
                Name = app.Name,
                Category = app.Category
            });

            foreach (var dto in anonymousDtos)
            {
                Console.WriteLine($"{dto.Name}: {dto.Reviews}");
            }

            //var genres = highRatedBeautyApps.SelectMany(app => app.Genres);
            //Console.WriteLine(string.Join(":", genres));
        }

        static void GetData(IEnumerable<GoogleApp> googleApps)
        {
            var highRatedApps = googleApps.Where(app => app.Rating > 4.6);
            var highRatedBeautyApps = googleApps.Where(app => app.Rating > 4.6 && app.Category == Category.BEAUTY);
            Display(highRatedBeautyApps);

            var firstHighRatedBeautyApp = highRatedBeautyApps.FirstOrDefault(app => app.Reviews < 200);
            // .SingleOrDefault
            // .LastOrDefault
            Console.WriteLine("firstHighRatedBeautyApp with less than 300 reviews");
            Console.WriteLine(firstHighRatedBeautyApp);
        }

        static void Display(IEnumerable<GoogleApp> googleApps)
        {
            foreach (var googleApp in googleApps)
            {
                Console.WriteLine(googleApp);
            }
        }

        static void Display(GoogleApp googleApp)
        {
            Console.WriteLine(googleApp);
        }

        static List<GoogleApp> LoadGoogleAps(string csvPath)
        {
            using (var reader = new StreamReader(csvPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<GoogleAppMap>();
                var records = csv.GetRecords<GoogleApp>().ToList();
                return records;
            }
        }
    }
}