import BaseService from "./BaseService";
import { ControllerRoutes, PriorityRoutes } from "@/helpers/RouteConstraints";

export default class PriorityService extends BaseService {
  constructor() {
    super(ControllerRoutes.Priority);
  }

  async getPriorityByVersion(productId, versionInfoId) {
    return await this.request("GET", PriorityRoutes.GetPriorityByVersion(productId, versionInfoId));
  }
}
