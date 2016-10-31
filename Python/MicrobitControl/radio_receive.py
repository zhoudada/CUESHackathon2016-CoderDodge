from microbit import *
import radio

CHANNEL = 17
RAD_PWR = 7
BR = 115200
FREQ = 10

display.scroll('S')
radio.config(channel = CHANNEL,power=RAD_PWR, queue=2)
radio.on()
uart.init(baudrate=BR)
display.scroll('D')

old = 'n'
while True:
    sleep(FREQ)
    msg = radio.receive_bytes()
    if msg!=None:   
        uart.write(msg)
        old = msg
    else:
        uart.write(old)
    