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
            PageResponseDTO<ProductDTO> pageResponse = new();
            pageResponse.Data = data;
            pageResponse.TotalItems = totalItems;
            pageResponse.CurrentPage = position;
            pageResponse.PageSize = skip;
            pageResponse.HasPreviousPage = position > 1;
            int numOfPages=pageResponse.TotalItems/skip;
            if(pageResponse.TotalItems%skip!=0)
                numOfPages++;
            pageResponse.HasNextPage = position < numOfPages;
            return pageResponse;


        }
    }
}
