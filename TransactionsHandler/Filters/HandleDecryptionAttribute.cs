using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TransactionsHandler.DTOs;
using TransactionsHandler.Services;

namespace TransactionsHandler.Filters;

public class HandleDecryptionAttribute : ActionFilterAttribute
{
    private readonly ICryptographyHelper _cryptographyHelper;
    private readonly IConfiguration _configuration;

    public HandleDecryptionAttribute(ICryptographyHelper cryptographyHelper,
        IConfiguration configuration)
    {
        _cryptographyHelper = cryptographyHelper;
        _configuration = configuration;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var request = context.ActionArguments["request"] as ProcessTransactionRequest;
        if (request is null)
        {
            var response = new ProcessTransactionResponse(TransactionCodes.Failure,
             TransactionMessages.Failure,
             ApprovalCode.Error.ToString("D"),
             DateTime.Now.ToString(""));
            context.Result = new BadRequestObjectResult(response);
            return;
        }

        //TODO: Could be refactored to use reflection to handle any type
        request.ProcessingCode = Decrypt(request.ProcessingCode, request.IV);
        request.SystemTraceNr = Decrypt(request.SystemTraceNr, request.IV);
        request.FunctionCode = Decrypt(request.FunctionCode, request.IV);
        request.CardNo = Decrypt(request.CardNo, request.IV);
        request.AmountTrxn = Decrypt(request.AmountTrxn, request.IV);
        request.CurrencyCode = Decrypt(request.CurrencyCode, request.IV);
    }

    private string Decrypt(string text, string iv)
    {
        var key = _configuration.GetValue<string>("EncryptionKey");
        return _cryptographyHelper.Decrypt(text,
            Encoding.UTF8.GetBytes(key!),
            Convert.FromBase64String(iv));
    }
}
