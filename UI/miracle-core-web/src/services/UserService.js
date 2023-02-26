import BaseService from "./BaseService";
import { ControllerRoutes, UserRoutes, AccountRoutes } from "@/helpers/RouteConstraints";

export default class UserService extends BaseService {
  constructor() {
    super(ControllerRoutes.User);
  }

  async userGetProfileImage(userId) {
    return await this.request("GET", UserRoutes.GetProfileImage(userId));
  }

  async getUserProducts(userId, paginationModel) {
    return await this.request("POST", UserRoutes.GetUserProducts(userId), paginationModel);
  }

  async getProductLimitation(userId, productId) {
    return await this.request("POST", UserRoutes.GetProductLimitation(userId, productId));
  }

  async userProductAdd(userId, productId, productLimitation) {
    return await this.request("POST", UserRoutes.UserProductAdd(userId, productId), productLimitation);
  }
  async userProductModuleAdd(userId, productId, moduleId, isActive) {
    return await this.request("GET", UserRoutes.UserProductModuleAdd(userId, productId, moduleId, isActive));
  }

  async removeUserProduct(userId, productId) {
    return await this.request("DELETE", UserRoutes.RemoveUserProduct(userId, productId));
  }

  async removeUserProductModule(userId, productId, moduleId) {
    return await this.request("DELETE", UserRoutes.RemoveUserProductModule(userId, productId, moduleId));
  }

  async userProductAddVersion(userId, productId, versionInfoIdList) {
    return await this.request("POST", UserRoutes.UserProductAddVersion(userId, productId), { versionInfoIdList });
  }

  async getProductVersions(userId, productId) {
    return await this.request("POST", UserRoutes.GetProductVersions(userId, productId));
  }

  async getProductModules(userId, productId, paginationModel) {
    return await this.request("POST", UserRoutes.GetProductModules(userId, productId), paginationModel);
  }

  async passwordReset(userId, model) {
    return await this.request("POST", AccountRoutes.ResetPassword(userId), model);
  }
}
