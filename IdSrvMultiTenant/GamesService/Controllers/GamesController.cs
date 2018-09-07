using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using GamesService.Models;
using GamesService.Models.Dtos;
using GamesService.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GamesController : ControllerBase
    {
        private readonly GamesDbContext _context;

        //TODO: Move Database operations in manager or repository
        public GamesController(GamesDbContext context)
        {
            _context = context;
        }
        
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Game>> Get()
        {
            return _context.Games.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Game> Get([FromRoute]Guid id)
        {
            var game = _context.Games.FirstOrDefault(g => g.Id == id);
            if (game == null)
                return NotFound();
            
            return game;
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] GameDto value)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var game = new Game
                {
                    Console = value.Console,
                    Name = value.Name,
                    Type = value.Type
                };
                _context.Add(game);
                _context.SaveChanges();
                return Ok(game.Id.ToString());
            }
            catch (DbException e)
            {
                return BadRequest();
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] GameDto value)
        {
            var game = _context.Games.FirstOrDefault(g => g.Id == id);
            if (game == null)
                return NotFound();

            try
            {
                game.Name = value.Name;
                game.Console = value.Console;
                game.Type = value.Type;
                _context.SaveChanges();
                return Ok();
            }
            catch (DbException e)
            {
                return BadRequest();
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var game = _context.Games.FirstOrDefault(g => g.Id == id);
            if (game == null)
                return NotFound();

            try
            {
                _context.Games.Remove(game);
                _context.SaveChanges();
                return Ok();
            }
            catch (DbException)
            {
                return BadRequest();
            }
        }
    }
}