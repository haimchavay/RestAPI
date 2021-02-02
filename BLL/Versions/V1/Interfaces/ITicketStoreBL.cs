using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Versions.V1.Interfaces
{
    public interface ITicketStoreBL
    {
        Task<IActionResult> GetTicketsStores(IIdentity userIdentity);
        Task<IActionResult> GetTicketsStore(long storeId);
    }
}
