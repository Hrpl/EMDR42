using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Domain.Commons.Filters;

public class BaseSorting
{
    /// <summary>
    /// Множественная сортировка.
    /// В данном запросе работает поверх основной (SortColumn и SortDirection).
    /// Паттерн для данной строки - +field,-field,
    /// где "+" означает asc, а "-" desc.
    /// </summary>
    [RegularExpression(@"^(\-|\+)[a-zA-Z]+[a-zA-Z0-9]*$")]
    public string? Sorting { get; set; }

    protected BaseSorting(string? sorting)
    {
        Sorting = sorting;
    }

    public BaseSorting() { }
}
