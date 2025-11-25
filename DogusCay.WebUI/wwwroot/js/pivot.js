let globalJwtToken = null;
let pivotGridOptions = null;

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
            if (pivotGridOptions?.api) {
                pivotGridOptions.api.exportDataAsExcel();
            }
        });
});

//  PIVOT DATA YÜKLEME
async function loadPivotData() {

    const selectedTable = document.getElementById("tableSelector").value;

    if (!selectedTable) {
        alert("Lütfen bir tablo seçiniz.");
        return;
    }

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


//  DİNAMİK KOLON OLUŞTURMA

function buildColumnDefs(data) {
    const firstRow = data[0];

    // Parasal kolonları tespit etmek için anahtar kelimeler
    const moneyKeywords = [
        "CIRO", "FIYAT", "NET", "BRUT", "KATILIM",
        "BUTCE", "TUTAR", "TOPLAM", "ALTI", "KALAN", "PRICE", "SAYISI"
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


//  AG-GRID PIVOT RENDER

function renderPivotGrid(columnDefs, rowData) {

    const gridDiv = document.querySelector("#pivotGrid");

    if (pivotGridOptions?.api) {
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

        defaultColDef: {
            sortable: true,
            filter: true,
            resizable: true,
            minWidth: 150,
            flex: 1,
            enableRowGroup: true,
            enableValue: true
        },

        //  📌 TÜRKÇE LOCALE EKLENDİ
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
