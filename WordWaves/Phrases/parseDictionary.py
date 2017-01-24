import sys

def main():
  try:
	file = open(sys.argv[1], "r")
	for line in file:
		print(line, end='')

  except:
    print("Missing arguments. Expects: python parseDictionary.py inputDictionary.txt minLength maxLength outDir")

if __name__ == "__main__":
  main()