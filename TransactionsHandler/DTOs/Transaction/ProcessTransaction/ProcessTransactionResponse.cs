namespace TransactionsHandler.DTOs;

public enum ApprovalCode
{
    Approved = 123123,
    Declined = -1,
    Error = -2
}
public class ProcessTransactionResponse
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string ApprovalCode { get; set; } = string.Empty;
    public string DateTime { get; set; } = string.Empty;
    public string IV { get; set; } = string.Empty;

    public ProcessTransactionResponse() { }
    public ProcessTransactionResponse(string code,
        string message,
        string approvalCode,
        string dateTime)
    {
        Code = code;
        Message = message;
        ApprovalCode = approvalCode;
        DateTime = dateTime;
    }

}