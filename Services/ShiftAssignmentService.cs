using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantManagementSystem.DTOs.Requests;
using RestaurantManagementSystem.DTOs.Responses;
using RestaurantManagementSystem.Enums;

namespace RestaurantManagementSystem.Services
{
    public interface ShiftAssignmentService
    {
        Task<ShiftAssignmentResponse> RegisterShift(ShiftAssignmentRequest request);
        Task<ShiftAssignmentResponse> GetShiftAssignment(int userId, DateTime dateTime);
        Task<IEnumerable<ShiftAssignmentResponse>> FilterShiftAssignment(FilterShiftAssignmentRequest request);
        Task<IEnumerable<ShiftAssignmentResponse>> GetAllShiftAssignment(int userId);
        Task<IEnumerable<ShiftAssignmentResponse>> GetAllShiftAssignment();
        Task<ShiftAssignmentResponse> CheckIn(int assignmentId);
        Task<ShiftAssignmentResponse> CheckOut(int assignmentId);

        Task Canceled(int userId, int assignmentId);
    }
}