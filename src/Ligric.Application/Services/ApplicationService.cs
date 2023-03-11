using System;
using Ligric.Application.Bus;

namespace Ligric.Application.Services
{
    public abstract class ApplicationService : IApplicationService
    {
        #region Fields


        protected readonly IInMemoryBus _inMemoryBus;


        #endregion

        #region Ctors


        public ApplicationService(IInMemoryBus inMemoryBus)
        {
            _inMemoryBus = inMemoryBus;
        }



        #endregion


        #region Public Methods



        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }



        #endregion
    }
}