using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MySmoothieTry2.Model;

namespace MySmoothieTry2.Data
{
    public class SmoothieRestItemManager
    {
        IRestService restService;

        public SmoothieRestItemManager(IRestService service)
        {
            restService = service;
        }

        public Task<List<SmoothieRestItem>> GetTasksAsync()
        {
            return restService.RefreshDataAsync();
        }

        public Task SaveTaskAsync(SmoothieRestItem item, bool isNewItem = false)
        {
            return restService.SaveSmoothieRestItemAsync(item, isNewItem);
        }

        public Task DeleteTaskAsync(SmoothieRestItem item)
        {
            return restService.DeleteSmoothieRestItemAsync(item.ID);
        }
    }
}
