document.addEventListener("DOMContentLoaded", function () {
    let calendarEl = document.getElementById("calendar");
    let selectedCheckIn = null;
    let selectedCheckOut = null;
    let totalPriceField = document.getElementById("totalPrice");
    let pricePerNight = document.getElementById("pricePerNight")

    let submitBtn = document.getElementById("submitBtn");
    let unavailableDates = [];
    let selectedEvents = [];
    let cabinId = document.getElementById("reservationData").dataset.cabinId;


    fetch(`/Customer/Cabin/GetReservations?cabinId=${cabinId}`)
        .then(response => response.json())
        .then(data => {
            unavailableDates = data.map(event => ({
                start: new Date(event.start),
                end: new Date(event.end)
            }));

            let calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: "dayGridMonth",
                events: data,
                height: "auto",
                contentHeight: "auto",
                selectable: true,
                dayMaxEvents: true,
                schedulerLicenseKey: "GPL-My-Project-Is-Open-Source",

                selectAllow: function (info) {
                    let selectedDate = new Date(info.start);
                    return !unavailableDates.some(blocked =>
                        selectedDate >= blocked.start && selectedDate < blocked.end
                    );
                },
                select: function (info) {
                    let selectedDate = new Date(info.start);

                    if (selectedCheckIn && formatDate(selectedDate) === formatDate(selectedCheckIn)) {
                        resetSelection(calendar);
                        return;
                    }

                    if (!selectedCheckIn) {
                        selectedCheckIn = selectedDate;
                        document.getElementById("checkInDate").value = formatDate(selectedCheckIn);
                        highlightSelectedRange(calendar);
                    } else {
                        selectedCheckOut = selectedDate;

                        if (selectedCheckOut <= selectedCheckIn) {
                            alert("Check-out date must be after check-in date.");
                            return;
                        }

                        let isBlocked = unavailableDates.some(blocked =>
                            selectedCheckOut >= blocked.start && selectedCheckOut < blocked.end
                        );

                        if (isBlocked) {
                            alert("This date is already booked! Choose another one.");
                            return;
                        }

                        document.getElementById("checkOutDate").value = formatDate(selectedCheckOut);

                        let nights = Math.ceil((selectedCheckOut - selectedCheckIn) / (1000 * 60 * 60 * 24));
                        totalPriceField.textContent = (nights * pricePerNight).toFixed(2);

                        submitBtn.removeAttribute("disabled");

                        highlightSelectedRange(calendar);
                    }
                }
            });

            calendar.render();
        })
        .catch(error => console.error("❌ Error fetching reservations:", error));

    function highlightSelectedRange(calendar) {
        selectedEvents.forEach(event => calendar.getEventById(event.id)?.remove());
        selectedEvents = [];

        if (selectedCheckIn) {
            selectedEvents.push(
                calendar.addEvent({
                    id: "checkin",
                    title: "Check-in",
                    start: formatDate(selectedCheckIn),
                    classNames: ["checkin"]
                })
            );
        }

        if (selectedCheckOut) {
            selectedEvents.push(
                calendar.addEvent({
                    id: "checkout",
                    title: "Check-out",
                    start: formatDate(selectedCheckOut),
                    classNames: ["checkout"]
                })
            );

            // Highlight all days in between
            for (let d = new Date(selectedCheckIn); d < selectedCheckOut; d.setDate(d.getDate() + 1)) {
                selectedEvents.push(
                    calendar.addEvent({
                        id: "selected-range-" + d.getTime(),
                        start: formatDate(d),
                        classNames: ["selected-range"]
                    })
                );
            }
        }
    }
    //https://fullcalendar.io/docs/classname-input

    function resetSelection(calendar) {
        selectedCheckIn = null;
        selectedCheckOut = null;
        document.getElementById("checkInDate").value = "";
        document.getElementById("checkOutDate").value = "";
        totalPriceField.textContent = "0.00";
        submitBtn.setAttribute("disabled", "true");

        selectedEvents.forEach(event => calendar.getEventById(event.id)?.remove());
        selectedEvents = [];
    }


    function formatDate(date) {
        let offset = date.getTimezoneOffset() * 60000; // Convert offset to milliseconds
        return new Date(date - offset).toISOString().split("T")[0]; // Shift to local time
    }
});
