# HUSK AT INSTALLERE SenseHat på Rasberry Pi
import json
import time
from socket import AF_INET, SO_BROADCAST, SOCK_DGRAM, SOL_SOCKET, socket

DEVELOPMENT_MODE = True
DELAY_IN_SECONDS = 1
BROADCAST_IP = "255.255.255.255"
PORT = 65000
UNKNOWN_SENSOR_INDICATOR = "UNKNOWN"


def get_sense_hat():
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

    return SenseHat()


def get_serial_number(sense):
    if DEVELOPMENT_MODE:
        return sense.get_serial_number()
    try:
        with open("/proc/cpuinfo", "r") as f:
            for line in f:
                if line.startswith("Serial"):
                    return line.split(":")[1].strip()
    except Exception:
        return UNKNOWN_SENSOR_INDICATOR


def get_sensor_data(sense):
    temperature = round(sense.get_temperature(), 2)
    pressure = round(sense.get_pressure(), 2)
    humidity = round(sense.get_humidity(), 2)
    return {
        "Temperature": temperature,
        "Humidity": humidity,
        "Pressure": pressure,
    }


def broadcast_data(sock, data, ip, port):
    message = json.dumps(data)
    sock.sendto(message.encode("utf-8"), (ip, port))
    print("Data sent:", message)


def main():
    sense = get_sense_hat()
    with socket(AF_INET, SOCK_DGRAM) as sock:
        sock.setsockopt(SOL_SOCKET, SO_BROADCAST, 1)
        # Broadcasting the sensor data to the network by using UDP, and serializing the data to JSON
        try:
            while True:
                sensor_data = get_sensor_data(sense)
                serial_number = get_serial_number(sense)
                sensor_data["SerialNumber"] = serial_number
                broadcast_data(sock, sensor_data, BROADCAST_IP, PORT)
                time.sleep(DELAY_IN_SECONDS)
        except KeyboardInterrupt:
            print("Broadcast stopped.")


if __name__ == "__main__":
    main()
