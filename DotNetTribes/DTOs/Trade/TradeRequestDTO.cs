namespace DotNetTribes.DTOs.Trade
{
    public class TradeRequestDTO
    {
        public TypeAmountDTO offered_resource { get; set; }
        public TypeAmountDTO wanted_resource { get; set; }
    }
}