using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

//TODO: Update these using statements to match the name of your project
using Tam_Allyson_HW3.DAL;
using Tam_Allyson_HW3.Seeding;

//TODO: Update this namespace to match your project
namespace Tam_Allyson_HW3.Controllers
{
    public class SeedController : Controller
    {

        public IActionResult Confirm()
        {
            return View();
        }

        //You will need an instance of the AppDbContext class for this code to work
        //Create a private variable to hold the AppDbContext object
        private AppDbContext _context;

        //Create a constructor for this class that accepts an instance of AppDbContext
        //The code in Startup.cs configures the project to provide an instance of AppDbContext
        //when Controller classes are instantiated.
        public SeedController(AppDbContext context)
        {
            //Set the private variable equal to the instance that was
            //passed into the constructor
            _context = context;
        }

        //This is the action method for the Seeding page with the two buttons
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult SeedAllGenres()
        {
            try
            {
                //call the SeedAllGenres method from your Seeding folder
                //you will need to pass in the instance of AppDbContext
                //that you set in the constructor
                SeedGenres.SeedAllGenres(_context);
            }
            catch (Exception ex)
            {
                //add the error messages to a list of strings
                List<String> errorList = new List<String>();

                errorList.Add("There was a problem adding this genre.");

                //Add the outer message
                errorList.Add(ex.Message);

                //Add the message from the inner exception, if there is one
                if (ex.InnerException != null)
                {
                    errorList.Add(ex.InnerException.Message);

                    //Add additional inner exception messages, if there are any
                    if (ex.InnerException.InnerException != null)
                    {
                        errorList.Add(ex.InnerException.InnerException.Message);
                    }
                }
                

                //return the user to the error view
                return View("Error", errorList);
            }

            //everything is okay - send user to confirmation page
            return View("Confirm");
        }

        public IActionResult SeedAllBooks()
        {
            try
            {
                // Ensure _context is not null before proceeding
                if (_context == null)
                {
                    return View("Error", new List<string> { "Database context is null." });
                }

                // Call the SeedBooks method
                SeedBooks.SeedAllBooks(_context);
            }
            catch (Exception ex)
            {
                List<string> errorList = new List<string> { "There was a problem adding this book.", ex.Message };

                // Check if InnerException exists before accessing it
                if (ex.InnerException != null)
                {
                    errorList.Add(ex.InnerException.Message);

                    // Check if InnerException.InnerException exists before accessing it
                    if (ex.InnerException.InnerException != null)
                    {
                        errorList.Add(ex.InnerException.InnerException.Message);
                    }
                }

                return View("Error", errorList);
            }

            return View("Confirm");
        }

    }
}