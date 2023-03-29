import time
import machine

class WaterSensor:

    waterHigh = [] 
    waterLow = []

    def __init__(self, sensorPin = 1, powerPin = 11):
        '''
        sensorPin: de pin waar de sensor op zit
        powerPin: de pin waar de power op zit
        Zet de power aan en initialiseer de adc
        '''
        self.adc = machine.ADC(sensorPin)
        self.power = machine.Pin(powerPin, machine.Pin.OUT)
        self.power.on()
    
    def getWaterLevel(self) -> bool:
        '''
            return: bool voor water hoog of laag
            meet de current van de sensor en return true als er water is
            Voert een callback uit als de water hoog of laag is
        '''
        current = self.adc.read_u16()
        tempWaarde = 65536 - current
        if(tempWaarde < 3000):
            self.notify("waterLow", False)
            return False
        else:
            self.notify("waterHigh", True)
            return True
        
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

        