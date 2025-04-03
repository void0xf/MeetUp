using ConversationService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace ConversationService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationsController : ControllerBase
    {
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Conversation>>> Get()
        {
            var conversations = await DB.Find<Conversation>().ExecuteAsync();
            return Ok(conversations);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Conversation>> Get(string id)
        {
            var conversation = await DB.Find<Conversation>().OneAsync(id);
            if (conversation == null)
            {
                return NotFound();
            }
            return Ok(conversation);
        }

        
        [HttpPost]
        public async Task<ActionResult<Conversation>> Post([FromBody] ConversationDTO conversation)
        {
            var newConversation = new Conversation();
            newConversation.IsDM = conversation.IsDM;
            newConversation.Messages = conversation.Messages;
            newConversation.Participants = conversation.Participants;

            await newConversation.SaveAsync();
            return CreatedAtAction(nameof(Get), new { id = newConversation.ID }, newConversation);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Conversation conversation)
        {
            var existingConversation = await DB.Find<Conversation>().OneAsync(id);
            if (existingConversation == null)
            {
                return NotFound();
            }

            existingConversation.IsDM = conversation.IsDM;
            existingConversation.Participants = conversation.Participants;
            existingConversation.Messages = conversation.Messages;

            await existingConversation.SaveAsync();
            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await DB.DeleteAsync<Conversation>(id);
            if (result.DeletedCount == 0)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
