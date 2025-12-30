//let globalJwtToken = null;
//let pivotGridOptions = null;
//let currentTableName = '';   // ⬅ Seçili tablo adını export için saklayacağız

//document.addEventListener("DOMContentLoaded", function () {

//    // JWT token al
//    const tokenFromDom = document.getElementById("jwtToken")?.value;
//    globalJwtToken = tokenFromDom;

//    if (!globalJwtToken) {
//        alert("JWT Token bulunamadı. Lütfen tekrar giriş yapın.");
//        return;
//    }

//    // Pivot yükleme butonu
//    document
//        .getElementById("btnLoadPivot")
//        .addEventListener("click", loadPivotData);

//    // Excel'e aktar butonu
//    document
//        .getElementById("btnExportExcel")
//        .addEventListener("click", function () {
//            if (!pivotGridOptions?.api) {
//                alert("Önce verileri yükleyin.");
//                return;
//            }

//            const tableName =
//                currentTableName ||
//                document.getElementById("tableSelector")?.value ||
//                "Pivot";

//            const today = new Date().toISOString().slice(0, 10); // YYYY-MM-DD

//            pivotGridOptions.api.exportDataAsCsv({
//                fileName: `${tableName}_${today}.csv`,
//                columnSeparator: ';',

//                // 🔥 Excel'in tarih yapmasını engelle
//                processCellCallback: (params) => {
//                    if (params.value == null || params.value === '') return '';

//                    const colId = params.column.getColId().toUpperCase();

//                    // Text olarak kalmasını istediğin kolonlar
//                    const textColumns = [
//                        'MUSTERIEKGRUPKOD',
//                        // gerekiyorsa diğer kod kolonlarını da buraya ekle:
//                        // 'MUSTERIGRUPKOD',
//                        // 'DIST_KOD',
//                    ];

//                    if (textColumns.includes(colId)) {
//                        // Başına tab ekleyince Excel bunu TEXT olarak algılar
//                        return '\t' + params.value.toString();
//                    }

//                    return params.value;
//                }
//            });
//        });

//});

////  PIVOT DATA YÜKLEME
//async function loadPivotData() {

//    const selectedTable = document.getElementById("tableSelector").value;

//    if (!selectedTable) {
//        alert("Lütfen bir tablo seçiniz.");
//        return;
//    }

//    currentTableName = selectedTable;  // ⬅ Export için tablo adını kaydet

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


////  DİNAMİK KOLON OLUŞTURMA

//function buildColumnDefs(data) {
//    const firstRow = data[0];

//    // Parasal kolonları tespit etmek için anahtar kelimeler
//    const moneyKeywords = [
//        "CIRO", "FIYAT", "NET", "BRUT", "KATILIM",
//        "BUTCE", "TUTAR", "TOPLAM", "ALTI", "KALAN", "PRICE", "SAYISI"
//    ];

//    // AY kolonları (bunlar da parasal kabul edilecek)
//    const monthColumns = [
//        "OCAK", "ŞUBAT", "SUBAT", "MART", "NİSAN", "NISAN",
//        "MAYIS", "HAZİRAN", "HAZIRAN", "TEMMUZ", "AĞUSTOS", "AGUSTOS",
//        "EYLÜL", "EYLUL", "EKİM", "EKIM", "KASIM", "ARALIK"
//    ];

//    return Object.keys(firstRow).map(key => {
//        const upperKey = key.toUpperCase();

//        // Parasal mı?
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

//            // Eğer parasal kolon ise → Türkçe format + kusuratsız
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


////  AG-GRID PIVOT RENDER

//function renderPivotGrid(columnDefs, rowData) {

//    const gridDiv = document.querySelector("#pivotGrid");

//    if (pivotGridOptions?.api) {
//        pivotGridOptions.api.destroy();
//    }

//    pivotGridOptions = {
//        columnDefs,
//        rowData,

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
//            if (params.node.rowIndex % 2 === 0) {
//                return { backgroundColor: "#f0f2f5" };  // çok açık gri
//            }
//            return { backgroundColor: "#ffffff" };
//        },


//        defaultColDef: {
//            sortable: true,
//            filter: true,
//            resizable: true,
//            minWidth: 150,
//            flex: 1,
//            enableRowGroup: true,
//            enableValue: true
//        },

//        //  📌 TÜRKÇE LOCALE EKLENDİ
//        localeText: {
//            // Filtre seçenekleri
//            contains: 'İçerir',
//            notContains: 'İçermez',
//            equals: 'Eşittir',
//            notEqual: 'Eşit Değil',
//            startsWith: 'İle Başlar',
//            endsWith: 'İle Biter',
//            blank: 'Boş',
//            notBlank: 'Boş Değil',

//            // Ortak metinler
//            filterOoo: 'Filtre...',
//            applyFilter: 'Uygula',
//            clearFilter: 'Temizle',
//            andCondition: 'Ve',
//            orCondition: 'Veya',
//            reset: 'Sıfırla',
//            searchOoo: 'Ara...',
//            selectAll: 'Tümünü Seç',
//            noMatches: 'Eşleşme Yok',

//            // Tool panel
//            columns: 'Sütunlar',
//            filters: 'Filtreler',

//            // Menü
//            pinColumn: 'Sütunu Sabitle',
//            autosizeThiscolumn: 'Bu Sütunu Otomatik Genişlet',
//            autosizeAllColumns: 'Tüm Sütunları Otomatik Genişlet',
//            groupBy: 'Grupla',
//            ungroupBy: 'Gruptan Çıkar',

//            // Gruplama
//            rowGroupColumnsEmptyMessage: 'Gruplamak için sütun buraya sürükleyin',
//            valuesColumnsEmptyMessage: 'Özet için sütun buraya sürükleyin',
//            pivotColumnsEmptyMessage: 'Pivot için sütun buraya sürükleyin'
//        }
//    };

//    new agGrid.Grid(gridDiv, pivotGridOptions);
//}

let globalJwtToken = null;
let pivotGridOptions = null;
let currentTableName = '';    // Seçili tablo adını export için saklayacağız
// apiBaseUrl değişkeninin global olarak tanımlı olduğunu varsayıyorum.

document.addEventListener("DOMContentLoaded", function () {

    // JWT token al
    const tokenFromDom = document.getElementById("jwtToken")?.value;
    globalJwtToken = tokenFromDom;

    if (!globalJwtToken) {
        alert("JWT Token bulunamadı. Lütfen tekrar giriş yapın.");
        return;
    }

    // Pivot yükleme butonu
    document
        .getElementById("btnLoadPivot")
        .addEventListener("click", loadPivotData);

    // Excel'e aktar butonu
    document
        .getElementById("btnExportExcel")
        .addEventListener("click", function () {
            if (!pivotGridOptions?.api) {
                alert("Önce verileri yükleyin.");
                return;
            }

            const tableName =
                currentTableName ||
                document.getElementById("tableSelector")?.value ||
                "Pivot";

            const today = new Date().toISOString().slice(0, 10); // YYYY-MM-DD

            // AG Grid, exportDataAsCsv ile CSV dosyası oluşturur
            pivotGridOptions.api.exportDataAsCsv({
                fileName: `${tableName}_${today}.csv`,
                columnSeparator: ';',

                // 🔥 Excel'in tarih yapmasını engellemek ve kod kolonlarını metin olarak tutmak için
                processCellCallback: (params) => {
                    if (params.value == null || params.value === '') return '';

                    const colId = params.column.getColId().toUpperCase();

                    // Text olarak kalmasını istediğin kolonlar
                    const textColumns = [
                        'MUSTERIEKGRUPKOD',
                        // Gerekiyorsa diğer kod kolonlarını buraya ekleyin:
                        // 'MUSTERIGRUPKOD',
                        // 'DIST_KOD',
                    ];

                    if (textColumns.includes(colId)) {
                        // Başına tab (\t) ekleyince Excel bunu TEXT olarak algılar
                        return '\t' + params.value.toString();
                    }

                    return params.value;
                }
            });
        });

});

// PIVOT DATA YÜKLEME
async function loadPivotData() {

    const selectedTable = document.getElementById("tableSelector").value;

    if (!selectedTable) {
        alert("Lütfen bir tablo seçiniz.");
        return;
    }

    currentTableName = selectedTable;  // ⬅ Export için tablo adını kaydet

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

    // Parasal kolonları tespit etmek için anahtar kelimeler
    const moneyKeywords = [
        "CIRO", "FIYAT", "NET", "BRUT", "KATILIM",
        "BUTCE", "TUTAR", "TOPLAM", "ALTI", "KALAN", "PRICE","ALINANHIZMET", "SAYISI"
    ];

    // AY kolonları (bunlar da parasal kabul edilecek)
    const monthColumns = [
        "OCAK", "ŞUBAT", "SUBAT", "MART", "NİSAN", "NISAN",
        "MAYIS", "HAZİRAN", "HAZIRAN", "TEMMUZ", "AĞUSTOS", "AGUSTOS",
        "EYLÜL", "EYLUL", "EKİM", "EKIM", "KASIM", "ARALIK"
    ];

    return Object.keys(firstRow).map(key => {
        const upperKey = key.toUpperCase();

        // Parasal mı?
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

            // Eğer parasal kolon ise → Türkçe format + kusuratsız
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


// AG-GRID PIVOT RENDER

function renderPivotGrid(columnDefs, rowData) {

    const gridDiv = document.querySelector("#pivotGrid");

    if (pivotGridOptions?.api) {
        // Eski grid varsa yok et (destroy)
        pivotGridOptions.api.destroy();
    }

    pivotGridOptions = {
        columnDefs,
        rowData,

        pivotMode: false,

        rowGroupPanelShow: 'always',

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

        suppressDragLeaveHidesColumns: false,

        getRowStyle: params => {
            if (params.node.rowIndex % 2 === 0) {
                return { backgroundColor: "#f0f2f5" };  // çok açık gri
            }
            return { backgroundColor: "#ffffff" };
        },


        defaultColDef: {
            sortable: true,
            filter: true,
            resizable: true,
            minWidth: 50, // ⬅ Sütunların istediğiniz kadar daralması için düşük bir değer
            // flex: 1 Kaldırıldı, sütunların serbestçe daralmasına izin verildi
            enableRowGroup: true,
            enableValue: true,
            // ⬅ Sütun başlıklarını daha kalın ve modern yapmak için eklendi
            headerClass: 'modern-header-cell'
        },

        // 📌 TÜRKÇE LOCALE EKLENDİ
        localeText: {
            // Filtre seçenekleri
            contains: 'İçerir',
            notContains: 'İçermez',
            equals: 'Eşittir',
            notEqual: 'Eşit Değil',
            startsWith: 'İle Başlar',
            endsWith: 'İle Biter',
            blank: 'Boş',
            notBlank: 'Boş Değil',

            // Ortak metinler
            filterOoo: 'Filtre...',
            applyFilter: 'Uygula',
            clearFilter: 'Temizle',
            andCondition: 'Ve',
            orCondition: 'Veya',
            reset: 'Sıfırla',
            searchOoo: 'Ara...',
            selectAll: 'Tümünü Seç',
            noMatches: 'Eşleşme Yok',

            // Tool panel
            columns: 'Sütunlar',
            filters: 'Filtreler',

            // Menü
            pinColumn: 'Sütunu Sabitle',
            autosizeThiscolumn: 'Bu Sütunu Otomatik Genişlet',
            autosizeAllColumns: 'Tüm Sütunları Otomatik Genişlet',
            groupBy: 'Grupla',
            ungroupBy: 'Gruptan Çıkar',

            // Gruplama
            rowGroupColumnsEmptyMessage: 'Gruplamak için sütun buraya sürükleyin',
            valuesColumnsEmptyMessage: 'Özet için sütun buraya sürükleyin',
            pivotColumnsEmptyMessage: 'Pivot için sütun buraya sürükleyin'
        }
    };

    new agGrid.Grid(gridDiv, pivotGridOptions);
}