using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using System.Globalization;
using Betonarka_Kocovce.Models;

namespace Betonarka_Kocovce
{
    public static class CsvLayer
    {
        public const string dateTimeFormat = "dd-MM-yyyy";
        public static readonly string dataPath = "data";

        public static string GetCurrentDateTimeStr()
        {
            return DateTime.Now.ToString(dateTimeFormat);
        }

        public static void SaveMiesacky(List<double> data)
        {
            var currDateTime = GetCurrentDateTimeStr();
            var savePath = $"{dataPath}\\{currDateTime}";
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            var records = new List<MiesackyModel>
            {
                new MiesackyModel
                {
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
            using (var writer= new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.CurrentCulture))
            {
                csv.WriteRecords(records);
            }
        }
    }
}
