using Microsoft.Data.SqlClient;
using MillaWeb.Models;

namespace MillaWeb.Data;

public class ProductRepository
{
    private readonly string _connStr;
    public ProductRepository(IConfiguration config)
    {
        _connStr = config.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Missing connection string: Default");
    }

    public ProductDetailVM? GetProductDetail(int productId)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        // Ürün üst bilgisi
        using var cmd = new SqlCommand(@"
SELECT p.ProductID, p.ProductName, p.UnitPrice, p.Description, p.ImageUrl,
       b.BrandName, c.CategoryName, s.StoreName AS SupplierName
FROM dbo.Product p
JOIN dbo.Brand b ON b.BrandID = p.BrandID
JOIN dbo.Category c ON c.CategoryID = p.CategoryID
JOIN dbo.Supplier s ON s.SupplierID = p.SupplierID
WHERE p.ProductID=@id AND p.IsActive=1;
", conn);

        cmd.Parameters.AddWithValue("@id", productId);

        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;

        var vm = new ProductDetailVM
        {
            ProductID = r.GetInt32(0),
            ProductName = r.GetString(1),
            UnitPrice = r.GetDecimal(2),
            Description = r.IsDBNull(3) ? null : r.GetString(3),
            ImageUrl = r.IsDBNull(4) ? null : r.GetString(4),
            BrandName = r.GetString(5),
            CategoryName = r.GetString(6),
            SupplierName = r.GetString(7),
        };
        r.Close();

        // Variantlar
        using var cmd2 = new SqlCommand(@"
SELECT VariantID, Color, Size, StockQuantity, Sku
FROM dbo.ProductVariant
WHERE ProductID=@id
ORDER BY Color, Size;
", conn);
        cmd2.Parameters.AddWithValue("@id", productId);

        using var r2 = cmd2.ExecuteReader();
        while (r2.Read())
        {
            vm.Variants.Add(new ProductVariantVM
            {
                VariantID = r2.GetInt32(0),
                Color = r2.GetString(1),
                Size = r2.GetString(2),
                StockQuantity = r2.GetInt32(3),
                Sku = r2.IsDBNull(4) ? null : r2.GetString(4),
            });
        }

        return vm;
    }
    public CartItem? GetVariantForCart(int variantId)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand(@"
SELECT
    pv.VariantID,
    p.ProductID,
    p.ProductName,
    pv.Color,
    pv.Size,
    p.UnitPrice,
    p.ImageUrl
FROM dbo.ProductVariant pv
JOIN dbo.Product p ON p.ProductID = pv.ProductID
WHERE pv.VariantID = @vid AND p.IsActive = 1;
", conn);

        cmd.Parameters.AddWithValue("@vid", variantId);

        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;

        return new CartItem
        {
            VariantID = r.GetInt32(0),
            ProductID = r.GetInt32(1),
            ProductName = r.GetString(2),
            Color = r.GetString(3),
            Size = r.GetString(4),
            UnitPrice = r.GetDecimal(5),
            ImageUrl = r.IsDBNull(6) ? null : r.GetString(6),
            Quantity = 1
        };
    }

}
