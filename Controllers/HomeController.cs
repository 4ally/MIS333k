using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;        
using Tam_Allyson_HW3.DAL;            
using Tam_Allyson_HW3.Models;          
using Microsoft.EntityFrameworkCore;
using Tam_Allyson_HW3.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tam_Allyson_HW3.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        // Constructor
        private AppDbContext _context;
        public HomeController(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public IActionResult Index(string SearchString)
        {
            var query = _context.Books.Include(b => b.Genre).AsQueryable();

            ViewBag.AllBooks = _context.Books.Count();

            if (!string.IsNullOrEmpty(SearchString))
            {
                query = query.Where(b => b.Title.Contains(SearchString) ||
                                         b.Description.Contains(SearchString));
            }

            List<Book> selectedBooks = query.OrderBy(b => b.Title).ToList();

            ViewBag.SelectedBooks = selectedBooks.Count;

            return View(selectedBooks);
        }


        public IActionResult Details(int? id)//id is the id of the book you want to see
        {
            if (id == null) //BookID not specified
            {
                //user did not specify a BookID – take them to the error view
                return View("Error", new String[] { "BookID not specified - which book do you want to view?" });
            }
            //look up the book in the database based on the id; be sure to include the genre
            Book book = _context.Books.Include(b => b.Genre).FirstOrDefault(b => b.BookID == id);
            if (book == null) //No book with this id exists in the database
            {
                //there is not a book with this id in the database – show the user an error view
                return View("Error", new String[] { "Book not found in database" });
            }
            //if code gets this far, all is well – display the details
            return View(book);


        }

        private void PopulateGenresDropDown()
        {
            ViewBag.Genres = _context.Genres
                .Select(g => new SelectListItem
                {
                    Value = g.GenreID.ToString(),
                    Text = g.GenreName
                })
                .ToList();
        }


        public IActionResult DetailedSearch(SearchViewModel searchModel)
        {
            PopulateGenresDropDown();
            return View(new SearchViewModel());

        }

        public IActionResult DisplaySearchResults(SearchViewModel searchModel)
        {
            var query = _context.Books.Include(b => b.Genre).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                query = query.Where(b => b.Title.Contains(searchModel.Name) ||
                                         b.Author.Contains(searchModel.Name));
            }

            if (!string.IsNullOrEmpty(searchModel.Description))
            {
                query = query.Where(b => b.Description.Contains(searchModel.Description));
            }

            if (!string.IsNullOrEmpty(searchModel.Format) &&
                Enum.TryParse(searchModel.Format, out Format parsedFormat))
            {
                query = query.Where(b => b.BookFormat == parsedFormat);
            }

            if (searchModel.GenreID.HasValue)
            {
                query = query.Where(b => b.GenreID == searchModel.GenreID);
            }

            if (searchModel.Price.HasValue)
            {
                if (searchModel.PriceComparison == PriceSearchType.greaterThan)
                {
                    query = query.Where(b => b.Price >= searchModel.Price);
                }
                else if (searchModel.PriceComparison == PriceSearchType.lessThan)
                {
                    query = query.Where(b => b.Price <= searchModel.Price);
                }
            }


            if (searchModel.ReleasedAfter.HasValue)
            {
                query = query.Where(b => b.PublishedDate >= searchModel.ReleasedAfter);
            }

            List<Book> filteredBooks = query.OrderBy(b => b.Title).ToList();

            ViewBag.AllBooks = _context.Books.Count();
            ViewBag.SelectedBooks = filteredBooks.Count();

            return View("Index", filteredBooks);
        }


    }
}