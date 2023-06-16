import time
import _thread

# importeer de classes
import tempSens
import wifi
import setup
import env

#global variables
SLEEP_TIME = 0.5

if not setup.startSetup():
    print("Setup not done")
    setup.initDevice(["temp"])
else:
    print("Setup done")

class sensData:
    canSend = False
    waarde = 0.0

waterData = sensData()
tempData = sensData()
lock = _thread.allocate_lock()

def sensThread():
    sensor = tempSens.TempSensor()
    while True:
        lock.acquire()

        global waterData
        global tempData

        tempData.waarde = sensor.getTemp()
        tempData.canSend = True
        lock.release()

def main():
    global wifi
    wifi = wifi.Wifi()
    while True:
        lock.acquire()
        global waterData
        if(tempData.canSend):
            print(tempData.waarde)
            wifi.sendData(tempData.waarde, "temp")
            tempData.canSend = False
        lock.release()
        time.sleep(SLEEP_TIME)

_thread.start_new_thread(sensThread, ())
main()