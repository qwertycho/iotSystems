import time
import _thread
import random

lock = _thread.allocate_lock()

threadData = 0

def thread0():
    while True:
        rand = random.randint(0, 10)
        lock.acquire()
        global threadData
        threadData = rand
        lock.release()
        time.sleep(0.5)

def sent(message):
    print(message)

def main():
    _thread.start_new_thread(thread0, ())
    while True:
        localData = 0
        lock.acquire()
        global threadData
        localData = threadData
        threadData = 0
        lock.release()
        if(localData != 0):
            sent(localData)

main()
