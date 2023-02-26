import json


class ProductInfo:
    def __init__(self, jsonData):
        self.ProductTag = jsonData["ProductTag"]
        self.ProductVersion = jsonData["ProductVersion"]
        self.Platform = jsonData["Platform"]
        self.PackageInfos = [PackageInfo("")]

        self.PackageInfos.clear()
        for packageInfo in jsonData["PackageInfos"]:
            self.PackageInfos.append(PackageInfo(packageInfo))


class PackageInfo:
    def __init__(self, jsonData):

        if(jsonData == ""):
            self.Identifier = ""
            self.Packages = [Package("")]
        else:
            self.Identifier = jsonData["Identifier"]
            self.Packages = [Package("")]

            self.Packages.clear()
            for package in jsonData["Packages"]:
                self.Packages.append(Package(package))


class Package:

    def __init__(self, jsonData):

        if(jsonData == ""):
            self.Name = "",
            self.Target = "",
            self.Version = ""
        else:
            self.Name = jsonData["Name"]
            self.Version = jsonData["Version"]
            self.Target = jsonData["Target"]


class Settings:
    def __init__(self, jsonData):
        self.PackagesJsonDirectory = jsonData["PackagesJsonDirectory"]
        self.PackagesJsonName = jsonData["PackagesJsonName"]
        self.BaseApiUrl = jsonData["BaseApiUrl"]
        self.GetPackageApiUrl = jsonData["GetPackageApiUrl"]
