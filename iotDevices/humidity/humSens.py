import machine
import time
import adafruit_sht31d

# sda = 4
# scl = 5
i2c = machine.I2C(sda=machine.Pin(4), scl=machine.Pin(5))

sensor = adafruit_sht31d.SHT31D(i2c)

def getHumidity():
    humidity = sensor.relative_humidity
    return humidity

def getTemperature():
    temperature = sensor.temperature
    return temperature