using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Ligric.Service.CryptoApisService.Infrastructure.Conventions
{
    public class TableNameConvention : IHasManyToManyConvention, IClassConvention
    {
        public void Apply(IManyToManyCollectionInstance instance)
        {
            instance.Table($"{instance.TableName.Replace("Entity", "")}");
        }

        public void Apply(IClassInstance instance)
        {
            instance.Table($"{instance.TableName.Replace("Entity", "")}");
        }
    }
}
