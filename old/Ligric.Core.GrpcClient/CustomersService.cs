using DevPace.Core.DataTypes;
using DevPace.Protos;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utils.EventArgs;
using static DevPace.Protos.Customers;

namespace DevPace.Core.GrpcClient;

public class CustomersService : ICustomersService
{
    protected int customersChangedSyncNumber = 0;
    private readonly IAuthorizationService _authorizationService;
    private readonly CustomersClient _client;
    private readonly IMetadataRepository _metadata;
    private readonly Dictionary<Guid, CustomerDto> customersPrivate = new Dictionary<Guid, CustomerDto>();
    public IReadOnlyDictionary<Guid, CustomerDto> Customers => new ReadOnlyDictionary<Guid, CustomerDto>(customersPrivate);
    private CancellationTokenSource? _getAllSubscribeCancellationToken;
    private CancellationTokenSource? _getAllFiltredSubscribeCancellationToken;

    public event EventHandler<NotifyDictionaryChangedEventArgs<Guid, CustomerDto>>? CustomersChanged;

    public CustomersService(
        IAuthorizationService authorizationService,
        GrpcChannel grpcChannel,
        IMetadataRepository metadata)
    {
        _authorizationService = authorizationService;
        _client = new CustomersClient(grpcChannel);
        _metadata = metadata;

        _authorizationService.AuthorizationStateChanged += OnAuthorizationStateChanged;
    }

    public async void GetAll()
    {
        //if (_getAllSubscribeCancellationToken is not null && !_getAllSubscribeCancellationToken.IsCancellationRequested)
        //    return new Task<IEnumerator<CustomerDto>>;

        _getAllSubscribeCancellationToken = new CancellationTokenSource();
        using var streamingCall = _client.GetAll(new Protos.CustomersRequest(), cancellationToken: _getAllSubscribeCancellationToken.Token);

        customersPrivate.Clear();
        RaiseCustomersChanged(NotifyActionDictionaryChangedEventArgs.ClearKeyValuePairs<Guid, CustomerDto>(
            customersChangedSyncNumber++, 
            DateTimeOffset.Now.ToUnixTimeMilliseconds()));

        await foreach (CustomersStreamResponse customerResponse in streamingCall.ResponseStream
            .ToAsyncEnumerable()
            .Finally(() => streamingCall.Dispose())
            .WithEnforcedCancellation(_getAllSubscribeCancellationToken.Token))
        {
            CustomerDto customer = customerResponse.Customer.ToCustomerDto();
            customersPrivate.Add(customer.Guid, customer);
            RaiseCustomersChanged(NotifyActionDictionaryChangedEventArgs.AddKeyValuePair(
                customer.Guid, 
                customer, 
                customersChangedSyncNumber++, 
                DateTimeOffset.Now.ToUnixTimeMilliseconds()));
        };
    }

    public async IAsyncEnumerable<CustomerDto> GetAllFromFilter(CustomersFilterDto? filter = null)
    {
        //if (_getAllFiltredSubscribeCancellationToken is not null && !_getAllFiltredSubscribeCancellationToken.IsCancellationRequested)
        //    return new Task<IEnumerator<CustomerDto>>;

        _getAllFiltredSubscribeCancellationToken = new CancellationTokenSource();
        using var streamingCall = _client.GetAll(new CustomersRequest()
        {
            Filter = filter?.ToCustomersFilterRequest()
        }, cancellationToken: _getAllFiltredSubscribeCancellationToken.Token);

        await foreach (CustomersStreamResponse customerResponse in streamingCall.ResponseStream
            .ToAsyncEnumerable()
            .Finally(() => streamingCall.Dispose())
            .WithEnforcedCancellation(_getAllFiltredSubscribeCancellationToken.Token))
        {
            CustomerDto customer = customerResponse.Customer.ToCustomerDto();
            yield return customer;
        };
    }

    public void GetAllCancel() => GetAllCancelPrivate();

    private void OnAuthorizationStateChanged(object sender, UserAuthorizationState e)
    {
        if (e == UserAuthorizationState.Disconnected)
        {
            RaiseCustomersChanged(NotifyActionDictionaryChangedEventArgs.ClearKeyValuePairs<Guid, CustomerDto>(
                customersChangedSyncNumber++,
                DateTimeOffset.Now.ToUnixTimeMilliseconds()));
            GetAllCancel();
        }
        else
        {
            GetAll();
        }
    }

    public void Add(string name, string companyName, string phone, string email)
    {
        CustomerResponse response = _client.Add(new CustomerAddRequest
        {
            Name = name,
            CompanyName = companyName,
            Phone = phone,
            Email = email
        });

        if (!response.Result.IsSuccess)
            return;

        Guid guid = Guid.Parse(response.Guid);
        CustomerDto customer = new CustomerDto(guid, name, companyName, phone, email);
        customersPrivate.Add(guid, customer);

        RaiseCustomersChanged(NotifyActionDictionaryChangedEventArgs.AddKeyValuePair(
            guid,
            customer,
            customersChangedSyncNumber++,
            DateTimeOffset.Now.ToUnixTimeMilliseconds()));
    }

    public async Task AddAsync(string name, string companyName, string phone, string email)
    {
        CustomerResponse response = await _client.AddAsync(new CustomerAddRequest
        {
            Name = name,
            CompanyName = companyName,
            Phone = phone,
            Email = email
        });

        if (!response.Result.IsSuccess)
            return;

        Guid guid = Guid.Parse(response.Guid);
        CustomerDto customer = new CustomerDto(guid, name, companyName, phone, email);
        customersPrivate.Add(guid, customer);

        RaiseCustomersChanged(NotifyActionDictionaryChangedEventArgs.AddKeyValuePair(
            guid,
            customer,
            customersChangedSyncNumber++,
            DateTimeOffset.Now.ToUnixTimeMilliseconds()));
    }

    public bool Change(CustomerDto customer)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ChangeAsync(CustomerDto customer)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Remove customer from Id
    /// </summary>
    /// <param name="guid"></param>
    /// <returns>false if waf an error.</returns>
    public bool Remove(Guid guid)
    {
        ResponseResult response = _client.Remove(new CustomerRemoveRequest
        {
           Guid = guid.ToString()
        });

        if (!response.IsSuccess)
            return false;

        customersPrivate.Remove(guid);

        RaiseCustomersChanged(NotifyActionDictionaryChangedEventArgs.RemoveKeyValuePair<Guid, CustomerDto>(
            guid,
            customersChangedSyncNumber++,
            DateTimeOffset.Now.ToUnixTimeMilliseconds()));

        return true;
    }

    public async Task<bool> RemoveAsync(Guid guid)
    {
        ResponseResult response = await _client.RemoveAsync(new CustomerRemoveRequest
        {
            Guid = guid.ToString()
        });

        if (!response.IsSuccess)
            return false;

        customersPrivate.Remove(guid);

        RaiseCustomersChanged(NotifyActionDictionaryChangedEventArgs.RemoveKeyValuePair<Guid, CustomerDto>(
            guid,
            customersChangedSyncNumber++,
            DateTimeOffset.Now.ToUnixTimeMilliseconds()));

        return true;
    }

    private void GetAllCancelPrivate()
    {
        _getAllSubscribeCancellationToken?.Cancel();
        _getAllSubscribeCancellationToken?.Dispose();
    }
        
    private void GetAllFiltredCancel()
    {
        _getAllFiltredSubscribeCancellationToken?.Cancel();
        _getAllFiltredSubscribeCancellationToken?.Dispose();
    }

    protected void RaiseCustomersChanged(NotifyDictionaryChangedEventArgs<Guid, CustomerDto> eventArgs)
        => CustomersChanged?.Invoke(this, eventArgs);

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
