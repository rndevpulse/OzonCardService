using OzonCard.Cloud.Client.Data.Customers;

namespace OzonCard.Cloud.Client.Data.Categories;

public record GuestCategoriesResult(
    IEnumerable<Category> GuestCategories
);

