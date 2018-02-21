using Shared.Models.Read;
using Server.DAL;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Server.DAL
{
    interface IUnitOfWork: IDisposable
    {
	    int Complete();
    }
}
