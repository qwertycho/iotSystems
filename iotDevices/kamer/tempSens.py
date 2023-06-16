import machine
import time
import math

class TempSensor:

    _sensort_offset = 6.5

    maxTempE = []
    minTempE = []

    def __init__(self, sensorPin = 0, powerPin = 10, minTemp = 20, maxTemp = 30, samples = 20):
        '''
        sensorPin: de pin waar de sensor op zit
        powerPin: de pin waar de power op zit
        minTemp: de minimale temperatuur
        maxTemp: de maximale temperatuur
        samples: het aantal samples dat gemiddeld moet worden gemeten
        '''
        self.adc = machine.ADC(sensorPin)
        self.minTemp = minTemp
        self.maxTemp = maxTemp
        self.samples = samples
        self.power = machine.Pin(powerPin, machine.Pin.OUT)
        self.power.on()
    
    def getTemp(self):
        R0 = 10000
        T0 = 298.15
        B = 3950
        V = self.adc.read_u16() * 3.3 / 65535.0
        R = (3.3 * 10000 / V) - 10000
        inv_T = 1/T0 + (1/B) * math.log(R/R0)
        T = (1/inv_T) - 273.15
        return T
    
    def subsribe (self, event, callback):
        '''
        Voeg een callback toe aan een event array
        events: maxTemp, minTemp
        '''
        if event == "maxTemp":
            self.maxTempE.append(callback)
        elif event == "minTemp":
            self.minTempE.append(callback)

    def notify(self, event, value):
        '''
        Voert een callback uit
        events: maxTemp, minTemp
        '''
        if event == "maxTemp":
            for callback in self.maxTempE:
                callback(value)
        elif event == "minTemp":
            for callback in self.minTempE:
                callback(value)