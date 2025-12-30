//insert talep formu ve malyukleme talep formunu excele aktarırken sadece istediğimiz kolonların inmesini sağlamak için oluşturuldu.

namespace DogusCay.DTO.DTOs.Attributes
{

    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelIgnoreAttribute : Attribute
    {
    }
}
