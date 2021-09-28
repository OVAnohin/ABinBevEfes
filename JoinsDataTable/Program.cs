using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoinsDataTable
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime timeout = DateTime.Now;

            DataTable collection1 = new DataTable("ZM_CONTR_CHANGELOG");
            collection1.Columns.Add("Номер Договора", Type.GetType("System.String"));
            collection1.Columns.Add("Позиция Контракта", Type.GetType("System.String"));
            collection1.Columns.Add("Время", Type.GetType("System.String"));

            for (int i = 0; i < 10; i++)
            {
                DataRow newRow = collection1.NewRow();
                newRow["Номер Договора"] = "1000" + i;
                newRow["Позиция Контракта"] = "Позиция Контракта " + i;
                newRow["Время"] = "Краткое описание";
                collection1.Rows.Add(newRow);
            }
            ShowTable(collection1);

            DataTable collection2 = new DataTable("EKKO1");
            collection2.Columns.Add("ДокумЗакуп", Type.GetType("System.String"));
            collection2.Columns.Add("БЕ", Type.GetType("System.String"));
            collection2.Columns.Add("Поставщик", Type.GetType("System.String"));

            for (int i = 0; i < 10; i++)
            {
                if (i == 3 || i == 4 || i == 7 || i == 8)
                    continue;

                DataRow newRow = collection2.NewRow();
                newRow["ДокумЗакуп"] = "1000" + i;
                newRow["БЕ"] = "БЕ=" + i;
                newRow["Поставщик"] = "Поставщик" + i;
                collection2.Rows.Add(newRow);
            }
            ShowTable(collection2);

            //var JoinResult = (from p in dataTableZM.AsEnumerable()
            //                  join t in dataTableEKKO1.AsEnumerable()
            //                  on p.Field<string>("Номер Договора") equals t.Field<string>("ДокумЗакуп")
            //                  select new
            //                  {
            //                      EmpId = p.Field<string>("Номер Договора"),
            //                      BE = t.Field<string>("БЕ"),
            //                      Post = t.Field<string>("Поставщик")
            //                  }).ToList();
            //var JoinResult = (from p in dataTableZM.AsEnumerable()
            //                  join t in dataTableEKKO1.AsEnumerable()
            //                  on p.Field<string>("Номер Договора") equals t.Field<string>("ДокумЗакуп") into tempJoin
            //                  from leftJoin in tempJoin.DefaultIfEmpty()
            //                  select new
            //                  {
            //                      //col.DataType
            //                      EmpId = p.Field<string>("Номер Договора"),
            //                      BE = leftJoin == null ? "" : leftJoin.Field<string>("БЕ"),
            //                      Post = leftJoin == null ? "" : leftJoin.Field<string>("Поставщик"),
            //                      Position = p.Field<string>("Позиция Контракта"),
            //                      Descript = p.Field<string>("Краткое описание")
            //                  }).ToList();

            //foreach (var item in JoinResult)
            //{
            //    Console.WriteLine(item.EmpId + " " + item.BE + " " + item.Post + " " + item.Position + " " + item.Descript);
            //}

            //var test = JoinDataTables(transactionInfo, transactionItems,
            //(row1, row2) => row1.Field<int>("TransactionID") == row2.Field<int>("TransactionID"));
            DataTable shablon = new DataTable();
            shablon.Columns.Add("Поставщик", Type.GetType("System.String"));
            shablon.Columns.Add("Номер Договора", Type.GetType("System.String"));
            shablon.Columns.Add("БЕ", Type.GetType("System.String"));
            shablon.Columns.Add("Позиция Контракта", Type.GetType("System.String"));
            shablon.Columns.Add("Время", Type.GetType("System.String"));

            string comparisonField1 = "Номер Договора";
            string comparisonField2 = "ДокумЗакуп";

            // DataTable resultTable = JoinDataTables(dataTableZM, dataTableEKKO1, (row1, row2) => row1.Field<string>("Номер Договора").Equals(row2.Field<string>("ДокумЗакуп")));



            DataTable resultTable = JoinDataTables(collection1, collection2, shablon, comparisonField1, comparisonField2);

            Console.WriteLine(resultTable.Columns[0].ColumnName);
            resultTable.Columns["Поставщик"].ColumnName = "Кредитор";
            resultTable.AcceptChanges();
            Console.WriteLine(resultTable.Columns[0].ColumnName);

            ShowTable(resultTable);

            //DataView dv = new DataView(resultTable);
            string fieldName = "Позиция Контракта";
            string filter = "Позиция Контракта 1";
            //dv.RowStateFilter = DataViewRowState.ModifiedCurrent;
            ////table.DefaultView.RowFilter = string.Format("customer_id={0} and customer_age={1}",id, age);
            //dv.RowFilter = string.Format("[Кредитор] = Поставщик0", fieldName, filter);

            DataTable result = new DataTable();
            result = resultTable.Clone();
            for (int i = 0; i < resultTable.Rows.Count; i++)
            {
                if (resultTable.Rows[i][fieldName].Equals(filter))
                {
                    result.ImportRow(resultTable.Rows[i]);
                }
            }
            ShowTable(result);
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

        private static DataTable JoinDataTables(DataTable t1, DataTable t2, DataTable result, string field1, string field2)
        {
            for (int i = 0; i < t1.Rows.Count; i++)
            {
                DataRow insertRow = result.NewRow();
                for (int j = 0; j < t1.Columns.Count; j++)
                {
                    DataRow dataRow = t1.Rows[i];
                    DataColumn dataColumn = t1.Columns[j];
                    insertRow[dataColumn.ColumnName] = dataRow[dataColumn];
                }
                result.Rows.Add(insertRow);
            }

            for (int i = 0; i < result.Rows.Count; i++)
            {
                for (int ii = 0; ii < t2.Rows.Count; ii++)
                {
                    for (int jj = 0; jj < t2.Columns.Count; jj++)
                    {
                        if (result.Columns.Contains(t2.Columns[jj].ColumnName))
                            if (result.Rows[i][field1].Equals(t2.Rows[ii][field2]))
                                result.Rows[i][t2.Columns[jj].ColumnName] = t2.Rows[ii][jj];
                    }
                }
            }

            return result;
        }
    }
}
