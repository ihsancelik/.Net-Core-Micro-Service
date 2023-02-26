import BaseService from "./BaseService";
import { ControllerRoutes, ServerInfoRoutes } from "@/helpers/RouteConstraints";

export default class ServerInfoService extends BaseService {
  constructor() {
    super(ControllerRoutes.ServerInfo);
  }

  async getInfo() {
    return await this.request("GET", ServerInfoRoutes.GetInfo());
  }

  async getDependencyExceptions(libName) {
    return await this.request("GET", ServerInfoRoutes.GetDependencyExceptions(libName));
  }
}
