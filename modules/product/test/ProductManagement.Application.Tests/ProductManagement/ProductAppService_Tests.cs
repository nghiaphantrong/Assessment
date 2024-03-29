﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace ProductManagement
{
    public class ProductAppService_Tests : ProductManagementApplicationTestBase
    {
        private readonly IProductAppService _productAppService;
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly ProductManagementTestData _testData;

        public ProductAppService_Tests()
        {
            _productAppService = GetRequiredService<IProductAppService>();
            _productRepository = GetRequiredService<IRepository<Product, Guid>>();
            _testData = GetRequiredService<ProductManagementTestData>();
        }

        [Fact]
        public async Task GetListPagedAsync()
        {
            var result = await _productAppService.GetListPagedAsync(new ProductQueryDto());

            result.Items.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task GetAsync()
        {
            var product = (await _productRepository.GetListAsync()).OrderBy(item => item.CreationTime).FirstOrDefault();

            var result = await _productAppService.GetAsync(product.Id);

            result.ShouldNotBeNull();
            result.Name.ShouldBe(_testData.ProductName1);
            result.Code.ShouldBe(_testData.ProductCode1);
            result.Price.ShouldBe(_testData.ProductPrice1);
            result.StockCount.ShouldBe(_testData.ProductStockCount1);
        }

        [Fact]
        public async Task CreateAsync()
        {
            var result = await _productAppService.CreateAsync(new CreateProductDto()
            {
                Code = "Code",
                Name = "Name",
                Branch = "Branch",
                Colour = "Colour",
                Price = 15,
                StockCount = 14
            });

            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task UpdateAsync()
        {
            var product = (await _productRepository.GetListAsync()).FirstOrDefault();

            var result = await _productAppService.UpdateAsync(product.Id, new UpdateProductDto()
            {
                Name = product.Name,
                Colour = product.Colour,
                Branch = product.Branch,
                ImageName = product.ImageName,

                Price = 15, //Value to update
                StockCount = 14 // Value to update
            });

            result.ShouldNotBeNull();
            result.Price.ShouldBe(15);
            result.StockCount.ShouldBe(14);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            var product = (await _productRepository.GetListAsync()).LastOrDefault();

            await _productAppService.DeleteAsync(product.Id);

            var result = await _productRepository.FindAsync(product.Id);

            result.ShouldBeNull();
        }

    }
}
