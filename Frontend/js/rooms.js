const baseUrl = 'https://tfe.datamatikereksamen.dk/api/';

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
            roomTargetTemperature: 0,
            targetTemperatureDeviation: 0,

            // Assign Sensor
            allSensors: [],

            // Chart
            chartInstance: null,
            sensorChartData: null,

            // Other
            sensorIdToAssign: null,
            userTargetTemperature: null,

        }
    },

    created() {

        this.getRooms();

    },

    methods: {

        // Rooms
        getRooms() {
            axios.get(baseUrl + 'Rooms')
                .then(response => {
                    this.rooms = response.data;

                    // Default på 1 rum fundet/returneret
                    this.currentRoomId = this.rooms[0].id;
                    this.setCurrentRoom(0, this.rooms[0].id);
                    this.getChart();
                })
        },

        // Setters
        setCurrentRoom(key, roomId) {
            this.currentRoomId = roomId;
            this.room = this.rooms[key];
            this.targetRoomTemperature = this.room.targetTemperature;
            this.sensors = this.room.sensors;

            if (this.sensors.length > 0) {
                this.currentRoomTemperature = this.room.sensors[0].sensorData[0].temperature;
            } else {
                this.currentRoomTemperature = '-';
            }

            this.updateDeviation();
            this.getChart();
        },

        createRoom() {
            const roomData = {
                name: this.roomName,
                targetTemperature: this.roomTargetTemperature,
            };

            axios.post(baseUrl + 'Rooms', roomData)
                .then(response => {
                    this.roomName = '';
                    this.roomTargetTemperature = '';

                    const modal = document.getElementById('createRoomModel');
                    const bootstrapModal = bootstrap.Modal.getInstance(modal);

                    bootstrapModal.hide();

                    this.getRooms();
                })
                .catch(error => {
                    console.error('Error creating room:', error.response?.data || error.message);
                });
        },

        deleteRoom(id) {
            axios.delete(baseUrl + 'Rooms/' + id)
                .then(response => {
                    this.currentRoomId = null;
                    this.getRooms();
                }).catch(error => {
                console.error('Could not delete room', error.response?.data || error.message);
            });
        },

        // Sensors
        getSensors() {
            axios.get(baseUrl + 'Sensors')
                .then(response => {
                    this.allSensors = response.data;
                })
        },

        assignSensor() {
            const sensorData = {
                id: this.currentRoomId,
                sensorId: this.sensorIdToAssign
            };


            axios.post(baseUrl + 'Rooms/' + this.currentRoomId + '/addsensor/' + this.sensorIdToAssign, sensorData)
                .then(response => {
                    const modal = document.getElementById('assignSensorToRoom');
                    const bootstrapModal = bootstrap.Modal.getInstance(modal);

                    bootstrapModal.hide();

                    this.getRooms();
                })
                .catch(error => {
                    console.error('Error assigning sensor to room:', error.response?.data || error.message);
                });
        },

        deleteSensor(id) {

            console.error('Method not implemented');

        },

        // Temperate Deviation
        updateTargetTemperature() {
            this.targetRoomTemperature = this.userTargetTemperature;
            this.updateDeviation();
            this.getChart();
        },

        updateDeviation() {
            this.targetTemperatureDeviation = (parseFloat(this.targetRoomTemperature) - parseFloat(this.currentRoomTemperature)).toFixed(2)

            const deviationDisplay = document.getElementById("deviationDisplay");

            if (this.targetTemperatureDeviation < 0) {
                deviationDisplay.classList.remove('red');
                deviationDisplay.classList.add('green');
            } else {
                deviationDisplay.classList.add('red');
                deviationDisplay.classList.remove('green');
            }
        },

        getSensorChartData() {
            axios.get(baseUrl + 'Sensors/' + 1 + '/grouped-by-hour')
                .then(response => {
                    this.sensorChartData = response.data;
                })
        },

        getChart()
        {
            // Get Data
            const roomTemp = [];
            const targetTemp = [];
            const timeline = [];

            Object.values(this.sensorChartData).forEach(value => {
                roomTemp.push(value.temperature);
                targetTemp.push(this.targetRoomTemperature);
                timeline.push(value.timestamp);
            });

            const ctx = document.getElementById('myChart');

            Object.values(Chart.instances).forEach(value => {
                value.destroy();
            });

            new Chart(ctx, {
                type: 'line',
                data: {
                    labels: timeline,
                    datasets: [{
                        label: 'Room Temperature',
                        data: roomTemp,
                        borderWidth: 2
                    },
                        {
                            label: 'Target Temperature',
                            data: targetTemp,
                            borderWidth: 2
                        }]
                },
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });
        }

    },

    mounted() {
        this.getRooms();
        this.getSensors();
        this.getSensorChartData();
    },

}).mount("#app");
