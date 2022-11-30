using Ligric.Application.Cusomers.AddCustomer;
using Ligric.Application.Cusomers.GetCustomers;
using Ligric.Application.Customers;
using Ligric.Application.Customers.GetCustomers;
using Ligric.Application.Orders.RemoveCustomer;
using Ligric.Application.Users.CheckUserExists;
using Ligric.GrpcServer.Extensions;
using Ligric.Protos;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Ligric.GrpcServer.Services
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CustomersService : Customers.CustomersBase
    {
        private readonly IMediator _mediator;

        public CustomersService(IMediator mediator/*, ILogger<CustomersService> logger*/)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        public override async Task GetAll(CustomersRequest request, IServerStreamWriter<CustomersStreamResponse> responseStream, ServerCallContext context)
        {
            Guid requestId = Guid.NewGuid();
            CustomersFilterRequest? requestFilters = request?.Filter;
            CustomersFilter? cuctomersFilter = null;

            if (requestFilters is not null)
            {
                Enum.TryParse(typeof(CustomersFilterFlags), requestFilters.Flags, out object? flags);
                CustomersFilterFlags customersFilter = flags is null ? CustomersFilterFlags.Default : (CustomersFilterFlags)flags;
                cuctomersFilter = new CustomersFilter(customersFilter, requestFilters.Name, requestFilters.CompanyName, requestFilters.Phone, requestFilters.Email);
            }

            var customersQuery = new GetCustomersQuery(cuctomersFilter);
            IEnumerable<CustomerDetailsDto> customers = await _mediator.Send(customersQuery);

            var peer = context.Peer;
            //_logger.LogInformation($"{peer} frame captures subscribes.");
            context.CancellationToken.Register(() => /*_logger.LogInformation($"{peer} cancel frame captures subscription.")*/ { });

            // TODO : HARD CODE
            // Should be normal handler 
            try
            {
                await customers.ToAsyncEnumerable()
                    .ForEachAwaitAsync(async (x) => await responseStream.WriteAsync(
                        new CustomersStreamResponse
                        {
                            Result = ResponseExtensions.GetSuccessResponseResult(),
                            RequestId = requestId.ToString(),
                            Customer = x.ToCustomer()
                        }), context.CancellationToken)
                    .ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                //_logger.LogError($"{peer} frame captures unsubscribed.");
            }
            catch (Exception ex)
            {
                //_logger.LogError($"{peer} frame captures unsubscribed. Was some error: {ex}");
            }
        }

        [AllowAnonymous]
        public override async Task<CustomerResponse> Add(CustomerAddRequest request, ServerCallContext context)
        {
            var addCommand = new AddCustomerCommand(request.Name, request.CompanyName, request.Phone, request.Email);
            var response = await _mediator.Send(addCommand);
            return new CustomerResponse
            {
                Result = ResponseExtensions.GetSuccessResponseResult(),
                Guid = response.Id.ToString()
            };
        }

        [AllowAnonymous]
        public override async Task<ResponseResult> Remove(CustomerRemoveRequest request, ServerCallContext context)
        {
            var removeCommand = new RemoveCustomerCommand(Guid.Parse(request.Guid));
            var response = await _mediator.Send(removeCommand);
            return ResponseExtensions.GetSuccessResponseResult();
        }
    }
}
