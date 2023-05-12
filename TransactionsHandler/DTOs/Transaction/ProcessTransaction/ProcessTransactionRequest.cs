namespace TransactionsHandler.DTOs;

public class ProcessTransactionRequest
{
    public string ProcessingCode { get; set; } = string.Empty;
    public string SystemTraceNr { get; set; } = string.Empty;
    public string FunctionCode { get; set; } = string.Empty;
    public string CardNo { get; set; } = string.Empty;
    public string CardHolder { get; set; } = string.Empty;
    public string AmountTrxn { get; set; } = string.Empty;
    public string CurrencyCode { get; set; } = string.Empty;
    public string IV { get; set; } = string.Empty;

}