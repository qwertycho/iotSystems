import time
import machine

class WaterSensor:

    waterHigh = [] 
    waterLow = []

    def __init__(self, sensorPin = 0, powerPin = 2):
        '''
        sensorPin: de pin waar de sensor op zit
        powerPin: de pin waar de power op zit
        Zet de power aan en initialiseer de adc
        '''
        self.powerpin = machine.Pin(powerPin, machine.Pin.OUT)
        self.powerpin.on()
        self.sensorsPin = machine.Pin(sensorPin, machine.Pin.IN)
    
    def getWaterLevel(self) -> bool:
        sum = 0
        for i in range(0, 50):
            sum += self.sensorsPin.value()
            time.sleep(0.01)
        print(sum)
        return sum > 25
        
    def subsribe (self, event, callback) -> None:
        '''
        Voeg een callback toe aan een event array
        events: waterHigh, waterLow
        '''
        if event == "waterHigh":
            self.waterHigh.append(callback)
        elif event == "waterLow":
            self.waterLow.append(callback)

    def notify(self, event, value) -> None:
        '''
        Voert alle callbacks uit van een event
        '''
        if event == "waterHigh":
            for callback in self.waterHigh:
                callback(value)
        elif event == "waterLow":
            for callback in self.waterLow:
                callback(value)

        