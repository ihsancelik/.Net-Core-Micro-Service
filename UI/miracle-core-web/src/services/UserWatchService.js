import BaseService from "./BaseService";
import { ControllerRoutes, UserWatchRoutes } from "@/helpers/RouteConstraints";

export default class UserWatchService extends BaseService {
  constructor() {
    super(ControllerRoutes.UserWatch);
  }

  async getOnlineUsers() {
    return await this.request("GET", UserWatchRoutes.GetOnlineUsers());
  }
}
