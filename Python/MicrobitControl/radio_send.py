from microbit import *
import radio

CHANNEL = 17
RAD_PWR = 7 
FREQ = 10 #send mesage at 1/50ms

display.scroll('S')
radio.config(channel = CHANNEL,power=RAD_PWR)
radio.on()
display.scroll('D')

while True:
    sleep(FREQ) 
    msg = 'n'
    if button_a.is_pressed(): 
        msg = 'A'
    elif button_b.is_pressed():
        msg = 'B'
    else:
        gesture = str(accelerometer.current_gesture())
    
        if gesture == "left":
            msg = 'r'
            #display.show()
        elif gesture == "right":
            msg = 'l'
        elif gesture == "shake":
            msg = 's'
        elif gesture == "face up":
            msg = 'b'
        elif gesture == "face down":
            msg = 'f'
 
    radio.send( msg )
    #display.show( msg )
    """
    if button_b.was_pressed():
        break
      
display.scroll('E')
"""