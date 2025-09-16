using AutoMapper;
using DogusCay.Business.Abstract;
using DogusCay.DTO.DTOs.PaymentTypeDtos;
using DogusCay.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DogusCay.API.Controllers
{
    [Authorize]
    [Route("api/paymenttypes")]
    [ApiController]

    public class PaymentTypesController(IPaymentTypeService _paymentTypeService, IMapper _mapper) : ControllerBase
    {
   
        [HttpGet]
        public IActionResult Get()
        {
            var values = _paymentTypeService.TGetList();
            var coursePaymentTypes = _mapper.Map<List<ResultPaymentTypeDto>>(values);
            return Ok(coursePaymentTypes);
        }

        [HttpGet("{id}")]

        public IActionResult GetById(int id)
        {
            var value = _paymentTypeService.TGetById(id);
            return Ok(value);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _paymentTypeService.TDelete(id);
            return Ok("Ödeme tipi Silindi");
        }

        [HttpPost]
        public IActionResult Create(CreatePaymentTypeDto createPaymentTypeDto)
        {
            var newValue = _mapper.Map<PaymentType>(createPaymentTypeDto);
            _paymentTypeService.TCreate(newValue);
            return Ok(" Ödeme tipi Oluşturuldu");
        }

        [HttpPut]
        public IActionResult Update(UpdatePaymentTypeDto updatePaymentTypeDto)
        {
            var value = _mapper.Map<PaymentType>(updatePaymentTypeDto);
            _paymentTypeService.TUpdate(value);
            return Ok("Ödeme tipi Güncellendi");
        }

    }

}
