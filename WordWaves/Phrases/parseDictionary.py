import sys

def main():
  try:
	file = open(sys.argv[1], "r")
	words = []
	for line in file:
		words.append(line)

	print(len(words))

  except:
    print("Missing arguments. Expects: python parseDictionary.py inputDictionary.txt minLength maxLength outDir")

if __name__ == "__main__":
  main()