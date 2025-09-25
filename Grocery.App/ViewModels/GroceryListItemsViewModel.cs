using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace Grocery.App.ViewModels
{
    public partial class GroceryListItemsViewModel : BaseViewModel
    {
        private readonly IGroceryListItemsService _groceryListItemsService;
        private readonly IProductService _productService;

        public ObservableCollection<GroceryListItem> MyGroceryListItems { get; set; } = [];

        [ObservableProperty]
        private ObservableCollection<Product> availableProducts = [];

        [ObservableProperty]
        private GroceryList groceryList = new(0, "None", DateOnly.MinValue, "", 0);

        public GroceryListItemsViewModel(
            IGroceryListItemsService groceryListItemsService,
            IProductService productService)
        {
            _groceryListItemsService = groceryListItemsService;
            _productService = productService;

            // ✅ Start met alle producten tonen
            LoadProducts();
        }

        private void LoadProducts()
        {
            AvailableProducts = new ObservableCollection<Product>(_productService.GetAll());
        }

        // ✅ UC08: Zoekfunctie
        [RelayCommand]
        private void Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // Geen zoekterm → toon alle producten
                LoadProducts();
            }
            else
            {
                var filtered = _productService.GetAll()
                    .Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

                AvailableProducts = new ObservableCollection<Product>(filtered);
            }
        }
    }
}
