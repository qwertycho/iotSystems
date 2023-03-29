import network
import socket
import urequests
import ubinascii
import time

import secrets

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
        while self.wlan.isconnected() == False:
            print("MAC: " + mac)
            print('Waiting for connection...')
            time.sleep(1)

    def makeRequest(self):
        if(self.wlan.isconnected() == False):
            self.connect()
        try:
            url = "https://komkommer.eu/api/public/feitjes?action=random"
            response = urequests.get(url)
            return response.json() 
        except:
            self.connect()