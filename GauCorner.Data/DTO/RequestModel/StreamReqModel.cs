namespace GauCorner.Data.DTO.RequestModel
{
    public class StreamConfigDto
    {
        public Guid Id { get; set; }
        public string AlternativeName { get; set; } = null!;
        public string Value { get; set; } = null!;
    }
}