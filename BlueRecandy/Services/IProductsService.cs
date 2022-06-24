﻿using BlueRecandy.Models;

namespace BlueRecandy.Services
{
	public interface IProductsService
	{

		bool IsProductExists(int id);

		Task<Product?> GetProductById(int? id);

		IQueryable<Product?> GetProductsIncludeOwner();

		IEnumerable<Product> GetProductsByOwner(string? ownerId);
	}
}
