namespace YouBikeAPI.Parameters
{
    public class PageParameters
    {
        private int pageNum = 1;

        private int pageSize = 20;

        private const int maxPageSize = 30;

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
                pageSize = ((value > maxPageSize) ? maxPageSize : value);
            }
        }
    }
}
