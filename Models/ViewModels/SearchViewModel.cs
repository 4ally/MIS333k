using System;
using System.ComponentModel.DataAnnotations;

namespace Tam_Allyson_HW3.Models
{
    public enum PriceSearchType
    {
        greaterThan, lessThan
    }

    public class SearchViewModel
	{
        [Display(Name = "Search by Title")]
        public string ?Name { get; set; }

        [Display(Name = "Search by Description")]
        public string ?Description { get; set; }

        [Display(Name = "Search by Format")]
        public string ?Format { get; set; }

        [Display(Name = "Search by Genre")]
        public int? GenreID { get; set; }

        [Display(Name = "Search by Price")]
        public decimal? Price { get; set; }

        [Display(Name = "Price Filter")]
        public PriceSearchType? PriceComparison { get; set; }

        [Display(Name = "Search by Published Date")]
        [DataType(DataType.Date)]
        public DateTime? ReleasedAfter { get; set; }
    }
}

