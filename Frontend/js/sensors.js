const baseUrl = 'https://tfe.datamatikereksamen.dk/api/';

Vue.createApp({
    data() {
        return {
            // Assign Sensor
            allSensors: [],
        }
    },

    created() {
    },

    methods: {
        // Sensors
        getSensors() {
            axios.get(baseUrl + 'Sensors')
                .then(response => {
                    this.allSensors = response.data;
                })
        },
    },

    mounted() {
        this.getSensors();
    },

}).mount("#app");
