import requests
import os
import json


class bcolors:
    FAIL = '\033[91m'
    ENDC = '\033[0m'


class helpers:
    def readJsonFile(path):
        file = open(path)
        jsonData = json.load(file)
        return jsonData

    def getPackageInfoString(package):
        return f'Package: {package["Platform"]} {package["Identifier"]} {package["Name"]} {package["Version"]}'

    def getErrorString(package, errorList):
        errorString = bcolors.FAIL
        errorString += f'\n{helpers.getPackageInfo(package)} package has errors! \nErrors:({len(errorList)}) \n'
        for error in errorList:
            errorString += f'- {error} \n'

        errorString += f'\n{bcolors.ENDC}'
        return errorString

    def downloadWithUrl(url, name, target):
        basePath = os.getcwd()
        if(target != ""):
            fileDirectory = os.path.join(basePath, target)
            filePath = os.path.join(fileDirectory, name)
        else:
            filePath = os.path.join(basePath, name)

        if not os.path.exists(fileDirectory):
            os.makedirs(fileDirectory)

        response = requests.get(url)
        open(filePath, "wb").write(response.content)

    def download(postModel):
        responseJson = requests.post(
            getPackageUrl, headers=headers, json=postModel).json()
        errorList = responseJson["errorList"]

        if(len(errorList) == 0):
            downloadUrl = f'http://localhost:5010{responseJson["data"]}'
            packageName = postModel["Name"]
            target = package["Target"]
            print(downloadUrl, packageName)
            downloadWithUrl(downloadUrl, packageName, "", target)
        else:
            print(getErrors(package, errorList))
