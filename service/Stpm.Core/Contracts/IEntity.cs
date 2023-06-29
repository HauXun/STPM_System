namespace Stpm.Core.Contracts;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
}

//public interface IEntityWithStructKey<TKey> : IEntity<TKey> where TKey : struct
//{
//}

//public interface IEntityWithClassKey<TKey> : IEntity<TKey> where TKey : class
//{
//}
