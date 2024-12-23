using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using project.Database;
using project.Models;

namespace project.Controllers
{
    public class UserBooksController : Controller
    {
        private readonly AppDBContext _context;

        public UserBooksController(AppDBContext context)
        {
            _context = context;
        }

        // GET: UserBooks
        public async Task<IActionResult> Index()
        {
            var appDBContext = _context.UserBooks.Include(u => u.Customer);
            return View(await appDBContext.ToListAsync());
        }

        // GET: UserBooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBook = await _context.UserBooks
                .Include(u => u.Customer)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (userBook == null)
            {
                return NotFound();
            }

            return View(userBook);
        }

        // GET: UserBooks/Create
        public async Task<IActionResult> Create(int? id)
        {
            var book = await _context.Books.FindAsync(id);
            //ViewData["CustomerId"] = new SelectList(_context.Customers, "ID", "Email");
            return View(book);
        }

        // POST: UserBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int bookId, int Quantity)
        {
            int Id = Convert.ToInt32(HttpContext.Session.GetString("ID"));

            var book = await _context.Books.FirstOrDefaultAsync(b => b.BookId == bookId);

            if (Quantity > book.Count)
            {
                ModelState.AddModelError("Quantity", "The requested quantity exceeds the available stock.");
                return View(book); 
            }

            UserBook userbook = new UserBook
            {
                CustomerId = Id,
                BookId = bookId,
                Quantity = Quantity,
                PurchaseDate = DateTime.Today
            };

            _context.UserBooks.Add(userbook);

            book.Count -= Quantity;

            _context.Books.Update(book);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Myorders));
        }


        public async Task<IActionResult> Myorders()
        {
            var customerIdString = HttpContext.Session.GetString("ID");
            if (string.IsNullOrEmpty(customerIdString))
            {
                return RedirectToAction("Login");
            }

            int customerId = Convert.ToInt32(customerIdString);
            var orders = await _context.UserBooks
                .Include(ub => ub.Book) 
                .Where(ub => ub.CustomerId == customerId)
                .ToListAsync();

            return View(orders); 
        }

        // GET: UserBooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBook = await _context.UserBooks.FindAsync(id);
            if (userBook == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "ID", "Email", userBook.CustomerId);
            return View(userBook);
        }

        // POST: UserBooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,BookId,PurchaseDate")] UserBook userBook)
        {
            if (id != userBook.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserBookExists(userBook.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "ID", "Email", userBook.CustomerId);
            return View(userBook);
        }

        // GET: UserBooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBook = await _context.UserBooks
                .Include(u => u.Customer)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (userBook == null)
            {
                return NotFound();
            }

            return View(userBook);
        }

        // POST: UserBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id,int bookid)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.BookId == bookid);

            var userBook = await _context.UserBooks
                .FirstOrDefaultAsync(ub => ub.CustomerId == id && ub.BookId == bookid);
            book.Count += userBook.Quantity;

            _context.Books.Update(book);
            if (userBook != null)
            {
                _context.UserBooks.Remove(userBook);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Myorders));
        }

        private bool UserBookExists(int id)
        {
            return _context.UserBooks.Any(e => e.CustomerId == id);
        }
    }
}
