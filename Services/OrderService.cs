using AutoMapper;
using DTOs;
using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _mapper = mapper;

        }
        public async Task<OrderDTO> GetOrderById(int id)
        {
            return _mapper.Map<Order, OrderDTO>(await _orderRepository.GetOrderById(id));
        }

        public async Task<OrderDTO> AddOrder(OrderDTO order)
        {
            if(await CheckOrderSum(order))
                return _mapper.Map<Order, OrderDTO>(await _orderRepository.AddOrder(_mapper.Map < OrderDTO, Order > (order)));
            return null;
        }

        private async Task<bool> CheckOrderSum(OrderDTO order)
        {
            double? sum = 0;
            foreach (var item in order.OrderItems)
            {
                Product product =await _productRepository.GetProductById(item.ProductId);
                if (product != null) 
                    sum += product.Price * item.Quantity;
            }
            if(sum==order.OrderSum)
                return true;
            return false;
        }
    }
}
