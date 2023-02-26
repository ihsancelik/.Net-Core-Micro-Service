import { AboutRoutes, ControllerRoutes } from "../helpers/RouteConstraints";
import BaseService from "./BaseService";

export default class AboutService extends BaseService {
  constructor() {
    super(ControllerRoutes.About);
  }

  async get() {
    return await this.request("GET", AboutRoutes.Get());
  }
  async getImage() {
    return await this.request("GET", AboutRoutes.GetImage());
  }
}
