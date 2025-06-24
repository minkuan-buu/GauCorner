public class StreamConfigResModel
{
    public Guid Id { get; set; }
    public Guid TypeId { get; set; }
    public string TypeName { get; set; } = null!;
    public string AlternativeName { get; set; } = null!;
    public string Value { get; set; } = null!;
}