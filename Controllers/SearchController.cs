using System;
using AppBackEnd.Data;
using AppBackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppBackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        public AppDbContext Context { get; private set; }

        public SearchController(AppDbContext context)
        {
            Context = context;
        }

        [HttpGet("Books")]
        public IEnumerable<Book> SearchBooks(string query, int curPage, int pageSize)
        {
            // you can throw whatever book property you want
            // at it and it will spit out a list of books
            var books = Context.Books
                .Where(p =>
                    p.Title.Contains(query) ||
                    p.Author.Contains(query) ||
                    p.ISBN.Contains(query) ||
                    p.Publisher.Contains(query))
                .ToList().Skip(pageSize * curPage)
                .Take(pageSize);
            return books;
        }
        [HttpGet("Book/isbn")]
        public IEnumerable<Book> SearchBookISBN(string query, int curPage, int pageSize)
        {
            // searching by isbn when adding a book
            var books = Context.Books
                .Where(p => p.ISBN.Contains(query))
                .ToList().Skip(pageSize * curPage)
                .Take(pageSize);
            return books;
        }
        [HttpGet("Book/title")]
        public IEnumerable<Book> SearchBookTitle(string query, int curPage, int pageSize)
        {
            // searching by isbn when adding a book
            var books = Context.Books
                .Where(p => p.Title.Contains(query))
                .ToList().Skip(pageSize * curPage)
                .Take(pageSize);
            return books;
        }
        [HttpGet("Offers")]
        public IEnumerable<OfferDtoAll> SearchOffers(string query, int curPage, int pageSize)
        {
            // search in offer description or user
            // and it will spit out a list of offers
            var offers = Context.Offers
                .Where(p =>
                    p.Description.Contains(query) ||
                    p.BiblioUser.UserName.Contains(query))
                .Select(x => new OfferDtoAll{
                Title = x.Book.Title,
                UserName = x.BiblioUser.UserName,
                Phone = x.BiblioUser.PhoneNumber,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                Price = x.Price})
                .ToList().Skip(pageSize * curPage)
                .Take(pageSize);;
            return offers;
        }
        [HttpGet("Offer/title")]
        public IEnumerable<OfferDtoAll> SearchOfferTitle(string query, int curPage, int pageSize)
        {
            // searching by isbn when adding a book
            var offers = Context.Offers
                .Where(p => p.Book.Title.Contains(query))
                .Select(x => new OfferDtoAll{
                Title = x.Book.Title,
                UserName = x.BiblioUser.UserName,
                Phone = x.BiblioUser.PhoneNumber,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                Price = x.Price})
                .ToList().Skip(pageSize * curPage)
                .Take(pageSize);;
            return offers;
        }
        [HttpGet("Offer/bid")]
        public IEnumerable<OfferDtoAll> SearchOfferBookId(int query, int curPage, int pageSize)
        {
            // searching by isbn when adding a book
            var offers = Context.Offers
                .Where(p => p.Book.Id == query)
                .Select(x => new OfferDtoAll{
                Title = x.Book.Title,
                UserName = x.BiblioUser.UserName,
                Phone = x.BiblioUser.PhoneNumber,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                Price = x.Price})
                .ToList().Skip(pageSize * curPage)
                .Take(pageSize);;
            return offers;
        }
    }
}