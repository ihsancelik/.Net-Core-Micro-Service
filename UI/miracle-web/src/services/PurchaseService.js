import { ControllerRoutes } from "@/helpers/RouteConstraints";

import BaseService from "./BaseService";

export default class PurchaseService extends BaseService {
  constructor() {
    super(ControllerRoutes.Purchase);
  }
}
