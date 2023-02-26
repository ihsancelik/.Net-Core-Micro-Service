import BaseService from "./BaseService";
import { ControllerRoutes } from "@/helpers/RouteConstraints";

export default class ProductTagService extends BaseService {
  constructor() {
    super(ControllerRoutes.ProductTag);
  }
}