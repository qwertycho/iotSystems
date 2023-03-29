import time
import machine

class Relais:

    pins = []

    def __init__(self, aantalRelais, startPin ):
        '''
        aantalRelais: het aantal relais dat je wilt gebruiken
        startPin: de pin waar de eerste relais op zit
        De pins moeten naast elkaar zitten op volgorde
        De pins worden toegevoegd aan een array in een for loop
        '''
        for i in range(aantalRelais):
            self.pins.append(machine.Pin(startPin + i, machine.Pin.OUT))
        
    def setRelais(self, relais, state) -> None:
        '''
        relais: het relais dat je wilt aanpassen (index van de array)
        state: de state die je wilt aanpassen (True of False)
        Zet de state van een relais aan of uit
        '''
        self.pins[relais].value(state)

    def getRelais(self, relais) -> bool:
        '''
        relais: het relais dat je wilt aanpassen (index van de array)
        return: de state van het relais (True of False)
        Krijg de state van een relais
        '''
        return self.pins[relais].value()
    