namespace Savannah.Utilities
{
    internal interface IHashProvider
    {
        string GetHashFor(string value);
    }
}