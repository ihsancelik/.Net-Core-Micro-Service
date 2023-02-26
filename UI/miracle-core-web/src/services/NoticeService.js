import BaseService from "./BaseService";
import { ControllerRoutes } from "@/helpers/RouteConstraints";

export default class NoticeService extends BaseService {
  constructor() {
    super(ControllerRoutes.Notice);
  }
}