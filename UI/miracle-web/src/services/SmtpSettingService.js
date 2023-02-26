import BaseService from "./BaseService";
import { ControllerRoutes } from "@/helpers/RouteConstraints";

export default class SmtpSettingService extends BaseService {
  constructor() {
    super(ControllerRoutes.SmtpSetting);
  }
}
