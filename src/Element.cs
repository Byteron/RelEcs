namespace RelEcs;

internal readonly struct Element<T> where T : class
{
    public readonly T Value;
    public Element(T value) => Value = value;
}