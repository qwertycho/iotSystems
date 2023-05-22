class Settings():
    settingsID = 0
    deviceID = 0
    maxTemp = 0
    minTemp = 0
    aanTijd = 0
    uitTijd = 0

def writeSettings(settings):
    '''
    scrhijf de settings naar settings.txt
    '''
    print("Writing new settings")
    file = open("settings.txt", "x")
    file.write(f'settingsID: {settings.settingsID} \n')
    file.write(f'deviceID: {settings.deviceID} \n')
    file.write(f'maxTemp: {settings.maxTemp} \n')
    file.write(f'minTemp: {settings.minTemp} \n')
    file.write(f'aanTijd: {settings.aanTijd} \n')
    file.write(f'uitTijd: {settings.uitTijd} \n')


def saveSettings(settings):
    '''
    sla nieuwe instellingen op in settings.txt
    '''
    if(os.path.isfile("settings.txt")):
        print("file gevonden")
    else:
        print("file niet gevonden")
        writeSettings(settings)
