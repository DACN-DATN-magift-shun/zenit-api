namespace Zenit.Share.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class MongoEntityAttribute : Attribute
    {
        public string CollectionName { get; }

        public MongoEntityAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }
    }
}
