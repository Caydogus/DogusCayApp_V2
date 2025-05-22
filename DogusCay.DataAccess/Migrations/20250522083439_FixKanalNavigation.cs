using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DogusCay.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixKanalNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Bu migration dosyasında yanlışlıkla tekrar oluşturulmaya çalışılan tablolar vardı.
            // AspNetRoles, AspNetUsers vb. tablolar zaten veritabanında mevcutsa, yeniden oluşturulmamalıdır.
            // Bu nedenle bu Up metodunu boş bırakıyoruz ya da sadece gerekli ilişkileri düzeltmek için bırakılan işlemleri ekliyoruz.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Aynı şekilde, Down metodunda da silme işlemleri yapılmamalı.
            // Bu alan da boş bırakılabilir.
        }
    }
}
