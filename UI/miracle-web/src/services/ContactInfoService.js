import { ControllerRoutes } from "@/helpers/RouteConstraints";
import BaseService from "./BaseService";

export default class ContactInfoService extends BaseService {
  constructor() {
    super(ControllerRoutes.ContactInfo);
  }
}
