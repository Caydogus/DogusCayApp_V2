let addedProducts = [];
let globalJwtToken = null;

// Configuration
const CONFIG = {
    DISCOUNTS: {
        '4': 7.5,  // DIST
        '5': 9.5   // LC
    }
};

// DOM Ready
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
    window.addEventListener('resize', renderSepet);
});

// --- 1. KANAL → NOKTA Zinciri ---
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

    // Load channels
    $.get(`${apiBaseUrl}kanals/dropdown`, function (data) {
        const kanalSelect = $('#KanalId');
        kanalSelect.append(data.map(k => `<option value="${k.kanalId}">${k.kanalName}</option>`));
    });

    // Event handlers
    $('#step1').off('change', '#KanalId').on('change', '#KanalId', function () {
        const kanalId = $(this).val();
        $('#DistributorId').empty().append('<option value="">Seçiniz</option>');
        $('#PointGroupTypeId').empty().append('<option value="">Seçiniz</option>');
        $('#PointId').empty().append('<option value="">Seçiniz</option>');

        if (kanalId === "4") {
            $('#distributorDiv').show();
            $('#pointGroupDiv').hide();
            $.get(`${apiBaseUrl}distributors/by-kanal/${kanalId}`, function (data) {
                data.forEach(d => $('#DistributorId').append(`<option value="${d.distributorId}">${d.distributorName}</option>`));
            });
        } else if (kanalId) {
            $('#distributorDiv, #pointGroupDiv').hide();
            $.get(`${apiBaseUrl}points/by-kanal/${kanalId}`, function (data) {
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
            $.get(`${apiBaseUrl}pointgrouptypes/by-distributor/${distributorId}`, function (data) {
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
            $.get(`${apiBaseUrl}points/by-group/${groupId}/distributors/${distributorId}`, function (data) {
                data.forEach(p => $('#PointId').append(`<option value="${p.pointId}">${p.pointName}</option>`));
            });
        }
    });
}

// --- 2. ÜRÜN SEÇİMİ ---
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
            <div id="cartSummaryArea" class="mt-3"></div>
            <div class="text-right mt-3">
                <button type="button" class="btn btn-secondary" onclick="$('#step2').hide(); $('#step1').show();">Geri</button>
                <button type="button" class="btn btn-primary" onclick="goToStep3()">Devam</button>
            </div>
        </div>
    `);

    renderSepet();

    // Load main categories
    $.get(`${apiBaseUrl}categories/maincategories`, function (data) {
        $('#CategoryId').empty().append('<option value="">Seçiniz</option>');
        data.forEach(c => $('#CategoryId').append(`<option value="${c.categoryId}">${c.categoryName}</option>`));
    });

    // Product selection controls visibility
    function toggleProductSelectionControls(showManualSubCategory, showManualSubSubCategory, showManualProduct, showAddAllCheckbox) {
        $('#subCategoryDiv, #subSubCategoryDiv, #productSelectDiv, #quantityDiv, #addBtnDiv, #addAllProductsCheckboxDiv').hide();
        $('#addAllProductsCheckbox').prop('checked', false);

        if (showManualSubCategory) $('#subCategoryDiv').show();
        if (showManualSubSubCategory) $('#subSubCategoryDiv').show();
        if (showManualProduct) {
            $('#productSelectDiv, #quantityDiv, #addBtnDiv').show();
        }
        if (showAddAllCheckbox) $('#addAllProductsCheckboxDiv').show();
    }

    // Event handlers for product selection
    $('#step2').off('change', '#CategoryId').on('change', '#CategoryId', function () {
        const id = $(this).val();
        $('#SubCategoryId, #SubSubCategoryId, #ProductId').empty().append('<option value="">Seçiniz</option>');

        if (id) {
            toggleProductSelectionControls(true, false, false, false);
            $.get(`${apiBaseUrl}categories/${id}/children`, function (data) {
                data.forEach(sc => $('#SubCategoryId').append(`<option value="${sc.categoryId}">${sc.categoryName}</option>`));
            });
        } else {
            toggleProductSelectionControls(false, false, false, false);
        }
    });

    $('#step2').off('change', '#SubCategoryId').on('change', '#SubCategoryId', function () {
        const id = $(this).val();
        $('#SubSubCategoryId, #ProductId').empty().append('<option value="">Seçiniz</option>');

        if (id) {
            toggleProductSelectionControls(true, true, false, true);
            $.get(`${apiBaseUrl}categories/${id}/children`, function (data) {
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
            $.get(`${apiBaseUrl}categories/${id}/products`, function (data) {
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
        updateDefaultDiscountForChannel(kanalId);
    });
}

// --- SEPET YÖNETİMİ ---
function renderSepet() {
    if (window.innerWidth <= 767) {
        renderProductCards();
    } else {
        renderProductTable();
    }
}


function renderProductCards() {
    let html = '';
    if (addedProducts.length === 0) {
        html = `<div class="text-center text-muted">Henüz ürün eklenmedi.</div>`;
    } else {
        addedProducts.forEach((p, index) => {
            const calculations = calculateProductValues(p);
            html += `
                <div class="cart-product-card${calculations.errorMsg ? " card-error" : ""}" id="product-row-${p.productId}">
                    <div class="prd-title">${index + 1}. ${p.productName}</div>
                    <div class="prd-row">
                        <span class="prd-label">Liste Fiyatı:</span>
                        <span class="prd-value">${p.price !== undefined && p.price !== null ? formatTL(calculations.price) : '-'}</span>
                    </div>
                    <div class="prd-row">
                        <span class="prd-label">Tonaj:</span>
                        <span class="prd-value">${calculations.totalWeight}</span>
                    </div>
                    <div class="prd-row">
                        <span class="prd-label">Miktar (Koli):</span>
                        <button class="btn btn-sm btn-outline-secondary" type="button" onclick="updateProductQuantity(${p.productId}, -1)">-</button>
                        <input type="number" min="1" value="${p.quantity}" style="width:90px;display:inline-block;text-align:center;" onchange="setProductQuantity(${p.productId}, this.value)" />
                        <button class="btn btn-sm btn-outline-secondary" type="button" onclick="updateProductQuantity(${p.productId}, 1)">+</button>
                    </div>
                    <div class="prd-row">
                        <span class="prd-label">Net Adet Fiyatı:</span>
                        <span class="prd-value">${formatTL(calculations.netAdetFiyat)}</span>
                    </div>
                    <div class="prd-row">
                        <span class="prd-label">Brüt Tutar:</span>
                        <span class="prd-value">${formatTL(calculations.brutTutar)}</span>
                    </div>
                    <div class="prd-row">
                        <span class="prd-label">Net Tutar:</span>
                        <span class="prd-value">${formatTL(calculations.netTutar)}</span>
                    </div>
                    <div class="prd-row">
                        <span class="prd-label">Maliyet:</span>
                        <span class="prd-value">${calculations.brutTutar > 0 ? calculations.maliyet.toFixed(2) + ' %' : '-'}</span>
                    </div>
                    <div class="prd-row">
                        <span class="prd-label">Sabit Bedel:</span>
                        <input type="number" min="0" value="${p.fixedPrice || ''}" style="width:90px;display:inline-block;text-align:center;" onchange="setProductFixedPrice(${p.productId}, this.value)" />
                    </div>
                    <div class="prd-row">
                        <span class="prd-label">İskonto1 (%):</span>
                        <input type="number" min="0" max="100" value="${p.discount1 || 0}" style="width:90px;display:inline-block;text-align:center;" onchange="setProductDiscount(${p.productId}, this.value)" />
                    </div>
                    <div class="prd-row">
                        <span class="prd-label">İskonto2 (%):</span>
                        <input type="number" min="0" max="100" value="${p.discount2 || 0}" style="width:90px;display:inline-block;text-align:center;" onchange="setProductDiscount2(${p.productId}, this.value)" />
                    </div>
                    <div class="prd-row prd-actions">
                        <button class="btn btn-sm btn-danger" onclick="removeProduct(${p.productId})">Sil</button>
                    </div>
                    ${calculations.errorMsg ? `<div class="text-danger" style="font-size:12px; margin-top:4px;">${calculations.errorMsg}</div>` : ""}
                </div>
            `;
        });
    }
    $('#cartSummaryArea').html(html);
}

function renderProductTable() {
    let html = '';
    if (addedProducts.length === 0) {
        html = `<div class="text-center text-muted">Henüz ürün eklenmedi.</div>`;
    } else {
        html += `
        <div class="table-responsive">
        <table class="table table-striped table-hover" id="productTable">
            <thead>
                <tr>
                    <th>#</th>
                    <th>Ürün</th>

                    <th>Miktar(Koli)</th>
                    <th>Maliyet</th>
                    <th>Sabit Bedel</th>
                    <th>İskonto(%)</th>
                    <th>İskonto2(%)</th>
                    <th>Liste Fiyatı</th>
                    <th>Tonaj</th>
                    <th>Net Adet Fiyatı</th>
                    <th>Brüt Tutar</th>
                    <th>Net Tutar</th>
                   
                    <th></th>
                </tr>
            </thead>
            <tbody>
        `;

        addedProducts.forEach((p, index) => {
            const calculations = calculateProductValues(p);
            html += `
                <tr${calculations.errorMsg ? ' class="table-danger"' : ''} id="product-row-${p.productId}">
                    <td>${index + 1}</td>
                    <td style="min-width: 120px">${p.productName}</td>
                 
                    <td>
                        <button class="btn btn-sm btn-outline-secondary" type="button" onclick="updateProductQuantity(${p.productId}, -1)">-</button>
                        <input type="number" min="1" style="width:90px;display:inline-block;text-align:center;" value="${p.quantity}"
                            onchange="setProductQuantity(${p.productId}, this.value)" />
                        <button class="btn btn-sm btn-outline-secondary" type="button" onclick="updateProductQuantity(${p.productId}, 1)">+</button>
                    </td>
                        <td>${calculations.brutTutar > 0 ? calculations.maliyet.toFixed(2) + ' %' : '-'}</td>
                    <td>
                        <input type="number" min="0" placeholder="Sabit Bedel" value="${p.fixedPrice !== null && p.fixedPrice !== undefined ? p.fixedPrice : ''}"
                            style="width:90px;display:inline-block;text-align:center;"
                            onchange="setProductFixedPrice(${p.productId}, this.value)" />
                    </td>
                    <td>
                        <input type="number" min="0" max="100" value="${p.discount1 || 0}" style="width:90px;display:inline-block;text-align:center;"
                            onchange="setProductDiscount(${p.productId}, this.value)" />
                    </td>
                    <td>
                        <input type="number" min="0" max="100" value="${p.discount2 || 0}" style="width:60px;display:inline-block;text-align:center;"
                            onchange="setProductDiscount2(${p.productId}, this.value)" />
                    </td>
                    <td>${p.price !== undefined && p.price !== null ? formatTL(calculations.price) : '-'}</td>
                    <td>${calculations.totalWeight}</td>
                    <td>${formatTL(calculations.netAdetFiyat)}</td>
                    <td>${formatTL(calculations.brutTutar)}</td>
                    <td>${formatTL(calculations.netTutar)}</td>
                
                    <td>
                        <button class='btn btn-sm btn-danger' type="button" onclick='removeProduct(${p.productId})'>Sil</button>
                        ${calculations.errorMsg ? `<div class="text-danger" style="font-size:11px;">${calculations.errorMsg}</div>` : ""}
                    </td>
                </tr>
            `;
        });

        html += `
            </tbody>
        </table>
        </div>
        `;
    }
    $('#cartSummaryArea').html(html);
}

function calculateProductValues(p) {
    const discount1 = p.discount1 || 0;
    const discount2 = p.discount2 || 0;
    const quantity = p.quantity || 0;
    const price = p.price || 0;
    const fixedPrice = p.fixedPrice !== null && p.fixedPrice !== undefined && p.fixedPrice !== '' ? Number(p.fixedPrice) : null;
    const koliIciAdet = p.koliIciAdet || 1;

    const discountedPrice1 = price * (1 - discount1 / 100);
    const discountedPrice2 = discountedPrice1 * (1 - discount2 / 100);
    const totalWeight = (p.weight && quantity) ? (p.weight * quantity).toFixed(2) : '-';
    const brutTutar = price * quantity;
    let netTutar = discountedPrice2 * quantity;
    let errorMsg = "";

    if (fixedPrice !== null && !isNaN(fixedPrice)) {
        if (fixedPrice > netTutar) {
            errorMsg = "Sabit tutar net tutardan fazla olamaz!";
            netTutar = 0;
        } else {
            netTutar -= fixedPrice;
        }
    }

    let netAdetFiyat = (quantity > 0 && koliIciAdet > 0) ? (netTutar / (quantity * koliIciAdet)) : 0;
    if (netAdetFiyat < 0) {
        errorMsg = "Net adet fiyatı eksi olamaz!";
        netAdetFiyat = 0;
    }

    const maliyet = (brutTutar > 0) ? Number(((1 - (netTutar / brutTutar)) * 100).toFixed(2)) : 0;
    if (maliyet >= 100) {
        errorMsg = "Maliyet %100 veya daha fazla olamaz!";
    }

    return {
        price,
        totalWeight,
        brutTutar,
        netTutar,
        netAdetFiyat,
        maliyet,
        errorMsg
    };
}

// --- ÜRÜN EKLEME FONKSİYONLARI ---
async function addProduct() {
    const productId = $('#ProductId').val();
    const productName = $('#ProductId option:selected').text();
    const quantity = parseInt($('#Quantity').val());

    if (!productId || isNaN(quantity) || quantity < 1) {
        alert("Lütfen geçerli bir ürün ve miktar seçiniz.");
        return;
    }

    const productDetails = await fetchProductDetails(productId);
    addOrUpdateProductInCart(productId, productName, quantity, productDetails);
    renderSepet();
}

async function fetchProductDetails(productId) {
    try {
        const res = await fetch(`${apiBaseUrl}products/get-product-info/${productId}`, {
            headers: { "Authorization": "Bearer " + globalJwtToken }
        });

        if (res.ok) {
            const data = await res.json();
            return {
                price: data.price ?? null,
                weight: data.approximateWeightKg ?? null,
                koliIciAdet: data.koliIciAdet ?? 1,
                categoryName: data.categoryName ?? null,
                subCategoryName: data.parentCategoryName ?? null
            };
        }
    } catch (err) {
        console.warn("Ürün detayları alınamadı:", err);
    }

    return {
        price: null,
        weight: null,
        koliIciAdet: 1,
        categoryName: null,
        subCategoryName: null
    };
}

function addOrUpdateProductInCart(productId, productName, quantity, productDetails) {
    const existingProductIndex = addedProducts.findIndex(p => p.productId == productId);
    const kanalId = $('#KanalId').val();
    const defaultDiscount = CONFIG.DISCOUNTS[kanalId] || 0;

    if (existingProductIndex !== -1) {
        addedProducts[existingProductIndex].quantity += quantity;
    } else {
        addedProducts.push({
            productId: parseInt(productId),
            productName,
            quantity,
            price: productDetails.price,
            weight: productDetails.weight,
            koliIciAdet: productDetails.koliIciAdet,
            discount1: defaultDiscount,
            discount2: 0,
            fixedPrice: null,
            categoryName: productDetails.categoryName,
            subCategoryName: productDetails.subCategoryName,
            subSubCategoryName: null,
            _userChangedDiscount: false
        });
    }
}

async function addAllProductsFromCategory() {
    const selectedSubCategoryId = $('#SubCategoryId').val();
    if (!selectedSubCategoryId) {
        $('#addAllProductsCheckbox').prop('checked', false);
        return;
    }

    const kanalId = $('#KanalId').val();
    const defaultDiscount = CONFIG.DISCOUNTS[kanalId] || 0;

    try {
        if (!globalJwtToken) throw new Error("JWT token bulunamadı. Lütfen giriş yapın.");

        const spinner = $('<div id="loadingSpinner" class="loading-overlay">Ürünler ekleniyor...</div>');
        $('body').append(spinner);

        const response = await fetch(`${apiBaseUrl}categories/${selectedSubCategoryId}/products-recursive`, {
            headers: { "Authorization": "Bearer " + globalJwtToken }
        });

        if (!response.ok) {
            $('#addAllProductsCheckbox').prop('checked', false);
            throw new Error(`API hatası: ${response.status}`);
        }

        const products = await response.json();
        if (!products || products.length === 0) {
            $('#addAllProductsCheckbox').prop('checked', false);
            return;
        }

        const productIds = products.map(p => p.productId);
        const bulkResponse = await fetch(`${apiBaseUrl}products/get-multiple-info`, {
            method: "POST",
            headers: {
                "Authorization": "Bearer " + globalJwtToken,
                "Content-Type": "application/json"
            },
            body: JSON.stringify(productIds)
        });

        if (!bulkResponse.ok) {
            $('#addAllProductsCheckbox').prop('checked', false);
            throw new Error(`Toplu ürün API hatası: ${bulkResponse.status}`);
        }

        const detailedProducts = await bulkResponse.json();
        if (!detailedProducts || detailedProducts.length === 0) {
            $('#addAllProductsCheckbox').prop('checked', false);
            return;
        }

        for (const product of detailedProducts) {
            const exists = addedProducts.some(p => p.productId === product.productId);
            if (!exists) {
                addedProducts.push({
                    productId: product.productId,
                    productName: product.productName,
                    quantity: 1,
                    price: product.price ?? null,
                    weight: product.approximateWeightKg ?? null,
                    koliIciAdet: product.koliIciAdet ?? 1,
                    discount1: defaultDiscount,
                    discount2: 0,
                    fixedPrice: null,
                    categoryName: product.categoryName ?? null,
                    subCategoryName: product.parentCategoryName ?? null,
                    subSubCategoryName: null,
                    _userChangedDiscount: false
                });
            }
        }

        renderSepet();

    } catch (error) {
        console.warn("Toplu ürün ekleme hatası:", error.message);
        $('#addAllProductsCheckbox').prop('checked', false);
    } finally {
        $('#loadingSpinner').remove();
    }
}

// --- SEPET GÜNCELLEME FONKSİYONLARI ---
function setProductDiscount(productId, value) {
    let discount1 = parseFloat(value);
    if (isNaN(discount1) || discount1 < 0) discount1 = 0;
    if (discount1 > 100) discount1 = 100;

    const product = addedProducts.find(p => p.productId === productId);
    if (product) {
        product.discount1 = discount1;
        product._userChangedDiscount = true;
        renderSepet();
    }
}

function setProductDiscount2(productId, value) {
    let discount2 = parseFloat(value);
    if (isNaN(discount2) || discount2 < 0) discount2 = 0;
    if (discount2 > 100) discount2 = 100;

    const product = addedProducts.find(p => p.productId === productId);
    if (product) {
        product.discount2 = discount2;
        renderSepet();
    }
}

function setProductFixedPrice(productId, value) {
    const product = addedProducts.find(p => p.productId === productId);
    if (product) {
        product.fixedPrice = value !== '' ? Number(value) : null;
        renderSepet();
    }
}

function setProductQuantity(productId, value) {
    const qty = parseInt(value);
    if (isNaN(qty) || qty < 1) return;

    const product = addedProducts.find(p => p.productId === productId);
    if (product) {
        product.quantity = qty;
        renderSepet();
    }
}

function updateProductQuantity(id, change) {
    const product = addedProducts.find(p => p.productId === id);
    if (product) {
        product.quantity += change;
        if (product.quantity <= 0) {
            removeProduct(id);
        } else {
            renderSepet();
        }
    }
}

function removeProduct(id) {
    addedProducts = addedProducts.filter(p => p.productId !== id);
    renderSepet();
}

function clearAllProducts() {
    if (confirm("Sepetteki tüm ürünleri silmek istediğinizden emin misiniz?")) {
        addedProducts = [];
        renderSepet();
        alert("Sepet temizlendi.");
    }
}

function updateDefaultDiscountForChannel(kanalId) {
    const defaultDiscount = CONFIG.DISCOUNTS[kanalId] || 0;
    addedProducts.forEach(p => {
        if (!p._userChangedDiscount) {
            p.discount1 = defaultDiscount;
        }
    });
    renderSepet();
}

// --- 3. TALEP ÖZETİ ---
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

            <div class="form-group mt-3">
                <label for="Note">Not</label>
                <textarea id="Note" class="form-control" rows="3" placeholder="Opsiyonel not ekleyebilirsiniz..."></textarea>
            </div>

            <div class="text-right mt-3">
                <button class="btn btn-secondary" onclick="$('#step3').hide(); $('#step2').show();">Geri</button>
                <button class="btn btn-success" onclick="submitTalep()">Gönder</button>
            </div>
        </div>
    `);
}

// --- 4. TALEP GÖNDERME ---
async function submitTalep() {
    const dto = {
        kanalId: parseInt($('#KanalId').val()),
        distributorId: $('#DistributorId').val() ? parseInt($('#DistributorId').val()) : null,
        pointGroupTypeId: $('#PointGroupTypeId').val() ? parseInt($('#PointGroupTypeId').val()) : null,
        pointId: parseInt($('#PointId').val()),
        note: $('#Note').val() || null,
        malYuklemeTalepFormDetails: addedProducts.map(p => {
            const discount1 = p.discount1 || 0;
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
            const maliyet = (brutTutar > 0) ? Number(((1 - (netTutar / brutTutar)) * 100).toFixed(2)) : 0;
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
                totalQuantity: totalQuantity
            };
        })
    };

    try {
        const res = await fetch(`${apiBaseUrl}malyuklemetalepforms`, {
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
        alert("Sunucuya ulaşırken beklenmedik bir hata oluştu: " + err.message);
    }
}

// --- UTILITY FUNCTIONS ---
function formatTL(value) {
    return Number(value).toLocaleString('tr-TR', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
}
