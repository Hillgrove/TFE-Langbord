# HUSK AT INSTALL SÅ VI KAN BRUGE SENSEHAT
# from sense_hat import SenseHat
import time
import socket
import json

# sense = SenseHat()
 
 
while True:
    
    # UDP_IP = "255.255.255.255" 
    UDP_IP = "192.168.1.255" 
    UDP_PORT = 65000      
    
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

    # temperature = sense.get_temperature()
    # pressure = sense.get_pressure()
    # humidity = sense.get_humidity()

    # Test Data:   (Husk at udkommentere senere)
    temperature = 17
    pressure = 1000
    humidity = 101

    # Create a dictionary with the sensor data        
    sensor_data = {"Temperature": round(temperature, 2),"Humidity": round(humidity, 2), "Pressure": round(pressure, 2) }
   
    # Broadcasting the sensor data to the network by using UDP, and serializing the data to JSON
    try: 
            
        message = json.dumps(sensor_data)
            
        sock.sendto(message.encode('utf-8'), (UDP_IP, UDP_PORT))
        # VI bruger følgende to linier til at teste om dataen bliver sendt og modtaget korrekt
        # socket.SOL_SOCKET og socket.SO_BROADCAST gør at vi kan sende til broadcast adressen
        sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        sock.setsockopt(socket.SOL_SOCKET, socket.SO_BROADCAST, 1)
        print("Data sent:", message)   
                
    except KeyboardInterrupt:
        print("Broadcast stopped.")
    finally:
        sock.close()

    time.sleep(10)

