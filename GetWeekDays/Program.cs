using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetWeekDays
{
    class Program
    {
        static void Main(string[] args)
        {
			string windowName = "Лист в ALVXXL01 (1) - Excel";
			string excelStr = " - Excel";
			int res1 = windowName.IndexOf(excelStr, 1);
			string result = windowName.Substring(0, res1);
            Console.WriteLine("->{0}<-", result);

			DateTime lastSunday = new DateTime();
			DateTime lastMonday = new DateTime();
			DateTime startDate = DateTime.Now;

			if (startDate.DayOfWeek == DayOfWeek.Sunday)
				startDate = startDate.AddDays(-1);

			bool isFound = false;
			while (!isFound)
			{
				if (startDate.DayOfWeek == DayOfWeek.Sunday)
				{
					isFound = true;
					lastSunday = startDate.Date;
				}
				else
				{
					startDate = startDate.AddDays(-1);
				}
			}

			startDate = lastSunday;
			isFound = false;
			while (!isFound)
			{
				if (startDate.DayOfWeek == DayOfWeek.Monday)
				{
					isFound = true;
					lastMonday = startDate.Date;
				}
				else
				{
					startDate = startDate.AddDays(-1);
				}
			}
		}
	}
}
