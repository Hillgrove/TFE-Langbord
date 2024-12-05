Vue.createApp({
    data() {
        return {

            userLocation: {
              city: "Roskilde",
              lon: 12.0746571,
              lat: 55.6240144
            },

            locations: [

                {
                    city: "Roskilde",
                    lon: 12.0746571,
                    lat: 55.6240144
                },

                {
                    city: "Holbæk",
                    lon: 11.6345319,
                    lat: 55.7160127,
                },

                {
                    city: "Køge",
                    lon: 12.0806785,
                    lat: 55.4468255,
                },

                {
                    city: "Brøndby",
                    lon: 12.3891614,
                    lat: 55.6483768,
                },

                {
                    city: "København",
                    lon: 12.5526248,
                    lat: 55.6713089,
                }
                ,
                {
                    city: "Istanbul",
                    lon: 28.6825564,
                    lat: 41.0053702,
                },

            ],

            sensorData: "",
            currentDate: "",
        }
    },

    created() {
        this.getCurrentWeather();
    },

    methods: {

        // Get Current Weather
        async getCurrentWeather()   {

            var requestUri = 'https://www.7timer.info/bin/civillight.php?lon='+ this.userLocation.lon +'&lat='+ this.userLocation.lat +'&ac=0&unit=metric&output=json&tzshift=0'

            try {
                const response = await axios.get(requestUri)
                this.result = await response.data
                this.sensorData = this.result.dataseries;
            } catch (ex) {
                alert(ex.message)
            }
        },

        changeUserLocation()
        {
            var selectionId = this.userLocationSelection;
            var selection = this.locations[selectionId];
            this.userLocation = selection;

            this.getCurrentWeather();

        }


    }
}).mount("#app")