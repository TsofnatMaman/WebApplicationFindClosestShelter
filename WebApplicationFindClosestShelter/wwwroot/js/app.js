// בסיס ל־API (אותו דומיין)
const API = "/api";

// מפה
let map = L.map("map", { zoomControl: true }).setView([31.778, 35.235], 12);
L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
    attribution: "&copy; OpenStreetMap",
}).addTo(map);

let markersLayer = L.layerGroup().addTo(map);
const $ = (s) => document.querySelector(s);
const resultsEl = $("#results");

// עוזר: יצירת URL ל-Closest מה-API: api/Address/Closest/{lon,lat}
function closestUrl(lon, lat) {
    return `${API}/Address/Closest/${lon},${lat}`;
}

// רינדור כרטיס
function renderItem(addr) {
    // addr.Location הוא מחרוזת "lon,lat" לפי ה-Mapper
    const [lon, lat] = addr.location.split(",").map(Number);
    const name = addr.shelter?.nameStr || addr.shelter?.name || "Shelter";
    return `
    <div class="item" data-lon="${lon}" data-lat="${lat}">
      <h3>${name} <span class="pill">#${addr.code}</span></h3>
      <div class="pill">Capacity: ${addr.capacity ?? "—"} | Open 24/7: ${addr.isOpen24_7 ? "כן" : "לא"}</div>
      <div class="pill">Contact: ${addr.contactPersonName ?? "—"} ${addr.contactPersonPhone ?? ""}</div>
    </div>`;
}

// הצגה במפה וברשימה
function showAddresses(list) {
    markersLayer.clearLayers();
    resultsEl.innerHTML = list.map(renderItem).join("");

    list.forEach((addr) => {
        const [lon, lat] = addr.location.split(",").map(Number);
        const title = addr.shelter?.nameStr || addr.shelter?.name || "Shelter";
        const m = L.marker([lat, lon]).addTo(markersLayer);
        m.bindPopup(`<b>${title}</b><br/>#${addr.code}`);
    });

    // זום לכל הסמנים
    let latlngs = list.map((a) => {
        const [lon, lat] = a.location.split(",").map(Number);
        return [lat, lon];
    });
    if (latlngs.length) map.fitBounds(latlngs, { padding: [20, 20] });

    // קליק על פריט ברשימה → פוקוס במפה
    document.querySelectorAll(".item").forEach((div) => {
        div.addEventListener("click", () => {
            const lat = parseFloat(div.dataset.lat);
            const lon = parseFloat(div.dataset.lon);
            map.setView([lat, lon], 16, { animate: true });
        });
    });
}

// שליפת נתונים ליד המיקום הנוכחי
async function loadNearMe() {
    try {
        const pos = await new Promise((resolve, reject) =>
            navigator.geolocation
                ? navigator.geolocation.getCurrentPosition(resolve, reject, { enableHighAccuracy: true, timeout: 8000 })
                : reject(new Error("No geolocation"))
        );

        const lat = pos.coords.latitude.toFixed(6);
        const lon = pos.coords.longitude.toFixed(6);

        const res = await fetch(closestUrl(lon, lat));
        if (!res.ok) throw new Error(await res.text());
        const list = await res.json();

        // סינון קל לפי select (אם יש)
        const type = $("#type").value;
        const q = $("#q").value.trim();
        const filtered = list.filter((a) => {
            const name = (a.shelter?.nameStr || a.shelter?.name || "").toUpperCase();
            const typeOk = !type || name === type;
            const textOk = !q || (name.includes(q.toUpperCase()));
            return typeOk && textOk;
        });

        showAddresses(filtered);
    } catch (e) {
        resultsEl.innerHTML = `<div class="item"><div class="pill" style="color:var(--err)">שגיאה: ${e.message}</div></div>`;
    }
}

$("#locate").addEventListener("click", loadNearMe);
$("#type").addEventListener("change", loadNearMe);
$("#q").addEventListener("input", () => {
    // דילי קטן כדי לא להציף קריאות
    clearTimeout(window.__qTimer);
    window.__qTimer = setTimeout(loadNearMe, 300);
});

// טעינה ראשונית (אם אין גיאולוקציה, נשארים בזום ברירת מחדל)
loadNearMe().catch(() => { });
