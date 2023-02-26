import { ControllerRoutes,FeedBackRoutes } from "@/helpers/RouteConstraints";
import BaseService from "./BaseService";

export default class FeedBackService extends BaseService {
  constructor() {
    super(ControllerRoutes.FeedBack);
  }
  async getOptions(){
    return await this.request("GET", FeedBackRoutes.GetOptions());
  }
}
