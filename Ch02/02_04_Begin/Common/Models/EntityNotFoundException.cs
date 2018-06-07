using System;

namespace HPlusSports
{
    public class EntityNotFoundException : ApplicationException
    {
        public object Id { get; }
        public Type Type { get; }

        public EntityNotFoundException(Type type, object id)
          : base($"Entity ${id?.ToString()} of type ${type?.Name} not found.")
        {
            Type = type;
            Id = id;
        }
    }

    public class EntityNotFoundException<T> : EntityNotFoundException
    {
        public EntityNotFoundException(object id) : base(typeof(T), id)
        {
        }
    }

}