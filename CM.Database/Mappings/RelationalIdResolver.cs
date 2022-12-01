using AutoMapper;
using CM.Entities.Base;
using CM.Model.Dto.Base;

namespace CM.Database.Mappings;

public class RelationalIdResolver<T, TU, TV, TW> : IValueResolver<T, TV, TW>
    where T : BaseDto
    where TU : BaseDto
    where TV : BaseEntity
    where TW : BaseEntity
{
    public TW Resolve(T source, TV destination, TW? destinationRelation, ResolutionContext context)
    {
        var relation = new object() as TW;

        var type = source.GetType();

        //Make sure that source has the correct property and not a field
        var propertyInfo = type.GetProperties()
            .FirstOrDefault(p => p.PropertyType == typeof(TU));

        if (propertyInfo == null)
        {
            return relation!;
        }

        var sourceRelation = propertyInfo.GetValue(source);

        if (sourceRelation == null)
            return relation!;

        relation = destinationRelation != null
            ? context.Mapper.Map(sourceRelation, destinationRelation)
            : context.Mapper.Map<TW>(sourceRelation);

        return relation;
    }
}