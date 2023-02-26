import BaseService from "./BaseService";
import { APIRoutes, ControllerRoutes } from "@/helpers/RouteConstraints";

export default class DependencyService extends BaseService {
  constructor() {
    super(ControllerRoutes.Dependency);
  }

  async stopServer() {
    let response = await this.request("GET", APIRoutes.StopServer());
    if (response) return true;
  }
}
