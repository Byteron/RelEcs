namespace RelEcs;

internal class Element<T> : IComponent where T : class, IElement
{
    public readonly T Value;
    public Element(T value) => Value = value;
}