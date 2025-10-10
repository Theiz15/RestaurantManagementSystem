using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RestaurantManagementSystem.DTOs.Requests;
using RestaurantManagementSystem.DTOs.Responses;
using RestaurantManagementSystem.Enums;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Repositories;

namespace RestaurantManagementSystem.Services.Impl
{
    public class ShiftAssignmentServiceImpl : ShiftAssignmentService
    {
        private readonly ShiftAssignmentRepository _shiftAssignmentRepository;
        private readonly IMapper _mapper;

        private readonly IUserRepository _userRepository;
        private readonly ShiftRepository _shiftRepository;

        public ShiftAssignmentServiceImpl(ShiftAssignmentRepository shiftAssignmentRepository, IMapper mapper, IUserRepository userRepository, ShiftRepository shiftRepository)
        {
            _shiftAssignmentRepository = shiftAssignmentRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _shiftRepository = shiftRepository;
        }
        public async Task Canceled(int userId, int assignmentId)
        {
            var shiftAssignment = _shiftAssignmentRepository.FindByUserIdAndAssignmentId(userId, assignmentId);

            if (shiftAssignment == null)
            {
                throw new System.Exception("Shift assignment not found!!!");
            };

            _shiftAssignmentRepository.Delete(shiftAssignment);
        }

        public async Task<ShiftAssignmentResponse> CheckIn(int assignmentId)
        {
            ShiftAssignment shiftAssignment = await _shiftAssignmentRepository.FindByIdAsync(assignmentId);
            // Shift shift = await _shiftRepository.FindByIdAsync(shiftAssignment.ShiftId);

            if (shiftAssignment == null)
            {
                throw new System.Exception("ShiftAssignment not found!!");
            }

            if (shiftAssignment.WorkDate != DateTime.Today || shiftAssignment.Shift.EndTime < DateTime.Now.TimeOfDay)
            {
                throw new System.Exception("Invalid date or shift has already ended. You can only check in on the scheduled day before shift ends.");
            }

            shiftAssignment.ActualStartTime = DateTime.Now;
            shiftAssignment.Status = ShiftStatus.InProgress;
            _shiftAssignmentRepository.Update(shiftAssignment);
            await _shiftAssignmentRepository.SaveChangesAsync();

            return _mapper.Map<ShiftAssignmentResponse>(shiftAssignment);
        }

        public async Task<ShiftAssignmentResponse> CheckOut(int assignmentId)
        {
            ShiftAssignment shiftAssignment = await _shiftAssignmentRepository.FindByIdAsync(assignmentId);

            if (shiftAssignment == null)
            {
                throw new System.Exception("ShiftAssignment not found!!");
            }

            if (shiftAssignment.WorkDate != DateTime.Today || shiftAssignment.Shift.EndTime < DateTime.Now.TimeOfDay)
            {
                throw new System.Exception("Invalid date or shift has already ended. You can only check in on the scheduled day before shift ends.");
            }

            shiftAssignment.ActualEndTime = DateTime.Now;
            shiftAssignment.Status = ShiftStatus.Completed;
            // Calculate hour worked
            TimeSpan duration = shiftAssignment.ActualEndTime.Value - shiftAssignment.ActualStartTime.Value;
            shiftAssignment.HoursWorked = (decimal)duration.TotalHours;

            _shiftAssignmentRepository.Update(shiftAssignment);
            await _shiftAssignmentRepository.SaveChangesAsync();

            return _mapper.Map<ShiftAssignmentResponse>(shiftAssignment);            
        }

        public async Task<IEnumerable<ShiftAssignmentResponse>> GetAllShiftAssignment(int userId)
        {
            var shiftAssignments = await _shiftAssignmentRepository.GetAllByUserIdAsync(userId);

            return _mapper.Map<IEnumerable<ShiftAssignmentResponse>>(shiftAssignments);

        }

        public async Task<IEnumerable<ShiftAssignmentResponse>> GetAllShiftAssignment()
        {
            var shiftAssignments = await _shiftAssignmentRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<ShiftAssignmentResponse>>(shiftAssignments);
        }

        public async Task<IEnumerable<ShiftAssignmentResponse>> FilterShiftAssignment(FilterShiftAssignmentRequest request)
        {
            var shiftAssignments = await _shiftAssignmentRepository.FilterShiftAssignment(request.userId, request.status);

            return _mapper.Map<IEnumerable<ShiftAssignmentResponse>>(shiftAssignments);
        }

        public async Task<ShiftAssignmentResponse> GetShiftAssignment(int userId, DateTime dateTime)
        {
            var shiftAssignment = _shiftAssignmentRepository.FindByUserIdAndDate(userId, dateTime);

            if (shiftAssignment == null)
            {
                throw new System.Exception("Shift assignment not found");
            }

            return _mapper.Map<ShiftAssignmentResponse>(shiftAssignment);
        }

        public async Task<ShiftAssignmentResponse> RegisterShift(ShiftAssignmentRequest request)
        {
            var user = await _userRepository.FindByIdAsync(request.UserId);

            if (user == null)
            {
                throw new System.Exception("User not found");
            }

            var shift =await _shiftRepository.FindByIdAsync(request.ShiftId);

            if (shift == null)
            {
                throw new System.Exception("Shift not found");
            }

            var ShiftAssignment = await _shiftAssignmentRepository.FindByUserIdAndShiftId(request.UserId, request.ShiftId, request.WorkDate);

            if (ShiftAssignment != null)
            {
                throw new System.Exception("You have already registered shift!");
            }

            var assignment = new ShiftAssignment
            {
                UserId = request.UserId,
                ShiftId = request.ShiftId,
                WorkDate = request.WorkDate,
                ActualStartTime = null,
                ActualEndTime = null,
                HoursWorked = 0,
                Status = ShiftStatus.Scheduled,
                User = user,
                Shift = shift
            };

            await _shiftAssignmentRepository.AddAsync(assignment);
            await _shiftAssignmentRepository.SaveChangesAsync();

            return _mapper.Map<ShiftAssignmentResponse>(assignment);
        }
    }
}