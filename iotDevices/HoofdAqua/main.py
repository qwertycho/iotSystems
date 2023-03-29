import time

# importeer de classes
import tempSens
import waterSens
import relais
import wifi

#global variables
SLEEP_TIME = 0.5

# maak een object van de classes
water = waterSens.WaterSensor()
sensor = tempSens.TempSensor()
relais = relais.Relais(1, 3)
wifi = wifi.Wifi()

# subscribe aan de events
def waterLow(value):
    relais.setRelais(0, 1)
                     
def waterHigh(value):
    relais.setRelais(0, 0)

def tempHigh(value):
    relais.setRelais(0, 1)

def tempLow(value):
    relais.setRelais(0, 0)


water.subsribe("waterLow", waterLow)
water.subsribe("waterHigh", waterHigh)

sensor.subsribe("tempHigh", tempHigh)
sensor.subsribe("tempLow", tempLow)

while True:
    print("temparatuur: " , sensor.getTemparature())
    print("water is vol: " , water.getWaterLevel())
    print(wifi.makeRequest())
    time.sleep(SLEEP_TIME)
