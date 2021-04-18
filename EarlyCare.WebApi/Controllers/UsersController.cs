using AutoMapper;
using EarlyCare.Core.Interfaces;
using EarlyCare.Core.Models;
using EarlyCare.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EarlyCare.WebApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IOtpService _otpService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMapper mapper, ILogger<UsersController> logger, IUserService userService, IOtpService otpService)
        {
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
            _otpService = otpService;
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
                var userResponseModel = _mapper.Map<User, UserResponseModel>(user);
                return await PrepareResponse(false, otpDetails, verifyOTPRequestModel, userResponseModel);
            }
            else
            {
                return await PrepareResponse(true, otpDetails, verifyOTPRequestModel);
            }
        }

        [HttpPost("createUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestModel CreateUserModel)
        {
            var user = _mapper.Map<CreateUserRequestModel, User>(CreateUserModel);

            var response = await _userService.InsertUser(user);

            return Ok();
        }

        [HttpGet("getVolunteers")]
        public async Task<IActionResult> GetVolunteers()
        {
            var response = await _userService.GetVolunteers();

            return Ok(new BaseResponseModel { Status = 1, Result = response });
        }

        [HttpGet("getVolunteer")]
        public async Task<IActionResult> GetVolunteers(int volunteerId)
        {
            var response = await _userService.GetVolunteer(volunteerId);

            return Ok(response);
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


    }
}