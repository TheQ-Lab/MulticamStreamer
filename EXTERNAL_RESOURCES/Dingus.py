import serial

from time import sleep

transmitter = serial.Serial('COM3', 115200)
sleep(2)

transmitter.write(b'r')
