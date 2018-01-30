using System;

namespace CarNBusAPI.Data
{
    interface ICarUnitOfWork: IDisposable
    {
	    ICarRepository Cars { get; }
		int Complete();
    }
}
