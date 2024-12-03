import random


class SenseHatEmulator:
    def __init__(self):
        pass

    def get_temperature(self):
        return random.uniform(15, 25)

    def get_humidity(self):
        return random.uniform(30, 70)

    def get_pressure(self):
        return random.uniform(1000, 1020)

    def  get_serial_number(self):
        return random.randint(1, 5)