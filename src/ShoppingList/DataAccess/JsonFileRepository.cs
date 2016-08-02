﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ShoppingList.Models;
using ShoppingList.Services;
using System.Linq;

namespace ShoppingList.DataAccess
{
    public class JsonFileRepository : IRepository
    {
        private readonly DirectoryInfo _rootFolder;
        private readonly JsonSerializer _serializer;

        private FileInfo ItemsFile => new FileInfo(Path.Combine(_rootFolder.FullName, "Items.json"));
        private FileInfo StoresFile => new FileInfo(Path.Combine(_rootFolder.FullName, "Stores.json"));
        private FileInfo ShoppingListsFile => new FileInfo(Path.Combine(_rootFolder.FullName, "ShoppingLists.json"));

        public JsonFileRepository(DirectoryInfo rootFolder, JsonSerializer serializer)
        {
            _rootFolder = rootFolder;
            _serializer = serializer;
            if (!_rootFolder.Exists)
                _rootFolder.Create();
        }

        public IEnumerable<string> GetItems()
        {
            if (!ItemsFile.Exists)
                return Enumerable.Empty<string>();

            using (var fs = ItemsFile.OpenText())
            {
                return (List<string>) _serializer.Deserialize(fs, typeof(List<String>));
            }

        }

        public void AddItem(IEnumerable<string> items)
        {
            var allItems = GetItems().Concat(items).Distinct().ToList();
            using (var fs = ItemsFile.CreateText()) {
                _serializer.Serialize(fs, allItems);                
            }
        }

        public void RemoveItem(IEnumerable<string> items)
        {
            var allItems = GetItems().Except(items).Distinct().ToList();
            using (var fs = ItemsFile.CreateText())
            {
                _serializer.Serialize(fs, allItems);
            }
        }

        public IEnumerable<Store> GetStores()
        {
            if (!StoresFile.Exists)
                return Enumerable.Empty<Store>();

            using (var fs = StoresFile.OpenText())
            using (var reader = new JsonTextReader(fs))
            {
                return _serializer.Deserialize<List<StoreDTO>>(reader).Select(dto => dto.ToModel()).Concat(new[] { Store.None });
            }

        }

        public void SaveStore(params Store[] stores)
        {
            var allStores = stores.Concat(GetStores()).Where(s => s.IsReal).Distinct();
            var dtos = allStores.Select(m => m.ToDto()).ToList();
            using (var fs = StoresFile.CreateText()) {
                _serializer.Serialize(fs, dtos);
            }
        }

        public void RemoveStore(params Guid[] storeIDs) {
            var currentStores = GetStores().ToList();
            var storesToRemove = currentStores.Join(storeIDs, s => s.ID, id => id, (s, id) => s);
            var storesToKeep = currentStores.Except(storesToRemove);

            var dtos = storesToKeep.Select(m => m.ToDto()).ToList();
            using (var fs = StoresFile.CreateText()) {
                _serializer.Serialize(fs, dtos);
            }

        }

        public IEnumerable<Models.ShoppingList> GetAllShoppingLists()
        {
            if (!ShoppingListsFile.Exists)
                return Enumerable.Empty<Models.ShoppingList>();

            using (var fs = ShoppingListsFile.OpenText())
            using (var reader = new JsonTextReader(fs))
            {
                var stores = GetStores().ToList();
                var shoppingListDtos = _serializer.Deserialize<List<ShoppingListDTO>>(reader);
                var result = shoppingListDtos.Select(dto => dto.ToModel(stores));
                return result;
            }
        }

        public Models.ShoppingList GetShoppingList(Guid id) {
            var allShoppingLists = GetAllShoppingLists();
            return allShoppingLists.FirstOrDefault(sl => sl.ID == id);
        }

        public void Save(params Models.ShoppingList[] lists)
        {
            var allLists = lists.Concat(GetAllShoppingLists()).Distinct();
            var dtos = allLists.Select(l => l.ToDto()).ToList();
            using (var fs = ShoppingListsFile.CreateText()) {
                _serializer.Serialize(fs, dtos);
            }
        }
    }
}