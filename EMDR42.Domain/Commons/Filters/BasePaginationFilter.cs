
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Commons.Filters;

public class BasePaginationFilter : BaseSorting
{
    public BasePaginationFilter()
    {
    }

    /// <inheritdoc />
    public BasePaginationFilter(int pageSize, int pageNumber)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
    }

    public BasePaginationFilter(int pageSize, int pageNumber, BaseSorting? sorting) : base(sorting?.Sorting)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
    }

    /// <summary>
    /// Размер страницы.
    /// </summary>
    [DefaultValue(10)]
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Номер страницы.
    /// </summary>
    [DefaultValue(1)]
    public int PageNumber { get; set; } = 1;

    public int Skip => (PageNumber - 1) * PageSize;
}
