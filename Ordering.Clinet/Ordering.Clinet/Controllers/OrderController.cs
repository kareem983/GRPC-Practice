using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Ordering.Clinet.DTOs;
using Ordering.Inventory.Protos;
using Ordering.Payment.Protos;

namespace Ordering.Clinet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        #region Private Prperties
        private UserPaymentService.UserPaymentServiceClient _paymentServiceClient;
        private InventoryOrderItemService.InventoryOrderItemServiceClient _inventoryServiceClient;

        #endregion

        #region Public Nethods
        [HttpPost("MakeOrder")]
        public async Task<OrderResponseDTO> MakeOrder([FromBody] OrderRequestDTO orderRequest)
        {
            OrderResponseDTO orderResponse = new() { OrderID = orderRequest.OrderId };

            orderResponse.PaymentProcessResponse = await ProcessOrderPayment(orderRequest);

            orderResponse.InventoryProcessResponse = await ProcessOrderInventory(orderRequest);

            orderResponse.OrderFinalStatus = orderResponse.PaymentProcessResponse.PaymentStatus
                && orderResponse.InventoryProcessResponse.InventoryItemStatus;

            return await Task.FromResult(orderResponse);
        }

        #endregion

        #region Private Methods
        private UserPaymentService.UserPaymentServiceClient GetPaymentClient()
        {
            if (_paymentServiceClient is null)
            {
                var channel = GrpcChannel.ForAddress("https://localhost:7038");
                _paymentServiceClient = new(channel);
            }

            return _paymentServiceClient;
        }

        private InventoryOrderItemService.InventoryOrderItemServiceClient GetInventoryClient()
        {
            if (_inventoryServiceClient is null)
            {
                var channel = GrpcChannel.ForAddress("https://localhost:7249");
                _inventoryServiceClient = new(channel);
            }

            return _inventoryServiceClient;
        }

        private async Task<PaymentProcessResponseDTO> ProcessOrderPayment(OrderRequestDTO orderRequest)
        {
            var paymentClient = GetPaymentClient();

            var paymentRequest = new UserPaymentRequest
            {
                UserID = orderRequest.UserID,
                OrderBalance = CalculateOrderBalance(orderRequest.Items)
            };

            var paymentResponse = await paymentClient.ProcessUserPaymentAsync(paymentRequest);

            PaymentProcessResponseDTO paymentProcessResponse = new();
            switch (paymentResponse.PaymentStatus)
            {
                case PaymentResponseStatus.UserNotFound: paymentProcessResponse.PaymentMessage = "User Doesn't Exist"; break;
                case PaymentResponseStatus.Failed: paymentProcessResponse.PaymentMessage = "User Balance Dosen't Cover the all items"; break;
                case PaymentResponseStatus.Success:
                    paymentProcessResponse.PaymentMessage = "User Payment Done Successfuly";
                    paymentProcessResponse.PaymentStatus = true; break;
            }

            return await Task.FromResult(paymentProcessResponse);
        }

        private async Task<InventoryProcessResponseDTO?> ProcessOrderInventory(OrderRequestDTO orderRequest)
        {
            var inventoryClient = GetInventoryClient();

            var orderItems = orderRequest.Items.Select(itm => new Item { ItemID = itm.Id, ItemQuantity = itm.Quantity }).ToList();
            var orderItemsRequest = new OrderItemsRequest();
            orderItemsRequest.Items.AddRange(orderItems);

            var inventoryResponse = await inventoryClient.ProcessOrderItemsAsync(orderItemsRequest);

            string finalMessage = "";
            foreach (var itemRes in inventoryResponse.ItemsResponse)
            {
                finalMessage += $"ItemId: {itemRes.ItemId} => ItemStatus: {itemRes.ItemExistingStatus}  ";
            }

            InventoryProcessResponseDTO inventoryProcessResponse = new()
            {
                InventoryItemStatus = inventoryResponse.OrderStatusSuccess,
                InventoryItemMessage = finalMessage
            };

            return await Task.FromResult(inventoryProcessResponse);
        }

        private double CalculateOrderBalance(List<ItemDTO> items)
        {
            return items.Sum(itm => itm.Quantity * itm.Price);
        }

        #endregion

    }
}
