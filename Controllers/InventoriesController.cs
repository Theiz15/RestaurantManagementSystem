using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Controllers
{
    [ApiController]
    [Route("api/inventories")] // Route cho Inventories
    public class InventoriesController : ControllerBase
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoriesController(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        // GET: api/inventories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetInventories()
        {
            var inventories = await _inventoryRepository.GetAllInventoriesAsync();
            return Ok(inventories);
        }

        // GET: api/inventories/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Inventory>> GetInventory(int id)
        {
            var inventory = await _inventoryRepository.GetInventoryByIdAsync(id);
            if (inventory == null)
            {
                return NotFound($"Inventory with ID {id} not found.");
            }
            return Ok(inventory);
        }

        // POST: api/inventories
        [HttpPost]
        public async Task<ActionResult<Inventory>> AddInventory(Inventory inventory)
        {
            // Thiết lập LastUpdated khi tạo mới
            inventory.LastUpdated = DateTime.UtcNow;
            await _inventoryRepository.AddInventoryAsync(inventory);
            return CreatedAtAction(nameof(GetInventory), new { id = inventory.Id }, inventory);
        }

        // PUT: api/inventories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventory(int id, Inventory inventory)
        {
            if (id != inventory.Id)
            {
                return BadRequest("Inventory ID mismatch.");
            }

            var existingInventory = await _inventoryRepository.GetInventoryByIdAsync(id);
            if (existingInventory == null)
            {
                return NotFound($"Inventory with ID {id} not found.");
            }

            // Cập nhật các trường cần thiết, tránh ghi đè các trường không mong muốn
            existingInventory.ItemName = inventory.ItemName;
            existingInventory.Quantity = inventory.Quantity;
            existingInventory.Unit = inventory.Unit;
            existingInventory.MinThreshold = inventory.MinThreshold;
            existingInventory.LastUpdated = DateTime.UtcNow; // Cập nhật thời gian
            // Không cập nhật InventoryItems trực tiếp ở đây, vì đó là của InventoryItemsController

            try
            {
                await _inventoryRepository.UpdateInventoryAsync(existingInventory); // Cập nhật existingInventory
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await InventoryExists(id))
                {
                    return NotFound($"Inventory with ID {id} not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/inventories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            var inventory = await _inventoryRepository.GetInventoryByIdAsync(id);
            if (inventory == null)
            {
                return NotFound($"Inventory with ID {id} not found.");
            }

            await _inventoryRepository.DeleteInventoryAsync(id);
            return NoContent();
        }

        private async Task<bool> InventoryExists(int id)
        {
            return await _inventoryRepository.GetInventoryByIdAsync(id) != null;
        }
    }
}