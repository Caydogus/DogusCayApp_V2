//let globalJwtToken = null;
//let pivotGridOptions = null;
//let currentTableName = '';
//let currentTotalRow = null;  // 🆕 Toplam satırını global sakla

//// 🆕 TOPLANACAK SÜTUN İSİMLERİ
//const COLUMNS_TO_SUM = [
//    "CIRO",
//    "FIYAT",
//    "NET",
//    "BRUT",
//    "KATILIM",
//    "BUTCE",
//    "TUTAR",
//    "TOPLAM",
//    "ALTI",
//    "KALAN",
//    "PRICE",
//    "ALINANHIZMET",
//    "SAYISI"
//];

//document.addEventListener("DOMContentLoaded", function () {

//    const tokenFromDom = document.getElementById("jwtToken")?.value;
//    globalJwtToken = tokenFromDom;

//    if (!globalJwtToken) {
//        alert("JWT Token bulunamadı. Lütfen tekrar giriş yapın.");
//        return;
//    }

//    document
//        .getElementById("btnLoadPivot")
//        .addEventListener("click", loadPivotData);

//    // Excel'e aktar butonu - Grid'den XLSX olarak export
//    document
//        .getElementById("btnExportExcel")
//        .addEventListener("click", async function () {

//            if (!pivotGridOptions?.api) {
//                alert("Önce verileri yükleyin.");
//                return;
//            }

//            try {
//                const tableName = currentTableName || "Pivot";
//                const today = new Date().toISOString().slice(0, 10);

//                // SheetJS kütüphanesini dinamik yükle
//                if (typeof XLSX === 'undefined') {
//                    const script = document.createElement('script');
//                    script.src = 'https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.18.5/xlsx.full.min.js';
//                    document.head.appendChild(script);

//                    await new Promise((resolve, reject) => {
//                        script.onload = resolve;
//                        script.onerror = reject;
//                    });
//                }

//                // Grid'den tüm satırları al (normal satırlar)
//                const allRows = [];
//                pivotGridOptions.api.forEachNode(node => {
//                    if (!node.group) {
//                        allRows.push(node.data);
//                    }
//                });

//                // Toplam satırını ekle
//                if (currentTotalRow) {
//                    allRows.push(currentTotalRow);
//                }

//                // XLSX çalışma kitabı oluştur
//                const worksheet = XLSX.utils.json_to_sheet(allRows);
//                const workbook = XLSX.utils.book_new();
//                XLSX.utils.book_append_sheet(workbook, worksheet, "Pivot");

//                // Dosyayı indir
//                XLSX.writeFile(workbook, `${tableName}_${today}.xlsx`);

//            } catch (error) {
//                console.error("Excel export hatası:", error);
//                alert("Excel dosyası oluşturulurken hata oluştu.");
//            }
//        });

//});

//// PIVOT DATA YÜKLEME
//async function loadPivotData() {

//    const selectedTable = document.getElementById("tableSelector").value;

//    if (!selectedTable) {
//        alert("Lütfen bir tablo seçiniz.");
//        return;
//    }

//    currentTableName = selectedTable;

//    const payload = {
//        tableName: selectedTable
//    };

//    try {

//        console.log("📤 API POST:", `${apiBaseUrl}pivots/get`, payload);

//        const response = await fetch(`${apiBaseUrl}pivots/get`, {
//            method: "POST",
//            headers: {
//                "Content-Type": "application/json",
//                "Authorization": "Bearer " + globalJwtToken
//            },
//            body: JSON.stringify(payload)
//        });

//        const raw = await response.text();
//        console.log("🔥 API RAW RESPONSE:", raw);

//        let data;
//        try {
//            data = JSON.parse(raw);
//        } catch (err) {
//            console.error("❌ JSON parse hatası:", err);
//            alert("API JSON yerine farklı veri döndürdü. Konsola bak.");
//            return;
//        }

//        if (!Array.isArray(data) || data.length === 0) {
//            alert("Bu tablo için veri bulunamadı.");
//            return;
//        }

//        const columnDefs = buildColumnDefs(data);
//        renderPivotGrid(columnDefs, data);

//    } catch (error) {
//        console.error("Pivot API hata:", error);
//        alert("Sunucu hatası! Pivot verisi alınamadı.");
//    }
//}


//// DİNAMİK KOLON OLUŞTURMA
//function buildColumnDefs(data) {
//    const firstRow = data[0];

//    const moneyKeywords = [
//        "CIRO", "FIYAT", "NET", "BRUT", "KATILIM",
//        "BUTCE", "TUTAR", "TOPLAM", "ALTI", "KALAN", "PRICE", "ALINANHIZMET", "SAYISI"
//    ];

//    const monthColumns = [
//        "OCAK", "ŞUBAT", "SUBAT", "MART", "NİSAN", "NISAN",
//        "MAYIS", "HAZİRAN", "HAZIRAN", "TEMMUZ", "AĞUSTOS", "AGUSTOS",
//        "EYLÜL", "EYLUL", "EKİM", "EKIM", "KASIM", "ARALIK"
//    ];

//    return Object.keys(firstRow).map(key => {
//        const upperKey = key.toUpperCase();

//        const isMoney =
//            moneyKeywords.some(k => upperKey.includes(k)) ||
//            monthColumns.some(m => upperKey === m);

//        return {
//            field: key,
//            sortable: true,
//            filter: true,
//            resizable: true,
//            enableRowGroup: true,
//            enablePivot: true,
//            enableValue: true,

//            valueFormatter: isMoney
//                ? (params) => {
//                    if (params.value == null || params.value === "") return "";
//                    const num = Number(params.value);
//                    if (isNaN(num)) return params.value;
//                    return num.toLocaleString("tr-TR", {
//                        minimumFractionDigits: 0,
//                        maximumFractionDigits: 0
//                    });
//                }
//                : undefined
//        };
//    });
//}

//// 🆕 TOPLAM SATIRI HESAPLAMA
//function calculateTotalRow(columnDefs, rowData) {
//    const totalRow = {};

//    columnDefs.forEach((colDef, index) => {
//        const field = colDef.field;
//        const upperField = field.toUpperCase();

//        // İlk kolona "TOPLAM" yaz
//        if (index === 0) {
//            totalRow[field] = "TOPLAM";
//            return;
//        }

//        // Sadece belirlenen sütunları topla
//        const shouldSum = COLUMNS_TO_SUM.some(col =>
//            upperField.includes(col.toUpperCase())
//        );

//        if (shouldSum) {
//            let sum = 0;
//            let hasNumericValue = false;

//            rowData.forEach(row => {
//                const value = row[field];
//                const num = Number(value);
//                if (!isNaN(num) && value !== null && value !== "") {
//                    sum += num;
//                    hasNumericValue = true;
//                }
//            });

//            totalRow[field] = hasNumericValue ? sum : "";
//        } else {
//            // Toplanmayacak sütunlar boş
//            totalRow[field] = "";
//        }
//    });

//    return totalRow;
//}


//// AG-GRID PIVOT RENDER
//function renderPivotGrid(columnDefs, rowData) {

//    const gridDiv = document.querySelector("#pivotGrid");

//    if (pivotGridOptions?.api) {
//        pivotGridOptions.api.destroy();
//    }

//    // 🆕 Grid container'a dinamik yükseklik ver
//    gridDiv.style.height = 'calc(100vh - 200px)';  // Sayfa yüksekliği - üst alan
//    gridDiv.style.width = '100%';

//    // 🆕 Toplam satırını hesapla ve global'de sakla
//    const totalRow = calculateTotalRow(columnDefs, rowData);
//    currentTotalRow = totalRow;

//    pivotGridOptions = {
//        columnDefs,
//        rowData,

//        // 🆕 Alt kısma sabitlenmiş toplam satırı
//        pinnedBottomRowData: [totalRow],

//        pivotMode: false,

//        rowGroupPanelShow: 'always',

//        sideBar: {
//            toolPanels: [
//                {
//                    id: 'columns',
//                    labelDefault: 'Sütunlar',
//                    iconKey: 'columns',
//                    toolPanel: 'agColumnsToolPanel'
//                }
//            ],
//            defaultToolPanel: 'columns'
//        },

//        animateRows: true,

//        suppressDragLeaveHidesColumns: false,

//        getRowStyle: params => {
//            // 🆕 Toplam satırı için özel stil
//            if (params.node.rowPinned) {
//                return {
//                    backgroundColor: "#ffd966",
//                    fontWeight: "bold",
//                    borderTop: "2px solid #333"
//                };
//            }

//            if (params.node.rowIndex % 2 === 0) {
//                return { backgroundColor: "#f0f2f5" };
//            }
//            return { backgroundColor: "#ffffff" };
//        },

//        defaultColDef: {
//            sortable: true,
//            filter: true,
//            resizable: true,
//            minWidth: 50,
//            enableRowGroup: true,
//            enableValue: true,
//            headerClass: 'modern-header-cell'
//        },

//        localeText: {
//            contains: 'İçerir',
//            notContains: 'İçermez',
//            equals: 'Eşittir',
//            notEqual: 'Eşit Değil',
//            startsWith: 'İle Başlar',
//            endsWith: 'İle Biter',
//            blank: 'Boş',
//            notBlank: 'Boş Değil',
//            filterOoo: 'Filtre...',
//            applyFilter: 'Uygula',
//            clearFilter: 'Temizle',
//            andCondition: 'Ve',
//            orCondition: 'Veya',
//            reset: 'Sıfırla',
//            searchOoo: 'Ara...',
//            selectAll: 'Tümünü Seç',
//            noMatches: 'Eşleşme Yok',
//            columns: 'Sütunlar',
//            filters: 'Filtreler',
//            pinColumn: 'Sütunu Sabitle',
//            autosizeThiscolumn: 'Bu Sütunu Otomatik Genişlet',
//            autosizeAllColumns: 'Tüm Sütunları Otomatik Genişlet',
//            groupBy: 'Grupla',
//            ungroupBy: 'Gruptan Çıkar',
//            rowGroupColumnsEmptyMessage: 'Gruplamak için sütun buraya sürükleyin',
//            valuesColumnsEmptyMessage: 'Özet için sütun buraya sürükleyin',
//            pivotColumnsEmptyMessage: 'Pivot için sütun buraya sürükleyin'
//        }
//    };

//    new agGrid.Grid(gridDiv, pivotGridOptions);
//}

//let globalJwtToken = null;
//let pivotGridOptions = null;
//let currentTableName = '';
//let currentTotalRow = null;  // 🆕 Toplam satırını global sakla

//// 🆕 TOPLANACAK SÜTUN İSİMLERİ
//const COLUMNS_TO_SUM = [
//    "CIRO",
//    "FIYAT",
//    "NET",
//    "BRUT",
//    "KATILIM",
//    "BUTCE",
//    "TUTAR",
//    "TOPLAM",
//    "ALTI",
//    "KALAN",
//    "PRICE",
//    "ALINANHIZMET",
//    "SAYISI"
//];

//document.addEventListener("DOMContentLoaded", function () {

//    const tokenFromDom = document.getElementById("jwtToken")?.value;
//    globalJwtToken = tokenFromDom;

//    if (!globalJwtToken) {
//        alert("JWT Token bulunamadı. Lütfen tekrar giriş yapın.");
//        return;
//    }

//    document
//        .getElementById("btnLoadPivot")
//        .addEventListener("click", loadPivotData);

//    // Excel'e aktar butonu - Grid'den XLSX olarak export
//    document
//        .getElementById("btnExportExcel")
//        .addEventListener("click", async function () {

//            if (!pivotGridOptions?.api) {
//                alert("Önce verileri yükleyin.");
//                return;
//            }

//            try {
//                const tableName = currentTableName || "Pivot";
//                const today = new Date().toISOString().slice(0, 10);

//                // SheetJS kütüphanesini dinamik yükle
//                if (typeof XLSX === 'undefined') {
//                    const script = document.createElement('script');
//                    script.src = 'https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.18.5/xlsx.full.min.js';
//                    document.head.appendChild(script);

//                    await new Promise((resolve, reject) => {
//                        script.onload = resolve;
//                        script.onerror = reject;
//                    });
//                }

//                // Grid'den tüm satırları al (normal satırlar)
//                const allRows = [];
//                pivotGridOptions.api.forEachNode(node => {
//                    if (!node.group) {
//                        allRows.push(node.data);
//                    }
//                });

//                // Toplam satırını ekle
//                if (currentTotalRow) {
//                    allRows.push(currentTotalRow);
//                }

//                // XLSX çalışma kitabı oluştur
//                const worksheet = XLSX.utils.json_to_sheet(allRows);
//                const workbook = XLSX.utils.book_new();
//                XLSX.utils.book_append_sheet(workbook, worksheet, "Pivot");

//                // Dosyayı indir
//                XLSX.writeFile(workbook, `${tableName}_${today}.xlsx`);

//            } catch (error) {
//                console.error("Excel export hatası:", error);
//                alert("Excel dosyası oluşturulurken hata oluştu.");
//            }
//        });

//});

//// 🆕 KOLON GÖSTER/GİZLE PANELİ OLUŞTUR
//function createColumnTogglePanel() {
//    // Panel container
//    const panelContainer = document.createElement('div');
//    panelContainer.id = 'columnTogglePanel';
//    panelContainer.style.cssText = `
//        position: absolute;
//        top: 10px;
//        right: 20px;
//        z-index: 1000;
//        font-family: Arial, sans-serif;
//    `;

//    // Buton
//    const toggleButton = document.createElement('button');
//    toggleButton.id = 'columnToggleBtn';
//    toggleButton.innerHTML = '📋 Kolonları Göster/Gizle ▼';
//    toggleButton.style.cssText = `
//        background: #4CAF50;
//        color: white;
//        border: none;
//        padding: 10px 20px;
//        border-radius: 5px;
//        cursor: pointer;
//        font-size: 14px;
//        box-shadow: 0 2px 5px rgba(0,0,0,0.2);
//    `;

//    // Dropdown panel
//    const dropdownPanel = document.createElement('div');
//    dropdownPanel.id = 'columnDropdown';
//    dropdownPanel.style.cssText = `
//        display: none;
//        position: absolute;
//        top: 45px;
//        right: 0;
//        background: white;
//        border: 1px solid #ddd;
//        border-radius: 5px;
//        box-shadow: 0 4px 10px rgba(0,0,0,0.2);
//        max-height: 400px;
//        overflow-y: auto;
//        min-width: 250px;
//        padding: 10px;
//    `;

//    // Buton tıklama olayı
//    toggleButton.addEventListener('click', function (e) {
//        e.stopPropagation();
//        const isVisible = dropdownPanel.style.display === 'block';
//        dropdownPanel.style.display = isVisible ? 'none' : 'block';
//        toggleButton.innerHTML = isVisible ? '📋 Kolonları Göster/Gizle ▼' : '📋 Kolonları Göster/Gizle ▲';
//    });

//    panelContainer.appendChild(toggleButton);
//    panelContainer.appendChild(dropdownPanel);

//    // Grid div'in parent'ına ekle
//    const gridDiv = document.querySelector("#pivotGrid");
//    if (gridDiv && gridDiv.parentElement) {
//        gridDiv.parentElement.style.position = 'relative';
//        gridDiv.parentElement.appendChild(panelContainer);
//    }

//    // Panel dışına tıklanınca kapat
//    document.addEventListener('click', function (e) {
//        if (!panelContainer.contains(e.target)) {
//            dropdownPanel.style.display = 'none';
//            toggleButton.innerHTML = '📋 Kolonları Göster/Gizle ▼';
//        }
//    });
//}

//// 🆕 KOLON LİSTESİNİ GÜNCELLE
//function updateColumnTogglePanel(columnDefs) {
//    const dropdown = document.getElementById('columnDropdown');
//    if (!dropdown) return;

//    dropdown.innerHTML = '';

//    // Başlık
//    const header = document.createElement('div');
//    header.style.cssText = `
//        font-weight: bold;
//        margin-bottom: 10px;
//        padding-bottom: 5px;
//        border-bottom: 2px solid #4CAF50;
//        color: #333;
//    `;
//    header.textContent = 'Kolonları Seçin:';
//    dropdown.appendChild(header);

//    columnDefs.forEach(colDef => {
//        const checkboxContainer = document.createElement('label');
//        checkboxContainer.style.cssText = `
//            display: flex;
//            align-items: center;
//            padding: 8px;
//            cursor: pointer;
//            border-radius: 3px;
//            transition: background 0.2s;
//        `;
//        checkboxContainer.onmouseover = () => checkboxContainer.style.background = '#f0f0f0';
//        checkboxContainer.onmouseout = () => checkboxContainer.style.background = 'transparent';

//        const checkbox = document.createElement('input');
//        checkbox.type = 'checkbox';
//        checkbox.checked = true;
//        checkbox.style.cssText = `
//            margin-right: 10px;
//            cursor: pointer;
//            width: 16px;
//            height: 16px;
//        `;

//        const label = document.createElement('span');
//        label.textContent = colDef.field;
//        label.style.cssText = `
//            font-size: 13px;
//            color: #333;
//        `;

//        checkbox.addEventListener('change', function () {
//            if (pivotGridOptions?.api) {
//                const columnApi = pivotGridOptions.columnApi || pivotGridOptions.api;

//                if (columnApi.setColumnVisible) {
//                    columnApi.setColumnVisible(colDef.field, this.checked);
//                } else if (columnApi.setColumnsVisible) {
//                    columnApi.setColumnsVisible([colDef.field], this.checked);
//                }
//            }
//        });

//        checkboxContainer.appendChild(checkbox);
//        checkboxContainer.appendChild(label);
//        dropdown.appendChild(checkboxContainer);
//    });

//    // Tümünü seç/kaldır butonları
//    const buttonContainer = document.createElement('div');
//    buttonContainer.style.cssText = `
//        margin-top: 10px;
//        padding-top: 10px;
//        border-top: 1px solid #ddd;
//        display: flex;
//        gap: 5px;
//    `;

//    const selectAllBtn = document.createElement('button');
//    selectAllBtn.textContent = 'Tümünü Seç';
//    selectAllBtn.style.cssText = `
//        flex: 1;
//        padding: 6px;
//        background: #4CAF50;
//        color: white;
//        border: none;
//        border-radius: 3px;
//        cursor: pointer;
//        font-size: 12px;
//    `;
//    selectAllBtn.addEventListener('click', function (e) {
//        e.stopPropagation();
//        dropdown.querySelectorAll('input[type="checkbox"]').forEach(cb => {
//            cb.checked = true;
//            const colField = cb.parentElement.querySelector('span').textContent;
//            if (pivotGridOptions?.api) {
//                const columnApi = pivotGridOptions.columnApi || pivotGridOptions.api;
//                if (columnApi.setColumnVisible) {
//                    columnApi.setColumnVisible(colField, true);
//                } else if (columnApi.setColumnsVisible) {
//                    columnApi.setColumnsVisible([colField], true);
//                }
//            }
//        });
//    });

//    const deselectAllBtn = document.createElement('button');
//    deselectAllBtn.textContent = 'Tümünü Kaldır';
//    deselectAllBtn.style.cssText = `
//        flex: 1;
//        padding: 6px;
//        background: #f44336;
//        color: white;
//        border: none;
//        border-radius: 3px;
//        cursor: pointer;
//        font-size: 12px;
//    `;
//    deselectAllBtn.addEventListener('click', function (e) {
//        e.stopPropagation();
//        dropdown.querySelectorAll('input[type="checkbox"]').forEach(cb => {
//            cb.checked = false;
//            const colField = cb.parentElement.querySelector('span').textContent;
//            if (pivotGridOptions?.api) {
//                const columnApi = pivotGridOptions.columnApi || pivotGridOptions.api;
//                if (columnApi.setColumnVisible) {
//                    columnApi.setColumnVisible(colField, false);
//                } else if (columnApi.setColumnsVisible) {
//                    columnApi.setColumnsVisible([colField], false);
//                }
//            }
//        });
//    });

//    buttonContainer.appendChild(selectAllBtn);
//    buttonContainer.appendChild(deselectAllBtn);
//    dropdown.appendChild(buttonContainer);
//}

//// 🆕 TEK BİR CHECKBOX'I SENKRONIZE ET
//function syncCheckboxWithColumn(colId, isVisible) {
//    const dropdown = document.getElementById('columnDropdown');
//    if (!dropdown) return;

//    const checkboxes = dropdown.querySelectorAll('input[type="checkbox"]');
//    checkboxes.forEach(cb => {
//        const label = cb.parentElement.querySelector('span');
//        if (label && label.textContent === colId) {
//            cb.checked = isVisible;
//        }
//    });
//}

//// 🆕 TÜM CHECKBOX'LARI SENKRONIZE ET
//function syncAllCheckboxes() {
//    if (!pivotGridOptions?.api) return;

//    const dropdown = document.getElementById('columnDropdown');
//    if (!dropdown) return;

//    const checkboxes = dropdown.querySelectorAll('input[type="checkbox"]');
//    checkboxes.forEach(cb => {
//        const colId = cb.parentElement.querySelector('span').textContent;
//        const columnApi = pivotGridOptions.columnApi || pivotGridOptions.api;

//        try {
//            // Kolonun görünür olup olmadığını kontrol et
//            let isVisible = false;

//            if (columnApi.getColumn) {
//                const column = columnApi.getColumn(colId);
//                isVisible = column ? column.isVisible() : false;
//            } else if (columnApi.getAllColumns) {
//                const allColumns = columnApi.getAllColumns();
//                const column = allColumns.find(col => col.getColId() === colId);
//                isVisible = column ? column.isVisible() : false;
//            }

//            cb.checked = isVisible;
//        } catch (error) {
//            console.error('Checkbox senkronizasyon hatası:', colId, error);
//        }
//    });
//}

//// PIVOT DATA YÜKLEME
//async function loadPivotData() {

//    const selectedTable = document.getElementById("tableSelector").value;

//    if (!selectedTable) {
//        alert("Lütfen bir tablo seçiniz.");
//        return;
//    }

//    currentTableName = selectedTable;

//    const payload = {
//        tableName: selectedTable
//    };

//    try {

//        console.log("📤 API POST:", `${apiBaseUrl}pivots/get`, payload);

//        const response = await fetch(`${apiBaseUrl}pivots/get`, {
//            method: "POST",
//            headers: {
//                "Content-Type": "application/json",
//                "Authorization": "Bearer " + globalJwtToken
//            },
//            body: JSON.stringify(payload)
//        });

//        const raw = await response.text();
//        console.log("🔥 API RAW RESPONSE:", raw);

//        let data;
//        try {
//            data = JSON.parse(raw);
//        } catch (err) {
//            console.error("❌ JSON parse hatası:", err);
//            alert("API JSON yerine farklı veri döndürdü. Konsola bak.");
//            return;
//        }

//        if (!Array.isArray(data) || data.length === 0) {
//            alert("Bu tablo için veri bulunamadı.");
//            return;
//        }

//        const columnDefs = buildColumnDefs(data);
//        renderPivotGrid(columnDefs, data);

//    } catch (error) {
//        console.error("Pivot API hata:", error);
//        alert("Sunucu hatası! Pivot verisi alınamadı.");
//    }
//}


//// DİNAMİK KOLON OLUŞTURMA
//function buildColumnDefs(data) {
//    const firstRow = data[0];

//    const moneyKeywords = [
//        "CIRO", "FIYAT", "NET", "BRUT", "KATILIM",
//        "BUTCE", "TUTAR", "TOPLAM", "ALTI", "KALAN", "PRICE", "ALINANHIZMET", "SAYISI"
//    ];

//    const monthColumns = [
//        "OCAK", "ŞUBAT", "SUBAT", "MART", "NİSAN", "NISAN",
//        "MAYIS", "HAZİRAN", "HAZIRAN", "TEMMUZ", "AĞUSTOS", "AGUSTOS",
//        "EYLÜL", "EYLUL", "EKİM", "EKIM", "KASIM", "ARALIK"
//    ];

//    return Object.keys(firstRow).map(key => {
//        const upperKey = key.toUpperCase();

//        const isMoney =
//            moneyKeywords.some(k => upperKey.includes(k)) ||
//            monthColumns.some(m => upperKey === m);

//        return {
//            field: key,
//            sortable: true,
//            filter: true,
//            resizable: true,
//            enableRowGroup: true,
//            enablePivot: true,
//            enableValue: true,

//            valueFormatter: isMoney
//                ? (params) => {
//                    if (params.value == null || params.value === "") return "";
//                    const num = Number(params.value);
//                    if (isNaN(num)) return params.value;
//                    return num.toLocaleString("tr-TR", {
//                        minimumFractionDigits: 0,
//                        maximumFractionDigits: 0
//                    });
//                }
//                : undefined
//        };
//    });
//}

//// 🆕 TOPLAM SATIRI HESAPLAMA
//function calculateTotalRow(columnDefs, rowData) {
//    const totalRow = {};

//    columnDefs.forEach((colDef, index) => {
//        const field = colDef.field;
//        const upperField = field.toUpperCase();

//        // İlk kolona "TOPLAM" yaz
//        if (index === 0) {
//            totalRow[field] = "TOPLAM";
//            return;
//        }

//        // Sadece belirlenen sütunları topla
//        const shouldSum = COLUMNS_TO_SUM.some(col =>
//            upperField.includes(col.toUpperCase())
//        );

//        if (shouldSum) {
//            let sum = 0;
//            let hasNumericValue = false;

//            rowData.forEach(row => {
//                const value = row[field];
//                const num = Number(value);
//                if (!isNaN(num) && value !== null && value !== "") {
//                    sum += num;
//                    hasNumericValue = true;
//                }
//            });

//            totalRow[field] = hasNumericValue ? sum : "";
//        } else {
//            // Toplanmayacak sütunlar boş
//            totalRow[field] = "";
//        }
//    });

//    return totalRow;
//}


//// AG-GRID PIVOT RENDER
//function renderPivotGrid(columnDefs, rowData) {

//    const gridDiv = document.querySelector("#pivotGrid");

//    if (pivotGridOptions?.api) {
//        pivotGridOptions.api.destroy();
//    }

//    // 🆕 Grid container'a dinamik yükseklik ver
//    gridDiv.style.height = 'calc(100vh - 200px)';  // Sayfa yüksekliği - üst alan
//    gridDiv.style.width = '100%';

//    // 🆕 Toplam satırını hesapla ve global'de sakla
//    const totalRow = calculateTotalRow(columnDefs, rowData);
//    currentTotalRow = totalRow;

//    pivotGridOptions = {
//        columnDefs,
//        rowData,

//        // 🆕 Alt kısma sabitlenmiş toplam satırı
//        pinnedBottomRowData: [totalRow],

//        pivotMode: false,

//        rowGroupPanelShow: 'always',

//        sideBar: {
//            toolPanels: [
//                {
//                    id: 'columns',
//                    labelDefault: 'Sütunlar',
//                    iconKey: 'columns',
//                    toolPanel: 'agColumnsToolPanel'
//                }
//            ],
//            defaultToolPanel: 'columns'
//        },

//        animateRows: true,

//        suppressDragLeaveHidesColumns: false,

//        // 🆕 Kolon görünürlüğü değiştiğinde checkbox'ları güncelle
//        onColumnVisible: function (params) {
//            syncCheckboxWithColumn(params.column.getColId(), params.visible);
//        },

//        // 🆕 Kolonlar değiştiğinde tüm checkbox'ları senkronize et
//        onDisplayedColumnsChanged: function () {
//            syncAllCheckboxes();
//        },

//        getRowStyle: params => {
//            // 🆕 Toplam satırı için özel stil
//            if (params.node.rowPinned) {
//                return {
//                    backgroundColor: "#ffd966",
//                    fontWeight: "bold",
//                    borderTop: "2px solid #333"
//                };
//            }

//            if (params.node.rowIndex % 2 === 0) {
//                return { backgroundColor: "#f0f2f5" };
//            }
//            return { backgroundColor: "#ffffff" };
//        },

//        defaultColDef: {
//            sortable: true,
//            filter: true,
//            resizable: true,
//            minWidth: 50,
//            enableRowGroup: true,
//            enableValue: true,
//            headerClass: 'modern-header-cell'
//        },

//        localeText: {
//            contains: 'İçerir',
//            notContains: 'İçermez',
//            equals: 'Eşittir',
//            notEqual: 'Eşit Değil',
//            startsWith: 'İle Başlar',
//            endsWith: 'İle Biter',
//            blank: 'Boş',
//            notBlank: 'Boş Değil',
//            filterOoo: 'Filtre...',
//            applyFilter: 'Uygula',
//            clearFilter: 'Temizle',
//            andCondition: 'Ve',
//            orCondition: 'Veya',
//            reset: 'Sıfırla',
//            searchOoo: 'Ara...',
//            selectAll: 'Tümünü Seç',
//            noMatches: 'Eşleşme Yok',
//            columns: 'Sütunlar',
//            filters: 'Filtreler',
//            pinColumn: 'Sütunu Sabitle',
//            autosizeThiscolumn: 'Bu Sütunu Otomatik Genişlet',
//            autosizeAllColumns: 'Tüm Sütunları Otomatik Genişlet',
//            groupBy: 'Grupla',
//            ungroupBy: 'Gruptan Çıkar',
//            rowGroupColumnsEmptyMessage: 'Gruplamak için sütun buraya sürükleyin',
//            valuesColumnsEmptyMessage: 'Özet için sütun buraya sürükleyin',
//            pivotColumnsEmptyMessage: 'Pivot için sütun buraya sürükleyin'
//        }
//    };

//    const gridInstance = new agGrid.Grid(gridDiv, pivotGridOptions);

//    // Column API'yi de sakla (bazı AG Grid versiyonları için)
//    if (gridInstance.columnApi) {
//        pivotGridOptions.columnApi = gridInstance.columnApi;
//    }

//    // 🆕 Paneli oluştur (ilk kez) veya güncelle
//    if (!document.getElementById('columnTogglePanel')) {
//        createColumnTogglePanel();
//    }

//    // 🆕 Kolon panelini güncelle
//    updateColumnTogglePanel(columnDefs);
//}
let globalJwtToken = null;
let pivotGridOptions = null;
let currentTableName = '';
let currentTotalRow = null;
let currentTheme = 'light'; // 🆕 Tema durumu

// 🆕 TOPLANACAK SÜTUN İSİMLERİ
const COLUMNS_TO_SUM = [
    "CIRO",
    "FIYAT",
    "NET",
    "BRUT",
    "KATILIM",
    "BUTCE",
    "TUTAR",
    "TOPLAM",
    "ALTI",
    "KALAN",
    "PRICE",
    "ALINANHIZMET",
    "SAYISI"
];

document.addEventListener("DOMContentLoaded", function () {

    const tokenFromDom = document.getElementById("jwtToken")?.value;
    globalJwtToken = tokenFromDom;

    if (!globalJwtToken) {
        alert("JWT Token bulunamadı. Lütfen tekrar giriş yapın.");
        return;
    }

    document
        .getElementById("btnLoadPivot")
        .addEventListener("click", loadPivotData);

    // Excel'e aktar butonu - Grid'den XLSX olarak export
    document
        .getElementById("btnExportExcel")
        .addEventListener("click", async function () {

            if (!pivotGridOptions?.api) {
                alert("Önce verileri yükleyin.");
                return;
            }

            try {
                const tableName = currentTableName || "Pivot";
                const today = new Date().toISOString().slice(0, 10);

                // SheetJS kütüphanesini dinamik yükle
                if (typeof XLSX === 'undefined') {
                    const script = document.createElement('script');
                    script.src = 'https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.18.5/xlsx.full.min.js';
                    document.head.appendChild(script);

                    await new Promise((resolve, reject) => {
                        script.onload = resolve;
                        script.onerror = reject;
                    });
                }

                // Grid'den tüm satırları al (normal satırlar)
                const allRows = [];
                pivotGridOptions.api.forEachNode(node => {
                    if (!node.group) {
                        allRows.push(node.data);
                    }
                });

                // Toplam satırını ekle
                if (currentTotalRow) {
                    allRows.push(currentTotalRow);
                }

                // XLSX çalışma kitabı oluştur
                const worksheet = XLSX.utils.json_to_sheet(allRows);
                const workbook = XLSX.utils.book_new();
                XLSX.utils.book_append_sheet(workbook, worksheet, "Pivot");

                // Dosyayı indir
                XLSX.writeFile(workbook, `${tableName}_${today}.xlsx`);

            } catch (error) {
                console.error("Excel export hatası:", error);
                alert("Excel dosyası oluşturulurken hata oluştu.");
            }
        });

});

// 🆕 TEMA DEĞİŞTİRİCİ OLUŞTUR
function createThemeSwitcher() {
    const switcherContainer = document.createElement('div');
    switcherContainer.id = 'themeSwitcher';
    switcherContainer.style.cssText = `
        position: absolute;
        top: 10px;
        right: 320px;
        z-index: 1000;
        display: flex;
        align-items: center;
        gap: 8px;
        background: white;
        padding: 7px 15px;
        border-radius: 50px;
        box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        transition: all 0.3s ease;
        height: 40px;
        box-sizing: border-box;
    `;

    const label = document.createElement('span');
    label.textContent = '🌞';
    label.style.cssText = `
        font-size: 18px;
        transition: all 0.3s ease;
        line-height: 1;
    `;

    // Toggle switch
    const toggleSwitch = document.createElement('label');
    toggleSwitch.style.cssText = `
        position: relative;
        display: inline-block;
        width: 50px;
        height: 24px;
        cursor: pointer;
    `;

    const checkbox = document.createElement('input');
    checkbox.type = 'checkbox';
    checkbox.style.cssText = `
        opacity: 0;
        width: 0;
        height: 0;
    `;

    const slider = document.createElement('span');
    slider.style.cssText = `
        position: absolute;
        cursor: pointer;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background-color: #ccc;
        transition: .4s;
        border-radius: 34px;
    `;

    const sliderButton = document.createElement('span');
    sliderButton.style.cssText = `
        position: absolute;
        content: "";
        height: 18px;
        width: 18px;
        left: 3px;
        bottom: 3px;
        background-color: white;
        transition: .4s;
        border-radius: 50%;
    `;

    slider.appendChild(sliderButton);
    toggleSwitch.appendChild(checkbox);
    toggleSwitch.appendChild(slider);

    checkbox.addEventListener('change', function () {
        if (this.checked) {
            // Karanlık tema
            currentTheme = 'dark';
            label.textContent = '🌙';
            slider.style.backgroundColor = '#2196F3';
            sliderButton.style.transform = 'translateX(26px)';
            applyTheme('dark');
        } else {
            // Açık tema
            currentTheme = 'light';
            label.textContent = '🌞';
            slider.style.backgroundColor = '#ccc';
            sliderButton.style.transform = 'translateX(0)';
            applyTheme('light');
        }
    });

    switcherContainer.appendChild(label);
    switcherContainer.appendChild(toggleSwitch);

    // Grid div'in parent'ına ekle
    const gridDiv = document.querySelector("#pivotGrid");
    if (gridDiv && gridDiv.parentElement) {
        gridDiv.parentElement.style.position = 'relative';
        gridDiv.parentElement.appendChild(switcherContainer);
    }

    // Varsayılan olarak açık tema uygula
    applyTheme('light');
}

// 🆕 TEMA UYGULA
function applyTheme(theme) {
    if (theme === 'dark') {
        // Karanlık tema stilleri - SİYAH TONLARI
        document.body.style.cssText = `
            background: #1a1a1a;
            min-height: 100vh;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            padding: 20px;
            margin: 0;
            color: #eee;
        `;

        // 🆕 Tüm label ve başlıkları beyaz yap
        document.querySelectorAll('label, h1, h2, h3, h4, h5, h6, .label, .heading').forEach(el => {
            el.style.color = '#eee';
        });

        // AG Grid dark tema için CSS ekle
        applyGridTheme('dark');

    } else {
        // Açık tema - ESKİ HALİ (basit)
        document.body.style.cssText = `
            min-height: 100vh;
            font-family: Arial, sans-serif;
            padding: 20px;
            margin: 0;
        `;

        // Label'ları sıfırla
        document.querySelectorAll('label, h1, h2, h3, h4, h5, h6, .label, .heading').forEach(el => {
            el.style.color = '';
        });

        // AG Grid light tema
        applyGridTheme('light');
    }

    // Butonları güncelle
    updateButtonStyles(theme);

    // 🆕 Excel butonunu özellikle zorla güncelle
    const btnExportExcel = document.getElementById("btnExportExcel");
    if (btnExportExcel) {
        if (theme === 'dark') {
            btnExportExcel.style.setProperty('background', '#2a7a3f', 'important');
            btnExportExcel.style.setProperty('color', 'white', 'important');
        } else {
            btnExportExcel.style.removeProperty('background');
            btnExportExcel.style.removeProperty('color');
        }
    }
}

// 🆕 GRID TEMA UYGULA
function applyGridTheme(theme) {
    const gridDiv = document.querySelector("#pivotGrid");
    if (!gridDiv) return;

    if (theme === 'dark') {
        // AG Grid'e dark class ekle
        gridDiv.classList.add('ag-theme-alpine-dark');
        gridDiv.classList.remove('ag-theme-alpine');

        // Grid stillerini güncelle - SİYAH TONLARI
        gridDiv.style.background = '#2a2a2a';
        gridDiv.style.borderRadius = '12px';
        gridDiv.style.boxShadow = '0 8px 32px rgba(0, 0, 0, 0.8)';
        gridDiv.style.padding = '20px';
    } else {
        // AG Grid'e light class ekle
        gridDiv.classList.add('ag-theme-alpine');
        gridDiv.classList.remove('ag-theme-alpine-dark');

        // ESKİ BASİT HAL - ekstra stil yok
        gridDiv.style.background = '';
        gridDiv.style.borderRadius = '';
        gridDiv.style.boxShadow = '';
        gridDiv.style.padding = '';
    }

    // Grid'i yeniden render et
    if (pivotGridOptions?.api) {
        // Tüm satırları yeniden çiz
        pivotGridOptions.api.redrawRows();
        pivotGridOptions.api.refreshCells({ force: true });

        // Header'ı da yenile
        pivotGridOptions.api.refreshHeader();

        // Kolonları genişlet
        pivotGridOptions.api.sizeColumnsToFit();
    }

    // 🆕 Header yazı renklerini manuel olarak düzelt
    setTimeout(() => {
        const headers = document.querySelectorAll('.ag-header-cell-label, .ag-header-cell-text');
        headers.forEach(header => {
            if (theme === 'dark') {
                header.style.color = '#eee';
            } else {
                header.style.color = '#000'; // Light modda siyah
            }
        });
    }, 100);
}

// 🆕 BUTON STİLLERİNİ GÜNCELLE
function updateButtonStyles(theme) {
    const btnLoadPivot = document.getElementById("btnLoadPivot");
    const btnExportExcel = document.getElementById("btnExportExcel");
    const tableSelector = document.getElementById("tableSelector");

    // ORTAK BOYUTLAR - HER İKİ TEMADA AYNI
    const commonStyle = `
        padding: 10px 20px;
        border-radius: 5px;
        font-size: 14px;
        font-weight: normal;
        cursor: pointer;
        margin-right: 10px;
        height: 40px;
        line-height: 20px;
        box-sizing: border-box;
    `;

    if (theme === 'dark') {
        // Karanlık tema buton stilleri
        if (btnLoadPivot) {
            btnLoadPivot.style.cssText = commonStyle + `
                background: #0f3460;
                color: #eee;
                border: 2px solid #16213e;
            `;
        }

        if (btnExportExcel) {
            btnExportExcel.style.cssText = commonStyle + `
                background: #e94560;
                color: white;
                border: none;
            `;
        }

        if (tableSelector) {
            tableSelector.style.cssText = `
                background: #0f3460;
                color: #eee;
                border: 2px solid #16213e;
                padding: 8px 12px;
                border-radius: 5px;
                font-size: 14px;
                cursor: pointer;
                outline: none;
                margin-right: 10px;
                min-width: 200px;
                height: 40px;
                line-height: 22px;
                box-sizing: border-box;
            `;
        }

    } else {
        // Açık tema - ESKİ BASİT HAL ama aynı boyutlar
        if (btnLoadPivot) {
            btnLoadPivot.style.cssText = commonStyle;
        }

        if (btnExportExcel) {
            btnExportExcel.style.cssText = commonStyle;
        }

        if (tableSelector) {
            tableSelector.style.cssText = `
                padding: 8px 12px;
                margin-right: 10px;
                min-width: 200px;
                cursor: pointer;
                height: 40px;
                line-height: 22px;
                box-sizing: border-box;
                border-radius: 5px;
                font-size: 14px;
            `;
        }
    }

    // Kolon göster/gizle butonunu güncelle
    updateColumnToggleButton(theme);

    // Tema switcher'ı güncelle
    updateThemeSwitcher(theme);
}

// 🆕 KOLON BUTONUNU GÜNCELLE
function updateColumnToggleButton(theme) {
    const toggleButton = document.getElementById('columnToggleBtn');
    if (!toggleButton) return;

    // ORTAK BOYUTLAR
    const commonStyle = `
        padding: 10px 20px;
        border-radius: 5px;
        cursor: pointer;
        font-size: 14px;
        border: none;
        color: white;
        height: 40px;
        line-height: 20px;
        box-sizing: border-box;
    `;

    if (theme === 'dark') {
        toggleButton.style.cssText = commonStyle + `
            background: #2a7a3f;
        `;
    } else {
        // ESKİ BASİT HAL
        toggleButton.style.cssText = commonStyle + `
            background: #4CAF50;
        `;
    }

    // 🆕 DROPDOWN'A DARK MOD UYGULANMASIN - HEP BEYAZ KALSIN
    const dropdown = document.getElementById('columnDropdown');
    if (dropdown) {
        // Her zaman beyaz
        dropdown.style.background = 'white';
        dropdown.style.boxShadow = '0 4px 10px rgba(0,0,0,0.2)';
        dropdown.style.borderRadius = '5px';
    }
}

// 🆕 TEMA SWITCHER'I GÜNCELLE
function updateThemeSwitcher(theme) {
    const themeSwitcher = document.getElementById('themeSwitcher');
    if (!themeSwitcher) return;

    // ORTAK BOYUTLAR
    const commonStyle = `
        position: absolute;
        top: 10px;
        right: 320px;
        z-index: 1000;
        display: flex;
        align-items: center;
        gap: 8px;
        padding: 7px 15px;
        border-radius: 50px;
        transition: all 0.3s ease;
        height: 40px;
        box-sizing: border-box;
    `;

    if (theme === 'dark') {
        // SİYAH TONLARI
        themeSwitcher.style.cssText = commonStyle + `
            background: #2a2a2a;
            color: #eee;
            box-shadow: 0 2px 8px rgba(0,0,0,0.8);
            border: 1px solid #3a3a3a;
        `;
    } else {
        themeSwitcher.style.cssText = commonStyle + `
            background: white;
            color: #333;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        `;
    }
}

// 🆕 KOLON GÖSTER/GİZLE PANELİ OLUŞTUR
function createColumnTogglePanel() {
    // Panel container
    const panelContainer = document.createElement('div');
    panelContainer.id = 'columnTogglePanel';
    panelContainer.style.cssText = `
        position: absolute;
        top: 10px;
        right: 20px;
        z-index: 1000;
        font-family: Arial, sans-serif;
    `;

    // Buton
    const toggleButton = document.createElement('button');
    toggleButton.id = 'columnToggleBtn';
    toggleButton.innerHTML = '📋 Kolonları Göster/Gizle ▼';
    // Başlangıçta her zaman yeşil
    toggleButton.style.cssText = `
        background: #4CAF50;
        color: white;
        border: none;
        padding: 10px 20px;
        border-radius: 5px;
        cursor: pointer;
        font-size: 14px;
        height: 40px;
        line-height: 20px;
        box-sizing: border-box;
    `;

    // Dropdown panel
    const dropdownPanel = document.createElement('div');
    dropdownPanel.id = 'columnDropdown';
    dropdownPanel.style.cssText = `
        display: none;
        position: absolute;
        top: 45px;
        right: 0;
        background: white;
        border: 1px solid #ddd;
        border-radius: 5px;
        box-shadow: 0 4px 10px rgba(0,0,0,0.2);
        max-height: 400px;
        overflow-y: auto;
        min-width: 250px;
        padding: 10px;
    `;

    // Buton tıklama olayı
    toggleButton.addEventListener('click', function (e) {
        e.stopPropagation();
        const isVisible = dropdownPanel.style.display === 'block';
        dropdownPanel.style.display = isVisible ? 'none' : 'block';
        toggleButton.innerHTML = isVisible ? '📋 Kolonları Göster/Gizle ▼' : '📋 Kolonları Göster/Gizle ▲';
    });

    panelContainer.appendChild(toggleButton);
    panelContainer.appendChild(dropdownPanel);

    // Grid div'in parent'ına ekle
    const gridDiv = document.querySelector("#pivotGrid");
    if (gridDiv && gridDiv.parentElement) {
        gridDiv.parentElement.style.position = 'relative';
        gridDiv.parentElement.appendChild(panelContainer);
    }

    // Panel dışına tıklanınca kapat
    document.addEventListener('click', function (e) {
        if (!panelContainer.contains(e.target)) {
            dropdownPanel.style.display = 'none';
            toggleButton.innerHTML = '📋 Kolonları Göster/Gizle ▼';
        }
    });
}

// 🆕 KOLON LİSTESİNİ GÜNCELLE
function updateColumnTogglePanel(columnDefs) {
    const dropdown = document.getElementById('columnDropdown');
    if (!dropdown) return;

    dropdown.innerHTML = '';

    // Başlık
    const header = document.createElement('div');
    header.style.cssText = `
        font-weight: bold;
        margin-bottom: 10px;
        padding-bottom: 5px;
        border-bottom: 2px solid #4CAF50;
        color: #333;
    `;
    header.textContent = 'Kolonları Seçin:';
    dropdown.appendChild(header);

    // Checkbox container için padding ekle
    const checkboxWrapper = document.createElement('div');

    columnDefs.forEach(colDef => {
        const checkboxContainer = document.createElement('label');
        checkboxContainer.style.cssText = `
            display: flex;
            align-items: center;
            padding: 8px;
            cursor: pointer;
            border-radius: 3px;
            transition: background 0.2s;
        `;
        checkboxContainer.onmouseover = () => checkboxContainer.style.background = '#f0f0f0';
        checkboxContainer.onmouseout = () => checkboxContainer.style.background = 'transparent';

        // 🆕 Dark modda farklı hover rengi
        if (currentTheme === 'dark') {
            checkboxContainer.onmouseover = () => checkboxContainer.style.background = '#3a3a3a';
            checkboxContainer.onmouseout = () => checkboxContainer.style.background = 'transparent';
        }

        const checkbox = document.createElement('input');
        checkbox.type = 'checkbox';
        checkbox.checked = true;
        checkbox.style.cssText = `
            margin-right: 10px;
            cursor: pointer;
            width: 16px;
            height: 16px;
        `;

        const label = document.createElement('span');
        label.textContent = colDef.field;
        label.style.cssText = `
            font-size: 13px;
            color: #333;
        `;

        checkbox.addEventListener('change', function () {
            if (pivotGridOptions?.api) {
                const columnApi = pivotGridOptions.columnApi || pivotGridOptions.api;

                if (columnApi.setColumnVisible) {
                    columnApi.setColumnVisible(colDef.field, this.checked);
                } else if (columnApi.setColumnsVisible) {
                    columnApi.setColumnsVisible([colDef.field], this.checked);
                }
            }
        });

        checkboxContainer.appendChild(checkbox);
        checkboxContainer.appendChild(label);
        checkboxWrapper.appendChild(checkboxContainer);
    });

    dropdown.appendChild(checkboxWrapper);

    // Tümünü seç/kaldır butonları
    const buttonContainer = document.createElement('div');
    buttonContainer.style.cssText = `
        margin-top: 10px;
        padding-top: 10px;
        border-top: 1px solid #ddd;
        display: flex;
        gap: 5px;
    `;

    const selectAllBtn = document.createElement('button');
    selectAllBtn.textContent = 'Tümünü Seç';
    selectAllBtn.style.cssText = `
        flex: 1;
        padding: 6px;
        background: #4CAF50;
        color: white;
        border: none;
        border-radius: 3px;
        cursor: pointer;
        font-size: 12px;
    `;
    selectAllBtn.addEventListener('click', function (e) {
        e.stopPropagation();
        checkboxWrapper.querySelectorAll('input[type="checkbox"]').forEach(cb => {
            cb.checked = true;
            const colField = cb.parentElement.querySelector('span').textContent;
            if (pivotGridOptions?.api) {
                const columnApi = pivotGridOptions.columnApi || pivotGridOptions.api;
                if (columnApi.setColumnVisible) {
                    columnApi.setColumnVisible(colField, true);
                } else if (columnApi.setColumnsVisible) {
                    columnApi.setColumnsVisible([colField], true);
                }
            }
        });
    });

    const deselectAllBtn = document.createElement('button');
    deselectAllBtn.textContent = 'Tümünü Kaldır';
    deselectAllBtn.style.cssText = `
        flex: 1;
        padding: 6px;
        background: #f44336;
        color: white;
        border: none;
        border-radius: 3px;
        cursor: pointer;
        font-size: 12px;
    `;
    deselectAllBtn.addEventListener('click', function (e) {
        e.stopPropagation();
        checkboxWrapper.querySelectorAll('input[type="checkbox"]').forEach(cb => {
            cb.checked = false;
            const colField = cb.parentElement.querySelector('span').textContent;
            if (pivotGridOptions?.api) {
                const columnApi = pivotGridOptions.columnApi || pivotGridOptions.api;
                if (columnApi.setColumnVisible) {
                    columnApi.setColumnVisible(colField, false);
                } else if (columnApi.setColumnsVisible) {
                    columnApi.setColumnsVisible([colField], false);
                }
            }
        });
    });

    buttonContainer.appendChild(selectAllBtn);
    buttonContainer.appendChild(deselectAllBtn);
    dropdown.appendChild(buttonContainer);
}

// 🆕 TEK BİR CHECKBOX'I SENKRONIZE ET
function syncCheckboxWithColumn(colId, isVisible) {
    const dropdown = document.getElementById('columnDropdown');
    if (!dropdown) return;

    const checkboxes = dropdown.querySelectorAll('input[type="checkbox"]');
    checkboxes.forEach(cb => {
        const label = cb.parentElement.querySelector('span');
        if (label && label.textContent === colId) {
            cb.checked = isVisible;
        }
    });
}

// 🆕 TÜM CHECKBOX'LARI SENKRONIZE ET
function syncAllCheckboxes() {
    if (!pivotGridOptions?.api) return;

    const dropdown = document.getElementById('columnDropdown');
    if (!dropdown) return;

    const checkboxes = dropdown.querySelectorAll('input[type="checkbox"]');
    checkboxes.forEach(cb => {
        const colId = cb.parentElement.querySelector('span').textContent;
        const columnApi = pivotGridOptions.columnApi || pivotGridOptions.api;

        try {
            // Kolonun görünür olup olmadığını kontrol et
            let isVisible = false;

            if (columnApi.getColumn) {
                const column = columnApi.getColumn(colId);
                isVisible = column ? column.isVisible() : false;
            } else if (columnApi.getAllColumns) {
                const allColumns = columnApi.getAllColumns();
                const column = allColumns.find(col => col.getColId() === colId);
                isVisible = column ? column.isVisible() : false;
            }

            cb.checked = isVisible;
        } catch (error) {
            console.error('Checkbox senkronizasyon hatası:', colId, error);
        }
    });
}

// PIVOT DATA YÜKLEME
async function loadPivotData() {

    const selectedTable = document.getElementById("tableSelector").value;

    if (!selectedTable) {
        alert("Lütfen bir tablo seçiniz.");
        return;
    }

    currentTableName = selectedTable;

    const payload = {
        tableName: selectedTable
    };

    try {

        console.log("📤 API POST:", `${apiBaseUrl}pivots/get`, payload);

        const response = await fetch(`${apiBaseUrl}pivots/get`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + globalJwtToken
            },
            body: JSON.stringify(payload)
        });

        const raw = await response.text();
        console.log("🔥 API RAW RESPONSE:", raw);

        let data;
        try {
            data = JSON.parse(raw);
        } catch (err) {
            console.error("❌ JSON parse hatası:", err);
            alert("API JSON yerine farklı veri döndürdü. Konsola bak.");
            return;
        }

        if (!Array.isArray(data) || data.length === 0) {
            alert("Bu tablo için veri bulunamadı.");
            return;
        }

        const columnDefs = buildColumnDefs(data);
        renderPivotGrid(columnDefs, data);

    } catch (error) {
        console.error("Pivot API hata:", error);
        alert("Sunucu hatası! Pivot verisi alınamadı.");
    }
}


// DİNAMİK KOLON OLUŞTURMA
function buildColumnDefs(data) {
    const firstRow = data[0];

    const moneyKeywords = [
        "CIRO", "FIYAT", "NET", "BRUT", "KATILIM",
        "BUTCE", "TUTAR", "TOPLAM", "ALTI", "KALAN", "PRICE", "ALINANHIZMET", "SAYISI"
    ];

    const monthColumns = [
        "OCAK", "ŞUBAT", "SUBAT", "MART", "NİSAN", "NISAN",
        "MAYIS", "HAZİRAN", "HAZIRAN", "TEMMUZ", "AĞUSTOS", "AGUSTOS",
        "EYLÜL", "EYLUL", "EKİM", "EKIM", "KASIM", "ARALIK"
    ];

    return Object.keys(firstRow).map(key => {
        const upperKey = key.toUpperCase();

        const isMoney =
            moneyKeywords.some(k => upperKey.includes(k)) ||
            monthColumns.some(m => upperKey === m);

        return {
            field: key,
            sortable: true,
            filter: true,
            resizable: true,
            enableRowGroup: true,
            enablePivot: true,
            enableValue: true,

            valueFormatter: isMoney
                ? (params) => {
                    if (params.value == null || params.value === "") return "";
                    const num = Number(params.value);
                    if (isNaN(num)) return params.value;
                    return num.toLocaleString("tr-TR", {
                        minimumFractionDigits: 0,
                        maximumFractionDigits: 0
                    });
                }
                : undefined
        };
    });
}

// 🆕 TOPLAM SATIRI HESAPLAMA
//function calculateTotalRow(columnDefs, rowData) {
//    const totalRow = {};

//    columnDefs.forEach((colDef, index) => {
//        const field = colDef.field;
//        const upperField = field.toUpperCase();

//        // İlk kolona "TOPLAM" yaz
//        if (index === 0) {
//            totalRow[field] = "TOPLAM";
//            return;
//        }

//        // Sadece belirlenen sütunları topla
//        const shouldSum = COLUMNS_TO_SUM.some(col =>
//            upperField.includes(col.toUpperCase())
//        );

//        if (shouldSum) {
//            let sum = 0;
//            let hasNumericValue = false;

//            rowData.forEach(row => {
//                const value = row[field];
//                const num = Number(value);
//                if (!isNaN(num) && value !== null && value !== "") {
//                    sum += num;
//                    hasNumericValue = true;
//                }
//            });

//            totalRow[field] = hasNumericValue ? sum : "";
//        } else {
//            // Toplanmayacak sütunlar boş
//            totalRow[field] = "";
//        }
//    });

//    return totalRow;
//}


// AG-GRID PIVOT RENDER


function calculateTotalRow(columnDefs, rowData) {
    const totalRow = {};
    columnDefs.forEach((colDef, index) => {
        const field = colDef.field;
        const upperField = field.toUpperCase();
        // İlk kolona "TOPLAM" yaz
        if (index === 0) {
            totalRow[field] = "TOPLAM";
            return;
        }
        // Sadece belirlenen sütunları topla
        const shouldSum = COLUMNS_TO_SUM.some(col =>
            upperField.includes(col.toUpperCase())
        );
        if (shouldSum) {
            let sum = 0;
            let hasNumericValue = false;
            rowData.forEach(row => {
                const value = row[field];
                const num = Number(value);
                if (!isNaN(num) && value !== null && value !== "") {
                    sum += num;
                    hasNumericValue = true;
                }
            });
            totalRow[field] = hasNumericValue ? sum : "";
        } else {
            // Toplanmayacak sütunlar boş
            totalRow[field] = "";
        }
    });
    return totalRow;
}

// 🆕 FİLTREYE GÖRE TOPLAM GÜNCELLE
function updatePinnedTotalRow(api) {
    if (!api) return;

    const columnDefs = pivotGridOptions.columnDefs;
    const visibleRows = [];

    api.forEachNodeAfterFilter(node => {
        if (!node.group && !node.rowPinned) {
            visibleRows.push(node.data);
        }
    });

    const newTotalRow = calculateTotalRow(columnDefs, visibleRows);
    currentTotalRow = newTotalRow;

    api.setPinnedBottomRowData([newTotalRow]);
}
function renderPivotGrid(columnDefs, rowData) {

    const gridDiv = document.querySelector("#pivotGrid");

    if (pivotGridOptions?.api) {
        pivotGridOptions.api.destroy();
    }

    // 🆕 Grid container'a dinamik yükseklik ve temaya göre stil ver
    gridDiv.style.height = 'calc(100vh - 200px)';
    gridDiv.style.width = '100%';

    if (currentTheme === 'dark') {
        // SİYAH TONLARI
        gridDiv.style.background = '#2a2a2a';
        gridDiv.style.borderRadius = '12px';
        gridDiv.style.boxShadow = '0 8px 32px rgba(0, 0, 0, 0.8)';
        gridDiv.style.padding = '20px';
        gridDiv.classList.add('ag-theme-alpine-dark');
        gridDiv.classList.remove('ag-theme-alpine');
    } else {
        // ESKİ BASİT HAL - ekstra stil yok
        gridDiv.style.background = '';
        gridDiv.style.borderRadius = '';
        gridDiv.style.boxShadow = '';
        gridDiv.style.padding = '';
        gridDiv.classList.add('ag-theme-alpine');
        gridDiv.classList.remove('ag-theme-alpine-dark');
    }

    // 🆕 Toplam satırını hesapla ve global'de sakla
    const totalRow = calculateTotalRow(columnDefs, rowData);
    currentTotalRow = totalRow;

    pivotGridOptions = {
        columnDefs,
        rowData,

        // 🆕 Alt kısma sabitlenmiş toplam satırı
        pinnedBottomRowData: [totalRow],

        pivotMode: false,

        rowGroupPanelShow: 'always',

        // 🆕 Kolonları otomatik genişlet
        onGridReady: (params) => {
            params.api.sizeColumnsToFit();
        },

        // 🆕 Pencere boyutu değişince kolonları yeniden genişlet
        onGridSizeChanged: (params) => {
            params.api.sizeColumnsToFit();
        },

        sideBar: {
            toolPanels: [
                {
                    id: 'columns',
                    labelDefault: 'Sütunlar',
                    iconKey: 'columns',
                    toolPanel: 'agColumnsToolPanel'
                }
            ],
            defaultToolPanel: 'columns'
        },

        animateRows: true,
        // 🆕 Filtre değişince toplamı güncelle
        onFilterChanged: function (params) {
            updatePinnedTotalRow(params.api);
        },


        suppressDragLeaveHidesColumns: false,

        // 🆕 AG Grid'in kendi hover'ını kapat
        suppressRowHoverHighlight: false,

        // 🆕 Kolon görünürlüğü değiştiğinde checkbox'ları güncelle
        onColumnVisible: function (params) {
            syncCheckboxWithColumn(params.column.getColId(), params.visible);
        },

        // 🆕 Kolonlar değiştiğinde tüm checkbox'ları senkronize et
        onDisplayedColumnsChanged: function () {
            syncAllCheckboxes();
        },

        getRowStyle: params => {
            // 🆕 Toplam satırı için özel stil - HER İKİ TEMADA SARI
            if (params.node.rowPinned) {
                return {
                    backgroundColor: "#ffd966",
                    fontWeight: "bold",
                    borderTop: "2px solid #333",
                    color: "#333"
                };
            }

            if (currentTheme === 'dark') {
                // SİYAH TONLARI
                return params.node.rowIndex % 2 === 0
                    ? { backgroundColor: "#1f1f1f", color: "#eee" }
                    : { backgroundColor: "#2a2a2a", color: "#eee" };
            } else {
                // ESKİ BASİT HAL - renk belirtme, default olsun
                return params.node.rowIndex % 2 === 0
                    ? { backgroundColor: "#f0f2f5", color: "#000" }
                    : { backgroundColor: "#ffffff", color: "#000" };
            }
        },

        // 🆕 HOVER EFEKTI - artık gerek yok, CSS'de hallettik

        defaultColDef: {
            sortable: true,
            filter: true,
            resizable: true,
            minWidth: 50,
            enableRowGroup: true,
            enableValue: true,
            headerClass: 'modern-header-cell',
            cellStyle: currentTheme === 'dark' ? { color: '#eee' } : { color: '#000' }
        },

        localeText: {
            contains: 'İçerir',
            notContains: 'İçermez',
            equals: 'Eşittir',
            notEqual: 'Eşit Değil',
            startsWith: 'İle Başlar',
            endsWith: 'İle Biter',
            blank: 'Boş',
            notBlank: 'Boş Değil',
            filterOoo: 'Filtre...',
            applyFilter: 'Uygula',
            clearFilter: 'Temizle',
            andCondition: 'Ve',
            orCondition: 'Veya',
            reset: 'Sıfırla',
            searchOoo: 'Ara...',
            selectAll: 'Tümünü Seç',
            noMatches: 'Eşleşme Yok',
            columns: 'Sütunlar',
            filters: 'Filtreler',
            pinColumn: 'Sütunu Sabitle',
            autosizeThiscolumn: 'Bu Sütunu Otomatik Genişlet',
            autosizeAllColumns: 'Tüm Sütunları Otomatik Genişlet',
            groupBy: 'Grupla',
            ungroupBy: 'Gruptan Çıkar',
            rowGroupColumnsEmptyMessage: 'Gruplamak için sütun buraya sürükleyin',
            valuesColumnsEmptyMessage: 'Özet için sütun buraya sürükleyin',
            pivotColumnsEmptyMessage: 'Pivot için sütun buraya sürükleyin'
        }
    };

    const gridInstance = new agGrid.Grid(gridDiv, pivotGridOptions);

    // Column API'yi de sakla (bazı AG Grid versiyonları için)
    if (gridInstance.columnApi) {
        pivotGridOptions.columnApi = gridInstance.columnApi;
    }

    // 🆕 Dark mod için CSS ekle
    addDarkModeStyles();

    // 🆕 Paneli oluştur (ilk kez) veya güncelle
    if (!document.getElementById('columnTogglePanel')) {
        createColumnTogglePanel();
    }

    // 🆕 Tema switcher oluştur (ilk kez)
    if (!document.getElementById('themeSwitcher')) {
        createThemeSwitcher();
    }

    // 🆕 Kolon panelini güncelle
    updateColumnTogglePanel(columnDefs);

    // 🆕 Grid hemen refresh et
    setTimeout(() => {
        if (pivotGridOptions?.api) {
            pivotGridOptions.api.redrawRows();
        }
    }, 100);
}

// 🆕 DARK MODE HOVER STİLLERİ - SADECE GRID İÇİN
function addDarkModeStyles() {
    // Eğer style zaten eklendiyse tekrar ekleme
    if (document.getElementById('darkModeStyles')) return;

    const style = document.createElement('style');
    style.id = 'darkModeStyles';
    style.textContent = `
        /* 🆕 LIGHT MOD - ÇOK GÜÇLÜ KURALLAR */
        .ag-theme-alpine .ag-cell {
            color: #000 !important;
            background-color: inherit !important;
        }
        
        .ag-theme-alpine .ag-row .ag-cell {
            color: #000 !important;
        }
        
        .ag-theme-alpine .ag-header-cell-label,
        .ag-theme-alpine .ag-header-cell-text {
            color: #000 !important;
        }
        
        .ag-theme-alpine .ag-header-cell {
            color: #000 !important;
        }
        
        /* Dark mod satır hover efekti - SİYAH TONLARI */
        .ag-theme-alpine-dark .ag-row:hover,
        .ag-theme-alpine-dark .ag-row.ag-row-hover {
            background-color: #3a3a3a !important;
        }
        
        .ag-theme-alpine-dark .ag-row:hover > .ag-cell,
        .ag-theme-alpine-dark .ag-row.ag-row-hover > .ag-cell {
            background-color: #3a3a3a !important;
            color: #fff !important;
        }
        
        /* Tüm cell'lerin rengi beyaz olsun - SADECE DARK */
        .ag-theme-alpine-dark .ag-cell {
            color: #fff !important;
        }
        
        .ag-theme-alpine-dark .ag-row .ag-cell {
            color: #fff !important;
        }
        
        /* Dark mod normal satır renkleri - SİYAH TONLARI */
        .ag-theme-alpine-dark .ag-row-odd {
            background-color: #1f1f1f !important;
        }
        
        .ag-theme-alpine-dark .ag-row-even {
            background-color: #2a2a2a !important;
        }
        
        .ag-theme-alpine-dark .ag-row .ag-cell {
            border-color: #1f1f1f !important;
        }
        
        /* Dark mod header stilleri - SİYAH */
        .ag-theme-alpine-dark .ag-header-cell-label,
        .ag-theme-alpine-dark .ag-header-cell-text {
            color: #eee !important;
        }
        
        .ag-theme-alpine-dark .ag-header {
            background-color: #1f1f1f !important;
            border-bottom: 2px solid #2a2a2a !important;
        }
        
        .ag-theme-alpine-dark .ag-header-cell {
            background-color: #1f1f1f !important;
            border-color: #2a2a2a !important;
        }
        
        /* Dark mod arka plan - boş alan için - SİYAH */
        .ag-theme-alpine-dark .ag-root-wrapper {
            background-color: #2a2a2a !important;
        }
        
        .ag-theme-alpine-dark .ag-body-viewport {
            background-color: #2a2a2a !important;
        }
        
        .ag-theme-alpine-dark .ag-center-cols-viewport,
        .ag-theme-alpine-dark .ag-center-cols-container {
            background-color: transparent !important;
        }
        
        /* Pinned (toplam) satırı normal durum - SARI - HER İKİ TEMA */
        .ag-row-pinned {
            background-color: #ffd966 !important;
        }
        
        .ag-row-pinned .ag-cell {
            background-color: #ffd966 !important;
            color: #333 !important;
            font-weight: bold !important;
        }
        
        /* Dark modda da toplam satırı koyu yazı olsun */
        .ag-theme-alpine-dark .ag-row-pinned .ag-cell {
            background-color: #ffd966 !important;
            color: #333 !important;
            font-weight: bold !important;
        }
        
        /* Pinned (toplam) satırı için hover - SARI KALMALI */
        .ag-row-pinned:hover,
        .ag-row-pinned.ag-row-hover {
            background-color: #ffd966 !important;
        }
        
        .ag-row-pinned:hover > .ag-cell,
        .ag-row-pinned.ag-row-hover > .ag-cell {
            background-color: #ffd966 !important;
            color: #333 !important;
        }
        
        /* Dark modda hover'da da toplam satırı koyu yazı */
        .ag-theme-alpine-dark .ag-row-pinned:hover > .ag-cell,
        .ag-theme-alpine-dark .ag-row-pinned.ag-row-hover > .ag-cell {
            background-color: #ffd966 !important;
            color: #333 !important;
        }
    `;
    document.head.appendChild(style);
}