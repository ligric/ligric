using DevPace.Core.DataTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utils.EventArgs;

namespace DevPace.Core;

public interface ICustomersService : IDisposable
{
    IReadOnlyDictionary<Guid, CustomerDto> Customers { get; }

    event EventHandler<NotifyDictionaryChangedEventArgs<Guid, CustomerDto>> CustomersChanged;

    void Add(string name, string companyName, string phone, string email);
    Task AddAsync(string name, string companyName, string phone, string email);

    void GetAll();
    IAsyncEnumerable<CustomerDto> GetAllFromFilter(CustomersFilterDto? filter = null);
    void GetAllCancel();

    bool Remove(Guid id);
    Task<bool> RemoveAsync(Guid id);

    bool Change(CustomerDto customer);
    Task<bool> ChangeAsync(CustomerDto customer);
}