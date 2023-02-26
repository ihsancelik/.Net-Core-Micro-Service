import BaseService from "./BaseService";
import { ControllerRoutes, VersionInfoRoutes } from "@/helpers/RouteConstraints";

export default class VersionInfoService extends BaseService {
  constructor() {
    super(ControllerRoutes.VersionInfo);
  }

  async getVersionListByProductId(productId, paginationModel) {
    return await this.request("POST", VersionInfoRoutes.GetVersionListByProductId(productId), paginationModel);
  }

  async getVersionListByUserProduct(productId) {
    return await this.request("GET", VersionInfoRoutes.GetVersionListByUserProduct(productId));
  }

}
