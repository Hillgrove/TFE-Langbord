const baseUrl = "https://www.7timer.info/bin/civillight.php?lon=12.0746571&lat=55.6240144&ac=0&unit=metric&output=json&tzshift=0"

Vue.createApp({
    data() {
        return {
            sensorData: null,
            currentDate: null,
        }
    },

    async created() {
        this.getAll(baseUrl); // + 'sensor')
    },

    methods: {

        async getAll(url) {
            try {
                const response = await axios.get(url) // http://localhost:5207/api/sensor
                this.result = await response.data
                this.sensorData = this.result.dataseries;

                console.log(this.result);
                console.log(this.sensorData);

            } catch (ex) {
                alert(ex.message)
            }
        }
    }
}).mount("#app")