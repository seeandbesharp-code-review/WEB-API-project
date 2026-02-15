using AutoMapper;
using DTOs;
using Entities;
using Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<PageResponseDTO<ProductDTO>> GetProducts(int position, int skip, int?[] categoryIds,
            string? description, int? maxPrice, int? minPrice)
        {

            var (items, totalItems) = await _productRepository.GetProducts(position,skip,categoryIds,description,maxPrice,minPrice);
            List<ProductDTO> data = _mapper.Map<List<Product>, List<ProductDTO>>(items);
            int numOfPages = totalItems / skip;
            if (totalItems % skip != 0)
                numOfPages++;
            PageResponseDTO<ProductDTO> pageResponse = new(
             data,
            totalItems ,
            position,
            skip,
            position > 1,
            position < numOfPages
            );
            return pageResponse;
        }
        public async Task<ProductDTO> GetProductById(int id)
        {
            return _mapper.Map<Product, ProductDTO>(await _productRepository.GetProductById(id));
        }
        public async Task<ProductDTO> AddProduct(PostProductDTO product)
        {
            return _mapper.Map<Product, ProductDTO>(await _productRepository.AddProduct(_mapper.Map<PostProductDTO, Product>(product)));
        }

        public async Task UpdateProduct(int id, ProductDTO product)
        {
            await _productRepository.UpdateProduct(id, _mapper.Map<ProductDTO, Product>(product));
        }
    }
}
