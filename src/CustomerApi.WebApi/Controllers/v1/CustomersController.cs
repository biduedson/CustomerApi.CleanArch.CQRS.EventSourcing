using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Asp.Versioning;
using CustomerApi.Application.Customer.Commands;
using CustomerApi.Application.Customer.Responses;
using CustomerApi.Query.Application.Customer.Queries;
using CustomerApi.Query.QueriesModel;
using CustomerApi.WebApi.Extensions;
using CustomerApi.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApi.WebApi.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
public class CustomersController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse<CreatedCustomerResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody][Required] CreateCustomerCommand command) =>
        (await mediator.Send(command)).ToActionResult();

    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromBody][Required] UpdateCustomerCommand command) =>
        (await mediator.Send(command)).ToActionResult();

    [HttpDelete("{id:guid}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([Required] Guid id) =>
        (await mediator.Send(new DeleteCustomerCommand(id))).ToActionResult();

    [HttpGet("{id:guid}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResponse<CustomerQueryModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById([Required] Guid id) =>
       (await mediator.Send(new GetCustomerByIdQuery(id))).ToActionResult();


    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CustomerQueryModel>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll() =>
        (await mediator.Send(new GetAllCustomerQuery())).ToActionResult();
}
