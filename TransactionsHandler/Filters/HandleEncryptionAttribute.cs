using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using TransactionsHandler.DTOs;
using TransactionsHandler.Services;

namespace TransactionsHandler.Filters;

public class HandleEncryptionAttribute : Attribute, IResultFilter
{
    private readonly ICryptographyHelper _cryptographyHelper;
    private readonly IConfiguration _configuration;

    public HandleEncryptionAttribute(ICryptographyHelper cryptographyHelper,
        IConfiguration configuration)
    {
        _cryptographyHelper = cryptographyHelper;
        _configuration = configuration;
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        var result = context.Result as ObjectResult;
        var response = result?.Value as ProcessTransactionResponse;
        if (response is null)
            return;
        var iv = _cryptographyHelper.GetIV();

        //TODO: Could be refactored to use reflection to handle any type
        result!.Value = new ProcessTransactionResponse
        {
            Code = Encrypt(response.Code, iv),
            Message = Encrypt(response.Message, iv),
            ApprovalCode = Encrypt(response.ApprovalCode, iv),
            DateTime = Encrypt(response.DateTime, iv),
            IV = Convert.ToBase64String(iv)
        };
    }

    private string Encrypt(string text, byte[] iv)
    {
        var key = _configuration.GetValue<string>("EncryptionKey");
        return _cryptographyHelper.Encrypt(text,
            Encoding.UTF8.GetBytes(key!),
            iv);
    }
}
