using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace ReadExcelSheetIntoDataTable
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(int hWnd, out int lpdwProcessId);

        public static Process GetExcelProcess(Excel.Application excelApp)
        {
            int id;
            GetWindowThreadProcessId(excelApp.Hwnd, out id);

            return Process.GetProcessById(id);
        }

        [STAThread]
        static void Main(string[] args)
        {
            string fullFileName = @"d:\Work\ContractualCost.xlsb";
            object oMissing = System.Reflection.Missing.Value;

            Excel.Application excelApp = new Excel.Application();
            Process excelAppProcess = GetExcelProcess(excelApp);
            excelApp.DisplayAlerts = false;
            excelApp.FileValidationPivot = Excel.XlFileValidationPivotMode.xlFileValidationPivotRun;
            Excel.Workbook excelWb = excelApp.Workbooks.Open(fullFileName);
            Excel.Worksheet excelWs = excelWb.Worksheets[1] as Excel.Worksheet;
            Excel.Range excelRange = excelWs.UsedRange;

            string sheetName = excelWs.Name;
            int rows = excelRange.Rows.Count;
            int cols = excelRange.Columns.Count;

            excelWb.Close(oMissing, oMissing, oMissing);
            excelApp.Quit();
            excelApp = null;
            excelAppProcess.Kill();

            DataTable dataFromExcel = new DataTable(sheetName);

            using (OleDbConnection oleDbConnection = new OleDbConnection())

            {
                oleDbConnection.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fullFileName + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;'";
                using (OleDbCommand oleDbCommand = new OleDbCommand())
                {
                    oleDbCommand.CommandText = "Select TOP 1 * from [" + sheetName + "$]";
                    oleDbCommand.Connection = oleDbConnection;
                    OleDbDataAdapter adapter = new OleDbDataAdapter(oleDbCommand.CommandText, oleDbCommand.Connection);
                    adapter.Fill(dataFromExcel);
                }

                oleDbConnection.Close();
            }

            ShowTable(dataFromExcel);
            excelApp = new Excel.Application();
            excelAppProcess = GetExcelProcess(excelApp);
            excelApp.DisplayAlerts = false;
            excelApp.FileValidationPivot = Excel.XlFileValidationPivotMode.xlFileValidationPivotRun;
            excelWb = excelApp.Workbooks.Open(fullFileName);
            excelWs = excelWb.Worksheets[1] as Excel.Worksheet;
            excelRange = excelWs.UsedRange;

            DataRow newRow = dataFromExcel.NewRow();
            for (int c = 0; c < dataFromExcel.Columns.Count; c++)
            {
                Console.WriteLine(dataFromExcel.Columns[c].DataType.ToString());
            }


            excelWb.Close(oMissing, oMissing, oMissing);
            excelApp.Quit();
            excelApp = null;
            excelAppProcess.Kill();

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
