using DogusCay.WebUI.DTOs.TalepFormDtos;
using DogusCay.WebUI.Models;

namespace DogusCay.WebUI.Mappers
{
    public static class TalepFormMappingExtensions
    {
        public static CreateTalepFormDto ToCreateDto(this TalepFormViewModel model)
        {
            return new CreateTalepFormDto
            {
                AppUserId = 9, // TODO: Giriş yapan kullanıcıdan alınmalı
                TalepTip = (TalepTip)model.TalepTip,
                KanalId = model.KanalId,
                DistributorId = model.DistributorId,
                PointGroupTypeId = model.PointGroupTypeId,
                PointId = model.PointId,
                ValidFrom = model.ValidFrom,
                ValidTo = model.ValidTo,
                Note = model.Note,
                Items = model.Items.Select(item => new CreateTalepFormItemDto
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    KoliIciAdet = item.KoliIciAdet,
                    ApproximateWeightKg = item.ApproximateWeightKg,
                    Iskonto1 = item.Iskonto1,
                    Iskonto2 = item.Iskonto2,
                    Iskonto3 = item.Iskonto3,
                    Iskonto4 = item.Iskonto4,
                    ValidFrom = item.ValidFrom,
                    ValidTo = item.ValidTo
                }).ToList()
            };
        }
    }
}
