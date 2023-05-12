using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using TransactionsHandler.DTOs;
using TransactionsHandler.Filters;
using TransactionsHandler.Services;

namespace TransactionsHandler.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly ILogger<TransactionsController> _logger;

    public TransactionsController(ILogger<TransactionsController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [ServiceFilter(typeof(HandleDecryptionAttribute))]
    [ServiceFilter(typeof(HandleEncryptionAttribute))]
    public ActionResult<ProcessTransactionResponse> ProcessTransaction(
        ProcessTransactionRequest request)
    {
        _logger.LogInformation(JsonSerializer.Serialize(request));
        var response = new ProcessTransactionResponse
        {
            Code = "00",
            Message = "Success",
            ApprovalCode = "123123",
            DateTime = DateTime.Now.ToString()
        };
        return Ok(response);
    }
}
