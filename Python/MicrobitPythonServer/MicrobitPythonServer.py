import serial
import time 
import json
from socket import *

PySocket = socket (AF_INET,SOCK_STREAM)

#connect 
address = ('localhost',9999)
PySocket.bind(address)
PySocket.listen(1)



PORT = 'COM3'
BR = 115200
TIMEOUT = 50/1000

ser = serial.Serial(port=PORT, baudrate=BR, timeout=TIMEOUT)


connection,client_address = PySocket.accept();

old = 'n'

try:
        while True:
                time.sleep(TIMEOUT)
                s = ser.readline()
                #print s
                if not s or len(s)<1:
                        s = old
                else:
                        old = s

        #c = s[0]
                for c in s:
                        
                        if (c=='A'):
                                data = {"Left":False,"Right":False,"Front":False,"ButtonA":True,"ButtonB":False}
                                break
                        elif (c=='B'):
                                data = {"Left":False,"Right":False,"Front":False,"ButtonA":False,"ButtonB":True}
                                break
                        elif (c == 'l'):
                                data = {"Left":True,"Right":False,"Front":False,"ButtonA":False,"ButtonB":False}
                                break
                        elif(c=='r'):
                                data = {"Left":False,"Right":True,"Front":False,"ButtonA":False,"ButtonB":False}
                                break
                        elif(c=='f'):
                                data = {"Left":False,"Right":False,"Front":True,"ButtonA":False,"ButtonB":False}
                                break
                        else:
                                data = {"Left":False,"Right":False,"Front":False,"ButtonA":False,"ButtonB":False}

                jsonData = json.dumps(data)
                #print jsonData

                connection.sendall(jsonData+'\n')
except Exception as e:
        print e
finally:
        PySocket.close()
        ser.close()

#print type(jsonData)

#PySocket.connect(address)

#send data
#while True:

        #time.sleep(0.1)



 


