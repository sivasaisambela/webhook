using Microsoft.AspNetCore.Mvc;
using WebHookApi.Models;



namespace WebHookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebHookController : ControllerBase
    {

       
        [HttpGet("test")]
        public ActionResult TestEndpoint()
        {
            return Ok("The API is working!");
        }

        private static readonly Dictionary<string, Subscription> Subscriptions = new Dictionary<string, Subscription>();

        [HttpPost("subscribe")]
        public ActionResult Subscribe([FromBody] Subscription subscription)
        {

           
            if (subscription == null)
            {
                return BadRequest("No data provided.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            subscription.SubscriptionId = System.Guid.NewGuid().ToString();
            Subscriptions.Add(subscription.SubscriptionId, subscription);
            return Ok(subscription);
        }

       
        [HttpGet("subscriptions/{userId}")]
        public ActionResult GetSubscriptions(string userId)
        {
            var userSubscriptions = Subscriptions.Values.Where(s => s.UserId == userId);
            return Ok(userSubscriptions);
        }

       
        [HttpPost("simulate-event")]
        public async Task<ActionResult> SimulateEvent([FromBody] EventSimulator eventSimulator)
        {
            HttpClient client = new HttpClient();
            foreach (var sub in Subscriptions.Values.Where(s => s.EventType == eventSimulator.EventType))
            {
                var content = new StringContent($"Event {sub.EventType} occurred!");
                await client.PostAsync(sub.CallbackUrl, content);
            }
            return Ok($"Event {eventSimulator.EventType} simulated!");
        }

        public class EventSimulator
        {
            public string EventType { get; set; }
        }
    }
}
