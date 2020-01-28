using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Glossary.Models;

namespace Glossary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GlossaryController : ControllerBase
    {

        // this can eventually link to a database and you cache the load of the files
        private static List<GlossaryItem> Glossary = new List<GlossaryItem> {
            new GlossaryItem
            {
                Term= "Access Token",
                Definition = "A credential that can be used by an application to access an API. It informs the API that the bearer of the token has been authorized to access the API and perform specific actions specified by the scope that has been granted."
            },
            new GlossaryItem
            {
                Term= "JWT",
                Definition = "An open, industry standard RFC 7519 method for representing claims securely between two parties. "
            },
            new GlossaryItem
            {
                Term= "OpenID",
                Definition = "An open standard for authentication that allows applications to verify users are who they say they are without needing to collect, store, and therefore become liable for a user’s login information."
            }
        };

        [HttpGet]
        public ActionResult<List<GlossaryItem>> Get() => Ok(Glossary);

        [HttpGet]
        [Route("{term}")]
        public ActionResult<GlossaryItem> Get(string term){

            GlossaryItem glossaryItem = Glossary.Find(item => item.Term.Equals(term, StringComparison.InvariantCultureIgnoreCase));

            if(glossaryItem == null){
                return NotFound();
            } 

            return Ok(glossaryItem);
        }

        // create new variable
        [HttpPost]
        public ActionResult Post(GlossaryItem glossaryItem)
        {
            GlossaryItem existingGlossaryItem = Glossary.Find(item =>
                    item.Term.Equals(glossaryItem.Term, StringComparison.InvariantCultureIgnoreCase));

            if (existingGlossaryItem != null)
            {
                return Conflict("Cannot create the term because it already exists.");
            }
            else
            {
                Glossary.Add(glossaryItem);
                string resourceUrl = Path.Combine(Request.Path.ToString(), Uri.EscapeUriString(glossaryItem.Term));
                return Created(resourceUrl, glossaryItem);
            }
        }

        // Update existing
        [HttpPut]
        public ActionResult Put(GlossaryItem glossaryItem)
        {
            GlossaryItem existingGlossaryItem = Glossary.Find(item =>
            item.Term.Equals(glossaryItem.Term, StringComparison.InvariantCultureIgnoreCase));

            if (existingGlossaryItem == null)
            {
                return BadRequest("Cannot update a nont existing term.");
            } else
            {
                existingGlossaryItem.Definition = glossaryItem.Definition;
                return Ok();
            }
        }

        // Deleting an item
        [HttpDelete]
        [Route("{term}")]
        public ActionResult Delete(string term)
        {
            var glossaryItem = Glossary.Find(item =>
                   item.Term.Equals(term, StringComparison.InvariantCultureIgnoreCase));

            if (glossaryItem == null)
            {
                return NotFound();
            }
            else
            {
                Glossary.Remove(glossaryItem);
                return NoContent();
            }
        }
    }
}