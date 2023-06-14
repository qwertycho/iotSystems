import time
import _thread

# importeer de classes
import tempSens
import waterSens
import relais
import wifi
import env

#global variables
SLEEP_TIME = 0.5

# maak een object van de classes
relais = relais.Relais(1, 3)

# subscribe aan de events
def waterLow(value):
    relais.setRelais(0, 1)
                     
def waterHigh(value):
    relais.setRelais(0, 0)

def tempHigh(value):
    relais.setRelais(0, 1)

def tempLow(value):
    relais.setRelais(0, 0)

class sensData:
    canSend = False
    waarde = 0.0

waterData = sensData()
tempData = sensData()
lock = _thread.allocate_lock()

def sensThread():

    water = waterSens.WaterSensor()
    sensor = tempSens.TempSensor()

    water.subsribe("waterLow", waterLow)
    water.subsribe("waterHigh", waterHigh)

    sensor.subsribe("tempHigh", tempHigh)
    sensor.subsribe("tempLow", tempLow)
 
    while True:
        lock.acquire()

        global waterData
        global tempData

        waterData.waarde = water.getWaterLevel()
        waterData.canSend = True
        tempData.waarde = sensor.getTemp()
        tempData.canSend = True
        lock.release()

def main():
    global wifi
    wifi = wifi.Wifi()
    while True:
        lock.acquire()
        global waterData
        global tempData
        if(waterData.canSend):
            print(waterData.waarde)
            waterData.canSend = False
        if(tempData.canSend):
            print(tempData.waarde)
            tempData.canSend = False
        lock.release()
        print(wifi.makeRequest(env.TEST_URL))
        time.sleep(SLEEP_TIME)

_thread.start_new_thread(sensThread, ())
main()