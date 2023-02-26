import BaseService from "./BaseService";
import { ControllerRoutes, PlatformRoutes } from "@/helpers/RouteConstraints";

export default class PlatformService extends BaseService {
  constructor() {
    super(ControllerRoutes.Platform);
  }

  async getListByProductId(productId) {
    return await this.request("GET", PlatformRoutes.GetListByProductId(productId));
  }
}
