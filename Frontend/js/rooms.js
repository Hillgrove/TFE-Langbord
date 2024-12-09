

const baseUrl = 'http://localhost:5137/api/';

Vue.createApp({
    data() {
        return {
            currentRoomId: null, // HVis denne er null så vises afsnittet med rum-data ikke.
            targetRoomTemperature: null,
            currentRoomTemperature: null,
            rooms: [],
            room: [],
            sensors: [],

            // Create Room
            roomName: null,
            roomTargetTemperature: null,

        }
    },

    created() {


    },

    methods: {

        getRooms() {
            axios.get(baseUrl+'Rooms')
                .then(response => {
                    this.rooms = response.data;

                    // Default på 1 rum fundet/returneret
                    this.currentRoomId = this.rooms[0].id;
                    this.setCurrentRoom(0, this.rooms[0].id);
                })
        },

        setCurrentRoom(key, roomId)
        {
            // Set Vars
            this.currentRoomId = roomId;
            this.room = this.rooms[key];

            this.targetRoomTemperature = this.room.targetTemperature;

            this.sensors = this.room.sensors;

            if(this.sensors.length > 0)
            {
                this.currentRoomTemperature = this.room.sensors[0].sensorData[0].temperature;
            } else {
                this.currentRoomTemperature = '-';
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


            // Charts
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


            /*
            if(myChart !== undefined)
            {
                myChart.destroy();
            }


            const ctx = document.getElementById('myChart');

            const myChart = new Chart(ctx, {
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

*/

            updateDeviation();



        },

        createRoom()
        {
            const roomData = {
                name: this.roomName,
                targetTemperature: this.roomTargetTemperature,
            };

            axios.post(baseUrl+'Rooms', roomData)
                .then(response => {
                    console.log('Room created successfully:', response.data);

                    this.roomName = '';
                    this.roomTargetTemperature = '';

                    const modal = document.getElementById('createRoomModel');
                    const bootstrapModal = bootstrap.Modal.getInstance(modal);

                    bootstrapModal.hide();

                    //this.currentRoomId = response.data.id; // if we wan

                    this.getRooms();

                })
                .catch(error => {
                    console.error('Error creating room:', error.response?.data || error.message);
                });
        },

        deleteRoom(id)
        {
            axios.delete(baseUrl+'Rooms/'+id)
                .then(response => {
                    console.log('Room Deleted', response.data);

                    this.currentRoomId = null;

                    this.getRooms();

                }) .catch(error => {
                console.error('Could not delete room', error.response?.data || error.message);
            });
        }

    },

    mounted() {
        this.getRooms();
    },

}).mount("#app")