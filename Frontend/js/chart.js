
    const ctx = document.getElementById('myChart');

    new Chart(ctx, {
    type: 'line',
    data: {
    labels: ['10:00', '11:00', '12:00', '13:00', '14:00', '15:00'],
    datasets: [{
    label: 'Room Temperature',
    data: [19, 18, 22, 23, 24, 21],
    borderWidth: 2
},
{
    label: 'Target Temperature',
    data: [22, 22, 22, 22, 22, 22],
    borderWidth: 2
}]
},
    options: {
    scales: {
    y: {
    beginAtZero: false
}
}
}
});


    window.onload = function(){
    updateDeviation()
};

    function calculateDeviation(targetTemp, currentTemp) { return (targetTemp-currentTemp) }
    function updateDeviation()
    {
        let deviation = calculateDeviation(wantedTempSlider.value, currentTemp);
        const deviationDisplay = document.getElementById("deviationDisplay");
        deviationDisplay.textContent = `${deviation}°C`;

        if(deviation < 0) {
        deviationDisplay.classList.remove('red');
        deviationDisplay.classList.add('green');
    }

        if(deviation > 0) {
        deviationDisplay.classList.add('red');
        deviationDisplay.classList.remove('green');
    }
    }

    // Vars
    let currentTemp = document.getElementById("currentTempValue").value;  // This is just a static example, adjust as needed.

    // Target Temperature
    const wantedTempSlider = document.getElementById("wantedTemp");
    const wantedTempDisplay = document.getElementById("wantedTempDisplay");

    wantedTempSlider.addEventListener("input", function () {
    wantedTempDisplay.textContent = `${wantedTempSlider.value}°C`;
    updateDeviation();
});
