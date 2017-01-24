import sys

def main():
  try:
    with open(sys.argv[1], "r") as ins:
      array = []
      for line in ins:
        print(line)
        array.append(line)

  except:
    print("Missing arguments. Expects: python parseDictionary.py inputDictionary.txt minLength maxLength outDir")

if __name__ == "__main__":
  main()