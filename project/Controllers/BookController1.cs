using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.Database;
using project.Models;
using System;

namespace project.Controllers
{
    public class BookController1 : Controller
    {
        // GET: BookController1
        private readonly AppDBContext _appContext;
        public BookController1(AppDBContext appContext)
        {
            _appContext = appContext;
        }
        public ActionResult Index()
        {
            int Id = Convert.ToInt32(HttpContext.Session.GetString("ID"));
            var books = _appContext.Books.Where(e => e.AuthorId == Id).ToList();
            return View(books);
        }

        // GET: BookController1/Details/5
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var book = _appContext.Books.FirstOrDefault(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // GET: BookController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BookController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile file, [Bind("BookId,Title,Category,Count,Price")] Book book)
        {
            book.AuthorId = Convert.ToInt32(HttpContext.Session.GetString("ID"));
            try
            {

                if (file != null)
                {
                    string filename = file.FileName;
                    //  string  ext = Path.GetExtension(file.FileName);
                    string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images"));
                    using (var filestream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(filestream);
                    }

                    book.imgfile = filename;
                }
                _appContext.Books.Add(book);
                _appContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        public async Task<IActionResult> catalogue()
        {
            int customerId = Convert.ToInt32(HttpContext.Session.GetString("ID"));
            var x = await _appContext.Books.ToListAsync();
            var availableBooks = _appContext.Books
            .Where(book => !_appContext.UserBooks
                .Any(userBook => userBook.CustomerId == customerId && userBook.BookId == book.BookId))
            .ToList();
            return View(availableBooks);
        }
        public async Task<IActionResult> ViewAuthorBooks()
        {
            int authorId = Convert.ToInt32(HttpContext.Session.GetString("ID"));
            var books = await _appContext.Books.Where(e => e.AuthorId == authorId).ToListAsync();
            if (books == null || !books.Any())
            {
                // Log the issue or display an appropriate message
                return View("Create"); // Create a view for this if needed
            }
            return View(books.ToList());
        }


        // POST: BookController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,IFormFile file, [Bind("BookId,Title,Category,Count,Price")] Book book)
        {
            book.AuthorId = Convert.ToInt32(HttpContext.Session.GetString("ID"));
            try
            {
                var book1 = _appContext.Books.FirstOrDefault(b => b.BookId == id);  

                if (book1 != null)
                {
                    _appContext.Books.Remove(book1);  
                    _appContext.SaveChanges();       
                }
                if (file != null)
                {
                    string filename = file.FileName;
                    //  string  ext = Path.GetExtension(file.FileName);
                    string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images"));
                    using (var filestream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                    {
                        file.CopyToAsync(filestream);
                    }

                    book.imgfile = filename;
                }
                _appContext.Books.Add(book);
                _appContext.SaveChanges();
                string role = HttpContext.Session.GetString("Role");
                if (role == "Author")
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction(nameof(catalogue));
                }
                
            }
            catch
            {
                return View();
            }
        }

        // GET: BookController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BookController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var book = _appContext.Books.FirstOrDefault(b => b.BookId == id);  // Fetch the book to delete

                if (book != null)
                {
                    _appContext.Books.Remove(book);  // Remove the book from the database
                    _appContext.SaveChanges();       // Save changes
                }

                string role = HttpContext.Session.GetString("Role");
                if (role == "Author")
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction(nameof(catalogue));
                }
            }
            catch
            {
                return View();
            }
        }
    }
}