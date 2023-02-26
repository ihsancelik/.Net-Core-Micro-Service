import BaseService from "./BaseService";
import { APIRoutes, ControllerRoutes } from "@/helpers/RouteConstraints";

export default class ServerService extends BaseService {
  constructor() {
    super(ControllerRoutes.API);
  }

  async logoutUser(userId, username) {
    if (this.state.username !== username) {
      return false;
    } else {
      let response = await this.request("GET", APIRoutes.LogoutUser(userId));
      if (response) return true;
    }
  }

  async logoutAllUsers(username) {
    if (this.state.username !== username) {
      return false;
    } else {
      let response = await this.request("GET", APIRoutes.LogoutAllUsers());
      if (response) return true;
    }
  }
}
