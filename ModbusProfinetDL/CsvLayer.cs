﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using System.Globalization;
using BetonarkaDL.Models;
using CsvHelper.Configuration;

namespace BetonarkaDL
{
    public static class CsvLayer
    {
        public const string dateFormat = "dd-MM-yyyy";
        public const string dateTimeFormat = "HH:mm:ss dd-MM-yyyy";
        public const string dateTimeFormatCsv = "HH:mm:ss_dd-MM-yyyy";
        public static readonly string dataPath = AppDomain.CurrentDomain.BaseDirectory + "\\data";
        public static string lastTimeMiesacky;
        public static string lastTimePalety;

        private static string GetSavePath(DateTime now)
        {
            var ci = new CultureInfo("en-US");
            var currYear = now.ToString("yyyy");
            var currMonth = now.ToString("MMMM", ci);
            var currDate = now.ToString(dateFormat);
            var savePath = $"{dataPath}\\{currYear}\\{currMonth}\\{currDate}";
            return savePath;
        }

        public static void SaveData<T>(List<T> data, string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = (File.Exists(filePath) == false),
            };

            try
            {
                using (var stream = File.Open(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.WriteRecords(data);
                }
            }
            catch (Exception ex)
            {
                Library.WriteLog("Zapis do CSV suboru zlyhal:");
                Library.WriteLog(ex);
            }
        }

        public static void SaveMiesacky(List<double> data)
        {
            var currDateTime = DateTime.Now;
            lastTimeMiesacky = currDateTime.ToString(dateTimeFormat);
            var savePath = GetSavePath(currDateTime);
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

                }
            };
            if (data.Count > 4)
            {
                records[0].MalaCement = data[4];
                records[0].MalaCementBiely = data[5];
                records[0].MalaStuska = data[6];
            }

            var filePath = $"{savePath}\\dataBetonarka.csv";
            SaveData(records, filePath);
        }

        public static void SavePalety(List<int> data)
        {
            var currDateTime = DateTime.Now;
            lastTimePalety = currDateTime.ToString(dateTimeFormat);
            var savePath = GetSavePath(currDateTime);
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            var records = new List<PaletyModel>
            {
                new PaletyModel
                {
                    Cas = currDateTime.ToString(dateTimeFormatCsv),
                    ProgramTERAMEX = data[0],
                    VrstevPaleta = data[1],
                    ProgramHESS = data[2],
                    DenniPocitadlo = data[3]
                }
            };

            var filePath = $"{savePath}\\dataPalety.csv";
            SaveData(records, filePath);
        }
    }
}
