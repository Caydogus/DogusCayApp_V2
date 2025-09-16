import React, { useState, useEffect } from "react";

const apiBaseUrl = "https://localhost:7076/api"; // Kendi API adresini gir!

// JWT tokenlý fetch için yardýmcý fonksiyon
function authFetch(url, options = {}) {
    const jwtToken = localStorage.getItem("jwtToken") || "";
    return fetch(url, {
        ...options,
        headers: {
            ...(options.headers || {}),
            "Authorization": `Bearer ${jwtToken}`
        }
    });
}

function formatTL(value) {
    return Number(value).toLocaleString('tr-TR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
}

function TalepForm() {
    // Kategori zinciri
    const [categories, setCategories] = useState([]);
    const [categoryId, setCategoryId] = useState("");
    const [subCategories, setSubCategories] = useState([]);
    const [subCategoryId, setSubCategoryId] = useState("");
    const [subSubCategories, setSubSubCategories] = useState([]);
    const [subSubCategoryId, setSubSubCategoryId] = useState("");
    const [products, setProducts] = useState([]);
    const [productId, setProductId] = useState("");

    // Kanal zinciri
    const [channels, setChannels] = useState([]);
    const [kanalId, setKanalId] = useState("");
    const [distributors, setDistributors] = useState([]);
    const [distributorId, setDistributorId] = useState("");
    const [pointGroupTypes, setPointGroupTypes] = useState([]);
    const [pointGroupTypeId, setPointGroupTypeId] = useState("");
    const [points, setPoints] = useState([]);
    const [pointId, setPointId] = useState("");

    // Form & hesaplama
    const [form, setForm] = useState({
        price: "",
        quantity: 1,
        iskonto1: "",
        iskonto2: "",
        iskonto3: "",
        iskonto4: "",
        sabitBedelTL: "",
        adetFarkDonusuTL: "",
        approximateWeightKg: "",
        koliIciAdet: "",
        erpCode: "",
        note: "",
        validFrom: new Date().toISOString().split('T')[0],
        validTo: new Date(Date.now() + 7 * 24 * 60 * 60 * 1000).toISOString().split('T')[0],
    });

    const [calc, setCalc] = useState({
        total: 0,
        totalBrut: 0,
        koliIciToplamAdet: 0,
        koliToplamAgirligi: 0,
        listeFiyat: 0,
        sonAdetFiyat: 0,
        maliyet: 0
    });

    const [message, setMessage] = useState("");
    const jwtToken = localStorage.getItem("jwtToken") || "";

    // Kategori zinciri
    useEffect(() => {
        authFetch(`${apiBaseUrl}/categories/MainCategories`)
            .then(res => {
                if (!res.ok) throw new Error("Kategori verisi alýnamadý");
                return res.json();
            })
            .then(setCategories)
            .catch(() => setMessage("Kategori verisi alýnamadý"));
    }, []);

    useEffect(() => {
        if (categoryId)
            authFetch(`${apiBaseUrl}/categories/${categoryId}/children`)
                .then(res => res.json())
                .then(setSubCategories);
        else setSubCategories([]);
        setSubCategoryId("");
        setSubSubCategoryId("");
        setProducts([]);
        setProductId("");
    }, [categoryId]);

    useEffect(() => {
        if (subCategoryId)
            authFetch(`${apiBaseUrl}/categories/${subCategoryId}/children`)
                .then(res => res.json())
                .then(setSubSubCategories);
        else setSubSubCategories([]);
        setSubSubCategoryId("");
        setProducts([]);
        setProductId("");
    }, [subCategoryId]);

    useEffect(() => {
        if (subSubCategoryId)
            authFetch(`${apiBaseUrl}/categories/${subSubCategoryId}/products`)
                .then(res => res.json())
                .then(setProducts);
        else setProducts([]);
        setProductId("");
    }, [subSubCategoryId]);

    useEffect(() => {
        if (productId) {
            authFetch(`${apiBaseUrl}/products/get-product-info/${productId}`)
                .then(res => res.json())
                .then(data => setForm(f => ({
                    ...f,
                    price: data.price || "",
                    koliIciAdet: data.koliIciAdet || "",
                    approximateWeightKg: data.approximateWeightKg || "",
                    erpCode: data.erpCode || ""
                })));
        }
    }, [productId]);

    // Kanal zinciri
    useEffect(() => {
        authFetch(`${apiBaseUrl}/Kanals/dropdown`)
            .then(res => res.json())
            .then(setChannels)
            .catch(() => setMessage("Kanal verisi alýnamadý"));
    }, []);

    useEffect(() => {
        setDistributors([]);
        setDistributorId("");
        setPointGroupTypes([]);
        setPointGroupTypeId("");
        setPoints([]);
        setPointId("");
        if (kanalId === "4") {
            authFetch(`${apiBaseUrl}/distributors/by-kanal/${kanalId}`)
                .then(res => res.json())
                .then(setDistributors);
        } else if (kanalId === "5" || kanalId === "6") {
            authFetch(`${apiBaseUrl}/points/by-kanal/${kanalId}`)
                .then(res => res.json())
                .then(setPoints);
        }
    }, [kanalId]);

    useEffect(() => {
        setPointGroupTypes([]);
        setPointGroupTypeId("");
        setPoints([]);
        setPointId("");
        if (distributorId) {
            authFetch(`${apiBaseUrl}/pointgrouptypes/by-distributor/${distributorId}`)
                .then(res => res.json())
                .then(setPointGroupTypes);
        }
    }, [distributorId]);

    useEffect(() => {
        setPoints([]);
        setPointId("");
        if (pointGroupTypeId && distributorId) {
            authFetch(`${apiBaseUrl}/points/by-group/${pointGroupTypeId}/distributor/${distributorId}`)
                .then(res => res.json())
                .then(setPoints);
        }
    }, [pointGroupTypeId, distributorId]);

    useEffect(() => {
        let {
            price, quantity, iskonto1, iskonto2, iskonto3, iskonto4, sabitBedelTL,
            adetFarkDonusuTL, approximateWeightKg, koliIciAdet
        } = form;
        price = parseFloat(price) || 0;
        quantity = parseInt(quantity) || 1;
        iskonto1 = parseFloat(iskonto1) || 0;
        iskonto2 = parseFloat(iskonto2) || 0;
        iskonto3 = parseFloat(iskonto3) || 0;
        iskonto4 = parseFloat(iskonto4) || 0;
        sabitBedelTL = parseFloat(sabitBedelTL) || 0;
        adetFarkDonusuTL = parseFloat(adetFarkDonusuTL) || 0;
        approximateWeightKg = parseFloat(approximateWeightKg) || 0;
        koliIciAdet = parseInt(koliIciAdet) || 0;

        let toplam = price * quantity;
        [iskonto1, iskonto2, iskonto3, iskonto4].forEach(isk => {
            if (isk > 0) toplam *= (100 - isk) / 100;
        });
        if (sabitBedelTL > 0) {
            toplam -= sabitBedelTL;
            if (toplam < 0) toplam = 0;
        }

        const totalBrut = quantity * price;
        const koliIciToplamAdet = quantity * koliIciAdet;
        const koliToplamAgirligi = (quantity * approximateWeightKg).toFixed(2);
        const listeFiyat = koliIciAdet > 0 ? (price / koliIciAdet).toFixed(2) : "0.00";
        let sonAdetFiyat = koliIciToplamAdet > 0 ? toplam / koliIciToplamAdet : 0;

        if (adetFarkDonusuTL > 0) {
            sonAdetFiyat -= adetFarkDonusuTL;
            toplam = sonAdetFiyat * koliIciToplamAdet;
        }

        let maliyet = 0;
        if (totalBrut !== 0) {
            maliyet = ((totalBrut - toplam) / totalBrut) * 100;
        }

        setCalc({
            total: toplam,
            totalBrut: totalBrut,
            koliIciToplamAdet: koliIciToplamAdet,
            koliToplamAgirligi: koliToplamAgirligi,
            listeFiyat: listeFiyat,
            sonAdetFiyat: sonAdetFiyat,
            maliyet: maliyet
        });
    }, [form]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setMessage("");
        if (!jwtToken) {
            setMessage("Oturum süresi dolmuþ olabilir. Lütfen tekrar giriþ yapýn.");
            return;
        }
        if (!kanalId || !pointId || !productId || !form.quantity) {
            setMessage("Zorunlu alanlarý doldurunuz!");
            return;
        }
        if (form.validTo < form.validFrom) {
            setMessage("Bitiþ tarihi, baþlangýç tarihinden önce olamaz!");
            return;
        }

        const dto = {
            kanalId: parseInt(kanalId),
            distributorId: distributorId ? parseInt(distributorId) : null,
            pointGroupTypeId: pointGroupTypeId ? parseInt(pointGroupTypeId) : null,
            pointId: pointId ? parseInt(pointId) : null,
            categoryId: categoryId ? parseInt(categoryId) : 0,
            subCategoryId: subCategoryId ? parseInt(subCategoryId) : null,
            subSubCategoryId: subSubCategoryId ? parseInt(subSubCategoryId) : null,
            productId: productId ? parseInt(productId) : 0,
            productName: products.find(p => p.productId === parseInt(productId))?.productName || "",
            erpCode: form.erpCode,
            approximateWeightKg: parseFloat(form.approximateWeightKg) || 0,
            koliIciAdet: parseInt(form.koliIciAdet) || 0,
            sabitBedelTL: parseFloat(form.sabitBedelTL) || 0,
            quantity: parseInt(form.quantity) || 1,
            price: parseFloat(form.price) || 0,
            iskonto1: parseFloat(form.iskonto1) || 0,
            iskonto2: parseFloat(form.iskonto2) || 0,
            iskonto3: parseFloat(form.iskonto3) || 0,
            iskonto4: parseFloat(form.iskonto4) || 0,
            koliToplamAgirligiKg: parseFloat(calc.koliToplamAgirligi) || 0,
            koliIciToplamAdet: parseInt(calc.koliIciToplamAdet) || 0,
            listeFiyat: parseFloat(calc.listeFiyat) || 0,
            sonAdetFiyati: parseFloat(calc.sonAdetFiyat) || 0,
            adetFarkDonusuTL: parseFloat(form.adetFarkDonusuTL) || 0,
            validFrom: form.validFrom,
            validTo: form.validTo,
            note: form.note,
            total: parseFloat(calc.total) || 0,
            totalBrut: parseFloat(calc.totalBrut) || 0
        };

        try {
            const res = await authFetch(`${apiBaseUrl}/talepforms`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(dto)
            });

            if (res.ok) {
                setMessage("Talep baþarýyla oluþturuldu!");
                setTimeout(() => {
                    window.location.href = "/Admin/TalepForm/Index";
                }, 1000);
            } else {
                const resultText = await res.text();
                setMessage("Talep kaydedilemedi: " + resultText);
            }
        } catch (err) {
            setMessage("Bir hata oluþtu: " + (err.message || "Bilinmeyen hata"));
        }
    };

    // RENDER
    return (
        <form onSubmit={handleSubmit}>
            <h2>Talep Formu (React)</h2>
            {message && <div style={{ color: "red", marginBottom: "10px" }}>{message}</div>}

            {/* Kategori zinciri */}
            <div>
                <label>Kategori:</label>
                <select value={categoryId} onChange={e => setCategoryId(e.target.value)}>
                    <option value="">Seçiniz</option>
                    {categories.map(cat => (
                        <option key={cat.categoryId} value={cat.categoryId}>{cat.categoryName}</option>
                    ))}
                </select>
            </div>
            <div>
                <label>Alt Kategori:</label>
                <select value={subCategoryId} onChange={e => setSubCategoryId(e.target.value)}>
                    <option value="">Seçiniz</option>
                    {subCategories.map(sub => (
                        <option key={sub.categoryId} value={sub.categoryId}>{sub.categoryName}</option>
                    ))}
                </select>
            </div>
            <div>
                <label>Alt Alt Kategori:</label>
                <select value={subSubCategoryId} onChange={e => setSubSubCategoryId(e.target.value)}>
                    <option value="">Seçiniz</option>
                    {subSubCategories.map(subsub => (
                        <option key={subsub.categoryId} value={subsub.categoryId}>{subsub.categoryName}</option>
                    ))}
                </select>
            </div>
            <div>
                <label>Ürün:</label>
                <select value={productId} onChange={e => setProductId(e.target.value)}>
                    <option value="">Seçiniz</option>
                    {products.map(p => (
                        <option key={p.productId} value={p.productId}>{p.productName}</option>
                    ))}
                </select>
            </div>

            {/* Kanal zinciri */}
            <div>
                <label>Kanal:</label>
                <select value={kanalId} onChange={e => setKanalId(e.target.value)}>
                    <option value="">Seçiniz</option>
                    {channels.map(ch => (
                        <option key={ch.KanalId} value={ch.KanalId}>{ch.KanalName}</option>
                    ))}
                </select>
            </div>
            {kanalId === "4" && (
                <div>
                    <label>Distributor:</label>
                    <select value={distributorId} onChange={e => setDistributorId(e.target.value)}>
                        <option value="">Seçiniz</option>
                        {distributors.map(d => (
                            <option key={d.distributorId} value={d.distributorId}>{d.distributorName}</option>
                        ))}
                    </select>
                </div>
            )}
            {distributorId && (
                <div>
                    <label>Nokta Grubu:</label>
                    <select value={pointGroupTypeId} onChange={e => setPointGroupTypeId(e.target.value)}>
                        <option value="">Seçiniz</option>
                        {pointGroupTypes.map(g => (
                            <option key={g.pointGroupTypeId} value={g.pointGroupTypeId}>{g.pointGroupTypeName}</option>
                        ))}
                    </select>
                </div>
            )}
            {(pointGroupTypeId || kanalId === "5" || kanalId === "6") && (
                <div>
                    <label>Nokta:</label>
                    <select value={pointId} onChange={e => setPointId(e.target.value)}>
                        <option value="">Seçiniz</option>
                        {points.map(p => (
                            <option key={p.pointId} value={p.pointId}>{p.pointName}</option>
                        ))}
                    </select>
                </div>
            )}

            {/* Ürün bilgileri ve hesaplama alanlarý */}
            <div>
                <label>Fiyat (TL):</label>
                <input type="number" step="0.01" value={form.price} onChange={e => setForm(f => ({ ...f, price: e.target.value }))} />
            </div>
            <div>
                <label>Adet:</label>
                <input type="number" min={1} value={form.quantity} onChange={e => setForm(f => ({ ...f, quantity: e.target.value }))} />
            </div>
            <div>
                <label>Ýskonto 1 (%):</label>
                <input type="number" value={form.iskonto1} onChange={e => setForm(f => ({ ...f, iskonto1: e.target.value }))} />
            </div>
            <div>
                <label>Ýskonto 2 (%):</label>
                <input type="number" value={form.iskonto2} onChange={e => setForm(f => ({ ...f, iskonto2: e.target.value }))} />
            </div>
            <div>
                <label>Ýskonto 3 (%):</label>
                <input type="number" value={form.iskonto3} onChange={e => setForm(f => ({ ...f, iskonto3: e.target.value }))} />
            </div>
            <div>
                <label>Ýskonto 4 (%):</label>
                <input type="number" value={form.iskonto4} onChange={e => setForm(f => ({ ...f, iskonto4: e.target.value }))} />
            </div>
            <div>
                <label>Sabit Bedel (TL):</label>
                <input type="number" value={form.sabitBedelTL} onChange={e => setForm(f => ({ ...f, sabitBedelTL: e.target.value }))} />
            </div>
            <div>
                <label>Adet Farký (TL):</label>
                <input type="number" value={form.adetFarkDonusuTL} onChange={e => setForm(f => ({ ...f, adetFarkDonusuTL: e.target.value }))} />
            </div>
            <div>
                <label>Koli Ýçi Adet:</label>
                <input type="number" value={form.koliIciAdet} onChange={e => setForm(f => ({ ...f, koliIciAdet: e.target.value }))} />
            </div>
            <div>
                <label>Ürün KG:</label>
                <input type="number" value={form.approximateWeightKg} onChange={e => setForm(f => ({ ...f, approximateWeightKg: e.target.value }))} />
            </div>
            <div>
                <label>ERP Kod:</label>
                <input type="text" value={form.erpCode} onChange={e => setForm(f => ({ ...f, erpCode: e.target.value }))} />
            </div>
            <div>
                <label>Not:</label>
                <input type="text" value={form.note} onChange={e => setForm(f => ({ ...f, note: e.target.value }))} />
            </div>
            <div>
                <label>Baþlangýç Tarihi:</label>
                <input type="date" value={form.validFrom} onChange={e => setForm(f => ({ ...f, validFrom: e.target.value }))} />
            </div>
            <div>
                <label>Bitiþ Tarihi:</label>
                <input type="date" value={form.validTo} onChange={e => setForm(f => ({ ...f, validTo: e.target.value }))} />
            </div>

            {/* Hesaplanmýþ Sonuçlar */}
            <div>
                <label>Toplam Tutar (TL):</label>
                <input type="text" readOnly value={formatTL(calc.total)} />
            </div>
            <div>
                <label>Brüt Tutar (TL):</label>
                <input type="text" readOnly value={formatTL(calc.totalBrut)} />
            </div>
            <div>
                <label>Koli Ýçi Toplam Adet:</label>
                <input type="text" readOnly value={calc.koliIciToplamAdet} />
            </div>
            <div>
                <label>Koli Toplam Aðýrlýk (kg):</label>
                <input type="text" readOnly value={calc.koliToplamAgirligi} />
            </div>
            <div>
                <label>Liste Fiyatý (TL):</label>
                <input type="text" readOnly value={formatTL(calc.listeFiyat)} />
            </div>
            <div>
                <label>Son Adet Fiyatý (TL):</label>
                <input type="text" readOnly value={formatTL(calc.sonAdetFiyat)} />
            </div>
            <div>
                <label>Maliyet (%):</label>
                <input type="text" readOnly value={calc.maliyet.toFixed(2)} />
            </div>

            <button type="submit" style={{ marginTop: "20px" }}>Kaydet</button>
        </form>
    );
}

export default TalepForm;
