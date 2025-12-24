using Microsoft.Data.SqlClient;
using MillaWeb.Models;
using System.Data;

namespace MillaWeb.Data;

public class AdminProductsRepository
{
    private readonly string _connStr;

    public AdminProductsRepository(IConfiguration config)
    {
        _connStr = config.GetConnectionString("Default")
                  ?? throw new InvalidOperationException("Missing connection string: Default");
    }

    public List<AdminProductRow> ListProducts()
    {
        var list = new List<AdminProductRow>();

        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminListProducts", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new AdminProductRow
            {
                ProductID = r.GetInt32(r.GetOrdinal("ProductID")),
                ProductName = r.GetString(r.GetOrdinal("ProductName")),
                CategoryName = r.GetString(r.GetOrdinal("CategoryName")),
                BrandName = r.GetString(r.GetOrdinal("BrandName")),
                SupplierName = r.GetString(r.GetOrdinal("SupplierName")),
                UnitPrice = r.GetDecimal(r.GetOrdinal("UnitPrice")),
                IsActive = r.GetBoolean(r.GetOrdinal("IsActive")),
                ImageUrl = r.IsDBNull(r.GetOrdinal("ImageUrl")) ? null : r.GetString(r.GetOrdinal("ImageUrl")),
            });
        }

        return list;
    }

    public int AddProduct(int categoryId, int brandId, int supplierId, string productName, decimal unitPrice, string? description, string? imageUrl)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminAddProduct", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@CategoryID", categoryId);
        cmd.Parameters.AddWithValue("@BrandID", brandId);
        cmd.Parameters.AddWithValue("@SupplierID", supplierId);
        cmd.Parameters.AddWithValue("@ProductName", productName);
        cmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
        cmd.Parameters.AddWithValue("@Description", (object?)description ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@ImageUrl", (object?)imageUrl ?? DBNull.Value);

        var newIdObj = cmd.ExecuteScalar();
        return Convert.ToInt32(newIdObj);
    }

    public void DeactivateProduct(int productId)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminDeactivateProduct", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ProductID", productId);

        cmd.ExecuteNonQuery();
    }

    public void DeleteProduct(int productId)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminDeleteProduct", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ProductID", productId);
        cmd.ExecuteNonQuery();
    }

    public void SetActive(int productId, bool isActive)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand("dbo.sp_AdminSetProductActive", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@ProductID", productId);
        cmd.Parameters.AddWithValue("@IsActive", isActive);

        cmd.ExecuteNonQuery();
    }

    public List<LookupItem> ListCategories()
    {
        var list = new List<LookupItem>();
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand(@"
SELECT CategoryID AS Id, CategoryName AS Name
FROM dbo.Category
ORDER BY CategoryName;", conn);

        using var r = cmd.ExecuteReader();
        while (r.Read())
            list.Add(new LookupItem { Id = r.GetInt32(0), Name = r.GetString(1) });

        return list;
    }

    public List<LookupItem> ListBrands()
    {
        var list = new List<LookupItem>();
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand(@"
SELECT BrandID AS Id, BrandName AS Name
FROM dbo.Brand
ORDER BY BrandName;", conn);

        using var r = cmd.ExecuteReader();
        while (r.Read())
            list.Add(new LookupItem { Id = r.GetInt32(0), Name = r.GetString(1) });

        return list;
    }

    public List<LookupItem> ListSuppliers()
    {
        var list = new List<LookupItem>();
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand(@"
SELECT SupplierID AS Id, StoreName AS Name
FROM dbo.Supplier
ORDER BY StoreName;", conn);

        using var r = cmd.ExecuteReader();
        while (r.Read())
            list.Add(new LookupItem { Id = r.GetInt32(0), Name = r.GetString(1) });

        return list;
    }
    public AdminProductEditVM? GetById(int id)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand(@"
SELECT ProductID, CategoryID, BrandID, SupplierID, ProductName, UnitPrice, Description, ImageUrl, IsActive
FROM dbo.Product
WHERE ProductID=@id;", conn);

        cmd.Parameters.AddWithValue("@id", id);

        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;

        return new AdminProductEditVM
        {
            ProductID = r.GetInt32(r.GetOrdinal("ProductID")),
            CategoryID = r.GetInt32(r.GetOrdinal("CategoryID")),
            BrandID = r.GetInt32(r.GetOrdinal("BrandID")),
            SupplierID = r.GetInt32(r.GetOrdinal("SupplierID")),
            ProductName = r.GetString(r.GetOrdinal("ProductName")),
            UnitPrice = r.GetDecimal(r.GetOrdinal("UnitPrice")),
            Description = r.IsDBNull(r.GetOrdinal("Description")) ? null : r.GetString(r.GetOrdinal("Description")),
            ImageUrl = r.IsDBNull(r.GetOrdinal("ImageUrl")) ? null : r.GetString(r.GetOrdinal("ImageUrl")),
            IsActive = r.GetBoolean(r.GetOrdinal("IsActive")),
        };
    }
    public void Update(AdminProductEditVM vm)
    {
        using var conn = new SqlConnection(_connStr);
        conn.Open();

        using var cmd = new SqlCommand(@"
UPDATE dbo.Product
SET CategoryID=@CategoryID,
    BrandID=@BrandID,
    SupplierID=@SupplierID,
    ProductName=@ProductName,
    UnitPrice=@UnitPrice,
    Description=@Description,
    ImageUrl=@ImageUrl,
    IsActive=@IsActive
WHERE ProductID=@ProductID;", conn);

        cmd.Parameters.AddWithValue("@ProductID", vm.ProductID);
        cmd.Parameters.AddWithValue("@CategoryID", vm.CategoryID);
        cmd.Parameters.AddWithValue("@BrandID", vm.BrandID);
        cmd.Parameters.AddWithValue("@SupplierID", vm.SupplierID);
        cmd.Parameters.AddWithValue("@ProductName", vm.ProductName);
        cmd.Parameters.AddWithValue("@UnitPrice", vm.UnitPrice);

        cmd.Parameters.AddWithValue("@Description", (object?)vm.Description ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@ImageUrl", (object?)vm.ImageUrl ?? DBNull.Value);

        cmd.Parameters.AddWithValue("@IsActive", vm.IsActive);

        cmd.ExecuteNonQuery();
    }

    // Controller'ın beklediği isimler için wrapper (isim uyumsuzluğu çözümü)
    public AdminProductEditVM? GetProductForEdit(int id) => GetById(id);

    public void UpdateProduct(AdminProductEditVM vm) => Update(vm);

}
