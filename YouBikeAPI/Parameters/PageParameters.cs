namespace YouBikeAPI.Parameters
{
	public class PageParameters
	{
		private int pageNum = 1;

		private int pageSize = 10;

		private const int maxPageSize = 25;

		public int PageNum
		{
			get
			{
				return pageNum;
			}
			set
			{
				if (value >= 1)
				{
					pageNum = value;
				}
			}
		}

		public int PageSize
		{
			get
			{
				return pageSize;
			}
			set
			{
				pageSize = ((value > 25) ? 25 : value);
			}
		}
	}
}
