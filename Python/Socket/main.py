import data_pb2
import time

data = data_pb2.data()

data.x=0.5
data.y=0.1
data.z=0.2
data.a=True
data.b=False

biInfo = data.SerializeToString()

from socket import *

PySocket = socket (AF_INET,SOCK_STREAM)

#connect 
address = ('localhost',12345)
PySocket.bind(address)
PySocket.listen(1)

#PySocket.connect(address)

#send data
while True:
	connection,clident_address = PySocket.accept();
	connection.sendall(biInfo)
	sleep(100)


