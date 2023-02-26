import { ControllerRoutes } from "@/helpers/RouteConstraints";
import BaseService from "./BaseService";

export default class ContactFormService extends BaseService {
  constructor() {
    super(ControllerRoutes.ContactForm);
  }
}
