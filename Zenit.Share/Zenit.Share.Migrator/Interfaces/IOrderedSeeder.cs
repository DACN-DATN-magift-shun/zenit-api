namespace Zenit.Share.Migrator.Interfaces;

public interface IOrderedSeeder : ISeeder
{
    public int Order { get; }
}