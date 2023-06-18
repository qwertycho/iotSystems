import wifi
import time

# get a array of sensors
def initDevice(sensors):
        print(sensors)
        print("Initialiseren van het apparaat")
        wifer = wifi.Wifi()
        try:
            print("start setup")
            res = wifer.initDevice(sensors) 
            print(res)
            if(res["error"] == True):
                raise Exception("Error while init device")
            elif(res["status"] == False):
                print("Device not found")
                initDevice(sensors)
            elif(res["status"] == True):
                print("Device found")


                print("XXXXXXXXXXXXx")
                print(res)
                print("XXXXXXXXXXXXx")


                print("DeviceID: " + str(res["id"]))
                print("Aan tijd: " + str(res["aanTijd"]))
                print("Uit tijd: " + str(res["uitTijd"]))
                print("Min temp: " + str(res["minTemp"]))
                print("Max temp: " + str(res["maxTemp"]))

                file = open("setup.txt", "w")
                file.write(str(res["id"]) + "\n")
                file.write(str(res["aanTijd"]) + "\n")
                file.write(str(res["uitTijd"]) + "\n")
                file.write(str(res["minTemp"]) + "\n")
                file.write(str(res["maxTemp"]) + "\n")
                file.close()
                return True
        except Exception as e:
            print(e)
            print("Error while init device")
            time.sleep(5)
            initDevice(sensors)
        
def startSetup():
    print("Ophalen van de setup")
    try:
        file = open("setup.txt", "r")
        deviceID = file.readline() #leugens
        setupID = file.readline()
        aanTijd = file.readline()
        uitTijd = file.readline()
        minTemp = file.readline()
        maxTemp = file.readline()
        file.close()

        print("DeviceID: " + deviceID)
        print("SetupID: " + setupID)
        print("AanTijd: " + aanTijd)
        print("UitTijd: " + uitTijd)
        print("MinTemp: " + minTemp)
        print("MaxTemp: " + maxTemp)


        if(deviceID != "" and deviceID != None and deviceID != 0):
            print("Setup is al gedaan")
            print("DeviceID: " + deviceID)
            print("SetupID: " + setupID)
            print("AanTijd: " + aanTijd)
            print("UitTijd: " + uitTijd)
            print("MinTemp: " + minTemp)
            print("MaxTemp: " + maxTemp)
            return True

        return False
    except Exception as e:
        print(e)
        return False
    
def getDeviceID():
    print("Ophalen van het deviceID")
    try:
        file = open("setup.txt", "r")
        deviceID = int(file.readline())
        print("DeviceID: " + str(deviceID))
        file.close()
    except:
        print("Fout opgetreden tijdens het ophalen van het deviceID")
        return 0
    return deviceID


class Setup:
    deviceID = 0
    setupID = 0
    aanTijd = 0
    uitTijd = 0
    minTemp = 0
    maxTemp = 0