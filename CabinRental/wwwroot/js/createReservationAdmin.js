let pricePerNightField = document.getElementById("pricePerNight");
let totalPriceField = document.getElementById("totalPrice");
let checkInDate = document.getElementById("checkInDate");
let checkOutDate = document.getElementById("checkOutDate")
let cabinDropDown = document.getElementById("cabinSelect");



document.addEventListener("DOMContentLoaded", function () {

    if (cabinDropDown) {
        cabinDropDown.addEventListener("change", function () {
            let selectedCabinId = cabinDropDown.value;

            if (selectedCabinId) {
                fetch(`/Admin/ReservationManager/GetCabinDetails?cabinId=${selectedCabinId}`)
                    .then(response => response.json())
                    .then(data => {
                        pricePerNightField.value = data.pricePerNight;
                        calculateTotalPrice(); // Recalculate total price
                        loadCalendarEvents(selectedCabinId);
                    })
                    .catch(error => console.error("❌ Error fetching cabin details:", error));

            }
        })
    }

});


function loadCalendarEvents(cabinId) {
    let calendarEl = document.getElementById("calendar");

    fetch(`/Customer/Cabin/GetReservations?cabinId=${cabinId}`)
        .then(response => response.json())
        .then(data => {
            let events = data.map(event => ({
                id: event.id,
                title: "Booked",
                start: event.start,
                end: event.end,

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
}

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
