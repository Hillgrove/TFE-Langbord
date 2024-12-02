import random


class SenseHatEmulator:
    def __init__(self):
        pass

    def get_temperature(self):
        return round(random.uniform(15, 25), 2)

    def get_humidity(self):
        return round(random.uniform(30, 70), 2)

    def get_pressure(self):
        return round(random.uniform(1000, 1020), 2)
