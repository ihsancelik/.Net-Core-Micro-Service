import BaseService from "./BaseService";
import { ControllerRoutes } from "@/helpers/RouteConstraints";

export default class RoleService extends BaseService {
  constructor() {
    super(ControllerRoutes.Role);
  }
}