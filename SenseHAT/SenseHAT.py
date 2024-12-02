# HUSK AT INSTALLERE SenseHat på Rasberry Pi
import json
import time
from socket import AF_INET, SO_BROADCAST, SOCK_DGRAM, SOL_SOCKET, socket

DEVELOPMENT_MODE = True
DELAY_IN_SECONDS = 1
BROADCAST_IP = "255.255.255.255"
PORT = 65000

if DEVELOPMENT_MODE:
    from sense_HAT_emulator import SenseHatEmulator as SenseHat

    print("Running in DEVELOPMENT MODE. Using SenseHat emulator.")
else:
    try:
        from sense_hat import SenseHat

        print("Running in PRODUCTION mode. Using real SenseHat.")
    except ImportError as e:
        print("Error: SenseHat library not found. Are you on a Raspberry Pi?")
        raise e

sense = SenseHat()

with socket(AF_INET, SOCK_DGRAM) as sock:
    sock.setsockopt(SOL_SOCKET, SO_BROADCAST, 1)

    # Broadcasting the sensor data to the network by using UDP, and serializing the data to JSON
    try:
        while True:
            temperature = sense.get_temperature()
            pressure = sense.get_pressure()
            humidity = sense.get_humidity()
            sensor_data = {
                "Temperature": round(temperature, 2),
                "Humidity": round(humidity, 2),
                "Pressure": round(pressure, 2),
            }

            message = json.dumps(sensor_data)

            sock.sendto(message.encode("utf-8"), (BROADCAST_IP, PORT))
            print("Data sent:", message)

            time.sleep(DELAY_IN_SECONDS)

    except KeyboardInterrupt:
        print("Broadcast stopped.")
