export const Base =
  process.env.NODE_ENV === "production" ? "https://apicore.test.com/api/" : "http://127.0.0.1:5101/api/";
const Authentication = Base + "authentication/";
const Account = Base + "account/";
const API = Base + "api/";
const Company = Base + "company/";
const Dependency = Base + "dependency/";
const Helper = Base + "helper/";
const Notice = Base + "notice/";
const Platform = Base + "platform/";
const Priority = Base + "priority/";
const Product = Base + "product/";
const ProductModule = Base + "productModule/";
const ProductTag = Base + "productTag/";
const ProductVersion = Base + "productVersion/";
const Role = Base + "role/";
const SmtpSetting = Base + "smtpSetting/";
const Logging = Base + "logging/";
const User = Base + "user/";
const UserWatch = Base + "userWatch/";
const VersionInfo = Base + "versionInfo/";
const ServerInfo = Base + "serverInfo/";

export const ControllerRoutes = {
  Account: Account,
  Authentication: Authentication,
  API: API,
  Company: Company,
  Dependency: Dependency,
  Helper: Helper,
  Logging: Logging,
  Notice: Notice,
  Platform: Platform,
  Priority: Priority,
  Product: Product,
  ProductModule: ProductModule,
  ProductTag: ProductTag,
  ProductVersion: ProductVersion,
  Role: Role,
  SmtpSetting: SmtpSetting,
  User: User,
  UserWatch: UserWatch,
  VersionInfo: VersionInfo,
  ServerInfo: ServerInfo,
};

export const CRUDRoutes = {
  ListAll(controller) {
    return controller + "listAll";
  },
  List(controller) {
    return controller + "list";
  },
  Create(controller) {
    return controller + "create";
  },
  Update(controller, objectId) {
    return controller + `update/${objectId}`;
  },
  Delete(controller, objectId) {
    return controller + `delete/${objectId}`;
  },
  GetById(controller, objectId) {
    return controller + `get/${objectId}`;
  },
  GetCount(controller) {
    return controller + "count";
  },
};

export const AccountRoutes = {
  ResetPassword(userId) {
    return Account + `resetpass/${userId}`;
  },
  ForgotPassword() {
    return Account + "forgotPassword";
  },
  ChangePassword() {
    return Account + "changePass";
  },
};

export const AuthenticateRoutes = {
  Authenticate() {
    return Authentication + "authenticate";
  },
  AuthenticateByRefreshToken() {
    return Authentication + "authenticateByRefreshToken";
  },
  RevokeAuthenticate() {
    return Authentication + "revokeAuthenticate";
  },
  RevokeAuthenticateByAdmin() {
    return Authentication + "revokeAuthenticateByAdmin";
  },
};

export const APIRoutes = {
  StopServer() {
    return API + "stopServer";
  },
  GetServerInfo() {
    return API + "getProducts";
  },
  GetOnlineUsers() {
    return API + "getOnlineUsers";
  },
  GetOfflineUsers() {
    return API + "getOfflineUsers";
  },
  LogoutAllUsers() {
    return API + "logoutAllUsers";
  },
  LogoutUser(userId) {
    return API + `logoutUser/${userId}`;
  },
};

export const LoggingRoutes = {
  GetApiLogs() {
    return Logging + "getApiLogs";
  },
};

export const UserRoutes = {
  GetProfileImage(userId) {
    return User + `getProfileImage/${userId}`;
  },
  GetUserByModel() {
    return User + "getUser";
  },
  SaveProductSettings() {
    return User + "saveProductSettings";
  },
  GetUserProducts(userId) {
    return User + `getProducts/${userId}`;
  },
  GetProductLimitation(userId, productId) {
    return User + `getProductLimitation/${userId}/${productId}`;
  },
  RemoveUserProduct(userId, productId) {
    return User + `removeProduct/${userId}/${productId}`;
  },
  RemoveUserProductModule(userId, productId, moduleId) {
    return User + `removeModule/${userId}/${productId}/${moduleId}`;
  },
  UserProductAdd(userId, productId) {
    return User + `addProduct/${userId}/${productId}`;
  },
  UserProductModuleAdd(userId, productId, moduleId, isActive) {
    return User + `addModule/${userId}/${productId}/${moduleId}/${isActive}`;
  },
  UserProductAddVersion(userId, productId) {
    return User + `addVersion/${userId}/${productId}`;
  },
  GetProductVersions(userId, productId) {
    return User + `getProductVersions/${userId}/${productId}`;
  },
  GetProductModules(userId, productId) {
    return User + `getProductModules/${userId}/${productId}`;
  },
};

export const PlatformRoutes = {
  GetListByProductId(productId) {
    return Platform + `getListByProductId/${productId}`;
  },
};

export const ProductRoutes = {
  AddVersion(productId, versionInfoId, priorityId) {
    return Product + `addVersion/${productId}/${versionInfoId}/${priorityId}`;
  },
  RemoveVersion(productId, versionInfoId) {
    return Product + `removeVersion/${productId}/${versionInfoId}`;
  },
  AddModule(productId, moduleId) {
    return Product + `addModule/${productId}/${moduleId}`;
  },
  RemoveModule(productId, moduleId) {
    return Product + `removeModule/${productId}/${moduleId}`;
  },
  AddSetup(platformId, productId, versionInfoId) {
    return Product + `addSetup/${platformId}/${productId}/${versionInfoId}`;
  },
  ExistSetup(platformId, productId, versionInfoId) {
    return Product + `existSetup/${platformId}/${productId}/${versionInfoId}`;
  },
};

export const ProductModuleRoutes = {
  GetListByProductId(productId) {
    return ProductModule + `getListByProductId/${productId}`;
  },
};

export const PriorityRoutes = {
  GetPriorityByVersion(productId, versionInfoId) {
    return Priority + `getPriorityByVersion/${productId}/${versionInfoId}`;
  },
};

export const ServerInfoRoutes = {
  GetInfo() {
    return ServerInfo + "getInfo";
  },
  GetDependencyExceptions(libName) {
    return ServerInfo + `getDependencyExceptions/${libName}`;
  },
};

export const VersionInfoRoutes = {
  GetVersionListByProductId(productId) {
    return VersionInfo + `getVersionListByProductId/${productId}`;
  },

  GetVersionListByUserProduct(productId) {
    return VersionInfo + `getVersionListByUserProduct/${productId}`;
  },
};

export const UserWatchRoutes = {
  GetOnlineUser(userId) {
    return UserWatch + `getOnlineUser/${userId}`;
  },
  GetOnlineUsers() {
    return UserWatch + "getOnlineUsers";
  },
};
