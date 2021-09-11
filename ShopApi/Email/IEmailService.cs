using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApi.Models;
using ShopApi.Wrappers;

namespace ShopApi.Email
{
    public interface IEmailService
    {
        public Task<BaseResponse<bool>> SendEmailAsync(EmailModel model);
    }
}
