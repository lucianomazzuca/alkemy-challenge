using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlkemyChallenge.Data;
using AlkemyChallenge.Models;
using AlkemyChallenge.Repositories;
using AlkemyChallenge.Services;
using AlkemyChallenge.Exceptions;

namespace AlkemyChallenge.Controllers
{
    [Route("api/characters")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly CharacterRepository _characterRepository;
        private readonly FileService _fileService;

        public CharacterController(CharacterRepository characterRepository, FileService fileService)
        {
            _characterRepository = characterRepository;
            _fileService = fileService;
        }

        // GET: api/Character
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Character>>> GetCharacters()
        {
            var characters = await _characterRepository.GetAll();
            return characters.ToList();
        }

        // GET: api/Character/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Character>> GetCharacter(int id)
        {
            var character = await _characterRepository.GetById(id);

            if (character == null)
            {
                return NotFound();
            }

            return character;
        }

        // PUT: api/Character/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCharacter(int id, [FromForm] Character character, [FromForm] IFormFile image)
        {
            if (image != null)
            {
                character.Image = await _fileService.SaveImage(image);
            }

            character.Id = id;
            await _characterRepository.Update(character);
            return NoContent();
        }

        // POST: api/Character
        [HttpPost]
        public async Task<ActionResult> PostCharacter([FromForm] Character character, [FromForm] IFormFile image)
        {
            if (image != null)
            {
                character.Image = await _fileService.SaveImage(image);
            }
            await _characterRepository.Add(character);

            return CreatedAtAction("GetCharacter", new { id = character.Id }, character);
        }

        // DELETE: api/Character/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacter(int id)
        {
            try
            {
                await _characterRepository.Delete(id);

            }
            catch (RecordNotFoundException)
            {
                return NotFound();
            }

            return Ok();
        }

    }
}
