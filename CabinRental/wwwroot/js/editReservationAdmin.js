let pricePerNightField = document.getElementById("pricePerNight");
let totalPriceField = document.getElementById("totalPrice");
let checkInDate = document.getElementById("checkInDate");
let checkOutDate = document.getElementById("checkOutDate")
let cabinId = document.getElementById("reservationData").dataset.cabinId;
let reservationId = document.getElementById("reservationData").dataset.reservationId;





document.addEventListener("DOMContentLoaded", function () {

    let calendarEl = document.getElementById("calendar");

    fetch(`/Customer/Cabin/GetReservations?cabinId=${cabinId}`)
        .then(response => response.json())
        .then(data => {
            let events = data.map(event => ({
                id: event.id,
                title: event.id == reservationId ? "User Reservation" : "Booked",
                start: event.start,
                end: event.end,
                classNames: event.id == reservationId ? ["checkout"] : [""]

            }));
            let calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: "dayGridMonth",
                events: events, // Fetch booked dates
                height: 500,
                contentHeight: "auto",
                schedulerLicenseKey: 'GPL-My-Project-Is-Open-Source'
            });
            calendar.render();
        })
        .catch(error => console.error("❌ Error fetching reservations:", error));
});
function calculateTotalPrice() {
    let checkIn = new Date(checkInDate.value);
    let checkOut = new Date(checkOutDate.value);
    let pricePerNight = parseFloat(pricePerNightField.value) || 0;

    if (!isNaN(checkIn) && !isNaN(checkOut) && checkOut > checkIn) {
        let nights = Math.ceil((checkOut - checkIn) / (1000 * 60 * 60 * 24));
        let totalPrice = nights * pricePerNight;
        totalPriceField.value = totalPrice.toFixed(2);
    } else {
        totalPriceField.value = "0.00";
    }
}
checkInDate.addEventListener("change", calculateTotalPrice);
checkOutDate.addEventListener("change", calculateTotalPrice);
