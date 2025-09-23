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
		public static Quarter? GetQuarterFromMonth(int month)
		{
			return month switch
			{
				>= 1 and <= 3 => Quarter.Winter,
				>= 4 and <= 6 => Quarter.Spring,
				>= 7 and <= 9 => Quarter.Summer,
				_ => Quarter.Fall
			};
		}
	}
}
