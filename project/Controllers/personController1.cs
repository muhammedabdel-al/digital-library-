using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.Database;
using project.Model_View;
using project.Models;

namespace project.Controllers
{
    public class personController1 : Controller
    {
        private readonly AppDBContext _appContext;
        public personController1(AppDBContext appContext)
        {
            _appContext = appContext;
        }

        // GET: personController1
        public ActionResult Index()
        {
            var person = _appContext.Persons.ToList();
            return View(person);
        }

        // GET: personController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: personController1/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: personController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormFile file, [Bind("Name,Discription,Email,Password,PhoneNumber,Age,Role")] Person person)
        {
            try
            {
                if (file != null)
                {
                    string filename = file.FileName;
                    string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images"));
                    using (var filestream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(filestream);
                    }

                    person.imgfile = filename;
                    _appContext.Persons.Add(person);
                    await _appContext.SaveChangesAsync();  // Ensure async method for database operations

                    // Redirect to Login after successful creation
                    return RedirectToAction(nameof(Login)); // Make sure the Login view exists
                }
                else
                {
                    ModelState.AddModelError("", "File upload failed.");
                }
            }
            catch (Exception ex)
            {
                // Log the error for debugging
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
            }

            return View(person);  // If there's an error, return the person model to the view for corrections
        }


        // GET: personController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: personController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // GET: personController1/Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // Verify the user's credentials
                var user = _appContext.Persons.FirstOrDefault(p => p.Email == model.Email && p.Password == model.Password);

                if (user != null)
                {
                    string ID = Convert.ToString(user.ID);
                    HttpContext.Session.SetString("Name", user.Name);
                    HttpContext.Session.SetString("ID", ID);
                    HttpContext.Session.SetString("Role", user.Role);
                    // Check the user's role
                    if (user.Role == "Admin")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (user.Role == "Customer")
                    {
                        return RedirectToAction("catalogue", "BookController1");
                    }
                    else if (user.Role == "Author")
                    {
                        return RedirectToAction("Index", "BookController1");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unknown role");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password");
                }
            }

            return View(model);
        }


        // GET: personController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: personController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
