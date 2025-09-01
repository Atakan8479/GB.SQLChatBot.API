using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SQLChatBot.Core
{
    public class ChatbotResponse
    {
        /// <summary>
        /// İşlem başarılı mı?
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Yapay zekanın oluşturduğu SQL sorgusu.
        /// </summary>
        public string Sql { get; set; } = string.Empty;

        /// <summary>
        /// Kullanıcıya gösterilecek açıklama mesajı.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// SQL çalıştırıldıysa gelen sonuçlar.
        /// </summary>
        public List<Dictionary<string, object>> Data { get; set; } = new();
    }
}

