using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebHookApi.Models
{
    public class Subscription
    {
        [NotMapped]
        public string SubscriptionId { get; set; }

        [Required]
        public string UserId { get; set; }
        [Required]
        public string EventType { get; set; }
        [Required]
        public string CallbackUrl { get; set; }
    }
}
