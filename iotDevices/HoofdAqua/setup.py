import wifi

class Setup:
    deviceID = 0
    setupID = 0
    aanTijd = 0
    uitTijd = 0
    minTemp = 0
    maxTemp = 0

def initDevice (self, sensors):
    self.wifi = wifi.Wifi()
    try:
        res = self.wifi.initDevice(sensors)   
        if(res["error"] == True):
            raise Exception("Error while init device")
        elif(res["status"] == False):
          return False
        elif(res["status"] == True):
            file = open("setup.txt", "w")
            file.write(str(res["data"]["deviceID"]) + "\n")
            file.write(str(res["data"]["id"]) + "\n")
            file.write(str(res["data"]["aanTijd"]) + "\n")
            file.write(str(res["data"]["uitTijd"]) + "\n")
            file.write(str(res["data"]["minTemp"]) + "\n")
            file.write(str(res["data"]["maxTemp"]) + "\n")
            file.close()
            return True
    except:
        print("Fout opgetreden tijdens het initialiseren van het apparaat")

def startSetup(self):
    try:
        file = open("setup.txt", "r")
        self.deviceID = file.readline()
        self.setupID = file.readline()
        self.aanTijd = file.readline()
        self.uitTijd = file.readline()
        self.minTemp = file.readline()
        self.maxTemp = file.readline()
        file.close()

        if(self.deviceID != "" and self.deviceID != None and self.deviceID != 0):
            return True

        return True
    except:
        print("Fout opgetreden tijdens het ophalen van de setup")
        return False        