import BaseService from "@/services/BaseService";
import { ControllerRoutes, ProductModuleRoutes } from "@/helpers/RouteConstraints";

export default class ProductModuleService extends BaseService {
  constructor() {
    super(ControllerRoutes.ProductModule);
  }

  async getListByProductId(productId, paginationModel) {
    return await this.request("POST", ProductModuleRoutes.GetListByProductId(productId), paginationModel);
  }
}
