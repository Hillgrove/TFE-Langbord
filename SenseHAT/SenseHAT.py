from sense_emu import SenseHat  # use this for development
# from sense_hat import SenseHat # use this when moving code to rasberry Pi


sense = SenseHat()

red = (255, 0, 0)
blue = (0, 0, 255)

while True:
    temp = sense.temp
    pixels = [red if i < temp else blue for i in range(64)]
    sense.set_pixels(pixels)
