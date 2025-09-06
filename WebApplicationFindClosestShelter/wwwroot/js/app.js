const API = "/api";

/* ===================== Map ===================== */
let map = L.map("map", { zoomControl: true }).setView([31.778, 35.235], 12);
L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
    attribution: "&copy; OpenStreetMap"
}).addTo(map);
let markersLayer = L.layerGroup().addTo(map);

/* ---- My location marker (purple pulsing) ---- */
let myMarker = null;
let myAccuracy = null;
const myIcon = L.divIcon({ className: "me-dot" });

function showMyLocation(lat, lon, accuracyMeters) {
    if (!myMarker) {
        myMarker = L.marker([lat, lon], { icon: myIcon, interactive: false }).addTo(map);
    } else {
        myMarker.setLatLng([lat, lon]);
    }
    if (accuracyMeters) {
        if (!myAccuracy) {
            myAccuracy = L.circle([lat, lon], { radius: accuracyMeters, opacity: 0.25, fillOpacity: 0.07 }).addTo(map);
        } else {
            myAccuracy.setLatLng([lat, lon]); myAccuracy.setRadius(accuracyMeters);
        }
    }
}

if (navigator.geolocation) {
    navigator.geolocation.watchPosition(
        (p) => {
            const lat = +p.coords.latitude.toFixed(6);
            const lon = +p.coords.longitude.toFixed(6);
            showMyLocation(lat, lon, p.coords.accuracy || null);
        },
        (err) => console.debug("geo watch error:", err),
        { enableHighAccuracy: true, maximumAge: 5000, timeout: 8000 }
    );
}

/* ===================== DOM helpers & state ===================== */
const $ = (s) => document.querySelector(s);
const resultsEl = $("#results");
const opinionsEl = $("#opinions");
let selectedAddress = null;
let currentResults = [];   // <<< מוצהר פעם אחת בלבד!

/* ===================== Utils ===================== */
async function geolocate() {
    return await new Promise((resolve, reject) =>
        navigator.geolocation
            ? navigator.geolocation.getCurrentPosition(
                (p) => resolve({ lat: +p.coords.latitude.toFixed(6), lon: +p.coords.longitude.toFixed(6) }),
                (e) => reject(e),
                { enableHighAccuracy: true, timeout: 8000 }
            )
            : reject(new Error("No geolocation"))
    );
}
function lonlatToStr({ lon, lat }) { return `${lon},${lat}`; }
function closestUrl(lon, lat) { return `${API}/Address/Closest/${lon},${lat}`; }

/* ===================== Render results & markers ===================== */
function renderItem(addr) {
    const [lon, lat] = addr.location.split(",").map(Number);
    const name = addr.shelter?.nameStr || addr.shelter?.name || "Shelter";
    const isSelected = selectedAddress && String(selectedAddress.code) === String(addr.code);
    return `
    <div class="item ${isSelected ? "selected" : ""}" data-code="${addr.code}" data-lon="${lon}" data-lat="${lat}">
      <h3>${name} <span class="pill">#${addr.code}</span></h3>
      <div class="pill">Capacity: ${addr.capacity ?? "—"} | 24/7: ${addr.isOpen24_7 ? "כן" : "לא"}</div>
      <div class="pill">Contact: ${addr.contactPersonName ?? "—"} ${addr.contactPersonPhone ?? ""}</div>
      <div class="inline mt8">
        <button class="btn accent btn-op-add" data-code="${addr.code}">הוסף חוות דעת</button>
        <button class="btn btn-op-refresh" data-code="${addr.code}">רענן חוות דעת</button>
      </div>
    </div>`;
}

function showAddresses(list) {
    markersLayer.clearLayers();
    resultsEl.innerHTML = list.map(renderItem).join("");

    list.forEach((addr) => {
        const [lon, lat] = addr.location.split(",").map(Number);
        const title = addr.shelter?.nameStr || addr.shelter?.name || "Shelter";
        L.marker([lat, lon]).addTo(markersLayer).bindPopup(`<b>${title}</b><br/>#${addr.code}`);
    });

    const latlngs = list.map((a) => {
        const [lon, lat] = a.location.split(",").map(Number);
        return [lat, lon];
    });
    if (latlngs.length) map.fitBounds(latlngs, { padding: [20, 20] });

    // select item
    document.querySelectorAll(".item").forEach((div) => {
        div.addEventListener("click", (e) => {
            if (e.target.closest("button")) return;
            const lat = parseFloat(div.dataset.lat);
            const lon = parseFloat(div.dataset.lon);
            map.setView([lat, lon], 16, { animate: true });
            const code = div.dataset.code;
            selectedAddress = currentResults.find((x) => String(x.code) === String(code));
            renderOpinionsPlaceholder();
            loadOpinions(code).catch(() => { });
            resultsEl.querySelectorAll(".item").forEach((x) => x.classList.remove("selected"));
            div.classList.add("selected");
        });
    });

    // opinion buttons
    document.querySelectorAll(".btn-op-add").forEach((btn) =>
        btn.addEventListener("click", (e) => {
            e.stopPropagation();
            const code = btn.dataset.code;
            selectedAddress = currentResults.find((x) => String(x.code) === String(code));
            openOpinionModal(); // add
        })
    );
    document.querySelectorAll(".btn-op-refresh").forEach((btn) =>
        btn.addEventListener("click", (e) => {
            e.stopPropagation();
            loadOpinions(btn.dataset.code);
        })
    );
}

/* ===================== Opinions UI ===================== */
function renderOpinionsPlaceholder() {
    if (!selectedAddress) {
        opinionsEl.innerHTML = `<div class="muted">בחרי כתובת כדי לראות חוות דעת.</div>`;
        return;
    }
    opinionsEl.innerHTML = `
    <div class="inline" style="padding:6px 8px;">
      <button class="btn accent" id="opinions-add-btn">הוסף חוות דעת</button>
      <button class="btn" id="opinions-refresh-btn">רענון</button>
    </div>
    <div id="opinions-list"><div class="muted">טוען…</div></div>
  `;
    $("#opinions-add-btn").addEventListener("click", () => openOpinionModal());
    $("#opinions-refresh-btn").addEventListener("click", () => loadOpinions(selectedAddress.code));
}

function renderOpinions(list) {
    if (!list?.length) {
        $("#opinions-list").innerHTML = `<div class="muted">אין חוות דעת עדיין.</div>`;
        return;
    }
    $("#opinions-list").innerHTML = list.map((o) => `
    <div class="op" data-id="${o.id}">
      <div class="hdr">
        <div><b>דירוג:</b> ${o.rating ?? "—"} ★</div>
        <div class="muted">${o.userName ?? "אנונימי"} · ${o.createdAt ? new Date(o.createdAt).toLocaleString() : ""}</div>
      </div>
      <div class="mt8">${(o.text ?? "").replaceAll("<", "&lt;")}</div>
      <div class="muted mt8">מיקום: ${o.lat ?? "—"}, ${o.lon ?? "—"}</div>
      <div class="inline mt8">
        <button class="btn" data-action="edit">עריכה</button>
        <button class="btn err" data-action="delete">מחיקה</button>
      </div>
    </div>
  `).join("");

    $("#opinions-list").querySelectorAll(".op").forEach((el) => {
        const id = el.dataset.id;
        el.querySelector('[data-action="edit"]').addEventListener("click", () => {
            const o = opinionsCache.find((x) => String(x.id) === String(id));
            openOpinionModal(o);
        });
        el.querySelector('[data-action="delete"]').addEventListener("click", async () => {
            if (!confirm("למחוק חוות דעת?")) return;
            await deleteOpinion(id);
            await loadOpinions(selectedAddress.code);
        });
    });
}

/* ===================== Data calls ===================== */
async function loadNearMe() {
    try {
        const { lat, lon } = await geolocate();
        showMyLocation(lat, lon, 30);
        // בקשת הכתובות הקרובות
        const res = await fetch(closestUrl(lon, lat));
        if (!res.ok) throw new Error(await res.text());
        const list = await res.json();

        const type = $("#type").value;
        const q = $("#q").value.trim().toUpperCase();
        const filtered = list.filter((a) => {
            const name = (a.shelter?.nameStr || a.shelter?.name || "").toUpperCase();
            const typeOk = !type || name === type;
            const textOk = !q || name.includes(q);
            return typeOk && textOk;
        });

        currentResults = filtered;
        showAddresses(filtered);
        if (filtered.length === 0) map.setView([lat, lon], 15, { animate: true });
    } catch (e) {
        resultsEl.innerHTML = `<div class="item"><div class="pill" style="color:var(--err)">שגיאה: ${e.message}</div></div>`;
    }
}

/* ---- Opinions endpoints ---- */
let opinionsCache = [];

async function getOpinions(addressCode) {
    const res = await fetch(`${API}/Opinion/byAddress?addressCode=${encodeURIComponent(addressCode)}`);
    if (!res.ok) throw new Error(await res.text());
    return await res.json();
}
async function postOpinion(body) {
    const res = await fetch(`${API}/Opinion`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(body),
    });
    if (!res.ok) throw new Error(await res.text());
    return await res.json();
}
async function putOpinion(id, body) {
    const res = await fetch(`${API}/Opinion/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(body),
    });
    if (!res.ok) throw new Error(await res.text());
    return await res.json();
}
async function deleteOpinion(id) {
    const res = await fetch(`${API}/Opinion/${id}`, { method: "DELETE" });
    if (!res.ok) throw new Error(await res.text());
}

async function loadOpinions(addressCode) {
    try {
        renderOpinionsPlaceholder();
        opinionsCache = await getOpinions(addressCode);
        renderOpinions(opinionsCache);
    } catch (e) {
        $("#opinions-list").innerHTML = `<div class="muted" style="color:#fca5a5">שגיאה: ${e.message}</div>`;
    }
}

/* ===================== Opinion Modal ===================== */
const modalOpinion = $("#modal-opinion");
let editingOpinion = null;

function openOpinionModal(op = null) {
    editingOpinion = op;
    $("#op-modal-title").textContent = op ? "עריכת חוות דעת" : "הוספת חוות דעת";
    $("#op-rating").value = op?.rating ?? "5";
    $("#op-name").value = op?.userName ?? "";
    $("#op-text").value = op?.text ?? "";
    $("#op-lat").value = op?.lat ?? "";
    $("#op-lon").value = op?.lon ?? "";
    $("#op-use-geo").checked = false;
    modalOpinion.classList.add("open");
}

$("#op-cancel").addEventListener("click", () => modalOpinion.classList.remove("open"));
$("#op-save").addEventListener("click", async () => {
    if (!selectedAddress) { alert("לא נבחרה כתובת"); return; }
    try {
        let lat = $("#op-lat").value.trim();
        let lon = $("#op-lon").value.trim();

        if ($("#op-use-geo").checked) {
            const g = await geolocate();
            lat = g.lat; lon = g.lon;
            $("#op-lat").value = lat; $("#op-lon").value = lon;
        }

        const body = {
            addressCode: selectedAddress.code,
            rating: +$("#op-rating").value,
            text: $("#op-text").value.trim(),
            userName: $("#op-name").value.trim() || null,
            lat: lat ? +lat : null,
            lon: lon ? +lon : null
        };

        if (editingOpinion) {
            await putOpinion(editingOpinion.id, body);
        } else {
            await postOpinion(body);
        }
        modalOpinion.classList.remove("open");
        await loadOpinions(selectedAddress.code);
    } catch (e) {
        alert("שגיאה: " + e.message);
    }
});

/* ===================== Add Address Modal ===================== */
const modalAddress = $("#modal-address");

function openAddressModal() {
    $("#ad-name").value = "";
    $("#ad-code").value = "";
    $("#ad-city").value = "";
    $("#ad-street").value = "";
    $("#ad-lat").value = "";
    $("#ad-lon").value = "";
    $("#ad-use-geo").checked = false;
    modalAddress.classList.add("open");
}

$("#open-add-address").addEventListener("click", openAddressModal);
$("#ad-cancel").addEventListener("click", () => modalAddress.classList.remove("open"));
$("#ad-fill-geo").addEventListener("click", async () => {
    try {
        const g = await geolocate();
        $("#ad-lat").value = g.lat; $("#ad-lon").value = g.lon;
        $("#ad-use-geo").checked = true;
    } catch (e) { alert("לא הצלחתי לקרוא מיקום: " + e.message); }
});

$("#ad-save").addEventListener("click", async () => {
    try {
        if ($("#ad-use-geo").checked && (!$("#ad-lat").value || !$("#ad-lon").value)) {
            const g = await geolocate();
            $("#ad-lat").value = g.lat; $("#ad-lon").value = g.lon;
        }
        const body = {
            name: $("#ad-name").value.trim(),
            code: $("#ad-code").value.trim() || null,
            city: $("#ad-city").value.trim() || null,
            street: $("#ad-street").value.trim() || null,
            location: lonlatToStr({ lon: +$("#ad-lon").value, lat: +$("#ad-lat").value })
        };
        await addAddress(body);
        modalAddress.classList.remove("open");
        await loadNearMe();
    } catch (e) { alert("שגיאה בשמירת כתובת: " + e.message); }
});

$("#add-address-here").addEventListener("click", async () => {
    try {
        const g = await geolocate();
        openAddressModal();
        $("#ad-lat").value = g.lat; $("#ad-lon").value = g.lon;
        $("#ad-use-geo").checked = true;
    } catch (e) { alert("לא הצלחתי לקרוא מיקום: " + e.message); }
});

/* ---- Address endpoints ---- */
async function addAddress(body) {
    const res = await fetch(`${API}/Address`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(body),
    });
    if (!res.ok) throw new Error(await res.text());
    return await res.json();
}

/* ===================== Filters & initial load ===================== */
$("#locate").addEventListener("click", loadNearMe);
$("#type").addEventListener("change", loadNearMe);
$("#q").addEventListener("input", () => {
    clearTimeout(window.__qTimer);
    window.__qTimer = setTimeout(loadNearMe, 300);
});

loadNearMe().catch(() => { });
