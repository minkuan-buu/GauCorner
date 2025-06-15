using GauCorner.Data.Entities;

public class VariantAttributeValue
{
    public Guid VariantId { get; set; }
    public Guid ValueId { get; set; }

    public ProductVariant Variant { get; set; }
    public AttributeValue Value { get; set; }
}
