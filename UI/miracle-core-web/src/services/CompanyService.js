import BaseService from "./BaseService";
import { ControllerRoutes } from "@/helpers/RouteConstraints";

export default class CompanyService extends BaseService {
  constructor() {
    super(ControllerRoutes.Company);
  }
}
