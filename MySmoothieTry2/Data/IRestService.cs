using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySmoothieTry2.Model;

namespace MySmoothieTry2.Data
{
    public interface IRestService
    {
        Task<List<SmoothieRestItem>> RefreshDataAsync();

        Task SaveSmoothieRestItemAsync(SmoothieRestItem item, bool isNewItem);

        Task DeleteSmoothieRestItemAsync(string id);
    }
}
