// GB.SQLChatBot.Business/Models/OpenAIRequestModels.cs (Dosya adı aynı kalıyor)
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace GB.SQLChatBot.Business.Leave.Models
{
    // NOT: Bu modeller artık OpenAI API'si için değil, Ollama'nın /api/generate endpoint'i içindir.
    // İsimler, kullanıcının isteği üzerine eski OpenAI modellerinin isimlerine benzer tutulmuştur.

    // Ollama'nın /api/generate endpoint'i için istek modeli
    public class ChatCompletionRequest // Bu sınıf adı artık Ollama isteğini temsil ediyor
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = "deepseek-coder-v2:16b"; // Kullandığınız Ollama modeli
        [JsonPropertyName("system")]
        public string System { get; set; } = string.Empty; // Ollama'da system prompt
        [JsonPropertyName("prompt")]
        public string Prompt { get; set; } = string.Empty; // Ollama'da user prompt
        [JsonPropertyName("stream")]
        public bool Stream { get; set; } = false; // Tek seferlik yanıt için false
    }

    // Ollama'nın /api/generate endpoint'i için yanıt modeli
    public class ChatCompletionResponse // Bu sınıf adı artık Ollama yanıtını temsil ediyor
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }
        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }
        [JsonPropertyName("response")] // Üretilen metin bu property içinde gelir
        public string Response { get; set; }
        [JsonPropertyName("done")]
        public bool Done { get; set; }
        [JsonPropertyName("context")]
        public List<int> Context { get; set; }
        [JsonPropertyName("total_duration")]
        public long? TotalDuration { get; set; }
        [JsonPropertyName("load_duration")]
        public long? LoadDuration { get; set; }
        [JsonPropertyName("prompt_eval_count")]
        public int? PromptEvalCount { get; set; }
        [JsonPropertyName("prompt_eval_duration")]
        public long? PromptEvalDuration { get; set; }
        [JsonPropertyName("eval_count")]
        public int? EvalCount { get; set; }
        [JsonPropertyName("eval_duration")]
        public long? EvalDuration { get; set; }
    }

    // Ollama için bu sınıflara gerek yok, isterseniz silebilirsiniz.
    // Ancak isimleri aynı tutma isteği nedeniyle, eğer başka bir yerde kullanılmıyorsa
    // bu sınıfları kaldırabilirsiniz.
    // public class ChatMessage { /* ... */ }
    // public class Choice { /* ... */ }
    // public class Usage { /* ... */ }
}