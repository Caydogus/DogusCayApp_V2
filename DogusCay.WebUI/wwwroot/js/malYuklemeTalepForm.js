const apiBaseUrl = "https://localhost:7076/api";
let addedProducts = [];
let globalJwtToken = null;

$(document).ready(function () {
    const tokenFromDom = document.getElementById("jwtToken")?.value;
    if (!tokenFromDom) {
        alert("Oturum süresi dolmuş olabilir veya JWT token bulunamadı. Lütfen yeniden giriş yapın.");
        console.error("JWT token bulunamadı.");
        return;
    }
    globalJwtToken = tokenFromDom;
    $.ajaxSetup({
        headers: { "Authorization": "Bearer " + globalJwtToken }
    });
    loadStep1();
});

function loadStep1() {
    $('#step1').html(`
        <div class="card p-4 shadow-sm mb-4">
            <h4>Kanal Seçimi</h4>
            <div class="row">
                <div class="form-group col-md-4">
                    <label for="KanalId">Kanal</label>
                    <select id="KanalId" class="form-control"><option value="">Seçiniz</option></select>
                </div>
                <div class="form-group col-md-4" id="distributorDiv" style="display:none;">
                    <label for="DistributorId">Distribütör</label>
                    <select id="DistributorId" class="form-control"></select>
                </div>
                <div class="form-group col-md-4" id="pointGroupDiv" style="display:none;">
                    <label for="PointGroupTypeId">Nokta Grubu</label>
                    <select id="PointGroupTypeId" class="form-control"></select>
                </div>
                <div class="form-group col-md-4">
                    <label for="PointId">Nokta</label>
                    <select id="PointId" class="form-control"></select>
                </div>
            </div>
            <div class="text-right">
                <button type="button" class="btn btn-primary" onclick="goToStep2()">Devam</button>
            </div>
        </div>
    `);

    $.get(`${apiBaseUrl}/kanals/dropdown`, function (data) {
        const kanalSelect = $('#KanalId');
        kanalSelect.append(data.map(k => `<option value="${k.kanalId}">${k.kanalName}</option>`));
    });

    $('#step1').off('change', '#KanalId').on('change', '#KanalId', function () {
        const kanalId = $(this).val();
        $('#DistributorId').empty().append('<option value="">Seçiniz</option>');
        $('#PointGroupTypeId').empty().append('<option value="">Seçiniz</option>');
        $('#PointId').empty().append('<option value="">Seçiniz</option>');
        if (kanalId === "4") {
            $('#distributorDiv').show();
            $('#pointGroupDiv').hide();
            $.get(`${apiBaseUrl}/distributors/by-kanal/${kanalId}`, function (data) {
                data.forEach(d => $('#DistributorId').append(`<option value="${d.distributorId}">${d.distributorName}</option>`));
            });
        } else if (kanalId) {
            $('#distributorDiv, #pointGroupDiv').hide();
            $.get(`${apiBaseUrl}/points/by-kanal/${kanalId}`, function (data) {
                data.forEach(p => $('#PointId').append(`<option value="${p.pointId}">${p.pointName}</option>`));
            });
        } else {
            $('#distributorDiv, #pointGroupDiv').hide();
        }
    });

    $('#step1').off('change', '#DistributorId').on('change', '#DistributorId', function () {
        const distributorId = $(this).val();
        $('#PointGroupTypeId').empty().append('<option value="">Seçiniz</option>');
        $('#PointId').empty().append('<option value="">Seçiniz</option>');
        if (distributorId) {
            $('#pointGroupDiv').show();
            $.get(`${apiBaseUrl}/pointgrouptypes/by-distributor/${distributorId}`, function (data) {
                data.forEach(g => $('#PointGroupTypeId').append(`<option value="${g.pointGroupTypeId}">${g.pointGroupTypeName}</option>`));
            });
        } else {
            $('#pointGroupDiv').hide();
        }
    });

    $('#step1').off('change', '#PointGroupTypeId').on('change', '#PointGroupTypeId', function () {
        const groupId = $(this).val();
        const distributorId = $('#DistributorId').val();
        $('#PointId').empty().append('<option value="">Seçiniz</option>');
        if (groupId && distributorId) {
            $.get(`${apiBaseUrl}/points/by-group/${groupId}/distributor/${distributorId}`, function (data) {
                data.forEach(p => $('#PointId').append(`<option value="${p.pointId}">${p.pointName}</option>`));
            });
        } else {
            $('#PointId').empty().append('<option value="">Seçiniz</option>');
        }
    });
}

function goToStep2() {
    const kanalId = $('#KanalId').val();
    const pointId = $('#PointId').val();
    if (!kanalId || !pointId) {
        alert("Lütfen Kanal ve Nokta seçiniz.");
        return;
    }

    $('#step1').hide();
    $('#step2').html(`
        <div class="card p-4 shadow-sm mb-4">
            <h4>Ürün Seçimi</h4>
            <div class="row">
                <div class="form-group col-md-3">
                    <label>Kategori</label>
                    <select id="CategoryId" class="form-control"></select>
                </div>
                <div class="form-group col-md-3" id="subCategoryDiv">
                    <label>Alt Kategori</label>
                    <select id="SubCategoryId" class="form-control"></select>
                </div>
                <div class="form-group col-md-3" id="subSubCategoryDiv">
                    <label>Marka</label>
                    <select id="SubSubCategoryId" class="form-control"></select>
                </div>
                <div class="form-group col-md-3" id="productSelectDiv">
                    <label>Ürün</label>
                    <select id="ProductId" class="form-control"></select>
                </div>
                <div class="form-group col-md-3" id="quantityDiv">
                    <label>Miktar (Koli)</label>
                    <input type="number" id="Quantity" class="form-control" value="1" min="1" />
                </div>
                <div class="form-group col-md-3" id="addBtnDiv">
                    <label>&nbsp;</label>
                    <button type="button" onclick="addProduct()" class="btn btn-success btn-block">Sepete Ekle</button>
                </div>
                <div class="form-group col-md-6 d-flex justify-content-end align-items-center" style="margin-top: 1.5rem;">
                    <div class="form-check mr-3" id="addAllProductsCheckboxDiv" style="display:none;">
                        <input class="form-check-input" type="checkbox" id="addAllProductsCheckbox">
                        <label class="form-check-label" for="addAllProductsCheckbox">
                            Bu kategori altındaki tüm ürünleri ekle
                        </label>
                    </div>
                    <button type="button" class="btn btn-danger" onclick="clearAllProducts()">Tümünü Temizle</button>
                </div>
            </div>
            <table class="table mt-3" id="productTable">
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Ürün</th>
                        <th>Liste Fiyatı</th>
                        <th>Tonaj</th>
                        <th>Miktar(Koli)</th>
                        <th>Brüt Tutar</th>
                        <th>Net Adet Fiyatı</th>
                        <th>Net Tutar</th>
                        <th>Maliyet</th>
                        <th>Sabit Fiyat</th>
                        <th>İskonto (%)</th>
                        <th>İskonto 2 (%)</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody></tbody>
            </table>
            <div class="text-right mt-3">
                <button type="button" class="btn btn-secondary" onclick="$('#step2').hide(); $('#step1').show();">Geri</button>
                <button type="button" class="btn btn-primary" onclick="goToStep3()">Devam</button>
            </div>
        </div>
    `);

    renderProductTable();

    $.get(`${apiBaseUrl}/categories/MainCategories`, function (data) {
        $('#CategoryId').empty().append('<option value="">Seçiniz</option>');
        data.forEach(c => $('#CategoryId').append(`<option value="${c.categoryId}">${c.categoryName}</option>`));
    });

    function toggleProductSelectionControls(showManualSubCategory, showManualSubSubCategory, showManualProduct, showAddAllCheckbox) {
        $('#subCategoryDiv, #subSubCategoryDiv, #productSelectDiv, #quantityDiv, #addBtnDiv, #addAllProductsCheckboxDiv').hide();
        $('#addAllProductsCheckbox').prop('checked', false);
        if (showManualSubCategory) { $('#subCategoryDiv').show(); }
        if (showManualSubSubCategory) { $('#subSubCategoryDiv').show(); }
        if (showManualProduct) {
            $('#productSelectDiv').show();
            $('#quantityDiv').show();
            $('#addBtnDiv').show();
        }
        if (showAddAllCheckbox) { $('#addAllProductsCheckboxDiv').show(); }
    }

    $('#step2').off('change', '#CategoryId').on('change', '#CategoryId', function () {
        const id = $(this).val();
        $('#SubCategoryId').empty().append('<option value="">Seçiniz</option>');
        $('#SubSubCategoryId').empty().append('<option value="">Seçiniz</option>');
        $('#ProductId').empty().append('<option value="">Seçiniz</option>');
        if (id) {
            toggleProductSelectionControls(true, false, false, false);
            $.get(`${apiBaseUrl}/categories/${id}/children`, function (data) {
                data.forEach(sc => $('#SubCategoryId').append(`<option value="${sc.categoryId}">${sc.categoryName}</option>`));
            });
        } else {
            toggleProductSelectionControls(false, false, false, false);
        }
    });

    $('#step2').off('change', '#SubCategoryId').on('change', '#SubCategoryId', function () {
        const id = $(this).val();
        $('#SubSubCategoryId').empty().append('<option value="">Seçiniz</option>');
        $('#ProductId').empty().append('<option value="">Seçiniz</option>');
        if (id) {
            toggleProductSelectionControls(true, true, false, true);
            $.get(`${apiBaseUrl}/categories/${id}/children`, function (data) {
                data.forEach(sc => $('#SubSubCategoryId').append(`<option value="${sc.categoryId}">${sc.categoryName}</option>`));
            });
        } else {
            toggleProductSelectionControls(true, false, false, false);
        }
    });

    $('#step2').off('change', '#SubSubCategoryId').on('change', '#SubSubCategoryId', function () {
        const id = $(this).val();
        $('#ProductId').empty().append('<option value="">Seçiniz</option>');
        if (id) {
            toggleProductSelectionControls(true, true, true, false);
            $.get(`${apiBaseUrl}/categories/${id}/products`, function (data) {
                data.forEach(p => $('#ProductId').append(`<option value="${p.productId}">${p.productName}</option>`));
            });
        } else {
            toggleProductSelectionControls(true, true, false, true);
        }
    });

    $('#step2').off('change', '#ProductId').on('change', '#ProductId', function () {
        const id = $(this).val();
        if (id) {
            toggleProductSelectionControls(true, true, true, false);
        } else {
            if ($('#SubSubCategoryId').val()) {
                toggleProductSelectionControls(true, true, true, false);
            } else if ($('#SubCategoryId').val()) {
                toggleProductSelectionControls(true, true, false, true);
            } else {
                toggleProductSelectionControls(true, false, false, false);
            }
        }
    });

    $('#step2').off('change', '#addAllProductsCheckbox').on('change', '#addAllProductsCheckbox', function () {
        if ($(this).is(':checked')) {
            $('#subSubCategoryDiv, #productSelectDiv, #quantityDiv, #addBtnDiv').hide();
            addAllProductsFromCategory();
        } else {
            if ($('#SubCategoryId').val()) {
                $('#subSubCategoryDiv, #productSelectDiv, #quantityDiv, #addBtnDiv').show();
            }
        }
    });
}

async function addProduct() {
    const productId = $('#ProductId').val();
    const productName = $('#ProductId option:selected').text();
    let quantity = parseInt($('#Quantity').val());
    if (!productId || isNaN(quantity) || quantity < 1) {
        alert("Lütfen geçerli bir ürün ve miktar seçiniz.");
        return;
    }
    let price = null;
    let weight = null;
    let koliIciAdet = 1;
    try {
        const res = await fetch(`${apiBaseUrl}/products/get-product-info/${productId}`, {
            headers: { "Authorization": "Bearer " + globalJwtToken }
        });
        if (res.ok) {
            const data = await res.json();
            price = data.price ?? null;
            weight = data.approximateWeightKg ?? null;
            koliIciAdet = data.koliIciAdet ?? 1; // <-- burada ekleniyor
        }
    } catch (err) {
        console.warn("Ürün detayları alınamadı:", err);
    }
    const existingProductIndex = addedProducts.findIndex(p => p.productId == productId);
    if (existingProductIndex !== -1) {
        addedProducts[existingProductIndex].quantity += quantity;
    } else {
        addedProducts.push({
            productId: parseInt(productId),
            productName: productName,
            quantity: quantity,
            price: price,
            weight: weight,
            koliIciAdet: koliIciAdet,
            discount: 0, // varsayılan iskonto
            discount2: 0,// 2. iskonto
            fixedPrice: null // Sabit Fiyat   

        });
    }
    renderProductTable();
}

async function addAllProductsFromCategory() {
    const selectedSubCategoryId = $('#SubCategoryId').val();
    if (!selectedSubCategoryId) {
        alert("Toplu ürün eklemek için lütfen önce bir Alt Kategori seçiniz.");
        $('#addAllProductsCheckbox').prop('checked', false);
        toggleProductSelectionControls(true, true, false, false);
        return;
    }
    try {
        if (!globalJwtToken) throw new Error("JWT token bulunamadı. Lütfen giriş yapın.");
        const response = await fetch(`${apiBaseUrl}/categories/${selectedSubCategoryId}/products-recursive`, {
            headers: { "Authorization": "Bearer " + globalJwtToken }
        });
        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(`API hatası: ${response.status} - ${errorText}`);
        }
        const products = await response.json();
        if (products.length === 0) {
            alert("Bu kategori altında eklenecek ürün bulunamadı.");
            $('#addAllProductsCheckbox').prop('checked', false);
            toggleProductSelectionControls(true, true, false, false);
            return;
        }
        let addedCount = 0;
        for (const product of products) {
            const existingProductIndex = addedProducts.findIndex(p => p.productId == product.productId);
            if (existingProductIndex === -1) {
                // Her ürün için detay çek
                let price = null;
                let weight = null;
                let koliIciAdet = 1;
                try {
                    const res = await fetch(`${apiBaseUrl}/products/get-product-info/${product.productId}`, {
                        headers: { "Authorization": "Bearer " + globalJwtToken }
                    });
                    if (res.ok) {
                        const data = await res.json();
                        price = data.price ?? null;
                        weight = data.approximateWeightKg ?? null;
                    }
                } catch { }
                addedProducts.push({
                    productId: product.productId,
                    productName: product.productName,
                    quantity: 1,
                    price: price,
                    weight: weight,
                    koliIciAdet: koliIciAdet // <-- burada ekleniyor

                });
                addedCount++;
            }
        }
        renderProductTable();
        alert(`${addedCount} ürün sepete eklendi.`);
    } catch (error) {
        alert("Tüm ürünleri çekerken bir hata oluştu: " + error.message);
        $('#addAllProductsCheckbox').prop('checked', false);
        toggleProductSelectionControls(true, true, false, false);
    }
}
function renderProductTable() {
    const tbody = $('#productTable tbody');
    tbody.empty();
    if (addedProducts.length === 0) {
        tbody.append('<tr><td colspan="13" class="text-center">Henüz ürün eklenmedi.</td></tr>');
    } else {
        addedProducts.forEach((p, index) => {
            const discount1 = p.discount || 0;
            const discount2 = p.discount2 || 0;
            const quantity = p.quantity || 0;
            const price = p.price || 0;
            const fixedPrice = p.fixedPrice !== null && p.fixedPrice !== undefined && p.fixedPrice !== '' ? Number(p.fixedPrice) : null;
            const koliIciAdet = p.koliIciAdet || 1;

            // İskontolu fiyatlar
            const discountedPrice1 = price * (1 - discount1 / 100);
            const discountedPrice2 = discountedPrice1 * (1 - discount2 / 100);

            // Hesaplamalar
            const totalWeight = (p.weight && quantity) ? (p.weight * quantity).toFixed(2) : '-';
            const brutTutar = price * quantity;
            let netTutar = discountedPrice2 * quantity;
            let errorMsg = "";

            // Sabit fiyat kontrolü
            if (fixedPrice !== null && !isNaN(fixedPrice)) {
                if (fixedPrice > netTutar) {
                    errorMsg = "Sabit tutar net tutardan fazla olamaz!";
                    netTutar = 0;
                } else {
                    netTutar = netTutar - fixedPrice;
                }
            }

            // Net Adet Fiyatı
            let netAdetFiyat = (quantity > 0 && koliIciAdet > 0) ? (netTutar / (quantity * koliIciAdet)) : 0;
            if (netAdetFiyat < 0) {
                errorMsg = "Net adet fiyatı eksi olamaz!";
                netAdetFiyat = 0;
            }

            // Maliyet kontrolü
            const maliyet = (brutTutar > 0) ? (1 - (netTutar / brutTutar)) : 0;
            if (maliyet >= 1) {
                errorMsg = "Maliyet %100 veya daha fazla olamaz!";
            }

            tbody.append(`
                <tr${errorMsg ? ' class="table-danger"' : ''}>
                    <td>${index + 1}</td>
                    <td>${p.productName}</td>
                    <td>${p.price !== undefined && p.price !== null ? formatTL(price) : '-'}</td>
                    <td>${totalWeight}</td>
                    <td>
                        <button class="btn btn-sm btn-outline-secondary" onclick="updateProductQuantity(${p.productId}, -1)">-</button>
                        <input type="number" min="1" style="width:60px;display:inline-block;text-align:center;" value="${quantity}" 
                            onchange="setProductQuantity(${p.productId}, this.value)" />
                        <button class="btn btn-sm btn-outline-secondary" onclick="updateProductQuantity(${p.productId}, 1)">+</button>
                    </td>
                    <td>${formatTL(brutTutar)}</td>
                    <td>${formatTL(netAdetFiyat)}</td>
                    <td>${formatTL(netTutar)}</td>
                    <td>${brutTutar > 0 ? (maliyet * 100).toFixed(2) + ' %' : '-'}</td>
                    <td>
                        <input type="number" min="0" placeholder="Sabit Fiyat" value="${p.fixedPrice !== null && p.fixedPrice !== undefined ? p.fixedPrice : ''}" 
                            style="width:90px;display:inline-block;text-align:center;"
                            onchange="setProductFixedPrice(${p.productId}, this.value)" />
                    </td>
                    <td>
                        <input type="number" min="0" max="100" value="${discount1}" style="width:60px;display:inline-block;text-align:center;"
                            onchange="setProductDiscount(${p.productId}, this.value)" /> %
                    </td>
                    <td>
                        <input type="number" min="0" max="100" value="${discount2}" style="width:60px;display:inline-block;text-align:center;"
                            onchange="setProductDiscount2(${p.productId}, this.value)" /> %
                    </td>
                    <td>
                        <button class='btn btn-sm btn-danger' onclick='removeProduct(${p.productId})'>Sil</button>
                        ${errorMsg ? `<div class="text-danger" style="font-size:11px;">${errorMsg}</div>` : ""}
                    </td>
                </tr>
            `);
        });
    }
}

//function renderProductTable() {
//    const tbody = $('#productTable tbody');
//    tbody.empty();
//    if (addedProducts.length === 0) {
//        tbody.append('<tr><td colspan="12" class="text-center">Henüz ürün eklenmedi.</td></tr>');
//    } else {
//        addedProducts.forEach((p, index) => {
//            const discount1 = p.discount || 0;
//            const discount2 = p.discount2 || 0;
//            const quantity = p.quantity || 0;
//            const price = p.price || 0;

//            // Hesaplamalar
//            const discountedPrice1 = price * (1 - discount1 / 100);
//            const discountedPrice2 = discountedPrice1 * (1 - discount2 / 100);

//            const totalWeight = (p.weight && quantity) ? (p.weight * quantity).toFixed(2) : '-';
//            const brutTutar = price * quantity;
//            const netTutar = discountedPrice2 * quantity;
//            const maliyet = (brutTutar > 0) ? (1 - (netTutar / brutTutar)) : 0;

//            tbody.append(`
//                <tr>
//                    <td>${index + 1}</td>
//                    <td>${p.productName}</td>
//                    <td>${p.price !== undefined && p.price !== null ? formatTL(price) : '-'}</td>
//                    <td>${totalWeight}</td>
//                    <td>
//                        <button class="btn btn-sm btn-outline-secondary" onclick="updateProductQuantity(${p.productId}, -1)">-</button>
//                        <input type="number" min="1" style="width:60px;display:inline-block;text-align:center;" value="${quantity}"
//                            onchange="setProductQuantity(${p.productId}, this.value)" />
//                        <button class="btn btn-sm btn-outline-secondary" onclick="updateProductQuantity(${p.productId}, 1)">+</button>
//                    </td>
//                    <td>${formatTL(brutTutar)}</td>
//                    <td>${formatTL(discountedPrice2)}</td>
//                    <td>${formatTL(netTutar)}</td>
//                    <td>${brutTutar > 0 ? (maliyet * 100).toFixed(2) + ' %' : '-'}</td>
//                    <td>
//                        <input type="number" min="0" max="100" value="${discount1}" style="width:60px;display:inline-block;text-align:center;"
//                            onchange="setProductDiscount(${p.productId}, this.value)" /> %
//                    </td>
//                    <td>
//                        <input type="number" min="0" max="100" value="${discount2}" style="width:60px;display:inline-block;text-align:center;"
//                            onchange="setProductDiscount2(${p.productId}, this.value)" /> %
//                    </td>
//                    <td><button class='btn btn-sm btn-danger' onclick='removeProduct(${p.productId})'>Sil</button></td>
//                </tr>
//            `);
//        });
//    }
//}

//iskonto1 fonksiyonu
//
function setProductDiscount(productId, value) {
    let discount = parseFloat(value);
    if (isNaN(discount) || discount < 0) discount = 0;
    if (discount > 100) discount = 100;
    const product = addedProducts.find(p => p.productId === productId);
    if (product) {
        product.discount = discount;
        renderProductTable();
    }
}

//iskonto2 fonksiyonu
function setProductDiscount2(productId, value) {
    let discount2 = parseFloat(value);
    if (isNaN(discount2) || discount2 < 0) discount2 = 0;
    if (discount2 > 100) discount2 = 100;
    const product = addedProducts.find(p => p.productId === productId);
    if (product) {
        product.discount2 = discount2;
        renderProductTable();
    }
}
function setProductFixedPrice(productId, value) {
    const product = addedProducts.find(p => p.productId === productId);
    if (product) {
        product.fixedPrice = value !== '' ? Number(value) : null;
        renderProductTable();
    }
}

function setProductQuantity(productId, value) {
    const qty = parseInt(value);
    if (isNaN(qty) || qty < 1) return;
    const product = addedProducts.find(p => p.productId === productId);
    if (product) {
        product.quantity = qty;
        renderProductTable();
    }
}

function removeProduct(id) {
    addedProducts = addedProducts.filter(p => p.productId !== id);
    renderProductTable();
}

function updateProductQuantity(id, change) {
    const product = addedProducts.find(p => p.productId === id);
    if (product) {
        product.quantity += change;
        if (product.quantity <= 0) {
            removeProduct(id);
        } else {
            renderProductTable();
        }
    }
}

function clearAllProducts() {
    if (confirm("Sepetteki tüm ürünleri silmek istediğinizden emin misiniz?")) {
        addedProducts = [];
        renderProductTable();
        alert("Sepet temizlendi.");
    }
}

function goToStep3() {
    if (addedProducts.length === 0) {
        alert("En az bir ürün ekleyin.");
        return;
    }
    $('#step2').hide();
    $('#step3').html(`
        <div class="card p-4 shadow-sm mb-4">
            <h4>Talebi Gönder</h4>
            <p>Toplam ${addedProducts.length} farklı ürün eklendi.</p>
            <div class="text-right">
                <button class="btn btn-secondary" onclick="$('#step3').hide(); $('#step2').show();">Geri</button>
                <button class="btn btn-success" onclick="submitTalep()">Gönder</button>
            </div>
        </div>
    `);
}

async function submitTalep() {
    const dto = {
        kanalId: parseInt($('#KanalId').val()),
        distributorId: $('#DistributorId').val() ? parseInt($('#DistributorId').val()) : null,
        pointGroupTypeId: $('#PointGroupTypeId').val() ? parseInt($('#PointGroupTypeId').val()) : null,
        pointId: parseInt($('#PointId').val()),
        malYuklemeTalepFormDetails: addedProducts.map(p => {
            const discount1 = p.discount || 0;
            const discount2 = p.discount2 || 0;
            const price = p.price || 0;
            const quantity = p.quantity || 0;
            const fixedPrice = p.fixedPrice !== undefined && p.fixedPrice !== null ? Number(p.fixedPrice) : null;
            const koliIciAdet = p.koliIciAdet || 1;

            const brutTutar = price * quantity;
            const discountedPrice1 = price * (1 - discount1 / 100);
            const discountedPrice2 = discountedPrice1 * (1 - discount2 / 100);
            let netTutar = discountedPrice2 * quantity;
            if (fixedPrice && !isNaN(fixedPrice)) {
                netTutar -= fixedPrice;
                if (netTutar < 0) netTutar = 0;
            }
            const netAdetFiyat = (quantity > 0 && koliIciAdet > 0) ? (netTutar / (quantity * koliIciAdet)) : 0;
            const maliyet = (brutTutar > 0) ? 1 - (netTutar / brutTutar) : 0;
            // Toplam adet (koli içi * koli sayısı)
            const totalQuantity = quantity * koliIciAdet;
            return {
                productId: p.productId,
                quantity: p.quantity,
                discount1: discount1,
                discount2: discount2,
                fixedPrice: fixedPrice,
                brutTutar: brutTutar,
                netTutar: netTutar,
                netAdetFiyat: netAdetFiyat,
                maliyet: maliyet,
                totalQuantity: totalQuantity // <-- EKLENDİ
            };
        })
    };
    try {
        const res = await fetch(`${apiBaseUrl}/MalYuklemeTalepForms`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + globalJwtToken
            },
            body: JSON.stringify(dto)
        });
        let result;
        try {
            result = await res.json();
        } catch {
            const text = await res.text();
            result = { message: text || res.statusText || "Sunucudan yanıt alınamadı." };
        }
        if (res.ok) {
            alert("✅ " + (result.message || "Talep başarıyla gönderildi!"));
            window.location.href = "/Admin/MalYuklemeTalepForm/Index";
        } else {
            alert(`❌ Hata (${res.status}): ${result.message || "Bilinmeyen bir hata oluştu."}`);
            if (res.status === 401) {
                alert("Oturumunuzun süresi dolmuş veya yetkiniz yok. Lütfen tekrar giriş yapın.");
            }
        }
    } catch (err) {
        alert("💥 Sunucuya ulaşırken beklenmedik bir hata oluştu: " + err.message);
    }
}

function formatTL(value) {
    return Number(value).toLocaleString('tr-TR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
}