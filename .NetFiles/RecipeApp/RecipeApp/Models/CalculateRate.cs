namespace RecipeApp.Models
{
	public class CalculateRate
	{
		public static double GetRating(List<int> rates)
		{
			if (rates == null || rates.Count == 0)
				return 0;

			int star1 = rates.Count(x => x == 1);
			int star2 = rates.Count(x => x == 2);
			int star3 = rates.Count(x => x == 3);
			int star4 = rates.Count(x => x == 4);
			int star5 = rates.Count(y => y == 5);

			double rating = (double)(5 * star5 + 4 * star4 + 3 * star3 + 2 * star2 + star1) / (star1 + star2 + star3 + star4 + star5);
			return rating;
		}
	}
}
