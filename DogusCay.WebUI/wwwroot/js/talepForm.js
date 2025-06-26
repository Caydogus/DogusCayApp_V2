
function formatTL(value) {
    return Number(value).toLocaleString('tr-TR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
}

$(document).ready(function () {
    const apiBaseUrl = "https://localhost:7076/api";
    const token = document.getElementById("jwtToken")?.value;

    if (!token) {
        alert("Oturum süresi dolmuş olabilir. Lütfen yeniden giriş yapın.");
        return;
    }

    // Token'ı tüm jQuery isteklerine otomatik ekle
    $.ajaxSetup({
        headers: {
            "Authorization": "Bearer " + token
        }
    });

    $('#distributorDiv, #pointGroupDiv').hide();

    // Ana kategori yükle
    $.get(`${apiBaseUrl}/categories/MainCategories`, function (data) {
        const catSelect = $('#CategoryId');
        catSelect.empty().append('<option value="">Seçiniz</option>');
        data.forEach(cat => {
            catSelect.append(`<option value="${cat.categoryId}">${cat.categoryName}</option>`);
        });
    });

    $('#CategoryId').change(function () {
        const categoryId = $(this).val();
        $('#SubCategoryId').empty().append('<option value="">Yükleniyor...</option>');
        $('#SubSubCategoryId').empty().append('<option value="">Önce alt kategori seçiniz</option>');
        $('#ProductId').empty().append('<option value="">Ürün bekleniyor...</option>');

        if (categoryId) {
            $.get(`${apiBaseUrl}/categories/${categoryId}/children`, function (data) {
                const subSelect = $('#SubCategoryId');
                subSelect.empty().append('<option value="">Seçiniz</option>');
                data.forEach(sub => {
                    subSelect.append(`<option value="${sub.categoryId}">${sub.categoryName}</option>`);
                });
            });
        }
    });

    $('#SubCategoryId').change(function () {
        const subCategoryId = $(this).val();
        $('#SubSubCategoryId').empty().append('<option value="">Yükleniyor...</option>');
        $('#ProductId').empty().append('<option value="">Ürün bekleniyor...</option>');

        if (subCategoryId) {
            $.get(`${apiBaseUrl}/categories/${subCategoryId}/children`, function (data) {
                const subSubSelect = $('#SubSubCategoryId');
                subSubSelect.empty().append('<option value="">Seçiniz</option>');
                data.forEach(subsub => {
                    subSubSelect.append(`<option value="${subsub.categoryId}">${subsub.categoryName}</option>`);
                });
            });
        }
    });

    $('#SubSubCategoryId').change(function () {
        const subSubCategoryId = $(this).val();
        $('#ProductId').empty().append('<option value="">Yükleniyor...</option>');

        if (subSubCategoryId) {
            $.get(`${apiBaseUrl}/categories/${subSubCategoryId}/products`, function (data) {
                const productSelect = $('#ProductId');
                productSelect.empty().append('<option value="">Seçiniz</option>');
                data.forEach(p => {
                    productSelect.append(`<option value="${p.productId}">${p.productName}</option>`);
                });
            });
        }
    });

    $('#KanalId').change(function () {
        const kanalId = $(this).val();
        $('#DistributorId, #PointGroupTypeId, #PointId').empty().append('<option value="">Seçiniz</option>');

        if (kanalId === "4") {
            $('#distributorDiv').show();
            $('#pointGroupDiv').hide();

            $.get(`${apiBaseUrl}/distributors/by-kanal/${kanalId}`)
                .done(data => {
                    const select = $('#DistributorId');
                    data.forEach(d => {
                        select.append(`<option value="${d.distributorId}">${d.distributorName}</option>`);
                    });
                })
                .fail(() => alert("❌ Distributor verisi alınamadı."));
        } else if (kanalId === "5" || kanalId === "6") {
            $('#distributorDiv, #pointGroupDiv').hide();

            $.get(`${apiBaseUrl}/points/by-kanal/${kanalId}`)
                .done(data => {
                    const select = $('#PointId');
                    select.empty().append('<option value="">Seçiniz</option>');
                    data.forEach(p => {
                        select.append(`<option value="${p.pointId}">${p.pointName}</option>`);
                    });
                })
                .fail(() => alert("❌ Nokta verisi alınamadı."));
        } else {
            $('#distributorDiv, #pointGroupDiv').hide();
        }
    });

    $('#DistributorId').change(function () {
        const distributorId = $(this).val();
        $('#PointGroupTypeId, #PointId').empty().append('<option value="">Seçiniz</option>');

        if (distributorId) {
            $.get(`${apiBaseUrl}/pointgrouptypes/by-distributor/${distributorId}`)
                .done(data => {
                    const select = $('#PointGroupTypeId');
                    data.forEach(g => {
                        select.append(`<option value="${g.pointGroupTypeId}">${g.pointGroupTypeName}</option>`);
                    });
                    $('#pointGroupDiv').show();
                })
                .fail(() => alert("❌ Nokta grubu verisi alınamadı."));
        } else {
            $('#pointGroupDiv').hide();
        }
    });

    $('#PointGroupTypeId').change(function () {
        const groupId = $(this).val();
        const distributorId = $('#DistributorId').val();
        $('#PointId').empty().append('<option value="">Seçiniz</option>');

        if (groupId && distributorId) {
            $.get(`${apiBaseUrl}/points/by-group/${groupId}/distributor/${distributorId}`)
                .done(data => {
                    const select = $('#PointId');
                    data.forEach(p => {
                        select.append(`<option value="${p.pointId}">${p.pointName}</option>`);
                    });
                })
                .fail(() => alert("❌ Nokta verisi alınamadı."));
        }
    });

    $('#ProductId').change(function () {
        const productId = $(this).val();
        if (productId) {
            $.get(`${apiBaseUrl}/products/get-product-info/${productId}`)
                .done(data => {
                    $('#Price').val(data.price || '');
                    $('#KoliIciAdet').val(data.koliIciAdet || '');
                    $('#ApproximateWeightKg').val(data.approximateWeightKg || '');
                    $('#ErpCode').val(data.erpCode || '');
                    $('#Quantity').val(1);
                    hesaplaIskonto();
                })
                .fail(() => alert("❌ Ürün bilgisi alınamadı."));
        }
    });


    // ------------------ İskonto Hesaplama ------------------

    $('#Iskonto1, #Iskonto2, #Iskonto3, #Iskonto4').on('input', function () {
        const cleaned = $(this).val().replace(",", ".");
        $(this).val(cleaned);
    });

    $('#Price, #Quantity, #Iskonto1, #Iskonto2, #Iskonto3, #Iskonto4, #AdetFarkDonusuTL, #SabitBedelTL').on('input', hesaplaIskonto);

    function hesaplaIskonto() {
        const safeParse = sel => parseFloat($(sel).val()?.toString().replace(",", ".")) || 0;
        const price = safeParse('#Price');
        let quantity = safeParse('#Quantity');
        const isk1 = safeParse('#Iskonto1');
        const isk2 = safeParse('#Iskonto2');
        const isk3 = safeParse('#Iskonto3');
        const isk4 = safeParse('#Iskonto4');
        const adetFarkTL = safeParse('#AdetFarkDonusuTL');
        const sabitBedel = safeParse('#SabitBedelTL'); // 🔹 Eksik olan bu
        const approxKg = safeParse('#ApproximateWeightKg');
        const koliIci = parseInt($('#KoliIciAdet').val()) || 0;

        if (quantity < 1) {
            quantity = 1;
            // $('#Quantity').val(1);
        }

        let toplam = price * quantity;
        [isk1, isk2, isk3, isk4].forEach(isk => {
            if (isk > 0) toplam *= (100 - isk) / 100;
        });
        // 🔹 Sabit bedel düş
        if (sabitBedel > 0) {
            toplam -= sabitBedel;
            if (toplam < 0) toplam = 0;
        }
        //brut total hesapla
        const totalBrut = quantity * price;// ✅ YENİ
        $('#TotalBrut').val(totalBrut.toFixed(2)); // ✅ YENİ


        const koliIciToplamAdet = quantity * koliIci;
        const koliToplamAgirligi = (quantity * approxKg).toFixed(2);
        const listeFiyat = koliIci > 0 ? (price / koliIci).toFixed(2) : "0.00";
        let sonAdetFiyat = koliIciToplamAdet > 0 ? toplam / koliIciToplamAdet : 0;

        if (adetFarkTL > 0) {
            sonAdetFiyat -= adetFarkTL;
            toplam = sonAdetFiyat * koliIciToplamAdet;
        }

        // 🔢 Maliyet hesaplama (%)
        let maliyet = 0;
        if (totalBrut !== 0) {
            maliyet = ((totalBrut - toplam) / totalBrut) * 100;
        }
        $('#Total').val(toplam.toFixed(2));
        $('#KoliToplamAgirligiKg').val(koliToplamAgirligi);
        $('#KoliIciToplamAdet').val(koliIciToplamAdet);
        $('#ListeFiyat').val(listeFiyat);
        $('#SonAdetFiyati').val(sonAdetFiyat.toFixed(2));

        $('#TotalDisplay').val(formatTL(toplam));
        $('#KoliToplamAgirligiKgDisplay').val(koliToplamAgirligi);
        $('#KoliIciToplamAdetDisplay').val(koliIciToplamAdet);
        $('#ListeFiyatDisplay').val(formatTL(listeFiyat));
        $('#SonAdetFiyatiDisplay').val(formatTL(sonAdetFiyat));
        $('#BrutTotal').val(totalBrut.toFixed(2));
        $('#BrutTotalDisplay').val(formatTL(totalBrut));
        $('#Maliyet').val(maliyet.toFixed(2));
        $('#MaliyetDisplay').val(maliyet.toFixed(2));
    }

    // ------------------ Tarihler ------------------
    const today = new Date().toISOString().split('T')[0];
    const nextWeek = new Date(Date.now() + 7 * 24 * 60 * 60 * 1000).toISOString().split('T')[0];
    $('#ValidFrom').val(today);
    $('#ValidTo').val(nextWeek);
    // -------------------------- Form Submit (fetch) --------------------------

    $('#talepForm').on('submit', async function (e) {
        e.preventDefault();

        const dto = {
            kanalId: parseInt($('#KanalId').val()) || 0,
            distributorId: parseInt($('#DistributorId').val()) || null,
            pointGroupTypeId: parseInt($('#PointGroupTypeId').val()) || null,
            pointId: parseInt($('#PointId').val()) || null,
            categoryId: parseInt($('#CategoryId').val()) || 0,
            subCategoryId: parseInt($('#SubCategoryId').val()) || null,
            subSubCategoryId: parseInt($('#SubSubCategoryId').val()) || null,
            productId: parseInt($('#ProductId').val()) || 0,
            productName: $('#ProductId option:selected').text(),
            erpCode: $('#ErpCode').val(),
            approximateWeightKg: parseFloat($('#ApproximateWeightKg').val()) || 0,
            koliIciAdet: parseInt($('#KoliIciAdet').val()) || 0,
            sabitBedelTL: parseFloat($('#SabitBedelTL').val()) || 0,
            quantity: parseInt($('#Quantity').val()) || 1,
            price: parseFloat($('#Price').val()) || 0,
            iskonto1: parseFloat($('#Iskonto1').val()) || 0,
            iskonto2: parseFloat($('#Iskonto2').val()) || 0,
            iskonto3: parseFloat($('#Iskonto3').val()) || 0,
            iskonto4: parseFloat($('#Iskonto4').val()) || 0,
            koliToplamAgirligiKg: parseFloat($('#KoliToplamAgirligiKg').val()) || 0,
            koliIciToplamAdet: parseInt($('#KoliIciToplamAdet').val()) || 0,
            listeFiyat: parseFloat($('#ListeFiyat').val()) || 0,
            sonAdetFiyati: parseFloat($('#SonAdetFiyati').val()) || 0,
            adetFarkDonusuTL: parseFloat($('#AdetFarkDonusuTL').val()) || 0,
            validFrom: $('#ValidFrom').val(),
            validTo: $('#ValidTo').val(),
            note: $('#Note').val(),
            total: parseFloat($('#Total').val()) || 0,
            totalBrut: parseFloat($('#TotalBrut').val()) || 0
        };

        const validFrom = new Date($('#ValidFrom').val());
        const validTo = new Date($('#ValidTo').val());
        if (validTo < validFrom) {
            alert("🛑 Bitiş tarihi, başlangıç tarihinden önce olamaz!");
            return;
        }
   
        function validateForm(dto) {
            let errors = [];

            if (!dto.kanalId) errors.push("Kanal seçimi zorunludur.");
            if (!dto.pointId) errors.push("Nokta seçimi zorunludur.");
            if (!dto.productId) errors.push("Ürün seçimi zorunludur.");
            if (!dto.quantity || dto.quantity <= 0) errors.push("Geçerli bir adet girilmelidir.");

            return errors;
        }
        const errors = validateForm(dto);
        if (errors.length > 0) {
            alert("🛑 Lütfen aşağıdaki alanları doldurunuz:\n\n" + errors.join('\n'));
            return;
        }

        try {
            const res = await fetch(`${apiBaseUrl}/talepforms`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": "Bearer " + token
                },
                body: JSON.stringify(dto)
            });

            const resultText = await res.text();
            console.log("📤 API yanıtı:", resultText);

            if (res.ok) {
                alert("✅ Talep başarıyla oluşturuldu.");
                window.location.href = "/Admin/TalepForm/Index";
            } else {
                alert("❌ Talep kaydedilemedi:\n" + resultText);
            }
        } catch (err) {
            console.error("❌ JavaScript Catch Hatası:", err);
            alert("Bir hata oluştu: " + (err.message || "Bilinmeyen hata"));
        }
    });

});
