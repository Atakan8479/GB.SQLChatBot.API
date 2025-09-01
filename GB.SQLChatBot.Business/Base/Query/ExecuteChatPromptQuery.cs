using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB.SQLChatBot.Business.Base.Query
{
    public class ExecuteChatPromptQuery : IRequest<List<Dictionary<string, object>>>
    {
        public string Prompt { get; set; }
    }
}
