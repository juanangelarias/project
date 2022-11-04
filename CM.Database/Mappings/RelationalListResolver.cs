using AutoMapper;
using CM.Entities.Base;
using CM.Model.Dto.Base;

namespace CM.Database.Mappings;

public class RelationalListIdResolver<T, U, V, W> : IValueResolver<T, V, IEnumerable<W>>
    where T : BaseDto
    where U : BaseDto
    where V : BaseEntity
    where W : BaseEntity        
{
    public IEnumerable<W> Resolve(T source, V destination, IEnumerable<W> destinationRelations, ResolutionContext context)
    {
        var relations = new List<W>();

        var type = source.GetType();

        //Make sure that source has the correct property and not a field.
        var propertyInfo = type.GetProperties().FirstOrDefault(p => p.PropertyType == typeof(IEnumerable<U>));

        if (propertyInfo == null)
        {
            return relations;
        }
            
        var sourceRelations = (IEnumerable<U>)propertyInfo.GetValue(source);

        if (sourceRelations == null)
        {
            return relations;
        }

        foreach (var sourceRelation in sourceRelations)
        {
            var destinationRelation = destinationRelations?.FirstOrDefault(dr => dr.Id == sourceRelation.Id);

            if (destinationRelation != null)
            {
                relations.Add(context.Mapper.Map(sourceRelation, destinationRelation));
            }
            else
            {
                relations.Add(context.Mapper.Map<W>(sourceRelation));
            }
        }

        return relations;
    }
}