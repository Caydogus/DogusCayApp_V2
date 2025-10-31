const formatTL = value =>
    Number(value).toLocaleString('tr-TR', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });

$(document).ready(() => {
    const token = document.getElementById("jwtToken")?.value;
    if (!token) {
        alert("Oturum süresi dolmuş olabilir. Lütfen yeniden giriş yapın.");
        return;
    }

    $.ajaxSetup({
        headers: { "Authorization": `Bearer ${token}` }
    });

    // DOM elementleri
    const elements = {
        category: $('#CategoryId'),
        subCategory: $('#SubCategoryId'),
        subSubCategory: $('#SubSubCategoryId'),
        product: $('#ProductId'),
        kanal: $('#KanalId'),
        distributor: $('#DistributorId'),
        pointGroup: $('#PointGroupTypeId'),
        point: $('#PointId'),
        price: $('#Price'),
        oneriRafFiyati: $('#OneriRafFiyati'),
        oneriAksiyonFiyati: $('#OneriAksiyonFiyati'),
        quantity: $('#Quantity'),
        discounts: ['#Iskonto1', '#Iskonto2', '#Iskonto3', '#Iskonto4'],
        weight: $('#ApproximateWeightKg'),
        boxContent: $('#KoliIciAdet'),
        fixedCost: $('#SabitBedelTL'),
        aksiyonSatisFiyati: $('#AksiyonSatisFiyati'),
        adetFark: $('#AdetFarkDonusuTL'),
        validFrom: $('#ValidFrom'),
        validTo: $('#ValidTo'),
        aksiyonTipi: $('#AksiyonTipi'),
        indirimTipi: $('#IndirimTipi'),
        form: $('#talepForm')
    };

    // UI state yönetimi
    const uiState = {
        showElement: (selector, show) =>
            show ? $(selector).show() : $(selector).hide(),

        resetDropdown: (selector) =>
            $(selector).empty().append('<option value="">Seçiniz</option>'),

        setLoading: (selector) =>
            $(selector).html('<option value="">Yükleniyor...</option>')
    };

    // === Kategori zinciri ===
    const categoryManager = {
        loadMainCategories: () => {
            $.get(`${apiBaseUrl}categories/maincategories`, data => {
                uiState.resetDropdown(elements.category);
                data.forEach(cat => elements.category.append(
                    `<option value="${cat.categoryId}">${cat.categoryName}</option>`
                ));
            });
        },

        loadSubCategories: categoryId => {
            if (!categoryId) return;

            uiState.setLoading(elements.subCategory);
            uiState.resetDropdown(elements.subSubCategory);
            uiState.resetDropdown(elements.product);

            $.get(`${apiBaseUrl}categories/${categoryId}/children`, data => {
                uiState.resetDropdown(elements.subCategory);
                data.forEach(sub => elements.subCategory.append(
                    `<option value="${sub.categoryId}">${sub.categoryName}</option>`
                ));
            });
        },

        loadSubSubCategories: subCategoryId => {
            if (!subCategoryId) return;

            uiState.setLoading(elements.subSubCategory);
            uiState.resetDropdown(elements.product);

            $.get(`${apiBaseUrl}categories/${subCategoryId}/children`, data => {
                uiState.resetDropdown(elements.subSubCategory);
                data.forEach(subsub => elements.subSubCategory.append(
                    `<option value="${subsub.categoryId}">${subsub.categoryName}</option>`
                ));
            });
        },

        loadProducts: subSubCategoryId => {
            if (!subSubCategoryId) return;

            uiState.setLoading(elements.product);
            $.get(`${apiBaseUrl}categories/${subSubCategoryId}/products`, data => {
                uiState.resetDropdown(elements.product);
                data.forEach(p => elements.product.append(
                    `<option value="${p.productId}">${p.productName}</option>`
                ));
            });
        }
    };

    // === Kanal ve dağıtıcı zinciri ===
    const channelManager = {
        handleKanalChange: kanalId => {
            uiState.resetDropdown(elements.distributor);
            uiState.resetDropdown(elements.pointGroup);
            uiState.resetDropdown(elements.point);

            if (kanalId === "4") {
                // DIST
                $(elements.discounts[0]).val("7.5").trigger('input');
                uiState.showElement('#distributorDiv', true);
                uiState.showElement('#pointGroupDiv', false);
                channelManager.loadDistributors(kanalId);
            }
            else if (kanalId === "5") {
                // LC
                $(elements.discounts[0]).val("9.5").trigger('input');
                uiState.showElement('#distributorDiv', false);
                uiState.showElement('#pointGroupDiv', false);
                channelManager.loadPoints(kanalId);
            }
            else if (kanalId === "6") {
                // NA
                $(elements.discounts[0]).val("").trigger('input');
                uiState.showElement('#distributorDiv', false);
                uiState.showElement('#pointGroupDiv', false);
                channelManager.loadPoints(kanalId);
            }
            else {
                // Diğer kanallar
                $(elements.discounts[0]).val("").trigger('input');
                uiState.showElement('#distributorDiv', false);
                uiState.showElement('#pointGroupDiv', false);
            }
        },

        loadDistributors: kanalId => {
            $.get(`${apiBaseUrl}distributors/by-kanal/${kanalId}`)
                .done(data => {
                    data.forEach(d => elements.distributor.append(
                        `<option value="${d.distributorId}">${d.distributorName}</option>`
                    ));
                })
                .fail(() => alert("❌ Distributor verisi alınamadı."));
        },

        loadPoints: kanalId => {
            $.get(`${apiBaseUrl}points/by-kanal/${kanalId}`)
                .done(data => {
                    uiState.resetDropdown(elements.point);
                    data.forEach(p => elements.point.append(
                        `<option value="${p.pointId}">${p.pointName}</option>`
                    ));
                })
                .fail(() => alert("❌ Nokta verisi alınamadı."));
        },

        handleDistributorChange: distributorId => {
            uiState.resetDropdown(elements.pointGroup);
            uiState.resetDropdown(elements.point);

            if (distributorId) {
                $.get(`${apiBaseUrl}pointgrouptypes/by-distributor/${distributorId}`)
                    .done(data => {
                        data.forEach(g => elements.pointGroup.append(
                            `<option value="${g.pointGroupTypeId}">${g.pointGroupTypeName}</option>`
                        ));
                        uiState.showElement('#pointGroupDiv', true);
                    })
                    .fail(() => alert("❌ Nokta grubu verisi alınamadı."));
            } else {
                uiState.showElement('#pointGroupDiv', false);
            }
        },

        handlePointGroupChange: groupId => {
            const distributorId = elements.distributor.val();
            uiState.resetDropdown(elements.point);

            if (groupId && distributorId) {
                $.get(`${apiBaseUrl}points/by-group/${groupId}/distributors/${distributorId}`)
                    .done(data => {
                        data.forEach(p => elements.point.append(
                            `<option value="${p.pointId}">${p.pointName}</option>`
                        ));
                    })
                    .fail(() => alert("❌ Nokta verisi alınamadı."));
            }
        }
    };

    // === Ürün bilgisi ===
    const productManager = {
        loadProductInfo: productId => {
            if (!productId) return;

            $.get(`${apiBaseUrl}products/get-product-info/${productId}`)
                .done(data => {
                    elements.price.val(data.price || '');
                    elements.boxContent.val(data.koliIciAdet || '');
                    elements.weight.val(data.approximateWeightKg || '');
                    $('#ErpCode').val(data.erpCode || '');
                    elements.oneriRafFiyati.val(data.oneriRafFiyati || '');
                    elements.oneriAksiyonFiyati.val(data.oneriAksiyonFiyati || '');

                    elements.quantity.val(1);
                    discountCalculator.calculateDiscount();
                })
                .fail(() => alert("❌ Ürün bilgisi alınamadı."));
        }
    };

    // === İskonto hesaplamaları ===
    const discountCalculator = {
        parseInput: selector => {
            const value = $(selector).val()?.toString().replace(",", ".");
            return parseFloat(value) || 0;
        },

        calculateDiscount: () => {
            const parse = discountCalculator.parseInput;
            const price = parse(elements.price);
            let quantity = parse(elements.quantity);
            const discounts = elements.discounts.map(d => parse(d));
            const adetFarkTL = parse(elements.adetFark);
            const sabitBedel = parse(elements.fixedCost);
            const approxKg = parse(elements.weight);
            const koliIci = parseInt(elements.boxContent.val()) || 0;

            quantity = quantity < 1 ? 1 : quantity;

            const brutTotal = quantity * price;
            $('#BrutTotal').val(brutTotal.toFixed(2));
            $('#BrutTotalDisplay').val(formatTL(brutTotal));

            let netTotal = brutTotal;
            discounts.forEach(discount => {
                if (discount > 0) netTotal *= (100 - discount) / 100;
            });

            if (sabitBedel > 0) {
                netTotal -= sabitBedel;
                if (netTotal < 0) netTotal = 0;
            }

            const koliIciToplamAdet = quantity * koliIci;
            const koliToplamAgirligi = (quantity * approxKg).toFixed(2);
            const listeFiyat = koliIci > 0 ? (price / koliIci).toFixed(2) : "0.00";
            let sonAdetFiyat = koliIciToplamAdet > 0 ? netTotal / koliIciToplamAdet : 0;

            if (adetFarkTL > 0) {
                sonAdetFiyat -= adetFarkTL;
                netTotal = sonAdetFiyat * koliIciToplamAdet;
            }

            let maliyet = brutTotal !== 0
                ? ((brutTotal - netTotal) / brutTotal) * 100
                : 0;

            $('#Total').val(netTotal.toFixed(2));
            $('#KoliToplamAgirligiKg').val(koliToplamAgirligi);
            $('#KoliIciToplamAdet').val(koliIciToplamAdet);
            $('#ListeFiyat').val(listeFiyat);
            $('#SonAdetFiyati').val(sonAdetFiyat.toFixed(2));
            $('#TotalDisplay').val(formatTL(netTotal));
            $('#KoliToplamAgirligiKgDisplay').val(koliToplamAgirligi);
            $('#KoliIciToplamAdetDisplay').val(koliIciToplamAdet);
            $('#ListeFiyatDisplay').val(formatTL(listeFiyat));
            $('#SonAdetFiyatiDisplay').val(formatTL(sonAdetFiyat));
            $('#Maliyet').val(maliyet.toFixed(2));
            $('#MaliyetDisplay').val(maliyet.toFixed(2));
        }
    };

    // === Form submit ===
    const formHandler = {
        validate: dto => {
            const errors = [];
            if (!dto.kanalId) errors.push("Kanal seçimi zorunludur.");
            if (!dto.pointId) errors.push("Nokta seçimi zorunludur.");
            if (!dto.productId) errors.push("Ürün seçimi zorunludur.");
            if (!dto.aksiyonSatisFiyati || dto.aksiyonSatisFiyati <= 0) errors.push("Aksiyon satış fiyatı girilmelidir.");
            if (!dto.quantity || dto.quantity <= 0) errors.push("Geçerli bir adet girilmelidir.");
            if (!dto.aksiyonTipi) errors.push("Aksiyon tipi seçimi zorunludur.");//23.10.2025 eklendi
            if (!dto.indirimTipi) errors.push("İndirim tipi seçimi zorunludur.");//23.10.2025 eklendi
            const validFrom = new Date(dto.validFrom);
            const validTo = new Date(dto.validTo);
            if (validTo < validFrom) errors.push("Bitiş tarihi başlangıçtan önce olamaz!");

            return errors;
        },

        handleSubmit: async e => {
            e.preventDefault();

            const dto = {
                kanalId: parseInt(elements.kanal.val()) || 0,
                distributorId: parseInt(elements.distributor.val()) || null,
                pointGroupTypeId: parseInt(elements.pointGroup.val()) || null,
                pointId: parseInt(elements.point.val()) || null,
                categoryId: parseInt(elements.category.val()) || 0,
                subCategoryId: parseInt(elements.subCategory.val()) || null,
                subSubCategoryId: parseInt(elements.subSubCategory.val()) || null,
                productId: parseInt(elements.product.val()) || 0,
                productName: elements.product.find('option:selected').text(),
                erpCode: $('#ErpCode').val(),
                approximateWeightKg: parseFloat(elements.weight.val()) || 0,
                koliIciAdet: parseInt(elements.boxContent.val()) || 0,
                sabitBedelTL: parseFloat(elements.fixedCost.val()) || 0,
                aksiyonSatisFiyati: parseFloat(elements.aksiyonSatisFiyati.val()) || 0,
                quantity: parseInt(elements.quantity.val()) || 1,
                price: parseFloat(elements.price.val()) || 0,
                oneriRafFiyati: parseFloat(elements.oneriRafFiyati.val()) || 0,
                oneriAksiyonFiyati: parseFloat(elements.oneriAksiyonFiyati.val()) || 0,
                iskonto1: discountCalculator.parseInput(elements.discounts[0]),
                iskonto2: discountCalculator.parseInput(elements.discounts[1]),
                iskonto3: discountCalculator.parseInput(elements.discounts[2]),
                iskonto4: discountCalculator.parseInput(elements.discounts[3]),
                koliToplamAgirligiKg: parseFloat($('#KoliToplamAgirligiKg').val()) || 0,
                koliIciToplamAdet: parseInt($('#KoliIciToplamAdet').val()) || 0,
                listeFiyat: parseFloat($('#ListeFiyat').val()) || 0,
                sonAdetFiyati: parseFloat($('#SonAdetFiyati').val()) || 0,
                adetFarkDonusuTL: parseFloat(elements.adetFark.val()) || 0,
                validFrom: elements.validFrom.val(),
                validTo: elements.validTo.val(),
                aksiyonTipi: $('#AksiyonTipi').val(), //23.10.2025 eklendi
                indirimTipi: $('#IndirimTipi').val(), //23.10.2025 eklendi
                note: $('#Note').val(),
                total: parseFloat($('#Total').val()) || 0,
                brutTotal: parseFloat($('#BrutTotal').val()) || 0
            };

            const errors = formHandler.validate(dto);
            if (errors.length > 0) {
                alert(`Lütfen düzeltin:\n\n${errors.join('\n')}`);
                return;
            }

            try {
                const response = await fetch(`${apiBaseUrl}talepforms`, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": `Bearer ${token}`
                    },
                    body: JSON.stringify(dto)
                });

                if (response.ok) {
                    alert("Talep başarıyla oluşturuldu.");
                    window.location.href = "/Admin/TalepForm/Index";
                } else {
                    const errorText = await response.text();
                    alert(`❌ Talep kaydedilemedi:\n${errorText}`);
                }
            } catch (error) {
                console.error("API Hatası:", error);
                alert(`Bir hata oluştu: ${error.message}`);
            }
        }
    };

    // === Init ===
    const init = () => {
        $('#distributorDiv, #pointGroupDiv').hide();

        const today = new Date().toISOString().split('T')[0];
        const nextWeek = new Date(Date.now() + 7 * 24 * 60 * 60 * 1000).toISOString().split('T')[0];
        elements.validFrom.val(today);
        elements.validTo.val(nextWeek);

        elements.discounts.forEach(discount => {
            $(discount).on('input', function () {
                $(this).val($(this).val().replace(",", "."));
            });
        });

        elements.category.change(() => categoryManager.loadSubCategories(elements.category.val()));
        elements.subCategory.change(() => categoryManager.loadSubSubCategories(elements.subCategory.val()));
        elements.subSubCategory.change(() => categoryManager.loadProducts(elements.subSubCategory.val()));
        elements.kanal.change(() => channelManager.handleKanalChange(elements.kanal.val()));
        elements.distributor.change(() => channelManager.handleDistributorChange(elements.distributor.val()));
        elements.pointGroup.change(() => channelManager.handlePointGroupChange(elements.pointGroup.val()));
        elements.product.change(() => productManager.loadProductInfo(elements.product.val()));

        const discountInputs = [
            elements.price, elements.quantity, elements.adetFark, elements.fixedCost,
            ...elements.discounts.map(d => $(d))
        ];
        discountInputs.forEach(input => {
            input.on('input', discountCalculator.calculateDiscount);
        });

        elements.form.on('submit', formHandler.handleSubmit);

        categoryManager.loadMainCategories();
    };

    init();
});
