using System;

namespace Server.DataRead
{
    internal interface ICarUnitOfWorkRead: IDisposable
    {
	    int Complete();
    }
}
