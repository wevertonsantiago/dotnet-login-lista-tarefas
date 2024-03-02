namespace configs;

public class EndpointLowercaseLetter : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value != null)
        {
            return value.ToString()?.ToLower();
        }
        else
        {
            return null;
        }
    }
}
