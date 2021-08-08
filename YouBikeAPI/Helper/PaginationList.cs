using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace YouBikeAPI.Helper
{
	public class PaginationList<T, Td> : List<Td>
	{
		private List<object> destinations;

		public int CurrPage { get; set; }

		public int PageSize { get; set; }

		public int TotalPages { get; set; }

		public int TotalCount { get; set; }

		public bool HasPrevious => CurrPage > 1;

		public bool HasNext => CurrPage < TotalPages;

		public PaginationList(List<object> destinations)
		{
			this.destinations = destinations;
		}

		public PaginationList(int currPage, int pageSize, int totalCount, List<Td> items)
		{
			CurrPage = currPage;
			PageSize = pageSize;
			TotalCount = totalCount;
			TotalPages = (int)Math.Ceiling((double)TotalCount / (double)PageSize);
			AddRange(items);
		}

		public static async Task<PaginationList<T, Td>> CreateAsync(int currPage, int pageSize, IQueryable<T> result, IMapper mapper)
		{
			int totalCount = await result.CountAsync();
			int skip = (currPage - 1) * pageSize;
			return new PaginationList<T, Td>(currPage, pageSize, totalCount, await result.ProjectTo(mapper.ConfigurationProvider, Array.Empty<Expression<Func<Td, object>>>()).Skip(skip).Take(pageSize)
				.ToListAsync());
		}
	}
}
