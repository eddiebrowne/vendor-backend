﻿using System.Collections.Generic;

namespace Domain
{
  public interface IDatabase
  {
    int AddProduct(IProduct product);
    IProduct GetProduct(int id);
    int RemoveProduct(int id);
    IEnumerable<IProduct> GetProducts();
  }
}