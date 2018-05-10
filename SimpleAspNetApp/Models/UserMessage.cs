using System;

namespace SimpleAspNetApp.Models {
    [Serializable]
    public class UserMessage {
        public string UserId { get; set; }
        public string Message { get; set; }
    }
}