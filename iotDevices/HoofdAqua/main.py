import machine
import time
import math

adc = machine.ADC(0)

def getTemparature(r, Ro=10000.0, To=25.0, beta=3950.0, samples=20):
    '''
    Krijg de temperatuur van een thermistor
    r: de weerstand van de thermistor
    Ro: de weerstand van de thermistor bij 25 graden
    To: de temperatuur van de thermistor bij 25 graden
    beta: de beta waarde van de thermistor
    samples: het aantal samples dat gemiddeld moet worden
    '''
    avarage = 0
    sensort_offset = 6.5
    for i in range(samples):
        steinhart = math.log(r / Ro) / beta      # log(R/Ro) / beta
        steinhart += 1.0 / (To + 273.15)         # log(R/Ro) / beta + 1/To
        steinhart = (1.0 / steinhart) - 273.15   # Invert, convert to C
        avarage += steinhart - sensort_offset
        time.sleep(0.01)
    return avarage / samples

min = 999
max = 0


while True:
    R = 10000 / (65535/adc.read_u16() - 1)
    print("current: ", getTemparature(R))
    if max < getTemparature(R):
        max = getTemparature(R)
    if min > getTemparature(R):
        min = getTemparature(R)
    print("max: ", max)
    print("min: ", min)
    time.sleep(1)
