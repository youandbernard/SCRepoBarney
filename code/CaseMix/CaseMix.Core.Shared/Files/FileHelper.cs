using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace CaseMix.Core.Shared.Files
{
    public static class FileHelper
    {
        public static DataTable CSVtoDataTableParserWithValidation(IFormFile file, string specialtyId, string deviceClassId, int deviceFamilyId, out List<string> errors, out int numRows)
        {
            DataTable dt = new DataTable();
            errors = new List<string>();
            numRows = 0;

            using (var stream = file.OpenReadStream())
            using (TextFieldParser parser = new TextFieldParser(stream))
            {
                parser.Delimiters = new[] { "," };
                parser.HasFieldsEnclosedInQuotes = true;

                int cycle = 0;
                int columnCnt = 0;

                while (!parser.EndOfData)
                {
                    string[] line = parser.ReadFields();
                    if (cycle == 0)
                    {
                        foreach (string h in line)
                        {
                            if (!string.IsNullOrWhiteSpace(h))
                            {
                                dt.Columns.Add(h);
                                columnCnt += 1;
                            }
                        }
                    }
                    else
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < columnCnt - 1; i++)
                        {
                            if (i != 3 && i != 5 && i != 9)
                            {
                                if (string.IsNullOrWhiteSpace(line[i]))
                                    errors.Add($"Error on row: {cycle + 1}, column: {i + 1}. Required fields must not be emtpy.");
                            }

                            if (i == 6)
                            {
                                if (string.IsNullOrWhiteSpace(line[i]))
                                    errors.Add($"Error on row: {cycle + 1}, column: {i + 1}. Specialty in file must not a blank.");
                                else
                                {
                                    if (line[i].ToString().Trim() != specialtyId)
                                    {
                                        errors.Add($"Error on row: {cycle + 1}, column: {i + 1}. Specialty in file does not match the one selected.");
                                    }
                                }
                            }
                            if (i == 7)
                            {
                                if (string.IsNullOrWhiteSpace(line[i]))
                                    errors.Add($"Error on row: {cycle + 1}, column: {i + 1}. Device Family in file must not a blank.");
                                else
                                {
                                    if (deviceFamilyId < 0)
                                    {
                                        errors.Add($"Error on row: {cycle + 1}, column: {i + 1}.  Device Family in file does not match the one selected.");
                                    }
                                }
                            }
                            if (i == 8)
                            {
                                if (string.IsNullOrWhiteSpace(line[i]))
                                    errors.Add($"Error on row: {cycle + 1}, column: {i + 1}. Medical Class in file must not a blank.");
                                else
                                {
                                    if (line[i].ToString().Trim() != deviceClassId)
                                    {
                                        errors.Add($"Error on row: {cycle + 1}, column: {i + 1}.  Medical Class in file does not match the one selected.");
                                    }
                                }
                            }

                            if (i == 9)
                            {
                                try
                                {
                                    Int64 line9 = !string.IsNullOrWhiteSpace(line[i]) ? Convert.ToInt64(line[i]) : 0;
                                    if (line9 > 0)
                                    {
                                        if (line9.ToString().Length < 8)
                                        {
                                            errors.Add($"Error on row: {cycle + 1}, column: {i + 1}. Length of GTIN Code must not less than 8. (Ranged 8 - 14 digits)");
                                        }
                                        else if (line9.ToString().Length > 14)
                                        {
                                            errors.Add($"Error on row: {cycle + 1}, column: {i + 1}. Length of GTIN Code must not greater than 14. (Ranged 8-14 digits)");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    errors.Add($"Error on row: {cycle + 1}, column: {i + 1}. {ex.Message}");
                                }

                            }

                            dr[i] = line[i];

                            if (errors.Count > 4)
                                return dt;

                        }
                        dt.Rows.Add(dr);
                    }

                    cycle += 1;
                }

                numRows = cycle - 1;
            }

            return dt;
        }

        public static DataTable CSVtoDataTableParser(IFormFile file)
        {
            DataTable dt = new DataTable();

            using (var stream = file.OpenReadStream())
            using (TextFieldParser parser = new TextFieldParser(stream))
            {
                parser.Delimiters = new[] { "," };
                parser.HasFieldsEnclosedInQuotes = true;

                int cycle = 0;
                int columnCnt = 0;

                while (!parser.EndOfData)
                {
                    string[] line = parser.ReadFields();

                    if (cycle == 0)
                    {
                        foreach (string h in line)
                        {
                            if (!string.IsNullOrWhiteSpace(h))
                            {
                                dt.Columns.Add(h);
                                columnCnt += 1;
                            }
                        }
                    }
                    else
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < columnCnt - 1; i++)
                        {
                            if (i == 1)
                            {
                                string col2row = line[i];
                                Regex rgx = new Regex("[^a-zA-Z0-9 -]");
                                col2row = rgx.Replace(col2row, "");

                                dr[i] = col2row.ToString().Trim();
                            }
                            else
                            {
                                dr[i] = line[i];
                            }
                        }
                        dt.Rows.Add(dr);
                    }

                    cycle += 1;
                }
            }

            return dt;
        }



        public static List<T> DataTableToModel<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    string modelColumn = pro.Name;
                    if (modelColumn.ToLower() == "uid")
                        modelColumn = "udi";

                    if (modelColumn.ToLower() == "bodystructuregroupid")
                        modelColumn = "specialty";

                    if (modelColumn.ToLower() == "deviceclassid")
                        modelColumn = "class";

                    if (modelColumn.ToLower() == "devicefamilyid")
                        modelColumn = "devicefamily";

                    if (modelColumn.ToLower().Trim() == column.ColumnName.ToLower().Trim())
                    {
                        var typeGEt = pro.PropertyType;
                        Type type = Nullable.GetUnderlyingType(pro.PropertyType) ?? pro.PropertyType;
                        string typeName = type.Name;

                        try
                        {
                            if (typeName == "Int16" || typeName == "Int32")
                            {
                                pro.SetValue(obj, Convert.ToInt32(dr[column.ColumnName]), null);
                            }
                            else if (typeName == "Int64")
                            {
                                if (dr[column.ColumnName] != null)
                                {
                                    if (dr[column.ColumnName].ToString().Length > 0)
                                    {
                                        pro.SetValue(obj, Convert.ToInt64(dr[column.ColumnName]), null);
                                    }
                                }

                            }
                            else if (typeName == "DateTime")
                            {
                                pro.SetValue(obj, DateTime.ParseExact((string)dr[column.ColumnName], "mm/dd/yyyy", CultureInfo.InvariantCulture), null);
                            }
                            else if (typeName == "Guid")
                            {
                                pro.SetValue(obj, Guid.Parse((string)dr[column.ColumnName]));
                            }
                            else
                            {
                                pro.SetValue(obj, dr[column.ColumnName], null);
                            }
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                    else
                        continue;
                }
            }
            return obj;
        }

        public static IEnumerable<T> ToList<T>(this DataTable? dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public static string GetExtension(this string name)
        {
            return Path.GetExtension(name);
        }
    }
}
