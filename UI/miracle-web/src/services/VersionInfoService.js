import BaseService from "@/services/BaseService";
import { ControllerRoutes } from "@/helpers/RouteConstraints";

export default class VersionInfoService extends BaseService {
  constructor() {
    super(ControllerRoutes.VersionInfo);
  }
}