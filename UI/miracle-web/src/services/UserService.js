import BaseService from "./BaseService";
import { ControllerRoutes, UserRoutes } from "@/helpers/RouteConstraints";

export default class UserService extends BaseService {
  constructor() {
    super(ControllerRoutes.User);
  }

  async getOutSource() {
    return await this.request("GET", UserRoutes.GetOutSource());
  }
  async updateOutSource(model) {
    return await this.request("PUT", UserRoutes.UpdateOutSource(), model);
  }
  async getImage() {
    return await this.request("GET", UserRoutes.GetProfileImage());
  }
}
