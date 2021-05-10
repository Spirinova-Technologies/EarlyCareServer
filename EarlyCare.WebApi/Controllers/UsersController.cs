using AutoMapper;
using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
using EarlyCare.Infrastructure.Constants;
using EarlyCare.Infrastructure.SharedModels;
using EarlyCare.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EarlyCare.WebApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IOtpService _otpService;
        private readonly ILogger<UsersController> _logger;
        private readonly IEmailService _emailService;

        public UsersController(IMapper mapper, ILogger<UsersController> logger, IUserService userService,
            IOtpService otpService, IUserRepository userRepository, IEmailService emailService)
        {
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
            _otpService = otpService;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        [HttpPost("sendOtp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOTPRequestModel sendOTPRequestModel)
        {
            var isMessageSent = await _otpService.SendVerificationOtp(sendOTPRequestModel.MobileNumber);

            return Ok(new BaseResponseModel { Status = isMessageSent ? 1 : 0, Message = "OTP has been sent to your mobile" });
        }

        [HttpPost("verifyOtp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOTPRequestModel verifyOTPRequestModel)
        {
            var user = await _userService.GetUserByPhoneNumber(verifyOTPRequestModel.MobileNumber);

            var otpDetails = await _otpService.GetOtpDetails(verifyOTPRequestModel.MobileNumber, verifyOTPRequestModel.Otp);

            if (user != null)
            {
                var userServices = await _userRepository.GetUsersServices(user.Id);
                var response = _userService.GenerateUserResponse(user, userServices);

                return await PrepareResponse(false, otpDetails, verifyOTPRequestModel, response);
            }
            else
            {
                return await PrepareResponse(true, otpDetails, verifyOTPRequestModel);
            }
        }

        [HttpPost("createUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestModel createUserModel)
        {
            var isEmailIdExists = await _userRepository.IsEmailIdExists(createUserModel.Email);

            if (isEmailIdExists)
            {
                return Ok(new BaseResponseModel { Status = 0, Message = "Email id already exists" });
            }

            var userRequest = _mapper.Map<CreateUserRequestModel, User>(createUserModel);

            var insertedUser = await _userService.InsertUser(userRequest);

            //insert the User service mapping
            foreach (var service in createUserModel.Services)
            {
                await _userRepository.InsertUserServiceData(new UserServiceData { ServiceId = service, UserId = insertedUser.Id });
            }

            var userServices = await _userRepository.GetUsersServices(insertedUser.Id);
            var response = _userService.GenerateUserResponse(insertedUser, userServices);

            return Ok(new BaseResponseModel { Status = 1, Message = "User crated successfully", Result = response });
        }

        [HttpPost("updateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequestModel userRequestModel)
        {
            var user = await _userRepository.GetUserById(userRequestModel.Id);

            user.FullName = userRequestModel.FullName;
            user.UserType = userRequestModel.UserType;

            var insertedUser = await _userRepository.UpdateUser(user);

            //delete all the user service mapping
            await _userRepository.DeleteUserServiceMapping(userRequestModel.Id);

            //insert the User service mapping
            foreach (var service in userRequestModel.Services)
            {
                await _userRepository.InsertUserServiceData(new UserServiceData { ServiceId = service, UserId = insertedUser.Id });
            }

            var userServices = await _userRepository.GetUsersServices(insertedUser.Id);
            var response = _userService.GenerateUserResponse(insertedUser, userServices);

            return Ok(new BaseResponseModel { Status = 1, Message = "User updated successfully", Result = response });
        }

        [HttpGet("getVolunteers")]
        public async Task<IActionResult> GetVolunteers(int? userId)
        {
            var user = userId.HasValue ? await _userRepository.GetUserById(userId.Value) : null;

            var hasApprovePermission = user != null && (user.UserType == 1);

            var response = await _userService.GetVolunteers(hasApprovePermission);

            return Ok(new BaseResponseModel { Status = 1, Result = response });
        }

        [HttpGet("getVolunteer")]
        public async Task<IActionResult> GetVolunteers([Required] int volunteerId)
        {
            var response = await _userService.GetVolunteer(volunteerId);

            return Ok(response);
        }

        [HttpGet("getUserById")]
        public async Task<IActionResult> GetUserById([Required] int userId)
        {
            var user = await _userRepository.GetUserById(userId);

            var userServices = await _userRepository.GetUsersServices(user.Id);
            var response = _userService.GenerateUserResponse(user, userServices);

            return Ok(new BaseResponseModel { Status = 1, Result = response });
        }

        [NonAction]
        private async Task<IActionResult> PrepareResponse(bool isNewUser, OtpDetails otpDetails, VerifyOTPRequestModel verifyOTPRequestModel, UserResponseModel userResponseModel = null)
        {
            if (otpDetails == null)
            {
                return Ok(new VerifyOtpResponseModel
                {
                    IsNewUser = isNewUser,
                    IsOtpVerified = false,
                    Message = "Please enter valid otp",
                    Status = 0
                });
            }

            //update status of the old once it is verified
            await _otpService.UpdateOtpDetailsAsync(verifyOTPRequestModel.MobileNumber);

            return Ok(new VerifyOtpResponseModel
            {
                IsNewUser = isNewUser,
                IsOtpVerified = true,
                Status = 1,
                Message = "Otp verified successfully",
                Result = userResponseModel
            });
        }

        [HttpPost("updateVerificationStatus")]
        public async Task<IActionResult> UpdateVerificationStatus([FromBody] UpdateVerificationStatusModel statusRequestModel)
        {
            var response = await _userRepository.UpdateVerificationStatus(statusRequestModel);

            var subject = statusRequestModel.MarkVerified == true ? Constants.VolunteerApprovedEmailSubject : Constants.VolunteerUnapprovedEmailSubject;
            var body = statusRequestModel.MarkVerified == true ?
                string.Format(Constants.VolunteerApprovedEmailBody, response.FullName, response.ApprovedByUser) :
                string.Format(Constants.VolunteerUnapprovedEmailBody, response.FullName, response.ApprovedByUser);

            //send email
            await _emailService.SendUpdateNotification(response.Id, subject, body);

            return Ok(new BaseResponseModel { Message = "Status updated successfully", Status = 1 });
        }
    }
}