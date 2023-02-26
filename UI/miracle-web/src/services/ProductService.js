import { ControllerRoutes } from "@/helpers/RouteConstraints";
import { ProductRoutes } from "@/helpers/RouteConstraints";
import BaseService from "./BaseService";

export default class ProductService extends BaseService {
  constructor() {
    super(ControllerRoutes.Product);
  }

  async addProductDetail(productId, model) {
    return await this.request("POST", ProductRoutes.AddProductDetail(productId), model);
  }

  async removeProductDetail(productId, productDetailId) {
    return await this.request("DELETE", ProductRoutes.RemoveProductDetail(productId, productDetailId));
  }

  async getProducts() {
    return await this.request("GET", ProductRoutes.GetProducts());
  }

  async productGetByTag(tag) {
    return await this.request("GET", ProductRoutes.GetByTag(tag));
  }

  async getImage(productId) {
    return await this.request("GET", ProductRoutes.GetImage(productId));
  }

  async getMiracleWorld(platform) {
    return await this.request("GET", ProductRoutes.GetMiracleWorld(platform));
  }
}
