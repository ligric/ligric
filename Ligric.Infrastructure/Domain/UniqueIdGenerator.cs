using Ligric.Domain.SeedWork;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;

namespace Ligric.Infrastructure.Domain
{
    internal class UniqueIdGenerator : IUniqueIdGenerator
    {
        private readonly SequentialGuidValueGenerator _sequentialGuidIdentityGenerator;

        public UniqueIdGenerator() 
        {
            _sequentialGuidIdentityGenerator = new SequentialGuidValueGenerator();
        }

        public Guid GetUniqueId()
        {
            return this._sequentialGuidIdentityGenerator.Next(null);
        }
    }
}
