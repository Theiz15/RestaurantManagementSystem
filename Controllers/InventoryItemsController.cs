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
            // Kiểm tra xem Inventory cha có tồn tại không
            var existingInventory = await _inventoryRepository.GetInventoryByIdAsync(inventoryId);
            if (existingInventory == null)
            {
                return NotFound($"Inventory with ID {inventoryId} not found.");
            }

            // 1. Tạo một đối tượng InventoryItem mới từ DTO
            var newInventoryItem = new InventoryItem
            {
                Name = itemDto.Name,
                Quantity = itemDto.Quantity,
                Unit = itemDto.Unit,
                InventoryId = inventoryId // 2. Gán InventoryId từ URL
            };

            // 3. Thêm đối tượng mới vào database
            await _inventoryRepository.AddInventoryItemAsync(newInventoryItem);

            // Trả về kết quả với đối tượng đã được tạo đầy đủ (bao gồm Id)
            return CreatedAtAction(nameof(GetInventoryItem), new { inventoryId = newInventoryItem.InventoryId, id = newInventoryItem.Id }, newInventoryItem);
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