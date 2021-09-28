using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateWord
{
    class Program
    {
        static void Main(string[] args)
        {
            DataTable collection1 = new DataTable("ZM_CONTR_CHANGELOG");
            collection1.Columns.Add("Номер Договора", Type.GetType("System.String"));
            collection1.Columns.Add("Позиция Контракта", Type.GetType("System.String"));
            collection1.Columns.Add("Время", Type.GetType("System.String"));

            for (int i = 0; i < 3; i++)
            {
                DataRow newRow = collection1.NewRow();
                newRow["Номер Договора"] = "1000" + i;
                newRow["Позиция Контракта"] = "Позиция Контракта " + i;
                newRow["Время"] = "Краткое описание";
                collection1.Rows.Add(newRow);
            }

            string pathToShablon = @"d:\Time\One.docx";
            string resultFileName = @"d:\Time\Two.docx";

            var wordApp = new Microsoft.Office.Interop.Word.Application();
            var wordDoc = wordApp.Documents.Open(pathToShablon);

            for (int i = 0; i < collection1.Rows.Count; i++)
            {
                for (int j = 0; j < collection1.Columns.Count; j++)
                {
                    DataRow dataRow = collection1.Rows[i];
                    DataColumn dataColumn = collection1.Columns[j];
                    //Console.WriteLine(wordDoc.Tables[1].Rows[i+1].Cells[j+1].Range.Text);
                    wordDoc.Tables[1].Rows[i+1].Cells[j+1].Range.Text = (string)dataRow[dataColumn];
                    //wordDoc.Tables[1].Rows[3].Cells[1].Range.Text = string.Format("ЗАПРОС № {0}", DateTime.Now.ToString("dd-MM") + "_" + num);
                    //wordDoc.Tables[1].Rows[i].Cells[j].Range.Text = (string)dataRow[dataColumn];
                    //wordDoc.Tables[0].Rows[i].Cells[j].Range.Text = dataRow[dataColumn];
                    //Console.Write(dataRow[dataColumn] + " ");
                }
                wordDoc.Tables[1].Rows.Add();
                Console.WriteLine();
            }

            
            //ShowTable(collection1);

            wordDoc.SaveAs2(resultFileName);

            ((Microsoft.Office.Interop.Word._Application)wordApp).Quit(Type.Missing, Type.Missing, Type.Missing);

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

    public class Transaction
    {
        public string TstName { get; set; }
        public string DateTime { get; set; }
    }
}
