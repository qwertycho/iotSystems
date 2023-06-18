import network
import urequests
import ubinascii
import time
import json
import deviceSetup

import env
import deviceSetup

TIMEOUT = 5

class Wifi:
    SSID = env.SSID
    PASSWORD = env.PASSWORD
    wlan = network.WLAN(network.STA_IF)
    macadress = ""

    def __init__(self):
        self.connect()

    # connect to wifi
    def connect(self):
        print("Connecting to wifi")
        self.wlan.active(True)
        self.wlan.connect(self.SSID, self.PASSWORD)
        bytes = self.wlan.config('mac')
        mac = ubinascii.hexlify(bytes,':').decode()
        self.macadress = mac
        tryCount = 0
        while self.wlan.isconnected() == False:
            if(tryCount > 15):
                print("No connection")
                tryCount = 0
                break
            print("MAC: " + mac)
            print('Waiting for connection...')
            print('Connection try: ' + str(tryCount))
            tryCount += 1
            time.sleep(1)

    def makeRequest(self, url):
            try:
                response = urequests.get(url, timeout=TIMEOUT)
                return response.json() 
            except:
                print("Error while making request")
                self.connect()
    
    def initDevice (self, sensors):
        print("Initialiseren van het apparaat bij de server")
        try:

            headers = {"Content-Type": "application/json"}
            response = urequests.post(env.INIT_URL, json={"Uuid": self.macadress, "Sensors": sensors}, headers=headers)

            return response.json()
        except Exception as e:
            print(e)
            print("Error while making request to init device")
            self.connect()
            return {"error": True, "statussss1": False}
        
    def sendData(self, value, type):
        try:
            print("Sending data")
            print("Value: " + str(value))
            print("Type: " + str(type))
            url = env.SEND_DATA_URL + str(type)
            print("URL: " + url)
            print(deviceSetup.getDeviceID())
            response = urequests.post(url, json={"id": deviceSetup.getDeviceID(), "type": str(type), "value": str(value)})
            print({"id": deviceSetup.getDeviceID(), "type": str(type), "value": str(value)})
            print(response.json())

            if response.json()["statusCode"] == 500 or response.json()["hasSetupChanged"] == True:
                print("Setup has changed")
                return False

            return response.json()
        except Exception as e:
            print("Error while making request to send data")
            print(e)
            self.connect()
            return {"error": True, "status": False}