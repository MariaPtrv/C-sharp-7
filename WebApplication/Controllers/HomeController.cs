using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private AuthorsDBContext db;
        public HomeController(AuthorsDBContext authors)
        {
            db = authors;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Автор";
            var authors = db.Authors.Include(a => a.Book).ToList();
            return View(authors);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Compans = new SelectList(db.Books, "Id", "Title");
            Author author = new Author();
            return View(author);
        }

        [HttpPost]
        public IActionResult Create(Author author)
        {
            if (ModelState.IsValid && author.Name != null && author.LastName != null)
            {
                db.Authors.Add(author);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(author);
        }

        [HttpGet]
        public IActionResult Edit_author(int Id)
        {
            var author = db.Authors.FirstOrDefault(p => p.Id == Id);
            ViewBag.Compans = new SelectList(db.Books, "Id", "Title");
            return View(author);
        }

        [HttpPost]
        public IActionResult Edit_author(Author author)
        {
            if (ModelState.IsValid)
            {
                db.Authors.Update(author);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(Author author)
        {
            db.Authors.Attach(author);
            db.Authors.Remove(author);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Show_book(int id)
        {
            var bo = db.Books.FirstOrDefault(b => b.Id == id);
            var au = db.Authors.Where(x => x.BookId == id);
            ViewData["Authors"] = au;
            return View(bo);
        }

        [HttpGet]
        public IActionResult Edit_book(int Id)
        {
            var book = db.Books.FirstOrDefault(p => p.Id == Id);
            ViewBag.Compans = new SelectList(db.Books, "Id", "Title");
            return View(book);
        }

        [HttpPost]
        public IActionResult Edit_book(Book book)
        {
            if (ModelState.IsValid)
            {
                db.Books.Update(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Statistics()
        {
            var authors = db.Authors.ToList();
            var books = db.Books.ToList();
            ViewData["nAuthors"] = authors.Count();
            ViewData["nBooks"] = books.Count();
            ViewData["nObjects"] = authors.Count() + books.Count();
            var grAuthor = authors.GroupBy(b => b.Name).ToList();
            ViewData["grAuthor"] = grAuthor;
            var grBook = authors.GroupBy(b => b.Book).ToList();
            ViewData["grBook"] = grBook;
            return View();
        }
    }
}
