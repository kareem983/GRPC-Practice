using Grpc.Core;
using Ordering.Inventory.Data;
using Ordering.Inventory.Protos;

namespace Ordering.Inventory.Services
{
    public class InventoryService : InventoryOrderItemService.InventoryOrderItemServiceBase
    {
        public override Task<OrderItemsResponse> ProcessOrderItems(OrderItemsRequest request, ServerCallContext context)
        {
            var database = new Database();
            var inventoryItems = database.GetItems();

            bool orderStatusSuccess = true;
            List<ItemResponse> itemsResponse = [];

            foreach (var item in request.Items)
            {
                var selectedItem = inventoryItems.Where(itm => itm.Id == item.ItemID).FirstOrDefault();
                
                var itemStatus = ItemExistingStatus.ItemAvailabe;

                if (selectedItem == null)
                {
                    itemStatus = ItemExistingStatus.ItemNotFound;
                    orderStatusSuccess = false;
                }
                else if (item.ItemQuantity > selectedItem.Quantity)
                {
                    itemStatus = ItemExistingStatus.ItemQuantityExceeded;
                    orderStatusSuccess = false;
                }

                itemsResponse.Add(new ItemResponse
                {
                    ItemId = item.ItemID,
                    ItemExistingStatus = itemStatus,
                });
            }

            var response  = new OrderItemsResponse
            {
                OrderStatusSuccess = orderStatusSuccess,
            };

            response.ItemsResponse.AddRange(itemsResponse);

            return Task.FromResult(response);
        }
    }
}
