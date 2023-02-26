import { ControllerRoutes, SliderRoutes } from "../helpers/RouteConstraints";
import BaseService from "./BaseService";

export default class SliderService extends BaseService {
  constructor() {
    super(ControllerRoutes.Slider);
  }

  async getImage(sliderId) {
    return await this.request("GET", SliderRoutes.GetImage(sliderId));
  }
}
