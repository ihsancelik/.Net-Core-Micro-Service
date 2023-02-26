import BaseService from "./BaseService";
import { ControllerRoutes, TicketRoutes } from "@/helpers/RouteConstraints";

export default class TicketService extends BaseService {
  constructor() {
    super(ControllerRoutes.Ticket);
  }

  async getImage(ticketMessageId) {
    return await this.request("GET", TicketRoutes.GetImage(ticketMessageId));
  }

  async getMessageGroups() {
    return await this.request("GET", TicketRoutes.GetMessageGroups());
  }

  async getTicketMessages(ticketGroupId) {
    return await this.request("GET", TicketRoutes.GetTicketMessages(ticketGroupId));
  }

  async getUserInfo(userId) {
    return await this.request("GET", TicketRoutes.GetUserInfo(userId));
  }
  async getGroupCloseInfo(groupId) {
    return await this.request("GET", TicketRoutes.GetGroupCloseInfo(groupId));
  }

  async isClosedChange(groupId) {
    return await this.request("GET", TicketRoutes.IsClosedChange(groupId));
  }

  async getGroupReadInfo(groupId) {
    return await this.request("GET", TicketRoutes.GetGroupReadInfo(groupId));
  }
}