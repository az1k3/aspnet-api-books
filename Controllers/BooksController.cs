using BooksAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        BooksContext db;
        public BooksController(BooksContext context)
        {
            db = context;

            //Первичное наполнение данными
            if (!db.Books.Any())
            {
                db.Books.AddRange(new List<Book>() 
                {
                    new Book()
                    {
                        Title = "Мартин Иден",
                        Author = "Джек Лондон",
                        Category = "Роман",
                        Price = 3850
                    },
                    new Book()
                    {
                        Title = "Богатый папа, бедный папа",
                        Author = "Роберт Кийосаки",
                        Category = "Бизнес",
                        Price = 5099.50
                    },
                });
                db.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> Get()
        {
            return await db.Books.ToListAsync();
        }

        // GET api/books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> Get(int id)
        {
            Book book = await db.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (book == null)
                return NotFound();
            return new ObjectResult(book);
        }


        // POST api/users
        [HttpPost]
        public async Task<ActionResult<Book>> Post(Book book)
        {
            if (book == null)
            {
                return BadRequest();
            }

            db.Books.Add(book);
            await db.SaveChangesAsync();
            return Ok(book);
        }


        // PUT api/books/
        [HttpPut]
        public async Task<ActionResult<Book>> Put(Book book)
        {
            if (book == null)
            {
                return BadRequest();
            }
            if (!db.Books.Any(x => x.Id == book.Id))
            {
                return NotFound();
            }

            db.Update(book);
            await db.SaveChangesAsync();
            return Ok(book);
        }


        // DELETE api/books/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> Delete(int id)
        {
            Book book = db.Books.FirstOrDefault(x => x.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            db.Books.Remove(book);
            await db.SaveChangesAsync();
            return Ok(book);
        }


    }
}
