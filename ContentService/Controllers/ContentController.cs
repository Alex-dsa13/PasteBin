using ContentService.Auth;
using ContentService.Models.Requests;
using ContentService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContentService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContentController : ControllerBase
    {
        private readonly IRecordService _recordService;
        private readonly IIdentityProvider _identityProvider;

        public ContentController(IRecordService recordService, IIdentityProvider identityProvider)
        {
            _recordService = recordService;
            _identityProvider = identityProvider;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateRecord(CreateRecordRequest request)
        {
            if (request == null) return BadRequest("request is null");

            request.SetUserId(_identityProvider.Current.UserId);

            var validateionData = request.IsRequestValid();
            if (!validateionData.valid) return BadRequest(validateionData.errorMessage);

            var record = await _recordService.CreateRecordAsync(request);

            return record == null
                ? BadRequest("can not create a record")
                : Ok(record);
        }

        [HttpGet]
        [Route("{id}/delete")]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            if (id <= 0) return BadRequest("id can not be 0 or less");

            await _recordService.DeleteRecordAsync(id);
            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetRecordById(int id)
        {
            if (id <= 0) return BadRequest("id can not be 0 or less");

            var record = await _recordService.GetRecordByIdAsync(id);
            if (record == null) return BadRequest("record is null");
            return Ok(record);
        }

        [HttpPost]
        [Route("user/getrecords")]
        public async Task<IActionResult> GetRecordsByUserId()
        {
            var userId = _identityProvider.Current.UserId;

            var records = await _recordService.GetRecordsByUserIdAsync(userId);
            return Ok(records);
        }

        [HttpPost]
        [Route("{id}/update")]
        public async Task<IActionResult> UpdateRecord(int id, UpdateRecordRequest request)
        {
            if (id <= 0) return BadRequest("id can not be 0 or less");
            if (request == null) return BadRequest("request is null");

            request.Id = id;
            var validateionData = request.IsRequestValid();
            if (!validateionData.valid) return BadRequest(validateionData.errorMessage);

            var updatedRecord = await _recordService.UpdateRecordAsync(request);
            return Ok(updatedRecord);
        }
    }
}
