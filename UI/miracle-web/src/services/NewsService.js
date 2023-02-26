import { ControllerRoutes, NewsRoutes } from "../helpers/RouteConstraints";
import BaseService from "./BaseService";

export default class NewsService extends BaseService {
  constructor() {
    super(ControllerRoutes.News);
  }
  async getImage(newsId) {
    return await this.request("GET", NewsRoutes.GetImage(newsId));
  }

  async getByTag(tag) {
    return await this.request("GET", NewsRoutes.GetByTag(tag));
  }
}
