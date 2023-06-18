import time
import _thread

# importeer de classes
import tempSens
import wifi
import deviceSetup
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
lock = _thread.allocate_lock()

def sensThread():
    sensor = tempSens.TempSensor()
    while True:
        lock.acquire()

        global tempData

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

        lock.release()
        time.sleep(SLEEP_TIME)

_thread.start_new_thread(sensThread, ())
main()