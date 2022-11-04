using AutoMapper;
using CM.Entities.Base;
using CM.Model.Dto.Base;

namespace CM.Database.Mappings;

public class RelationalIdResolver<T, U, V, W> : IValueResolver<T, V, W>
    where T : BaseDto
    where U : BaseDto
    where V : BaseEntity
    where W : BaseEntity
{
    public W Resolve(T source, V destination, W destinationRelation, ResolutionContext context)
    {
        var relation = new object() as W;

        var type = source.GetType();

        //Make sure that source has the correct property and not a field
        var propertyInfo = type.GetProperties()
            .FirstOrDefault(p => p.PropertyType == typeof(U));

        if (propertyInfo == null)
        {
            return relation;
        }

        var sourceRelation = propertyInfo.GetValue(source);

        if (sourceRelation == null)
            return relation;

        relation = destinationRelation != null
            ? context.Mapper.Map(sourceRelation, destinationRelation)
            : context.Mapper.Map<W>(sourceRelation);

        return relation;
    }
}