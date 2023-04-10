using FluentNHibernate.Mapping;

namespace Ligric.Backend.Domain.Base
{
    public static class MappingExtensions
    {
        public static void MapBase<T>(this ClassMap<T> mapClass) where T : EntityBase
        {
            mapClass.Id(x => x.Id).GeneratedBy.Identity();

            mapClass.Map(x => x.Deleted);
            mapClass.Map(x => x.UpdateDate);
            mapClass.Map(x => x.CreateDate).Generated.Insert();
        }
    }
}