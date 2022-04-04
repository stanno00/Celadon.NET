namespace DotNetTribes.DTOs.Trade
{
    public class TradeRequestDTO
    {
        public TypeAmountDTO OfferedResource { get; set; }
        public TypeAmountDTO WantedResource { get; set; }
    }
}