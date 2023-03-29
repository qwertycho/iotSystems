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

    def getSensorCurrent(self):
        '''
        Krijg de weerstand van de thermistor
        '''
        R = 10000 / (65535/self.adc.read_u16() - 1)
        return R

    def getTemparature(self, Ro=10000.0, To=25.0, beta=3950.0):
        '''
        Krijg de temperatuur van een thermistor
        r: de weerstand van de thermistor
        Ro: de weerstand van de thermistor bij 25 graden
        To: de temperatuur van de thermistor bij 25 graden
        beta: de beta waarde van de thermistor
        samples: het aantal samples dat gemiddeld moet worden
        voert een callback uit als de temperatuur te hoog of te laag is
        '''

        r = self.getSensorCurrent()

        avarage = 0

        for i in range(self.samples):
            steinhart = math.log(r / Ro) / beta      # log(R/Ro) / beta
            steinhart += 1.0 / (To + 273.15)         # log(R/Ro) / beta + 1/To
            steinhart = (1.0 / steinhart) - 273.15   # Invert, convert to C
            avarage += steinhart - self._sensort_offset
            time.sleep(0.01)

        if(avarage / self.samples > self.maxTemp):
            self.notify("maxTemp", avarage / self.samples)
        elif(avarage / self.samples < self.minTemp):
            self.notify("minTemp", avarage / self.samples)

        return avarage / self.samples
    
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