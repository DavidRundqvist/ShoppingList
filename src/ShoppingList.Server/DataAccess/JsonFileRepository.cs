using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ShoppingList.Server.Services;
using ShoppingList.Shared.Data;

namespace ShoppingList.Server.DataAccess
{
    public class JsonFileRepository : IRepository
    {
        private readonly DirectoryInfo _rootFolder;
        private readonly JsonSerializer _serializer;

        private FileInfo StoresFile => new FileInfo(Path.Combine(_rootFolder.FullName, "Stores.json"));
        private FileInfo ShoppingListsFile => new FileInfo(Path.Combine(_rootFolder.FullName, "ShoppingLists.json"));
        private FileInfo RecipesFile => new FileInfo(Path.Combine(_rootFolder.FullName, "Recipes.json"));

        private readonly object _lock = new object();


        public JsonFileRepository(DirectoryInfo rootFolder, JsonSerializer serializer)
        {
            _rootFolder = rootFolder;
            _serializer = serializer;
            if (!_rootFolder.Exists)
                _rootFolder.Create();
        }


        public IEnumerable<StoreDTO> GetStores()
        {
            lock (_lock)
            {
                if (!StoresFile.Exists)
                    return Enumerable.Empty<StoreDTO>();
                using (var fs = StoresFile.OpenText())
                {
                    using (var reader = new JsonTextReader(fs))
                    {
                        return _serializer.Deserialize<List<StoreDTO>>(reader);
                    }
                }
            }
        }


        public void Save(params StoreDTO[] stores)
        {
            lock (_lock)
            {
                var dtos = stores.Concat(GetStores()).Distinct().ToList();
                using (var fs = StoresFile.CreateText())
                {
                    _serializer.Serialize(fs, dtos);
                }
            }
        }

        public void RemoveStore(params Guid[] storeIDs) {
            var currentStores = GetStores().ToList();
            var storesToRemove = currentStores.Join(storeIDs, s => s.ID, id => id, (s, id) => s);
            var storesToKeep = currentStores.Except(storesToRemove);

            var dtos = storesToKeep.ToList();
            lock (_lock)
            {
                using (var fs = StoresFile.CreateText())
                {
                    _serializer.Serialize(fs, dtos);
                }
            }
        }


        public IEnumerable<ShoppingListDTO> GetShoppingLists()
        {
            lock (_lock)
            {
                if (!ShoppingListsFile.Exists)
                    return Enumerable.Empty<ShoppingListDTO>();

                using (var fs = ShoppingListsFile.OpenText())
                {
                    using (var reader = new JsonTextReader(fs))
                    {
                        var result = _serializer.Deserialize<List<ShoppingListDTO>>(reader).ToArray();
                        return result;
                    }
                }
            }
        }


        public ShoppingListDTO GetShoppingList(Guid id) {
            var allShoppingLists = GetShoppingLists();
            return allShoppingLists.FirstOrDefault(sl => sl.ID == id);
        }

        public void Save(ShoppingListDTO list)
        {
            lock (_lock)
            {
                // Load
                var allLists = GetShoppingLists().ToList();
                var existingList = allLists.FirstOrDefault(l => l.ID == list.ID);
                if (existingList != null)
                    allLists.Remove(existingList);
                allLists.Add(list);

                // Save
                using (var fs = ShoppingListsFile.CreateText())
                {
                    _serializer.Serialize(fs, allLists);
                }
            }
        }

        public IEnumerable<RecipeDTO> GetRecipes()
        {
            lock (_lock)
            {
                if (!RecipesFile.Exists)
                    return Enumerable.Empty<RecipeDTO>();

                using (var fs = RecipesFile.OpenText())
                {
                    using (var reader = new JsonTextReader(fs))
                    {
                        return _serializer.Deserialize<List<RecipeDTO>>(reader);
                    }
                }
            }
        }

        public void Save(RecipeDTO recipe)
        {
            var recipes = GetRecipes().ToList();
            recipes.RemoveAll(r => r.Name.Equals(recipe.Name, StringComparison.InvariantCultureIgnoreCase));
            recipes.Add(recipe);

            lock (_lock)
            {
                using (var fs = RecipesFile.CreateText())
                {
                    _serializer.Serialize(fs, recipes);
                }
            }

        }

        public void Delete(RecipeDTO recipe)
        {
            var recipes = GetRecipes().ToList();
            recipes.RemoveAll(r => r.Name.Equals(recipe.Name, StringComparison.InvariantCultureIgnoreCase));
            lock (_lock)
            {
                using (var fs = RecipesFile.CreateText())
                {
                    _serializer.Serialize(fs, recipes);
                }
            }

        }

        //public void AddItem(Guid shoppingListId, string item)
        //{
        //    ModifyList(shoppingListId, l => l.Add(item));
        //}

        //public void RemoveItem(Guid shoppingListId, string item)
        //{
        //    ModifyList(shoppingListId, l => l.Remove(item));
        //}

        //public void BuyItem(Guid shoppingListId, string item)
        //{
        //    ModifyList(shoppingListId, l => l.Buy(item));
        //}

        //public void UnbuyItem(Guid shoppingListId, string item)
        //{
        //    ModifyList(shoppingListId, l => l.UnBuy(item));
        //}

        //public void SetStore(Guid shoppingListId, string storeName)
        //{
        //    var store = GetStores().FirstOrDefault(s => s.Name == storeName);
        //    if (store == null)
        //    {
        //        store = new Store(storeName, Guid.NewGuid());
        //        SaveStore(store);
        //    }
        //    ModifyList(shoppingListId, l => l.Store = store);
        //}

        public void DeleteShoppingList(Guid shoppingListId)
        {
            lock (_lock)
            {
                // Load
                List<ShoppingListDTO> allLists = GetShoppingLists().ToList();
                var listDto = allLists.FirstOrDefault(l => l.ID == shoppingListId);
                if (listDto != null)
                    allLists.Remove(listDto);

                // Save
                using (var fs = ShoppingListsFile.CreateText()) {
                    _serializer.Serialize(fs, allLists);
                }
            }
        }

    }
}