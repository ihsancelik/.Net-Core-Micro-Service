import BaseService from "./BaseService";
import { ControllerRoutes, LoggingRoutes } from "@/helpers/RouteConstraints";

export default class LoggingService extends BaseService {
  constructor() {
    super(ControllerRoutes.Logging);
  }

  async logInitialize(model) {
    return await this.request("POST", LoggingRoutes.GetApiLogs(), model);
  }
}
