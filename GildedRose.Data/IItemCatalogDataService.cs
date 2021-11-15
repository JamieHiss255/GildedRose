using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace GildedRose
{
    public interface IItemCatalogDataService
    {
        void LoadTableData(List<CatalogItem> items);
        CatalogItem GetItem(int id);
        List<CatalogItem> GetItems();
    }
}
