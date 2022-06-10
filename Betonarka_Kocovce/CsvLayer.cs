using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using System.Globalization;
using Betonarka_Kocovce.Models;
using CsvHelper.Configuration;

namespace Betonarka_Kocovce
{
    public static class CsvLayer
    {
        public const string dateFormat = "dd-MM-yyyy";
        public const string dateTimeFormat = "HH:mm:ss dd-MM-yyyy";
        public const string dateTimeFormatCsv = "HH:mm:ss_dd-MM-yyyy";
        public static readonly string dataPath = "data";
        public static string lastTimeMiesacky;
        public static string lastTimePalety;

        public static string GetCurrentDateTimeStr(string format)
        {
            return DateTime.Now.ToString(format);
        }

        public static void SaveData<T>(List<T> data, string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = (File.Exists(filePath) == false),
            };

            using (var stream = File.Open(filePath, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(data);
            }
        }

        public static void SaveMiesacky(List<double> data)
        {
            var currDateTime = DateTime.Now;
            lastTimeMiesacky = currDateTime.ToString(dateTimeFormat);
            var savePath = $"{dataPath}\\{currDateTime.ToString(dateFormat)}";
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            var records = new List<MiesackyModel>
            {
                new MiesackyModel
                {
                    Cas = currDateTime.ToString(dateTimeFormatCsv),
                    VelkaCement = data[0],
                    VelkaCementBiely = data[1],
                    VelkaStuska = data[2],
                    VelkaPopol = data[3],
                    MalaCement = data[4],
                    MalaCementBiely = data[5],
                    MalaStuska = data[6],
                }
            };

            var filePath = $"{savePath}\\dataBetonarka.csv";
            SaveData(records, filePath);
        }

        public static void SavePalety(List<int> data)
        {
            var currDateTime = DateTime.Now;
            lastTimePalety = currDateTime.ToString(dateTimeFormat);
            var savePath = $"{dataPath}\\{currDateTime.ToString(dateFormat)}";
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            var records = new List<PaletyModel>
            {
                new PaletyModel
                {
                    Cas = currDateTime.ToString(dateTimeFormatCsv),
                    Sortiment = data[0],
                    VrstievPaleta = data[1]
                }
            };

            var filePath = $"{savePath}\\dataPalety.csv";
            SaveData(records, filePath);
        }
    }
}
