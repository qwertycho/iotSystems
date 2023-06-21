import time
import _thread

# importeer de classes
import wifi
import deviceSetup
import humSens
import env

wifir = wifi.Wifi()
wifir.connect()


#global 
SLEEP_TIME = 10

deviceSetup.initDevice(env.SENSORS)

class sensData:
    canSend = False
    waarde = 0.0

tempData = sensData()
humData = sensData()
lock = _thread.allocate_lock()

def sensThread():
    while True:
        lock.acquire()

        global tempData
        global humData


        humData.waarde = humSens.getHumidity()
        tempData.waarde = humSens.getTemperature()
        tempData.canSend = True
        humData.canSend = True
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

        if(humData.canSend):
            print(humData.waarde)
            if not wifir.sendData(humData.waarde, "hum"):
                deviceSetup.initDevice(env.SENSORS)
            humData.canSend = False

        lock.release()
        time.sleep(SLEEP_TIME)

_thread.start_new_thread(sensThread, ())
main()