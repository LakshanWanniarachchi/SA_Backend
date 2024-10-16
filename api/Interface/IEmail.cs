using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace api.Interface
{
    public interface IEmail
    {


        Task SendTestEmailAsync(string reciverName, string reciverEmail, string reciverMessage);

    }
}