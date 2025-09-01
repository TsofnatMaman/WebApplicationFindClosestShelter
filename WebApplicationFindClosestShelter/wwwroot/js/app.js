let map;
let shelters = [];
let selectedLocation = null;

async function initMap() {
    map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: 31.0461, lng: 34.8516 },
        zoom: 8,
    });

    map.addListener("click", (e) => openForm(e.latLng));

    await loadShelters();
    await loadAddresses();
}

async function loadShelters() {
    const res = await fetch("/api/Shelter");
    shelters = await res.json();
    const select = document.getElementById("shelterSelect");
    shelters.forEach(s => {
        const opt = document.createElement("option");
        opt.value = s.code;
        opt.textContent = s.nameStr;
        select.appendChild(opt);
    });
}

async function loadAddresses() {
    const res = await fetch("/api/Address");
    const addresses = await res.json();
    addresses.forEach(addAddressMarker);
}

function addAddressMarker(address) {
    if (!address.location) return;
    const parts = address.location.split(",").map(Number);
    const marker = new google.maps.Marker({
        position: { lng: parts[0], lat: parts[1] },
        map,
        title: address.shelter?.nameStr || "Shelter"
    });

    const info = new google.maps.InfoWindow({
        content: `<div><strong>${address.shelter?.nameStr || ""}</strong><br/>Capacity: ${address.capacity}<br/>Contact: ${address.contactPersonName}</div>`
    });

    marker.addListener("mouseover", () => info.open(map, marker));
    marker.addListener("mouseout", () => info.close());
}

function openForm(latLng) {
    selectedLocation = latLng;
    document.getElementById("address-form").reset();
    document.getElementById("form-container").classList.remove("hidden");
}

document.getElementById("cancelBtn").addEventListener("click", () => {
    document.getElementById("form-container").classList.add("hidden");
});

document.getElementById("address-form").addEventListener("submit", async (e) => {
    e.preventDefault();
    if (!selectedLocation) return;

    const payload = {
        location: `${selectedLocation.lng()},${selectedLocation.lat()}`,
        shelter: { code: parseInt(document.getElementById("shelterSelect").value) },
        contactPersonName: document.getElementById("contactName").value,
        contactPersonPhone: document.getElementById("contactPhone").value,
        capacity: parseInt(document.getElementById("capacity").value) || 0,
        currentNumberPeople: parseInt(document.getElementById("currentPeople").value) || 0,
        isOpen24_7: document.getElementById("isOpen").checked,
        contactPersonHasSMS: document.getElementById("sms").checked
    };

    await fetch("/api/Address", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload)
    });

    addAddressMarker({
        ...payload,
        shelter: shelters.find(s => s.code === payload.shelter.code)
    });

    document.getElementById("form-container").classList.add("hidden");
});