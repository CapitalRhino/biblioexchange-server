using System;
using AppBackEnd.Data;
using AppBackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AppBackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        public AppDbContext Context { get; private set; }

        public BookController(AppDbContext context)
        {
            Context = context;
        }

        [HttpGet("all")]
        public IEnumerable<Book> All()
        {
            return Context.Books.ToList();
        }

        [HttpGet()]
        public Book Get(int id)
        {
            Book book = Context.Books.FirstOrDefault(x => x.Id == id);
            return book;
        }

        [HttpGet("rnd")]
        public IEnumerable<Book> NRandomBooks(int n)
        {
            Random rnd = new Random();
            List<Book> temp = Context.Books.ToList();
            List<Book> output = new List<Book>();
            for (var i = 1; i <= n; i++)
            {
                int num = rnd.Next(0,temp.Count());
                Book book = temp[num];
                output.Add(book);
                temp.RemoveAt(num);
            }
            return output;
        }

        [HttpPost(),Authorize(Roles = UserRoles.User)]
        public async Task<ActionResult<Book>> Post(Book book)
        {
            try
            {
                Context.Books.Add(book);
                if (Context.SaveChanges() == 1)
                {
                    return Ok("Success");
                }
                else return BadRequest("Save error");
            }
            catch (System.Exception)
            {
                return BadRequest();
            }

        }

        [HttpPut(),Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<Book>> Update(int id, BookDtoAdd request)
        {
            try
            {
                var book = Context.Books.FirstOrDefault(x => x.Id == id);
                book.Title = request.Title;
                book.ISBN = request.ISBN;
                book.Author = request.Author;
                book.Description = request.Description;
                book.Year = request.Year;
                book.Language = request.Language;
                book.Nationality = request.Nationality;
                book.ImageUrl = request.ImageUrl;
                book.Publisher = request.Publisher;
                book.Genre = request.Genre;
                Context.Books.Update(book);
                if (Context.SaveChanges() == 1)
                {
                    return Ok("Success");
                }
                else return BadRequest("Update error");
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete(),Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<Book>> Delete(int id)
        {
            try
            {
                Book book = Context.Books.FirstOrDefault(x => x.Id == id);
                Context.Books.Remove(book);
                if (Context.SaveChanges() == 1)
                {
                    return Ok("Success");
                }
                else return BadRequest("Delete error");
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }
    }
}