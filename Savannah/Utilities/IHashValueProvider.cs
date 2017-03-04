namespace Savannah.Utilities
{
    internal interface IHashValueProvider
    {
        string GetHashFor(string value);
    }
}