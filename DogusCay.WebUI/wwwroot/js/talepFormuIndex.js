
const apiBaseUrl = "https://localhost:7076/api";
const token = document.getElementById("jwtToken")?.value;

async function deleteTalep(id) {
    if (!confirm("Bu talep formunu silmek istediğinize emin misiniz?")) return;

    if (!token) {
        alert("Oturum süresi dolmuş olabilir. Lütfen tekrar giriş yapın.");
        return;
    }

    try {
        const response = await fetch(`${apiBaseUrl}/talepforms/${id}`, {
            method: 'DELETE',
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (response.ok) {
            alert("Talep başarıyla silindi.");
            location.reload();
        } else {
            const error = await response.text();
            alert("Silme başarısız: " + error);
        }
    } catch (err) {
        alert("Hata oluştu: " + err);
    }
}

async function approveTalep(id) {
    await updateStatus(id, 'approve', "Talep onaylandı.");
}

async function rejectTalep(id) {
    await updateStatus(id, 'reject', "Talep reddedildi.");
}

async function updateStatus(id, action, successMessage) {
    if (!token) {
        alert("Oturum süresi dolmuş olabilir. Lütfen tekrar giriş yapın.");
        return;
    }

    try {
        const response = await fetch(`${apiBaseUrl}/talepforms/${action}/${id}`, {
            method: 'POST',
            headers: { 'Authorization': `Bearer ${token}` }
        });

        if (response.ok) {
            alert(successMessage);
            location.reload();
        } else {
            const error = await response.text();
            alert("İşlem başarısız: " + error);
        }
    } catch (err) {
        alert("Sunucu hatası: " + err);
    }
}

async function gonderKampanyaDonusu(formId) {
    const input = document.getElementById(`kampanyaInput_${formId}`);
    const value = parseInt(input.value);
    // Koli içi toplam adedi alınır
    const koliInput = document.getElementById(`koliIciToplamAdet_${formId}`);
    const koliIciToplamAdet = koliInput ? parseInt(koliInput.value) : null;
    if (!token) {
        alert("Oturum süresi dolmuş olabilir.");
        return;
    }

    if (isNaN(value) || value < 0) {
        alert("Lütfen geçerli bir sayı girin.");
        return;
    }
    // Kampanya dönüş adedi kontrolü
    if (koliIciToplamAdet !== null && value > koliIciToplamAdet) {
        alert("🛑 Kampanya dönüş adedi, koli içi toplam adetten büyük olamaz!");
        return;
    }
    try {
        const response = await fetch(`${apiBaseUrl}/talepforms/update-donus/${formId}`, {
            method: "PATCH",
            headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json"
            },
            body: JSON.stringify(value)
        });

        if (response.ok) {
            alert("Kampanya dönüş adedi kaydedildi.");
            location.reload();
        } else {
            const error = await response.text();
            alert("Hata: " + error);
        }
    } catch (err) {
        alert("Sunucu hatası: " + err);
    }
}
async function uploadImage(formId) {
    const token = document.getElementById("jwtToken")?.value; // 🔁 token eksikti, eklendi
    const input = document.getElementById(`imageInput_${formId}`);

    if (!token) {
        alert("Oturum süresi dolmuş olabilir.");
        return;
    }

    if (!input || !input.files || input.files.length === 0) {
        alert("Lütfen bir görsel seçin.");
        return;
    }

    const file = input.files[0];
    const formData = new FormData();
    formData.append("image", file);

    try {
        const response = await fetch(`${apiBaseUrl}/talepforms/upload-image/${formId}`, {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`
                // ❗ Content-Type belirtme! FormData ile fetch otomatik ayarlar.
            },
            body: formData
        });

        if (response.ok) {
            alert("Resim başarıyla yüklendi.");
            location.reload();
        } else {
            const error = await response.text();
            alert("Yükleme başarısız: " + error);
        }
    } catch (err) {
        alert("Sunucu hatası: " + err);
    }
}


