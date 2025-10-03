namespace WebApplication1.Models {
	public enum Quarter
	{
		Winter,
		Spring,
		Summer,
		Fall
	}

	public static class QuarterHelper
	{
		public class QuarterDate 
		{
			public required string Name { get; set; }
			public DateTime StartDate { get; set; }
			public DateTime EndDate { get; set; }
		}
		public static QuarterDate[] QuarterDates()
		{
			return new QuarterDate[] {
				new QuarterDate { Name = "Winter", StartDate = new DateTime(2025, 1, 6), EndDate = new DateTime(2025, 3, 21) },
				new QuarterDate { Name = "Spring", StartDate = new DateTime(2025, 3, 31), EndDate = new DateTime(2025, 6, 13) },
				new QuarterDate { Name = "Summer", StartDate = new DateTime(2025, 6, 23), EndDate = new DateTime(2025, 9, 5) },
				new QuarterDate { Name = "Fall", StartDate = new DateTime(2025, 9, 15), EndDate = new DateTime(2025, 11, 28) }
			};
		}
		public static Quarter GetQuarterFromDate(DateTime startDate)
		{
			foreach (var qd in QuarterDates())
			{
				if (startDate >= qd.StartDate && startDate <= qd.EndDate)
				{
					return Enum.Parse<Quarter>(qd.Name);
				}
			}
			// If not found, default to Fall or throw an exception as needed
			return Quarter.Fall;
		}

		public static Quarter GetNextQuarter(DateTime today)
		{
			var quarters = QuarterDates();
			var currentQuarter = GetQuarterFromDate(today);

			// Find the index of the current quarter
			int currentIndex = Array.FindIndex(quarters, qd => Enum.Parse<Quarter>(qd.Name) == currentQuarter);

			// Get the next quarter index (wrap around if at the end)
			int nextIndex = (currentIndex + 1) % quarters.Length;

			return Enum.Parse<Quarter>(quarters[nextIndex].Name);
		}
	}
}
