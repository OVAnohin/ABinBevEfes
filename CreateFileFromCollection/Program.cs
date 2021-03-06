using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CreateFileFromCollection
{
    class Program
    {
        static void Main(string[] args)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Name", Type.GetType("System.String"));
            dataTable.Columns.Add("ClassName", Type.GetType("System.String"));
            dataTable.Columns.Add("NativeWindowHandle", Type.GetType("System.Int32"));

            for (int i = 0; i < 10; i++)
            {
                DataRow newRow = dataTable.NewRow();
                newRow["Name"] = "Name " + i;
                newRow["ClassName"] = "ClassName " + i;
                newRow["NativeWindowHandle"] = i;
                dataTable.Rows.Add(newRow);
            }
            ShowTable(dataTable);
            DataTable newTable = dataTable.Clone();

            newTable.Columns.Add("2Place", Type.GetType("System.String")).SetOrdinal(1);
            for (int i = 0; i < dataTable.Rows.Count; i++)
                newTable.ImportRow(dataTable.Rows[i]);

            ShowTable(newTable);
            Console.ReadKey();
            //*********************************************

            string fileName = "Text.txt";
            string localFolder = @"d:\Time";
            string fullFileName = localFolder + @"\" + fileName;
            string columnName = "NativeWindowHandle";

            var file = new FileInfo(fullFileName);
            StreamWriter writer = file.CreateText();
            if (dataTable.Rows.Count == 0)
                writer.WriteLine("000000");

            for (int i = 0; i < dataTable.Rows.Count; i++)
                writer.WriteLine(dataTable.Rows[i][columnName].ToString());


            writer.Close();
            Thread.Sleep(1000);
            //*********************************************
            Console.ReadKey();
        }

        private static void ShowTable(DataTable dataFromExcel)
        {
            for (int i = 0; i < dataFromExcel.Rows.Count; i++)
            {
                for (int j = 0; j < dataFromExcel.Columns.Count; j++)
                {
                    DataRow dataRow = dataFromExcel.Rows[i];
                    DataColumn dataColumn = dataFromExcel.Columns[j];
                    Console.Write(dataRow[dataColumn] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine(new string('*', 20));
        }
    }
}
