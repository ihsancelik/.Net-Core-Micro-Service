import BaseService from "@/services/BaseService";
import { ControllerRoutes, LiveTicketRoutes } from "@/helpers/RouteConstraints";

export default class LiveTicketService extends BaseService {
  constructor() {
    super(ControllerRoutes.LiveTicket);
  }
  async getContents(roomName) {
    return await this.request("GET", LiveTicketRoutes.GetContents(roomName));
  }

  async getChats(roomName) {
    return await this.request("GET", LiveTicketRoutes.GetChats(roomName));
  }

  async getCustomerName(roomName) {
    return await this.request("GET", LiveTicketRoutes.GetCustomerName(roomName));
  }

  
  
}