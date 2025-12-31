
////let globalJwtToken = null;
////let pivotGridOptions = null;
////let currentTableName = '';    // Seçili tablo adını export için saklayacağız
////// apiBaseUrl değişkeninin global olarak tanımlı olduğunu varsayıyorum.

////document.addEventListener("DOMContentLoaded", function () {

////    // JWT token al
////    const tokenFromDom = document.getElementById("jwtToken")?.value;
////    globalJwtToken = tokenFromDom;

////    if (!globalJwtToken) {
////        alert("JWT Token bulunamadı. Lütfen tekrar giriş yapın.");
////        return;
////    }

////    // Pivot yükleme butonu
////    document
////        .getElementById("btnLoadPivot")
////        .addEventListener("click", loadPivotData);

////    //// Excel'e aktar butonu
////    //document
////    //    .getElementById("btnExportExcel")
////    //    .addEventListener("click", function () {
////    //        if (!pivotGridOptions?.api) {
////    //            alert("Önce verileri yükleyin.");
////    //            return;
////    //        }

////    //        const tableName =
////    //            currentTableName ||
////    //            document.getElementById("tableSelector")?.value ||
////    //            "Pivot";

////    //        const today = new Date().toISOString().slice(0, 10); // YYYY-MM-DD

////    //        // AG Grid, exportDataAsCsv ile CSV dosyası oluşturur
////    //        pivotGridOptions.api.exportDataAsCsv({
////    //            fileName: `${tableName}_${today}.csv`,
////    //            columnSeparator: ';',

////    //            // 🔥 Excel'in tarih yapmasını engellemek ve kod kolonlarını metin olarak tutmak için
////    //            //processCellCallback: (params) => {
////    //            //    if (params.value == null || params.value === '') return '';

////    //            //    const colId = params.column.getColId().toUpperCase();

////    //            //    // Text olarak kalmasını istediğin kolonlar
////    //            //    const textColumns = [
////    //            //        'MUSTERIEKGRUPKOD',
////    //            //        // Gerekiyorsa diğer kod kolonlarını buraya ekleyin:
////    //            //        // 'MUSTERIGRUPKOD',
////    //            //        // 'DIST_KOD',
////    //            //    ];

////    //            //    if (textColumns.includes(colId)) {
////    //            //        // Başına tab (\t) ekleyince Excel bunu TEXT olarak algılar
////    //            //        return '\t' + params.value.toString();
////    //            //    }

////    //            //    return params.value;
////    //            //}
////    //            useValueFormatterForExport: true,
////    //            processCellCallback: (params) => {
////    //                if (params.value == null || params.value === '') return '';

////    //                const colId = params.column.getColId().toUpperCase();

////    //                const textColumns = ['MUSTERIEKGRUPKOD'];

////    //                if (textColumns.includes(colId)) {
////    //                    return '\t' + params.value.toString();
////    //                }

////    //                const num = Number(params.value);
////    //                if (!isNaN(num)) {
////    //                    // 🔥 EXCEL FORMÜLÜ OLARAK VER
////    //                    // Excel TR ayarına göre virgüllü hesaplar
////    //                    return '=' + num.toString().replace('.', ',');
////    //                }

////    //                return params.value;
////    //            }



////    //        });
////    //    });
////    // Excel'e aktar butonu
////    document
////        .getElementById("btnExportExcel")
////        .addEventListener("click", function () {

////            if (!currentTableName) {
////                alert("Önce verileri yükleyin.");
////                return;
////            }

////            // 🔥 CSV YOK – GERÇEK EXCEL (.xlsx)
////            const url =
////                `${apiBaseUrl}pivots/export-excel?tableName=${encodeURIComponent(currentTableName)}`;

////            // JWT gerekiyorsa (senin projende var)
////            fetch(url, {
////                method: 'GET',
////                headers: {
////                    'Authorization': 'Bearer ' + globalJwtToken
////                }
////            })
////                .then(response => {
////                    if (!response.ok) {
////                        throw new Error("Excel indirilemedi");
////                    }
////                    return response.blob();
////                })
////                .then(blob => {
////                    const today = new Date().toISOString().slice(0, 10);
////                    const fileName = `${currentTableName}_${today}.xlsx`;

////                    const link = document.createElement("a");
////                    link.href = window.URL.createObjectURL(blob);
////                    link.download = fileName;
////                    document.body.appendChild(link);
////                    link.click();
////                    document.body.removeChild(link);
////                })
////                .catch(err => {
////                    console.error(err);
////                    alert("Excel indirme sırasında hata oluştu.");
////                });
////        });

////});

////// PIVOT DATA YÜKLEME
////async function loadPivotData() {

////    const selectedTable = document.getElementById("tableSelector").value;

////    if (!selectedTable) {
////        alert("Lütfen bir tablo seçiniz.");
////        return;
////    }

////    currentTableName = selectedTable;  // ⬅ Export için tablo adını kaydet

////    const payload = {
////        tableName: selectedTable
////    };

////    try {

////        console.log("📤 API POST:", `${apiBaseUrl}pivots/get`, payload);

////        const response = await fetch(`${apiBaseUrl}pivots/get`, {
////            method: "POST",
////            headers: {
////                "Content-Type": "application/json",
////                "Authorization": "Bearer " + globalJwtToken
////            },
////            body: JSON.stringify(payload)
////        });

////        const raw = await response.text();
////        console.log("🔥 API RAW RESPONSE:", raw);

////        let data;
////        try {
////            data = JSON.parse(raw);
////        } catch (err) {
////            console.error("❌ JSON parse hatası:", err);
////            alert("API JSON yerine farklı veri döndürdü. Konsola bak.");
////            return;
////        }

////        if (!Array.isArray(data) || data.length === 0) {
////            alert("Bu tablo için veri bulunamadı.");
////            return;
////        }

////        const columnDefs = buildColumnDefs(data);
////        renderPivotGrid(columnDefs, data);

////    } catch (error) {
////        console.error("Pivot API hata:", error);
////        alert("Sunucu hatası! Pivot verisi alınamadı.");
////    }
////}


////// DİNAMİK KOLON OLUŞTURMA

////function buildColumnDefs(data) {
////    const firstRow = data[0];

////    // Parasal kolonları tespit etmek için anahtar kelimeler
////    const moneyKeywords = [
////        "CIRO", "FIYAT", "NET", "BRUT", "KATILIM",
////        "BUTCE", "TUTAR", "TOPLAM", "ALTI", "KALAN", "PRICE","ALINANHIZMET", "SAYISI"
////    ];

////    // AY kolonları (bunlar da parasal kabul edilecek)
////    const monthColumns = [
////        "OCAK", "ŞUBAT", "SUBAT", "MART", "NİSAN", "NISAN",
////        "MAYIS", "HAZİRAN", "HAZIRAN", "TEMMUZ", "AĞUSTOS", "AGUSTOS",
////        "EYLÜL", "EYLUL", "EKİM", "EKIM", "KASIM", "ARALIK"
////    ];

////    return Object.keys(firstRow).map(key => {
////        const upperKey = key.toUpperCase();

////        // Parasal mı?
////        const isMoney =
////            moneyKeywords.some(k => upperKey.includes(k)) ||
////            monthColumns.some(m => upperKey === m);

////        return {
////            field: key,
////            sortable: true,
////            filter: true,
////            resizable: true,
////            enableRowGroup: true,
////            enablePivot: true,
////            enableValue: true,

////            // Eğer parasal kolon ise → Türkçe format + kusuratsız
////            valueFormatter: isMoney
////                ? (params) => {
////                    if (params.value == null || params.value === "") return "";
////                    const num = Number(params.value);
////                    if (isNaN(num)) return params.value;
////                    return num.toLocaleString("tr-TR", {
////                        minimumFractionDigits: 0,
////                        maximumFractionDigits: 0
////                    });
////                }
////                : undefined
////        };
////    });
////}


////// AG-GRID PIVOT RENDER

////function renderPivotGrid(columnDefs, rowData) {

////    const gridDiv = document.querySelector("#pivotGrid");

////    if (pivotGridOptions?.api) {
////        // Eski grid varsa yok et (destroy)
////        pivotGridOptions.api.destroy();
////    }

////    pivotGridOptions = {
////        columnDefs,
////        rowData,

////        pivotMode: false,

////        rowGroupPanelShow: 'always',

////        sideBar: {
////            toolPanels: [
////                {
////                    id: 'columns',
////                    labelDefault: 'Sütunlar',
////                    iconKey: 'columns',
////                    toolPanel: 'agColumnsToolPanel'
////                }
////            ],
////            defaultToolPanel: 'columns'
////        },

////        animateRows: true,

////        suppressDragLeaveHidesColumns: false,

////        getRowStyle: params => {
////            if (params.node.rowIndex % 2 === 0) {
////                return { backgroundColor: "#f0f2f5" };  // çok açık gri
////            }
////            return { backgroundColor: "#ffffff" };
////        },


////        defaultColDef: {
////            sortable: true,
////            filter: true,
////            resizable: true,
////            minWidth: 50, // ⬅ Sütunların istediğiniz kadar daralması için düşük bir değer
////            // flex: 1 Kaldırıldı, sütunların serbestçe daralmasına izin verildi
////            enableRowGroup: true,
////            enableValue: true,
////            // ⬅ Sütun başlıklarını daha kalın ve modern yapmak için eklendi
////            headerClass: 'modern-header-cell'
////        },

////        // 📌 TÜRKÇE LOCALE EKLENDİ
////        localeText: {
////            // Filtre seçenekleri
////            contains: 'İçerir',
////            notContains: 'İçermez',
////            equals: 'Eşittir',
////            notEqual: 'Eşit Değil',
////            startsWith: 'İle Başlar',
////            endsWith: 'İle Biter',
////            blank: 'Boş',
////            notBlank: 'Boş Değil',

////            // Ortak metinler
////            filterOoo: 'Filtre...',
////            applyFilter: 'Uygula',
////            clearFilter: 'Temizle',
////            andCondition: 'Ve',
////            orCondition: 'Veya',
////            reset: 'Sıfırla',
////            searchOoo: 'Ara...',
////            selectAll: 'Tümünü Seç',
////            noMatches: 'Eşleşme Yok',

////            // Tool panel
////            columns: 'Sütunlar',
////            filters: 'Filtreler',

////            // Menü
////            pinColumn: 'Sütunu Sabitle',
////            autosizeThiscolumn: 'Bu Sütunu Otomatik Genişlet',
////            autosizeAllColumns: 'Tüm Sütunları Otomatik Genişlet',
////            groupBy: 'Grupla',
////            ungroupBy: 'Gruptan Çıkar',

////            // Gruplama
////            rowGroupColumnsEmptyMessage: 'Gruplamak için sütun buraya sürükleyin',
////            valuesColumnsEmptyMessage: 'Özet için sütun buraya sürükleyin',
////            pivotColumnsEmptyMessage: 'Pivot için sütun buraya sürükleyin'
////        }
////    };

////    new agGrid.Grid(gridDiv, pivotGridOptions);
////}

//let globalJwtToken = null;
//let pivotGridOptions = null;
//let currentTableName = '';

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

//    // Excel'e aktar butonu - Grid'den CSV olarak export
//    document
//        .getElementById("btnExportExcel")
//        .addEventListener("click", function () {

//            if (!pivotGridOptions?.api) {
//                alert("Önce verileri yükleyin.");
//                return;
//            }

//            const tableName = currentTableName || "Pivot";
//            const today = new Date().toISOString().slice(0, 10);

//            // AG Grid'den CSV export (toplam satırı dahil)
//            pivotGridOptions.api.exportDataAsCsv({
//                fileName: `${tableName}_${today}.csv`,
//                columnSeparator: ';',

//                processCellCallback: (params) => {
//                    if (params.value == null || params.value === '') return '';

//                    // Sayısal değerleri Excel formülü olarak ver
//                    const num = Number(params.value);
//                    if (!isNaN(num) && typeof params.value !== 'string') {
//                        // Excel TR ayarına göre virgüllü
//                        return '=' + num.toString().replace('.', ',');
//                    }

//                    return params.value;
//                }
//            });
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

//    // 🆕 Toplam satırını hesapla
//    const totalRow = calculateTotalRow(columnDefs, rowData);

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

let globalJwtToken = null;
let pivotGridOptions = null;
let currentTableName = '';
let currentTotalRow = null;  // 🆕 Toplam satırını global sakla

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


// AG-GRID PIVOT RENDER
function renderPivotGrid(columnDefs, rowData) {

    const gridDiv = document.querySelector("#pivotGrid");

    if (pivotGridOptions?.api) {
        pivotGridOptions.api.destroy();
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
            // 🆕 Toplam satırı için özel stil
            if (params.node.rowPinned) {
                return {
                    backgroundColor: "#ffd966",
                    fontWeight: "bold",
                    borderTop: "2px solid #333"
                };
            }

            if (params.node.rowIndex % 2 === 0) {
                return { backgroundColor: "#f0f2f5" };
            }
            return { backgroundColor: "#ffffff" };
        },

        defaultColDef: {
            sortable: true,
            filter: true,
            resizable: true,
            minWidth: 50,
            enableRowGroup: true,
            enableValue: true,
            headerClass: 'modern-header-cell'
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

    new agGrid.Grid(gridDiv, pivotGridOptions);
}