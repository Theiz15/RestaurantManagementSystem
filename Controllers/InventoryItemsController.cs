using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.DTOs;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Controllers
{
    [ApiController]
    [Route("api/inventories/{inventoryId}/items")]
    public class InventoryItemsController : ControllerBase
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryItemsController(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        // GET: api/inventories/{inventoryId}/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInventoryItems(int inventoryId)
        {
            var inventoryItems = await _inventoryRepository.GetInventoryItemsByInventoryIdAsync(inventoryId);
            if (inventoryItems == null || !inventoryItems.Any())
            {
                return NotFound($"No inventory items found for inventory ID: {inventoryId}");
            }
            return Ok(inventoryItems);
        }

        // GET: api/inventories/{inventoryId}/items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryItem>> GetInventoryItem(int inventoryId, int id)
        {
            var inventoryItem = await _inventoryRepository.GetInventoryItemByIdAsync(id);

            if (inventoryItem == null || inventoryItem.InventoryId != inventoryId)
            {
                return NotFound($"Inventory item with ID {id} not found in inventory {inventoryId}.");
            }

            return Ok(inventoryItem);
        }

        // POST: api/inventories/{inventoryId}/items
        [HttpPost]
        public async Task<ActionResult<InventoryItem>> AddInventoryItem(int inventoryId, [FromBody] InventoryItemCreateDTO itemDto)
        {
            // 1. Kiểm tra xem Inventory cha có tồn tại không
            var parentInventory = await _inventoryRepository.GetInventoryByIdAsync(inventoryId);
            if (parentInventory == null)
            {
                return NotFound($"Inventory with ID {inventoryId} not found.");
            }

            // 2. Tìm kiếm item đã tồn tại theo tên và inventoryId
            var existingItem = await _inventoryRepository.GetInventoryItemByNameAsync(inventoryId, itemDto.Name);

            if (existingItem != null)
            {
                // 3. NẾU TỒN TẠI: Cộng dồn số lượng và cập nhật
                existingItem.Quantity += itemDto.Quantity;
                await _inventoryRepository.UpdateInventoryItemAsync(existingItem);

                // Trả về 200 OK cùng với item đã được cập nhật
                return Ok(existingItem);
            }
            else
            {
                // 4. NẾU CHƯA TỒN TẠI: Tạo mới như bình thường
                var newInventoryItem = new InventoryItem
                {
                    Name = itemDto.Name,
                    Quantity = itemDto.Quantity,
                    Unit = itemDto.Unit,
                    InventoryId = inventoryId
                };
                await _inventoryRepository.AddInventoryItemAsync(newInventoryItem);

                // Trả về 201 Created
                return CreatedAtAction(nameof(GetInventoryItem), new { inventoryId = newInventoryItem.InventoryId, id = newInventoryItem.Id }, newInventoryItem);
            }
        }

        // PUT: api/inventories/{inventoryId}/items/{id}
        // Trong InventoryItemsController.cs

        // ...

        // PUT: api/inventories/{inventoryId}/items/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventoryItem(int inventoryId, int id, [FromBody] InventoryItemUpdateDTO itemDto)
        {
            // Lấy item hiện có từ database
            var existingItem = await _inventoryRepository.GetInventoryItemByIdAsync(id);

            // Kiểm tra xem item có tồn tại và có thuộc đúng Inventory cha không
            if (existingItem == null || existingItem.InventoryId != inventoryId)
            {
                return NotFound($"Inventory item with ID {id} not found in inventory {inventoryId}.");
            }

            // Cập nhật các thuộc tính của item hiện có từ DTO
            existingItem.Name = itemDto.Name;
            existingItem.Quantity = itemDto.Quantity;
            existingItem.Unit = itemDto.Unit;

            try
            {
                await _inventoryRepository.UpdateInventoryItemAsync(existingItem);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await InventoryItemExists(id))
                {
                    return NotFound($"Inventory item with ID {id} not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/inventories/{inventoryId}/items/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryItem(int inventoryId, int id)
        {
            var inventoryItem = await _inventoryRepository.GetInventoryItemByIdAsync(id);
            if (inventoryItem == null || inventoryItem.InventoryId != inventoryId)
            {
                return NotFound($"Inventory item with ID {id} not found in inventory {inventoryId}.");
            }

            await _inventoryRepository.DeleteInventoryItemAsync(id);
            return NoContent();
        }

        private async Task<bool> InventoryItemExists(int id)
        {
            return await _inventoryRepository.GetInventoryItemByIdAsync(id) != null;
        }
    }
}