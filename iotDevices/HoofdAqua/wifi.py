import network
import urequests
import ubinascii
import time

import secrets

TIMEOUT = 2

class Wifi:
    SSID = secrets.secret.ssid
    PASSWORD = secrets.secret.password
    wlan = network.WLAN(network.STA_IF)
    macadress = ""

    def __init__(self):
        self.connect()

    # connect to wifi
    def connect(self):
        self.wlan.active(True)
        self.wlan.connect(self.SSID, self.PASSWORD)
        bytes = self.wlan.config('mac')
        mac = ubinascii.hexlify(bytes,':').decode()
        macadress = mac
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
        try:
            json = {"Uuid": self.macadress, "Sensors": sensors}
            response = urequests.post(secrets.secret.initUrl, json=json, timeout=TIMEOUT)
            return response.json()
        except:
            print("Error while making request")
            self.connect()
            raise Exception("Network error while init device")