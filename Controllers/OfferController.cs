using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Security.Cryptography;
using System;
using AppBackEnd.Data;
using AppBackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace AppBackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OfferController : ControllerBase
    {
        public AppDbContext Context { get; private set; }

        public OfferController(AppDbContext context)
        {
            Context = context;
        }
        
        [HttpGet("all")]
        public async Task<IEnumerable<OfferDtoAll>> All()
        {
            return Context.Offers.Select(x => new OfferDtoAll{
                Title = x.Book.Title,
                UserName = x.BiblioUser.UserName,
                Phone = x.BiblioUser.PhoneNumber,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                Price = x.Price
            }).ToList();
        }

        [HttpGet()]
        public Offer Get(int id)
        {
            Offer offer = Context.Offers.FirstOrDefault(x => x.Id == id);
            return offer;
        }

        [HttpGet("rnd")]
        public IEnumerable<OfferDtoAll> NRandomOffers(int n)
        {
            Random rnd = new Random();
            List<Offer> temp = Context.Offers.ToList();
            List<OfferDtoAll> output = new List<OfferDtoAll>();
            for (var i = 1; i <= n; i++)
            {
                int num = rnd.Next(0,temp.Count());
                Offer offer = temp[num];
                OfferDtoAll dtoOffer = new OfferDtoAll(){
                    Title = offer.Book.Title,
                    UserName = offer.BiblioUser.UserName,
                    Phone = offer.BiblioUser.PhoneNumber,
                    Description = offer.Description,
                    ImageUrl = offer.ImageUrl,
                    Price = offer.Price
                };
                output.Add(dtoOffer);
                temp.RemoveAt(num);
            }
            return output;
        }

        [HttpPost()]
        public async Task<ActionResult<Offer>> Post(OfferDtoAdd offerDto)
        {
            try
            {
                Book book = Context.Books.FirstOrDefault(x => x.Id == offerDto.BookId);
                BiblioUser user = Context.Users.FirstOrDefault(x => x.UserName == offerDto.UserName);
                Offer offer = new Offer{
                    BookId = book.Id,
                    Book = book,
                    BiblioUserId = user.Id,
                    BiblioUser= user,
                    Description = offerDto.Description,
                    UploadTime = DateTime.Now,
                    Price = offerDto.Price,
                    ImageUrl = offerDto.ImageUrl
                };
                Context.Offers.Add(offer);
                if (Context.SaveChanges() == 1)
                {
                    return Ok("Success");
                }
                else return BadRequest("Save error");
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut()]
        public async Task<ActionResult<Offer>> Update(int id, OfferDtoAdd input)
        {
            try
            {
                // user auth:
                // get access token
                // parse from it the username
                string tokenSplit = Request.Headers["Authorization"].FirstOrDefault().Split('.')[1];
                var json = Encoding.UTF8.GetString(Convert.FromBase64String(tokenSplit));
                var obj = JsonSerializer.Deserialize<AuthToken>(json);

                var offer = Context.Offers.FirstOrDefault(x => x.Id == id);

                if(obj.Username == offer.BiblioUser.UserName){
                    offer.Description = input.Description;
                    offer.ImageUrl = input.ImageUrl;
                    offer.Price = input.Price;
                    Context.Offers.Update(offer); 
                }
                else return Unauthorized("User does not own given offer");

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

        [HttpDelete()]
        public async Task<ActionResult<Offer>> Delete(int id)
        {
            try
            {
                // user auth:
                // get access token
                // parse from it the username
                string tokenSplit = Request.Headers["Authorization"].FirstOrDefault().Split('.')[1];
                var json = Encoding.UTF8.GetString(Convert.FromBase64String(tokenSplit));
                var obj = JsonSerializer.Deserialize<AuthToken>(json);

                var offer = Context.Offers.FirstOrDefault(x => x.Id == id);

                if(obj.Username == offer.BiblioUser.UserName){
                    Context.Offers.Remove(offer);
                }
                else return Unauthorized("User does not own given offer");
                
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