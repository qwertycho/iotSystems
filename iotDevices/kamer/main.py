import time
import _thread

# importeer de classes
import tempSens
import wifi
import deviceSetup
import waterSens
import env

wifir = wifi.Wifi()
wifir.connect()


#global 
SLEEP_TIME = 10

deviceSetup.initDevice(env.SENSORS)

class sensData:
    canSend = False
    waarde = 0.0
class waterData:
    canSend = False
    waarde = False

tempData = sensData()
waterData = waterData()
lock = _thread.allocate_lock()

def sensThread():
    sensor = tempSens.TempSensor()
    wSensor = waterSens.WaterSensor(powerPin=0, sensorPin=2)
    while True:
        lock.acquire()

        global tempData
        global waterData

        waterData.waarde = wSensor.getWaterLevel()
        waterData.canSend = True

        tempData.waarde = sensor.getTemp()
        tempData.canSend = True
        lock.release()

def main():
    global wifir
    while True:
        print("main")
        lock.acquire()
        global waterData
        if(tempData.canSend):
            print(tempData.waarde)
            if not wifir.sendData(tempData.waarde, "temp"):
                deviceSetup.initDevice(env.SENSORS)
            tempData.canSend = False
        
        if(waterData.canSend):
            print(waterData.waarde)
            if not wifir.sendData(waterData.waarde, "water"):
                deviceSetup.initDevice(env.SENSORS)
            waterData.canSend = False

        lock.release()
        time.sleep(SLEEP_TIME)

_thread.start_new_thread(sensThread, ())
main()