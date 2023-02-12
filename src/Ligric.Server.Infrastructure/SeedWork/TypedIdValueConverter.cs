using System;
using Ligric.Server.Domain.SeedWork;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Ligric.Infrastructure.SeedWork
{
    public class TypedIdValueConverter<TTypedIdValue> : ValueConverter<TTypedIdValue, Guid>
        where TTypedIdValue : TypedIdValueBase
    {
        public TypedIdValueConverter(ConverterMappingHints? mappingHints = null) 
            : base(id => id.Value, value => Create(value), mappingHints)
        {
        }

#pragma warning disable CS8603 // Possible null reference return.
		private static TTypedIdValue Create(Guid id) => Activator.CreateInstance(typeof(TTypedIdValue), id) as TTypedIdValue;
#pragma warning restore CS8603 // Possible null reference return.
	}
}
