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
