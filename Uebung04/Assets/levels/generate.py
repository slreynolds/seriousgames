#########
#
# generate Pacman level according to Uebung04 of Serious Games lecture
# 2017.05.24
#
##########
import random, os, sys
import argparse

parser = argparse.ArgumentParser(description='generate Pacman level according to Uebung04 of Serious Games lecture.')
parser.add_argument('-f', '--filename', type=str, 
                    help='filename to write output to')

parser.add_argument('-s', '--size', type=int, default=4,
                    help='size of the level (default: 4)')


parser.add_argument('-w', '--width', type=int, default=7,
                    help='width of the level (default: 11)')


args = parser.parse_args()

m = ""

for i in range(args.size):
    for x in range(args.width):
        a = random.randrange(0,4)
        if a == 0 or a == 3:
            m += "+"
        if a == 1:
            m += "-"
        if a == 2:
            m += "|"
    m += os.linesep

characters = ["Pacman", "Blinky", "Inky", "Pinky", "Clyde"];
cords = [];
for c in characters:
    done = False
    while not done:
        x = random.randrange(0,args.size)
        y = random.randrange(0,args.width)
        if not any([x,y] in s for s in cords):
            m += c + " " + str(x) + " " + str(y) + os.linesep
            cords.append([x,y])
            done = True

if(args.filename):
    with open(args.filename,"w") as f:
        f.write(m)
        print("Level succesfully written to " + args.filename)
else:
    print(m)