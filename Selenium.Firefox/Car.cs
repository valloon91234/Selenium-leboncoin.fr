using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Valloon.Selenium
{
    class Car
    {
        public String DirectoryName { get; set; }
        public String Brand { get; set; }
        public String Model { get; set; }
        public String Year { get; set; }
        public String Mileage { get; set; }
        public String Fuel { get; set; }
        public String Gearbox { get; set; }
        public String Subject { get; set; }
        public String Description { get; set; }
        public String Price { get; set; }
        public String Pictures { get; set; }
        public String Address { get; set; }
        public String Tel { get; set; }

        public static String CheckNull(String input)
        {
            if (String.IsNullOrWhiteSpace(input)) return null;
            return input;
        }

        public static Car[] ReadData(String excelFilename)
        {
            //String ext = Path.GetExtension(excelFilename);
            //if (ext == "xlsx") return ReadDataFromXlsx(excelFilename);
            //if (ext == "csv") return ReadDataFromCSV(excelFilename);
            return ReadDataFromTxt(excelFilename);
        }

        public static Car[] ReadDataFromTxt(String inputFilename)
        {
            List<Car> list = new List<Car>();
            String[] lines = File.ReadAllLines(inputFilename, Encoding.UTF8);
            int lineCount = lines.Length;
            String name = null, value = null;
            Car car = null;
            for (int i = 0; i < lineCount; i++)
            {
                String line = lines[i];
                if (line.StartsWith("#") || line.StartsWith("//")) continue;
                try
                {
                    String[] array = line.Split(new Char[] { '=' }, 2);
                    if (name != null && value != null)
                    {
                        int quoteCount = line.Split('\"').Length;
                        if (quoteCount > 1)
                        {
                            value += "\n" + line.Replace("\"", "");
                        }
                        else
                        {
                            value += "\n" + line;
                            continue;
                        }
                    }
                    else if (String.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }
                    else
                    {
                        name = array[0].Trim().ToLower();
                        value = array.Length > 1 ? array[1].Trim() : null;
                        if (String.IsNullOrWhiteSpace(value)) value = null;
                        if (value != null)
                        {
                            int quoteCount = value.Split('\"').Length;
                            if (quoteCount == 2)
                            {
                                value = value.Replace("\"", "");
                                continue;
                            }
                            else if (quoteCount == 3)
                            {
                                value = value.Replace("\"", "");
                            }
                        }
                    }
                    if (value != null) value = value.Trim();
                    if (name == "folder")
                    {
                        car = new Car();
                        car.DirectoryName = value;
                    }
                    else if (name == "brand")
                    {
                        car.Brand = value;
                    }
                    else if (name == "model")
                    {
                        car.Model = value;
                    }
                    else if (name == "year")
                    {
                        car.Year = value;
                    }
                    else if (name == "mileage")
                    {
                        car.Mileage = value;
                    }
                    else if (name == "fuel")
                    {
                        car.Fuel = value;
                    }
                    else if (name == "gearbox")
                    {
                        car.Gearbox = value;
                    }
                    else if (name == "subject")
                    {
                        car.Subject = value;
                    }
                    else if (name == "description")
                    {
                        car.Description = value;
                    }
                    else if (name == "price")
                    {
                        car.Price = value;
                    }

                    else if (name == "pictures")
                    {
                        car.Pictures = value;
                    }
                    else if (name == "address")
                    {
                        car.Address = value;
                    }
                    else if (name == "tel")
                    {
                        car.Tel = value;
                    }
                    else if (name == "end")
                    {
                        if (car != null) list.Add(car);
                    }
                    else
                    {
                        Robot.Print($"\tUnknown KEY : {name} on line {i + 1} = {line}");
                    }
                    name = null; value = null;
                }
                catch
                {
                    Robot.Print($"\tError on line {i + 1} = {line}");
                }
            }
            return list.ToArray();
        }

    }
}
