import serial
import sys
import os
from pynput import keyboard

from time import sleep
# Setup
if os.name == 'nt':
    serialPort = 'COM5'
else:
    serialPort = '/dev/ttyUSB0'

print("Dingus.py - Opening serial port")
transmitter = serial.Serial(serialPort, 115200)
print("linking up to <<" + serialPort + ">>...")
sleep(2)

print("Input your chars:")

'transmitter.write(b'r')'
'transmitter.write(keyP.encode())'

# import sys, os

def wait_key():
    ''' Wait for a key press on the console and return it. '''
    
    result = None
    if os.name == 'nt':
        import msvcrt
        result = msvcrt.getwch()
    else:
        import termios
        fd = sys.stdin.fileno()

        oldterm = termios.tcgetattr(fd)
        newattr = termios.tcgetattr(fd)
        newattr[3] = newattr[3] & ~termios.ICANON & ~termios.ECHO
        termios.tcsetattr(fd, termios.TCSANOW, newattr)

        try:
            result = sys.stdin.read(1)
        except IOError:
            pass
        finally:
            termios.tcsetattr(fd, termios.TCSAFLUSH, oldterm)

    return result
'''
def on_press(key):
    try:
        print('alphanumeric key {0} pressed'.format(
            key.char))
    except AttributeError:
        print('special key {0} pressed'.format(
            key))

def on_release(key):
    print('{0} released'.format(
        key))
    if key == keyboard.Key.esc:
        # Stop listener
        return False
'''
def on_pressKeyboard(key):
    try:
        print('alphanumeric key {0}'.format(
            key.char))
        if(key.char == 'r'):
            transmitter.write(b'r')
            print("                             <" + key.char + "> transmitted")
    except AttributeError:
        print('invalid key {0}'.format(
            key))
        if(key == keyboard.Key.esc):
            if os.name == 'nt':
                os._exit(0)
            else:
                os.kill(os.getpid(), signal.SIGINT)
            
'''
# Collect events until released
with keyboard.Listener(
        on_press=on_press,
        on_release=on_release) as listener:
    listener.join()
'''

# ...or, in a non-blocking fashion:
listener = keyboard.Listener(
    #on_press=on_press,
    #on_release=on_release
    on_press=on_pressKeyboard)
listener.start()

while True:
	sleep(1)
	
'''
	keyP = wait_key()
	if(keyP == 'r'):
		transmitter.write(b'r')
'''

'''
async def color_input(prompt):
    """User Input for a color value"""
    red_list = ["red", "r", "rot", "1"]
    green_list = [
        "green",
        "g",
        "grün",
        "gruen",
        "2",
    ]
    blue_list = [
        "blue",
        "b",
        "blau",
        "3",
    ]

    user_input = await asyncinput(prompt)
    if user_input.lower() in red_list:
        return gv.COLOR_RED
    elif user_input.lower() in green_list:
        return gv.COLOR_GREEN
    elif user_input.lower() in blue_list:
        return gv.COLOR_BLUE
    return 0
'''
